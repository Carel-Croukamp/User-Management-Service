using EntityFrameworkSqlServer.DataAccessLayer;
using EntityFrameworkSqlServer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace EntityFrameworkSqlServer.Controllers
{
    /// <summary>Class GroupController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.ControllerBase"/></summary>
    [ApiController, Route("api/[controller]")]
    public class GroupController : Controller
    {
        /// <summary>The products database context</summary>
        private readonly EntityFrameworkSqlServerContext _groupsDbContext;
        
        public GroupController(EntityFrameworkSqlServerContext groupDbContext)
        {
            _groupsDbContext = groupDbContext;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
        {
            return Ok(await _groupsDbContext.Groups.ToListAsync());
        }

        /// <summary>Creates the specified group.</summary>
        /// <param name="user">The user.</param>
        /// <returns>Task&lt;ActionResult&lt;user&gt;&gt;.</returns>
        [HttpPost(Name = "create")]
        public async Task<ActionResult<Group>> Create([FromBody] Group group)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _groupsDbContext.Groups.AddAsync(group);
            await _groupsDbContext.SaveChangesAsync();

            return Ok(group);
        }

        /// <summary>Updates the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userFromJson">The user from json.</param>
        /// <returns>Task&lt;ActionResult&lt;user&gt;&gt;.</returns>
        [HttpPut("update{id}")]
        public async Task<ActionResult<Group>> Update(int id, [FromBody] Group groupFromJson)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var group = await _groupsDbContext.Groups.FindAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            group.GroupName = groupFromJson.GroupName;
            group.IsActive = groupFromJson.IsActive;

            await _groupsDbContext.SaveChangesAsync();

            return Ok(group);
        }

        /// <summary>Deletes the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;ActionResult&lt;group&gt;&gt;.</returns>
        [HttpDelete("remove/{id}")]
        public async Task<ActionResult<Group>> Delete(int id)
        {
            var group = await _groupsDbContext.Groups.FindAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            _groupsDbContext.Remove(group);
            await _groupsDbContext.SaveChangesAsync();

            return Ok(group);
        }

        [HttpPut("permission/add/{groupId}")]
        public async Task<ActionResult<Permission>> AddGroupToPermission(int groupId, List<int> permissionIds)
        {
            var group = await _groupsDbContext.Groups.FindAsync(groupId);

            if (group == null)
            {
                return NotFound();
            }

            var permissionGroups = new List<GroupPermission>();
            foreach (var permissionId in permissionIds)
            {
                // Check if a UserGroups already exists for the given userId and groupId
                var existingUserGroup = await _groupsDbContext
                    .GroupPermissions
                    .AnyAsync(gp => gp.GroupId == groupId && gp.PermissionId == permissionId);

                if (!existingUserGroup)
                {
                    var permissionGroup = new GroupPermission()
                    {
                        GroupId = groupId,
                        PermissionId = permissionId
                    };
                    permissionGroups.Add(permissionGroup);
                }
            };
            await _groupsDbContext.GroupPermissions.AddRangeAsync(permissionGroups);
            await _groupsDbContext.SaveChangesAsync();

            return Ok(group);
        }
    }
}
