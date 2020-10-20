using MovieApp_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MovieApp_API.Controllers
{
    public class RatingController : ApiController
    {
        private film_dbEntities dm = new film_dbEntities();

        public List<Rating> Get()
        {
            return dm.Rating.ToList();
        }

        [HttpDelete]
        [Route("api/Rating/DeleteRating/{ratingID}")]
        public IHttpActionResult DeleteRating(int ratingID)
        {
            Rating rating = dm.Rating.Find(ratingID);
            dm.Rating.Remove(rating);
            dm.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult PostRating(int movieID, int userID, float ratingNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Rating newRating = new Rating()
            {
                MovieID = movieID,
                UserID =userID,
                RatingNumber=ratingNumber
            };


            dm.Rating.Add(newRating);
            dm.SaveChanges();

            return Ok();
        }


    }
}
