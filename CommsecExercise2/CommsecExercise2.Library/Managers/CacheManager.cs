using System;
using System.Web;
using System.Web.Caching;
using CommsecExercise2.Library.Interfaces;

namespace CommsecExercise2.Library.Managers
{
    public class CacheManager: ICacheManager
    {
  
        public void Set<T>(string key, T obj, DateTime cacheExpiryDatetime)
        {
            HttpRuntime.Cache.Add(key, obj, null, cacheExpiryDatetime, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        }

        public T Get<T>(string key) where T:class          
        {
            return HttpRuntime.Cache.Get(key) as T;   
        }
    }
}
