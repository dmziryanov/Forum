using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using RmsAuto.Common.Diagnostics;
using RmsAuto.Common.Configuration;

namespace RmsAuto.Common.References
{

    public interface IItemsProxy<TItem>
    {
        IEnumerable<TItem> GetItems();
    }
    
    //TODO: Разобраться с временен кэширования, а то слишком часто сюда лазит
    public class Reference<TKey, TItem> : IReference, IMgmtTarget
    {
        class CacheItem
        {
            public Exception LastError { get; set; }
            public Dictionary<TKey, TItem> ReferenceData { get; set; }
        }

        private string _name;
        private string _description;
        private Func<IItemsProxy<TItem>, IEnumerable<TItem>> _itemsExtractor;
     
        private Func<TItem, TKey> _keySelector;
        private readonly object _sync = new object();
        private readonly string _cacheKey = Guid.NewGuid().ToString();
        private IItemsProxy<TItem> _acctgProxy;


        private DateTime _initTime;
        private int _syncRequestCount;
        private DateTime? _lastSyncTime;
        private DateTime? _expirationTime;
                        
        public Reference(
            string name,
            string description,
            IItemsProxy<TItem> ItemsProxy,
            Func<IItemsProxy<TItem>, IEnumerable<TItem>> itemsExtractor, 
            Func<TItem, TKey> keySelector)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Reference' name cannot be empty", "name");
            if (itemsExtractor == null)
                throw new ArgumentNullException("itemsExtractor");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            _name = name;
            _description = description;
            _itemsExtractor = itemsExtractor;
            _keySelector = keySelector;
            _initTime = DateTime.Now;
            _acctgProxy = ItemsProxy;

            var cacheConfig = ReferenceCacheConfiguration.Current;
            var cacheSettings = cacheConfig != null ? cacheConfig.Settings[_name] : null;
      
        }

        public string Name
        {
            get { return _name; }
        }

        public string Description
        {
            get { return _description; }
        }

        IEnumerable IReference.Items
        {
            get { return _GetItems().Values; }
        }

        object IReference.this[object key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException("key");
                return this[(TKey)Convert.ChangeType(key, typeof(TKey))];
            }
        }

        public IEnumerable<TItem> Items
        {
            get { return _GetItems().Values; }
        }

     
        
        public TItem this[TKey key]
        {
            get 
            {
                var items = _GetItems(); 
                return items.ContainsKey(key) ? items[key] : default(TItem); 
            }
        }

        private Dictionary<TKey, TItem> _GetItems()
        {
            var cacheConfig = ReferenceCacheConfiguration.Current;
            var cacheSettings = cacheConfig != null ? cacheConfig.Settings[_name] : null;
            var cache = HttpRuntime.Cache;
            if (cacheSettings == null || cache == null)
                return _itemsExtractor(_acctgProxy).ToDictionary(_keySelector);
            
            var item = (CacheItem)cache[_cacheKey];
            if (item == null)
            {
                lock (_sync)
                {
                    item = (CacheItem)cache[_cacheKey];
                    if (item == null)
                    {
                        _syncRequestCount++;
                        _lastSyncTime = DateTime.Now;
                
                        try
                        {
                            item = new CacheItem
                            {
                                ReferenceData = _itemsExtractor(_acctgProxy).ToDictionary(_keySelector)
                            };
                            _expirationTime = _lastSyncTime.Value.AddMinutes(cacheSettings.ExpirationTimeout);
                        }
                        catch (Exception ex)
                        {
                            item = new CacheItem
                            {
                                LastError = ex
                            };
                            _expirationTime = _lastSyncTime.Value.AddMinutes(cacheSettings.ErrorCachingTimeout);
                        }
                                                
                        cache.Add(
                            _cacheKey,
                            item,
                            null,
                            _expirationTime.Value,
                            Cache.NoSlidingExpiration,
                            CacheItemPriority.Normal,
                            null);
                    }
                }
            }

            if (item.LastError != null)
                throw item.LastError;

            return item.ReferenceData;
        }



        #region IMgmtTarget Members

        void IMgmtTarget.Purge()
        {
            lock (_sync)
                HttpRuntime.Cache.Remove(_cacheKey);
        }

        CacheStatistics IMgmtTarget.GetStatistics()
        {
            lock (_sync)
            {
                var stats = new CacheStatistics()
                {
                    ReferenceName = _name,
                    ReferenceDescription = _description,
                    InitTime = _initTime,
                    DiagTime = DateTime.Now,
                    SyncRequestCount = _syncRequestCount,
                    LastSyncTime = _lastSyncTime
                };
                if (HttpRuntime.Cache[_cacheKey] != null)
                {
                    stats.ItemCount = _GetItems().Count;
                    stats.ExpirationTime = _expirationTime;
                }
                return stats;
            }
        }

        #endregion
    }
}
