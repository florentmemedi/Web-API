using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Domain;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public static List<User> users = new List<User>()
        {
            new User(){ Id = 1, Firstname = "Florent", Lastname = "Memedi", Age = 31},
            new User(){Id = 2, Firstname = "Sihana", Lastname = "Memedi", Age = 27},
            new User(){Id = 3, Firstname = "Matin", Lastname = "Memedi", Age = 1}
        };

        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            return users;
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            try
            {
                return users[id - 1];
            }
            catch (ArgumentOutOfRangeException)
            {

                return NotFound($"User with id {id} is not found!");
            }
            catch (Exception ex)
            {
                return BadRequest($"BROKEN: {ex.Message}");
            }
        }

        [HttpGet("{id}/validate")]
        public ActionResult<string> Validate(int id)
        {
            try
            {
                if (users[id - 1].Age >= 18)
                {
                    return ($"User with id {id} is an adult");
                }
                return ($"User with id {id} is not an adult");
            }
            catch (ArgumentOutOfRangeException)
            {

                return NotFound($"User with id {id} is not found!");
            }
            catch (Exception ex)
            {
                return BadRequest($"BROKEN: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Post()
        {
            string body;
            using (StreamReader sr = new StreamReader(Request.Body))
            {
                body = sr.ReadToEnd();
            }
            User user = JsonConvert.DeserializeObject<User>(body);
            users.Add(user);
            return Ok($"User with id {users.Count} added!");
        }
    }
}