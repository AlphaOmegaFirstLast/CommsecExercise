using System.Collections.Generic;
using MoviesLibrary;
namespace CommsecExercise1.WebApi.Interfaces
{
    public interface IMovieManager
    {
        List<MovieData> Get();
        MovieData GetById(int id);
        List<MovieData> GetSorted(string fieldName);
        List<MovieData> Search(string searchTerm);
        int Insert(MovieData movieData);
        void Update(MovieData movieData);
    }
}
