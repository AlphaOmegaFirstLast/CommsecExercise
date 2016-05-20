using System.Collections.Generic;
using System.ServiceModel;
using MoviesLibrary;

namespace CommsecExercise2.Library
{
    [ServiceContract]
    public interface IMovieService
    {
 
        [OperationContract]
        [ServiceKnownType(typeof(MovieData))]
        [ServiceKnownType(typeof(List<MovieData>))]
        List<MovieData> Get();


        [OperationContract]
        [ServiceKnownType(typeof(MovieData))]
        MovieData GetById(int id);

        [OperationContract]
        [ServiceKnownType(typeof(MovieData))]
        [ServiceKnownType(typeof(List<MovieData>))]
        List<MovieData> GetSorted(string fieldName);

        [OperationContract]
        [ServiceKnownType(typeof(MovieData))]
        [ServiceKnownType(typeof(List<MovieData>))]
        List<MovieData> Search(string searchTerm);

        [OperationContract]
        [ServiceKnownType(typeof(MovieData))]
        int Insert(MovieData movieData);

        [OperationContract]
        [ServiceKnownType(typeof(MovieData))]
        void Update(MovieData movieData);
    }
}