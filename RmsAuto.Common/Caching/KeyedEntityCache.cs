using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace RmsAuto.Common.Caching
{
    public class KeyedEntityCache<TKey, TEntity> : IKeyedEntities<TKey, TEntity>
        where TEntity : class
    {
        public enum ExpirationMode
        {
            TimeoutElapsed, DataChanged
        }

        private readonly object _sync = new object();
        private readonly string _cacheKey = Guid.NewGuid().ToString();
        private string _connectionString;
        private Func<TEntity, TKey> _extractKey;
        private ExpirationMode _expMode;

        public KeyedEntityCache(string connectionString, Func<TEntity, TKey> extractKey)
            : this(connectionString, extractKey, ExpirationMode.DataChanged)
        {
        }

        public KeyedEntityCache(
            string connectionString,
            Func<TEntity, TKey> extractKey,
            ExpirationMode expMode)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("ConnectionString cannot be empty", "connectionString");
            if (extractKey == null)
                throw new ArgumentNullException("extractKey");
            _connectionString = connectionString;
            _extractKey = extractKey;
            _expMode = expMode;
            if (_expMode == ExpirationMode.DataChanged)
                SqlDependency.Start(_connectionString);
        }

        private IDictionary<TKey, TEntity> _InnerMap
        {
            get
            {
                var cache = HttpRuntime.Cache;
                var map = (IDictionary<TKey, TEntity>)cache[_cacheKey];
                if (map == null)
                {
                    lock (_sync)
                    {
                        map = (IDictionary<TKey, TEntity>)cache[_cacheKey];
                        if (map == null)
                        {
                            SqlCacheDependency dependency;
                            DateTime absolutExpiration;
                            map = LoadMap(out dependency, out absolutExpiration);
                            cache.Add(
                                _cacheKey,
                                map,
                                dependency,
                                absolutExpiration,
                                Cache.NoSlidingExpiration,
                                CacheItemPriority.Normal,
                                null );
                        }
                    }
                }
                return map;
            }
        }
        
		public string CacheKey
		{
			get { return _cacheKey; }
		}

        public TEntity this[TKey key]
        {
            get { return _InnerMap[key]; }
        }

		public ICollection<TEntity> Values
		{
			get { return _InnerMap.Values; }
		}

        public bool ContainsKey(TKey key)
        {
            return _InnerMap.ContainsKey(key);
        }

        private IDictionary<TKey, TEntity> LoadMap(
            out SqlCacheDependency dependency,
            out DateTime absolutExpiration)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (DataContext dc = new DataContext(conn))
                {
                    SqlCommand cmd = (SqlCommand)dc.GetCommand(
                        dc.GetTable<TEntity>().AsQueryable<TEntity>());

                    if (_expMode == ExpirationMode.DataChanged)
                    {
                        dependency = new SqlCacheDependency(cmd);
                        absolutExpiration = Cache.NoAbsoluteExpiration;
                    }
                    else
                    {
                        dependency = null;
                        absolutExpiration = DateTime.Now.AddMinutes(_CacheTimeout);
                    }

                    var map = new Dictionary<TKey, TEntity>();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        foreach (var entity in dc.Translate<TEntity>(reader))
                            map.Add(_extractKey(entity), entity);
                    }
                    return map;
                }
            }
        }

        private static int _DefaultTimeout = 10;

        private static int _CacheTimeout
        {
            get
            {
                string cacheTimeout = ConfigurationManager.AppSettings["CacheTimeout"];
                return !string.IsNullOrEmpty(cacheTimeout) ?
                    int.Parse(cacheTimeout) : _DefaultTimeout;
            }
        }
    }
}
