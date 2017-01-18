using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace RmsAuto.Common.Caching
{
	public class SingleObjectCache<TObject>
		where TObject : class, new()
	{
		private readonly object _sync = new object();
		private readonly string _cacheKey = Guid.NewGuid().ToString();

		int? _cacheTimeoutMinutes;
		string[] _dependingOnCacheKeys;
		
		public SingleObjectCache()
		{
		}

		public SingleObjectCache( string[] dependingOnCacheKeys, int? cacheTimeoutMinutes )
		{
            //if( cacheTimeoutMinutes.HasValue && cacheTimeoutMinutes <= 0 )
            //    throw new ArgumentOutOfRangeException( "cacheTimeoutMinutes", "value must be null or positive integer number" );
			_dependingOnCacheKeys = dependingOnCacheKeys;
			_cacheTimeoutMinutes = cacheTimeoutMinutes;
		}

		public TObject CachedObject
		{
			get
			{
				TObject res = (TObject)HttpRuntime.Cache[ _cacheKey ];
				if( res == null )
				{
					lock( _sync )
					{
						res = (TObject)HttpRuntime.Cache[ _cacheKey ];
						if( res == null )
						{
							res = new TObject();
							HttpRuntime.Cache.Add( _cacheKey,
								res,
								new CacheDependency( null, _dependingOnCacheKeys ),
								_cacheTimeoutMinutes.HasValue ? DateTime.Now.AddMinutes( _cacheTimeoutMinutes.Value ) : Cache.NoAbsoluteExpiration,
								Cache.NoSlidingExpiration,
								CacheItemPriority.Normal,
								null );
						}
					}
				}
				return res;
			}
		}

		public string CacheKey
		{
			get { return _cacheKey; }
		}
	}
}
