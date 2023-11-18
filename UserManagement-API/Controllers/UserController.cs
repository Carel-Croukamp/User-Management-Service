using EntityFrameworkSqlServer.DataAccessLayer;
using EntityFrameworkSqlServer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkSqlServer.Controllers
{
    /// <summary>Class UserController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.ControllerBase"/></summary>
    [ApiController, Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        /// <summary>The users database context</summary>
        private readonly EntityFrameworkSqlServerContext _usersDbContext;

        public UserController(EntityFrameworkSqlServerContext usersDbContext)
        {
            _usersDbContext = usersDbContext;
        }

        /// <summary>Gets the users.</summary>
        /// <returns>Task&lt;ActionResult&lt;IEnumerable&lt;User&gt;&gt;&gt;.</returns>
        /// <remarks> GET api/values</remarks>
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return Ok(await _usersDbContext.Users.ToListAsync());
        }

        /// <summary>Creates the specified user.</summary>
        /// <param name="user">The user.</param>
        /// <returns>Task&lt;ActionResult&lt;user&gt;&gt;.</returns>
        [HttpPost("create")]
        public async Task<ActionResult<User>> Create([FromBody] User user)
        {
            if (!ModelState.IsValid)
            return BadRequest(ModelState);

            await _usersDbContext.Users.AddAsync(user);
            await _usersDbContext.SaveChangesAsync();

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

            var user = await _usersDbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.UserName = userFromJson.UserName;
            user.UserLastName = userFromJson.UserLastName;
            user.PhoneNumber = userFromJson.PhoneNumber;
            user.DateModified = DateTime.Now;
            user.Email = userFromJson.Email;

            await _usersDbContext.SaveChangesAsync();

            return Ok(user);
        }

        /// <summary>Deletes the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;ActionResult&lt;user&gt;&gt;.</returns>
        [HttpDelete("remove/{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            var user = await _usersDbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _usersDbContext.Remove(user);
            await _usersDbContext.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPut("groups/add/{userId}")]
        public async Task<ActionResult<User>> AddUserToGroups(int userId, List<int> groupIds)
        {
            var user = await _usersDbContext.Users.FindAsync(userId);
            
            if (user == null)
            {
                return NotFound();
            }

            var userGroups = new List<UserGroups>();
            foreach(var groupId in groupIds)
            {
                // Check if a UserGroups already exists for the given userId and groupId
                var existingUserGroup = await _usersDbContext
                    .UserGroups
                    .AnyAsync(ug => ug.UserId == userId && ug.GroupId == groupId);

                if (!existingUserGroup)
                {
                    var userGroup = new UserGroups()
                    {
                        GroupId = groupId,
                        UserId = userId
                    };
                    userGroups.Add(userGroup);
                }
            };
            await _usersDbContext.UserGroups.AddRangeAsync(userGroups);
            await _usersDbContext.SaveChangesAsync();

            return Ok(user);
        }

        [HttpGet("getUserCount")]
        public async Task<ActionResult<int>> GetUserCount()
        {
            var userCount = await _usersDbContext.Users.CountAsync();

            return Ok(userCount);
        }

        [HttpGet("getUserCountPerGroup")]
        public async Task<ActionResult<int>> GetUserCount(int groupId)
        {
            var userCount = await _usersDbContext.UserGroups.CountAsync(ug => ug.GroupId == groupId);

            return Ok(userCount);
        }


    }
}