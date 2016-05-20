using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web.Http;
using CommsecExercise2.WebApi.Models;
using CommsecExercise2.WebApi.MovieService;


namespace CommsecExercise2.WebApi.Controllers
{
    [RoutePrefix("api/Movie")]
    public class MovieController : ApiController
    {

        [HttpGet]
        [Route("Get")]
        [Route("")]
        public ApiResponse<List<MovieData>> Get()
        {
            var response = new ApiResponse<List<MovieData>>();
            var movieServiceClient = new MovieServiceClient();
            try
            {
                var result = movieServiceClient.Get();
                response.Data = result.ToList();
            }
            catch (FaultException e)
            {
                response.Status.IsSuccess = false;
                response.Status.Code = e.Code.Name;
                response.Status.Reason = e.Reason.ToString();
            }
            finally
            {
                movieServiceClient.Close();
            }

            return response;
        }

        [HttpGet]
        [Route("GetSorted/{fieldName}")]
        public async Task<ApiResponse<List<MovieData>>> GetSorted(string fieldName)
        {
            var response = new ApiResponse<List<MovieData>>();
            var movieServiceClient = new MovieServiceClient();
            try
            {
                var result = await movieServiceClient.GetSortedAsync(fieldName);
                response.Data = result.ToList();
            }
            catch (FaultException e)
            {
                response.Status.IsSuccess = false;
                response.Status.Code = e.Code.Name;
                response.Status.Reason = e.Reason.ToString();
            }
            finally
            {
                movieServiceClient.Close();
            }

            return response;
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ApiResponse<MovieData>> GetById(int id)
        {
            var response = new ApiResponse<MovieData>();
            var movieServiceClient = new MovieServiceClient();
            try
            {
                response.Data = await movieServiceClient.GetByIdAsync(id);
            }
            catch (FaultException e)
            {
                response.Status.IsSuccess = false;
                response.Status.Code = e.Code.Name;
                response.Status.Reason = e.Reason.ToString();
            }
            finally
            {
                movieServiceClient.Close();
            }

            return response;
        }

        [HttpGet]
        [Route("Search/{searchTerm}")]
        public async Task<ApiResponse<List<MovieData>>> Search(string searchTerm)
        {
            var response = new ApiResponse<List<MovieData>>();
            var movieServiceClient = new MovieServiceClient();
            try
            {
                var result = await movieServiceClient.SearchAsync(searchTerm);
                response.Data = result.ToList();
            }
            catch (FaultException e)
            {
                response.Status.IsSuccess = false;
                response.Status.Code = e.Code.Name;
                response.Status.Reason = e.Reason.ToString();
            }
            finally
            {
                movieServiceClient.Close();
            }

            return response;
        }


        [HttpPost]
        [Route("Insert")]
        public async Task<ApiResponse<MovieData>> Insert([FromBody]MovieData movieData)
        {
            var response = new ApiResponse<MovieData>();
            var movieServiceClient = new MovieServiceClient();
            try
            {
                var movieId = await movieServiceClient.InsertAsync(movieData);
                response.Data = await movieServiceClient.GetByIdAsync(movieId);
            }
            catch (FaultException e)
            {
                response.Status.IsSuccess = false;
                response.Status.Code = e.Code.Name;
                response.Status.Reason = e.Reason.ToString();
            }
            finally
            {
                movieServiceClient.Close();
            }

            return response;
        }


        [HttpPut]
        [Route("Update/{id}")]
        public async Task<ApiResponse<string>> Update(int id, [FromBody]MovieData movieData)
        {
            var response = new ApiResponse<string>();
            var movieServiceClient = new MovieServiceClient();
            try
            {
                await movieServiceClient.UpdateAsync(movieData);
                response.Data = "Data has been updated.";
            }
            catch (FaultException e)
            {
                response.Status.IsSuccess = false;
                response.Status.Code = e.Code.Name;
                response.Status.Reason = e.Reason.ToString();
            }
            finally
            {
                movieServiceClient.Close();
            }

            return response;
        }
    }
}
