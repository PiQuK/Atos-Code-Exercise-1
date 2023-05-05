using Atos_Code_Exercise_1.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Atos_Code_Exercise_1.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AtosCodeExercise1Controller : ControllerBase
    {

        private readonly ILogger<AtosCodeExercise1Controller> _logger;

        public AtosCodeExercise1Controller(ILogger<AtosCodeExercise1Controller> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Add user with given firstName, sureName, id
        /// </summary>
        /// <param name="firstName">string</param>
        /// <param name="sureName">string</param>
        /// <param name="id">Int</param>
        /// <returns>List of users</returns>
        [HttpPut(Name = "AddUserAsync")]
        [Produces("application/json")]
        public async Task<IEnumerable<AtosCodeExercise1>?> AddUserAsync(string firstName, string sureName, int id)
        {
            if (id < 1)
            {
                _logger.LogInformation(string.Concat("AddUserAsync() Given id<", id.ToString(), "> less than 1"));
                return null;
            }

            if (Users.PeopleListAll.FindIndex(x => x.Id == id) >= 0)
            {
                _logger.LogInformation(string.Concat("AddUserAsync() User with id<", id.ToString(), "> already exists"));
                return null;
            }

            var myTask = Task.Run(() => Users.PeopleListAll.Append<AtosCodeExercise1>(new AtosCodeExercise1 { FirstName = firstName, SureName = sureName, Id = id }));

            var result = await myTask;

            _logger.LogInformation(string.Concat("AddUserAsync() Added user: firstName<", firstName, ">, sureName<", sureName, ">, id<", id.ToString(), ">"));

            return result;
        }

        /// <summary>
        /// Delete user with given id
        /// </summary>
        /// <param name="id">Int</param>
        /// <returns>List of users</returns>
        [HttpPost(Name = "DeleteUserAsync")]
        [Produces("application/json")]
        public async Task<IEnumerable<AtosCodeExercise1>?> DeleteUserAsync([FromForm] int id)
        {
            int deleted = Users.PeopleListAll.RemoveAll(x => x.Id == id);

            if (deleted > 0)
            {
                _logger.LogInformation(string.Concat("DeleteUserAsync() Deleted users:", deleted.ToString(), ", with id= ", id.ToString()));
            }
            else
            {
                _logger.LogInformation(string.Concat("DeleteUserAsync() user not found to delete with id= ", id.ToString()));
                return null;
            }

            var myTask = Task.Run(() => Users.PeopleListAll.ToArray());

            var result = await myTask;

            return result;

        }

        /// <summary>
        /// Get list of users
        /// </summary>
        /// <returns>List of users</returns>
        /// 
        [HttpGet(Name = "ListAllUsersAsync")]
        [Produces("application/json")]
        public async Task<ICollection<AtosCodeExercise1>> ListAllUsersAsync()
        {
            var myTask = Task.Run(() => Users.PeopleListAll.ToArray());

            var result = await myTask;

            _logger.LogInformation(string.Concat("ListAllUsersAsync() Listed users number: ", Users.PeopleListAll.Count.ToString()));

            return result;
        }
    }
}