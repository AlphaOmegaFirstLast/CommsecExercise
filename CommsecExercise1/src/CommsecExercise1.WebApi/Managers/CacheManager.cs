using System;
using CommsecExercise1.WebApi.Interfaces;
using Microsoft.Framework.Caching.Memory;

namespace CommsecExercise1.WebApi.Managers
{
    public class CacheManager: ICacheManager
    {
        private readonly IMemoryCache _memoryCache;


        public CacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Set<T>(string key, T obj, DateTime cacheExpiryDatetime)    
        {
            var options = new MemoryCacheEntryOptions { AbsoluteExpiration = cacheExpiryDatetime };         

            _memoryCache.Set(key, obj, options);
        }

        public T Get<T>(string key) where T:class           
        {
            return _memoryCache.Get(key) as T;   
        }
    }
}
