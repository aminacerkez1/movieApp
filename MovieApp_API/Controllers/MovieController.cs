using Microsoft.AspNetCore.Http;
using MovieApp_API.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace MovieApp_API.Controllers
{
    public class MovieController : ApiController
    {
        private film_dbEntities dm = new film_dbEntities();

        public List<Movie> Get()
        {
            return dm.Movie.ToList();
        }

        [HttpDelete]
        [Route("api/Movie/DeleteMovie/{movieID}")]
        public IHttpActionResult DeleteMovie(int movieID)
        {
            Movie movie = dm.Movie.Find(movieID);
            dm.Movie.Remove(movie);
            dm.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult PostMovie(string name, string releaseYear, string description, string director)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Movie newMovie = new Movie()
            {
                Name = name,
                ReleaseYear = releaseYear,
                Description = description,
                Director = director
            };

            dm.Movie.Add(newMovie);
            dm.SaveChanges();

            return Ok();
        }


    }
}
