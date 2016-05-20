using System.Collections.Generic;
using MoviesLibrary;

namespace CommsecExercise2.Library.Interfaces
{
    public interface IMovieManager
    {
        List<MovieData> Get();
        MovieData GetById(int movieId);
        List<MovieData> GetSorted(string fieldName);
        List<MovieData> Search(string searchTerm);
        int Insert(MovieData movieData);
        void Update(MovieData movieData);
    }
}
