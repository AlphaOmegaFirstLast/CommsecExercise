using System.Collections.Generic;
using MoviesLibrary;

namespace CommsecExercise1.WebApi.Interfaces
{
    public interface IDataManager
    {
        List<MovieData> Get();
        MovieData GetById(int movieId);
        int Insert(MovieData movieData);
        void Update(MovieData movieData);
    }
}
