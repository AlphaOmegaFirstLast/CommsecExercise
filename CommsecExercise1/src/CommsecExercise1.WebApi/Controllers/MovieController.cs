using System;
using System.Linq;
using CommsecExercise1.WebApi.Interfaces;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CommsecExercise1.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        private readonly IMovieManager _movieManager;

        public MovieController(IMovieManager movieManager)
        {
            _movieManager = movieManager;
        }

        [HttpGet]
        [Route("Get")]
        [Route("")]
        public IActionResult Get()
        {
           return Ok(_movieManager.Get());
        }

        [HttpGet]
        [Route("GetById/{id}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var movie = _movieManager.GetById(id);
            if (movie != null)
            {
                return Ok(movie);
            }
            else
            {
                return HttpNotFound("No data found with this id.");
            }
        }

        [HttpGet]
        [Route("GetSorted/{fieldName}")]
        public IActionResult GetSorted(string fieldName)
        {
            try
            {
                return Ok(_movieManager.GetSorted(fieldName));
            }
            catch(Exception e)
            {
                return HttpBadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("Search/{searchTerm}")]
        public IActionResult Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return HttpBadRequest();
            }

            var searchResults =  _movieManager.Search(searchTerm);

            if (searchResults == null || !searchResults.Any())
            {
                return HttpNotFound("No data found that contains this Search term.");
            }
            else
            {
                return Ok(searchResults);
            }
        }

        [HttpPost]
        [Route("Insert")]
        public IActionResult Insert([FromBody]MoviesLibrary.MovieData movieData)
        {
            if (movieData == null)
            {
                return HttpBadRequest("Movie Data must not be null.");
            }
            if (string.IsNullOrEmpty(movieData.Title))
            {
                return HttpBadRequest("Movie Title is mandatory");
            }
            try
            {
                movieData.MovieId = _movieManager.Insert(movieData);

                // following Restful Standards for create object, redirect to the newly created resource
                return CreatedAtRoute("GetById", new { id = movieData.MovieId }, movieData);
            }
            catch (Exception e)
            {
                return HttpBadRequest(e.Message);
            }
        }


        [HttpPut]
        [Route("Update/{id}")]
        public IActionResult Update(int id,[FromBody]MoviesLibrary.MovieData movieData)
        {
            if (movieData == null)
            {
                return HttpBadRequest("Movie Data must not be null.");
            }
            if (movieData.MovieId != id)
            {
                return HttpBadRequest("MovieId must match the id in the route");
            }
            if (string.IsNullOrEmpty(movieData.Title))
            {
                return HttpBadRequest("Movie Title is mandatory");
            }
            var movie = _movieManager.GetById(movieData.MovieId);
            if (movie==null)
            {
                return HttpNotFound("Movie does not exist");
            }
            try
            {
                movieData.MovieId = id;
                _movieManager.Update(movieData);
            }
            catch (Exception e)
            {
                    return HttpBadRequest(e.Message);
            }

            return new NoContentResult();
        }
    }
}
