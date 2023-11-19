using EntityFrameworkSqlServer.DataAccessLayer;
using EntityFrameworkSqlServer.Entities;
using EntityFrameworkSqlServer.Services;
using Microsoft.AspNetCore.Mvc;


namespace EntityFrameworkSqlServer.Controllers
{
    /// <summary>Class UserController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.ControllerBase"/></summary>
    [ApiController, Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(EntityFrameworkSqlServerContext usersDbContext)
        {
            _userService = new UserService(usersDbContext);
        }

        /// <summary>Gets the users.</summary>
        /// <returns>Task&lt;ActionResult&lt;IEnumerable&lt;User&gt;&gt;&gt;.</returns>
        /// <remarks> GET api/values</remarks>
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return Ok(await _userService.GetUser());
        }

        /// <summary>Creates the specified user.</summary>
        /// <param name="user">The user.</param>
        /// <returns>Task&lt;ActionResult&lt;user&gt;&gt;.</returns>
        [HttpPost("create")]
        public async Task<ActionResult<User>> Create([FromBody] User user)
        {
            if (!ModelState.IsValid)
            return BadRequest(ModelState);

            await _userService.Create(user);

            return Ok(user);
        }

        /// <summary>Updates the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userFromJson">The user from json.</param>
        /// <returns>Task&lt;ActionResult&lt;user&gt;&gt;.</returns>
        [HttpPut("update/{id}")]
        public async Task<ActionResult<User>> Update(int id, [FromBody] User userFromJson)
        {
            if (!ModelState.IsValid)
            return BadRequest(ModelState);

            var user = await _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            await _userService.Update(id, userFromJson);

            return Ok(user);
        }

        /// <summary>Deletes the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;ActionResult&lt;user&gt;&gt;.</returns>
        [HttpDelete("remove/{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            var user = await _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }
            await _userService.Delete(id);           

            return Ok(user);
        }

        [HttpPut("groups/add/{userId}")]
        public async Task<ActionResult<User>> AddUserToGroups(int userId, List<int> groupIds)
        {
            var user = await _userService.GetUserById(userId);

            if (user == null)
            {
                return NotFound();
            }

            await _userService.AddUserToGroups(userId, groupIds);

            return Ok(user);
        }

        [HttpGet("getUserCount")]
        public async Task<ActionResult<int>> GetUserCount()
        {
            var userCount = await _userService.GetUsersCount();

            return Ok(userCount);
        }

        [HttpGet("getUserCountPerGroup")]
        public async Task<ActionResult<int>> GetUserCount(int groupId)
        {
            var userCount = await _userService.GetUsersPerGroupId(groupId);

            return Ok(userCount);
        }


    }
}