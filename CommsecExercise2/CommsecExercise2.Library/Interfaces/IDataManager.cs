using System.Collections.Generic;
using MoviesLibrary;

namespace CommsecExercise2.Library.Interfaces
{
    public interface IDataManager
    {
        List<MovieData> Get();
        MovieData GetById(int movieId);
        int Insert(MovieData movieData);
        void Update(MovieData movieData);
    }
}
