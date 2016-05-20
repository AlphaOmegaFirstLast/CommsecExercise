using System;

namespace CommsecExercise1.WebApi.Interfaces
{
    public interface ICacheManager
    {
        T Get<T>(string key) where T : class;
        void Set<T>(string key, T obj, DateTime cacheExpiryDatetime)  ;
    }
}