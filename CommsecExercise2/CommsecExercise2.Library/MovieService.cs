using System.Collections.Generic;
using CommsecExercise2.Library.Interfaces;
using CommsecExercise2.Library.Managers;
using MoviesLibrary;

namespace CommsecExercise2.Library
{
    public class MovieService : IMovieService
    {
        private readonly IMovieManager _movieManager;

        public MovieService()
        {
            // this constructor is used for model binding 
            // model binding in a wcf class library is done here as oppose to dependency injection in MVC

            ICacheManager cacheManager = new CacheManager();
            IDataManager dataManager = new DataManager();
            _movieManager = new MovieManager(dataManager , cacheManager);
        }

        public MovieService(IDataManager dataManager , ICacheManager cacheManager)
        {
            // this constructor will be used in testing for mocking data Manager & cache Manager 
            _movieManager = new MovieManager(dataManager , cacheManager);
        }

        public List<MovieData> Get()
        {
           return _movieManager.Get();
        }

        public MovieData GetById(int id)
        {
            return _movieManager.GetById(id);
        }

        public List<MovieData> GetSorted(string fieldName)
        {
            return _movieManager.GetSorted(fieldName);
        }

        public List<MovieData> Search(string searchTerm)
        {
            return _movieManager.Search(searchTerm);
        }

        public int Insert(MovieData movieData)
        {
            return _movieManager.Insert(movieData);
        }

        public void Update(MovieData movieData)
        {
             _movieManager.Update(movieData);
        }
    }
}
