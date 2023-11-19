using EntityFrameworkSqlServer.DataAccessLayer;
using EntityFrameworkSqlServer.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace EntityFrameworkSqlServer.Services
{
    public class UserService
    {
        /// <summary>The users database context</summary>
        private readonly EntityFrameworkSqlServerContext _usersDbContext;

        public UserService(EntityFrameworkSqlServerContext usersDbContext)
        {
            _usersDbContext = usersDbContext;
        }

        public async Task<IEnumerable<User>> GetUser()
        {
            return await _usersDbContext.Users.ToListAsync();
        }

        public async Task<User?> GetUserById(int userId)
        {
            return await _usersDbContext.Users.FindAsync(userId);
        }

        public async Task<User> Create(User user)
        {
            await _usersDbContext.Users.AddAsync(user);
            await _usersDbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> Update(int userId, User userFromJson)
        {
            var user = await _usersDbContext.Users.FindAsync(userId);

            user.UserName = userFromJson.UserName;
            user.UserLastName = userFromJson.UserLastName;
            user.PhoneNumber = userFromJson.PhoneNumber;
            user.DateModified = DateTime.Now;
            user.Email = userFromJson.Email;

            await _usersDbContext.SaveChangesAsync();

            return user;
        }


        public async Task<int> Delete(int id)
        {
            var user = await _usersDbContext.Users.FindAsync(id);

            if (user == null)
                return 0;

            _usersDbContext.Remove(user);
            return await _usersDbContext.SaveChangesAsync();
        }

        public async Task<int> GetUsersPerGroupId(int groupId)
        {
            var userCount = await _usersDbContext.UserGroups.CountAsync(ug => ug.GroupId == groupId);
            return userCount;
        }

        public async Task<int> GetUsersCount()
        {
            var userCount = await _usersDbContext.Users.CountAsync();
            return userCount;
        }

        public async Task AddUserToGroups(int userId, List<int> groupIds)
        {
            var userGroups = new List<UserGroups>();
            foreach (var groupId in groupIds)
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
        }
    }
}
