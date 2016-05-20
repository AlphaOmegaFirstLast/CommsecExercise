using System.Collections.Generic;
using CommsecExercise1.WebApi.Interfaces;
using MoviesLibrary;

namespace CommsecExercise1.WebApi.Managers
{
    class DataManager: IDataManager
    {
        //if datasource is a web service or database connection that implements IDisposable then we should use Using And Async               
        //if MovieDataSource exposes async methods then we should call them asynchronously (async await )


        public List<MovieData> Get()
        {
            var ds = new MovieDataSource();
            return ds.GetAllData();
        }

        public MovieData GetById(int movieId)
        {
            var ds = new MovieDataSource();
            return ds.GetDataById(movieId);
        }

        public int Insert(MovieData movieData)
        {
            var ds = new MovieDataSource();
            return ds.Create(movieData);
        }

        public void Update(MovieData movieData)
        {
            var ds = new MovieDataSource();
            ds.Update(movieData);
        }
    }
}
