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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SysUsers>>> GetUser()
    {
        return Ok(await _usersDbContext.Users.ToListAsync());
    }

    /// <summary>Creates the specified user.</summary>
    /// <param name="user">The user.</param>
    /// <returns>Task&lt;ActionResult&lt;user&gt;&gt;.</returns>
    [HttpPost]
    public async Task<ActionResult<SysUsers>> Create([FromBody] SysUsers user)
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
    [HttpPut("{id}")]
    public async Task<ActionResult<SysUsers>> Update(int id, [FromBody] SysUsers userFromJson)
    {
        if (!ModelState.IsValid)
        return BadRequest(ModelState);

        var user = await _usersDbContext.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        user.Name = userFromJson.Name;
        user.Email = userFromJson.Email;

        await _usersDbContext.SaveChangesAsync();

        return Ok(user);
    }

    /// <summary>Deletes the specified identifier.</summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task&lt;ActionResult&lt;user&gt;&gt;.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<SysUsers>> Delete(int id)
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
  }
}