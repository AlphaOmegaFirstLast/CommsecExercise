using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using CommsecExercise1.WebApi.Interfaces;
using MoviesLibrary;

namespace CommsecExercise1.WebApi.Managers
{
    public class MovieManager : IMovieManager
    {
        private static DateTime _cacheExpiryDatetime = DateTime.UtcNow;
        private readonly IDataManager _dataManager;
        private readonly ICacheManager _cacheManager;

        public MovieManager(IDataManager dataManager, ICacheManager cacheManager)
        {
            _dataManager = dataManager;
            _cacheManager = cacheManager;
        }

        public List<MovieData> Get()
        {
            return GetCachedMovieList();
        }

        public MovieData GetById(int id)
        {
            var movieList = GetCachedMovieList();   // check cache first.
            var movie = movieList.FirstOrDefault(m => m.MovieId == id);
            if (movie == null)
            {
                movie = _dataManager.GetById(id);
            }
            return movie;
        }

        public List<MovieData> GetSorted(string fieldName)
        {
            if (IsValidSortFieldName(fieldName))
            {
                var movieList = GetCachedMovieList();

                //in case using system.linq.Dynamic nuget package
                var movieListSorted = movieList.OrderBy(fieldName).ToList();
                return movieListSorted;
            }
            else
            {
                throw new Exception("Invalid Sort Field Name");
            }
        }

        public List<MovieData> Search(string searchTerm)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var movieList = GetCachedMovieList();
                var searchResultsList = movieList.Where(m => HasSearchTerm(m, searchTerm)).ToList();
                return searchResultsList;
            }
            else
            {
                throw new Exception("Invalid Search Term");
            }
        }

        public int Insert(MovieData movieData)
        {
            if (movieData != null && !string.IsNullOrEmpty(movieData.Title))
            {
                try
                {
                    movieData.MovieId = _dataManager.Insert(movieData);

                    //update cache by adding the newly created object
                    var movieList = GetCachedMovieList();
                    movieList.Add(movieData);
                    _cacheManager.Set(Constants.Constants.CacheKeys.MovieList, movieList, _cacheExpiryDatetime);

                    return movieData.MovieId;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            else
            {
                throw new Exception("Invalid Movie Data");
            }
        }

        public void Update(MovieData movieData)
        {
            if (movieData != null && !string.IsNullOrEmpty(movieData.Title))
            {
                try
                {
                    var movieList = GetCachedMovieList();
                    var tempMovieData = movieList.Find(m => m.MovieId == movieData.MovieId);

                    _dataManager.Update(movieData);

                    //update list in cache
                    movieList.Remove(tempMovieData);  //update list in cache
                    movieList.Add(movieData);
                    _cacheManager.Set(Constants.Constants.CacheKeys.MovieList, movieList, _cacheExpiryDatetime);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            else
            {
                throw new Exception("Invalid Movie Data.");
            }
        }

        private bool IsValidSortFieldName(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return false;
            }
            switch (fieldName.Trim().ToLower())
            {
                case "movieid":
                case "title":
                case "genre":
                case "classification":
                case "releasedate":
                case "rating":
                    return true;
                default:
                    return false;
            }
        }

        private bool HasSearchTerm(MovieData movieData, string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            //for better performance, the code checks the "cast" field first, before calling reflection to avoid its cost

            var isSearchtermFound = movieData.Cast.Count(c => c.ToLower().Contains(searchTerm)) > 0;

            if (!isSearchtermFound)
            {
                var propertyList = movieData.GetType().GetProperties();
                isSearchtermFound = propertyList
                            .Where(p => !p.Name.Equals("cast", StringComparison.OrdinalIgnoreCase))
                            .Count(p => Convert.ToString(p.GetValue(movieData)).ToLower().Contains(searchTerm))
                            > 0;
            }
            return isSearchtermFound;
        }

        private List<MovieData> GetCachedMovieList()
        {
            /* ----------------  Get data from cache at start up -------------------- */

            var movieList = _cacheManager.Get<List<MovieData>>(Constants.Constants.CacheKeys.MovieList);
            if (movieList == null)
            {
                movieList = _dataManager.Get();
                _cacheExpiryDatetime = DateTime.UtcNow.AddHours(24);
                _cacheManager.Set(Constants.Constants.CacheKeys.MovieList, movieList, _cacheExpiryDatetime);
            }
            return movieList;
        }

    }
}
