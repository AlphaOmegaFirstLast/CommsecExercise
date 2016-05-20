using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using CommsecExercise2.Library.Interfaces;
using MoviesLibrary;

namespace CommsecExercise2.Library.Managers
{
    public class MovieManager : IMovieManager
    {
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

        public MovieData GetById(int movieId)
        {
            var movieList = GetCachedMovieList();   // check cache first.
            var movie = movieList.FirstOrDefault(m => m.MovieId == movieId);
            if (movie == null)
            {
                movie = _dataManager.GetById(movieId);
            }
            return movie;
        }

        public List<MovieData> GetSorted(string fieldName)
        {
            if (IsValidSortFieldName(fieldName))
            {
                var movieList = GetCachedMovieList();

                // in case this demo does not allow using system.linq.Dynamic, then use custom logic
                // system.linq.Dynamic is used in CommsecExercise2

                var movieListSorted = movieList.OrderBy(m => GetSortField(fieldName, m)).ToList();
                return movieListSorted;
            }
            else
            {
                var faultCode = new FaultCode("Invalid Sort Field Name");
                var faultReason = string.Format("A valid input is any of (movieId,title, genre, classification, releaseDate, rating,).Actual input is {0}", fieldName);

                throw new FaultException(faultReason, faultCode);
                //  throw new Exception("Invalid Sort Field Name");
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
                var faultCode = new FaultCode("Invalid Search Term");
                var faultReason = "A search term must contain at least one character.";

                throw new FaultException(faultReason, faultCode);
            }
        }

        public int Insert(MovieData movieData)
        {
            if (movieData != null) 
            {
                if (!string.IsNullOrEmpty(movieData.Title))
                {
                    try
                    {
                        movieData.MovieId = _dataManager.Insert(movieData);
                        return movieData.MovieId;
                    }
                    catch (Exception e)
                    {
                        var faultCode = new FaultCode("Insert Operation Fail.");
                        var faultReason = e.Message;

                        throw new FaultException(faultReason, faultCode);
                    }
                }
                else
                {
                    var faultCode = new FaultCode("Invalid Movie Data");
                    var faultReason = "Title is mandatory";

                    throw new FaultException(faultReason, faultCode);
                }
            }
            else
            {
                var faultCode = new FaultCode("Invalid Movie Data");
                var faultReason = "Movie data must not be null.";

                throw new FaultException(faultReason, faultCode);
            }

        }

        public void Update(MovieData movieData)
        {
            if (movieData != null)
            {
                if (!string.IsNullOrEmpty(movieData.Title))
                {
                    try
                    {
                        _dataManager.Update(movieData);
                    }
                    catch (Exception e)
                    {
                        var faultCode = new FaultCode("Update Operation Fail.");
                        var faultReason = e.Message;

                        throw new FaultException(faultReason, faultCode);
                    }
                }
                else
                {
                    var faultCode = new FaultCode("Invalid Movie Data");
                    var faultReason = "Title is mandatory";

                    throw new FaultException(faultReason, faultCode);
                }
            }
            else
            {
                var faultCode = new FaultCode("Invalid Movie Data");
                var faultReason = "Movie data must not be null.";

                throw new FaultException(faultReason, faultCode);
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

        private dynamic GetSortField(string fieldName, MovieData movieData)
        {
            dynamic fieldValue = null;
            // for better performance, the code uses "switch" instead of using reflection and looping through the object properties.
            //this Code will not be called, if using System.Linq.Dynamic library is allowed to be used 

            switch (fieldName.Trim().ToLower())
            {
                case "movieid":
                    fieldValue = movieData.MovieId;
                    break;
                case "title":
                    fieldValue = movieData.Title;
                    break;
                case "genre":
                    fieldValue = movieData.Genre;
                    break;
                case "classification":
                    fieldValue = movieData.Classification;
                    break;
                case "releasedate":
                    fieldValue = movieData.ReleaseDate;
                    break;
                case "rating":
                    fieldValue = movieData.Rating;
                    break;
                default:
                    throw new Exception("Invalid Field Name");
            }
            return fieldValue;
        }

        private bool HasSearchTerm(MovieData movieData, string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            //for better performance, the code checks the "cast" field first, before calling reflection

            var isSearchtermFound = movieData.Cast.Count(c => c.ToLower().Contains(searchTerm)) > 0;

            if (!isSearchtermFound)
            {
                var propertyList = movieData.GetType().GetProperties();  //though reflection can slow performane little bit
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
                var cacheExpiryDatetime = DateTime.UtcNow.AddHours(24);
                _cacheManager.Set(Constants.Constants.CacheKeys.MovieList, movieList, cacheExpiryDatetime);
            }
            return movieList;
        }

    }
}
