using MovieApp_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;

namespace MovieApp_API.Controllers
{
    public class UserController : ApiController
    {
        private film_dbEntities dm = new film_dbEntities();

        public List<User> Get()
        {
            return dm.User.ToList();
        }

        [HttpDelete]
        [Route("api/User/DeleteUser/{userID}")]
        public IHttpActionResult DeleteUser(int userID)
        {
            User user = dm.User.Find(userID);
            dm.User.Remove(user);
            dm.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult PostUser(string firstName, string lastName, string username, string password, string passwordConfirmation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (password != passwordConfirmation)
            {
                throw new Exception("Passwordi se ne slažu");
            }
            User newUser = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Username = username
            };

            newUser.PassSalt = GenerateSalt();
            newUser.PassHash = GenerateHash(newUser.PassSalt, password);
            dm.User.Add(newUser);
            dm.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public User Authenticiraj(string username, string pass)
        {
            var user = dm.User.FirstOrDefault(x => x.Username == username);

            if (user != null)
            {
                var newHash = GenerateHash(user.PassSalt, pass);

                if (newHash == user.PassHash)
                {
                    return user;
                }
            }
            return null;
        }
        public static string GenerateSalt()
        {
            var buf = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }

        public static string GenerateHash(string salt, string password)
        {
            byte[] src = Convert.FromBase64String(salt);
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] dst = new byte[src.Length + bytes.Length];

            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }
    }
}
