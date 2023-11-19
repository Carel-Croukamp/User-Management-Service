using EntityFrameworkSqlServer.DataAccessLayer;
using EntityFrameworkSqlServer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkSqlServer.Controllers
{
    /// <summary>Class GroupController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.ControllerBase"/></summary>
    [ApiController, Route("api/[controller]")]
    public class PermissionController : Controller
    {
        /// <summary>The products database context</summary>
        private readonly EntityFrameworkSqlServerContext _permissionsDbContext;

        public PermissionController(EntityFrameworkSqlServerContext permissionDbContext)
        {
            _permissionsDbContext = permissionDbContext;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Permission>>> GetPermissions()
        {
            return Ok(await _permissionsDbContext.Permissions.ToListAsync());
        }

        /// <summary>Creates the specified permission.</summary>
        /// <param name="permission">The user.</param>
        /// <returns>Task&lt;ActionResult&lt;permission&gt;&gt;.</returns>
        [HttpPost]
        public async Task<ActionResult<Group>> Create([FromBody] Permission permission)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _permissionsDbContext.Permissions.AddAsync(permission);
            await _permissionsDbContext.SaveChangesAsync();

            return Ok(permission);
        }

        /// <summary>Updates the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userFromJson">The user from json.</param>
        /// <returns>Task&lt;ActionResult&lt;user&gt;&gt;.</returns>
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Permission>> Update(int id, [FromBody] Permission permissionFromJson)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var permission = await _permissionsDbContext.Permissions.FindAsync(id);

            if (permission == null)
            {
                return NotFound();
            }

            permission.PermissionName = permissionFromJson.PermissionName;

            await _permissionsDbContext.SaveChangesAsync();

            return Ok(permission);
        }

        /// <summary>Deletes the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;ActionResult&lt;group&gt;&gt;.</returns>
        [HttpDelete("remove/{id}")]
        public async Task<ActionResult<Permission>> Delete(int id)
        {
            var permission = await _permissionsDbContext.Permissions.FindAsync(id);

            if (permission == null)
            {
                return NotFound();
            }

            _permissionsDbContext.Remove(permission);
            await _permissionsDbContext.SaveChangesAsync();

            return Ok(permission);
        }
    }
}
