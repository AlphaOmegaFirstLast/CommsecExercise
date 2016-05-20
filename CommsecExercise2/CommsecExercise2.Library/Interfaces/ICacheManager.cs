using System;

namespace CommsecExercise2.Library.Interfaces
{
    public interface ICacheManager
    {
        T Get<T>(string key) where T : class;
        void Set<T>(string key, T obj, DateTime cacheExpiryDatetime);
    }
}