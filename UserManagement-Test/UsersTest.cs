using EntityFrameworkSqlServer.Controllers;
using EntityFrameworkSqlServer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using EntityFrameworkSqlServer.DataAccessLayer;
using EntityFrameworkSqlServer.Services;

namespace UserManagement_Test
{
    [TestFixture]
    public class UsersTest
    {
        [Test]
        public async Task CreateUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EntityFrameworkSqlServer.DataAccessLayer.EntityFrameworkSqlServerContext>()
             .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
             .Options;

            string username = "testuser";
            string email = "testuser@example.com";

            using (var context = new EntityFrameworkSqlServerContext(options))
            {
                // Add test data to the in-memory database
                context.Users.Add(new User { UserId = 1, UserName = "TestUser", UserLastName = "TestLastName", Email = "test@example.com" });
                context.SaveChanges();
            }

            using (var context = new EntityFrameworkSqlServerContext(options))
            {
                // Act - Your actual test logic using the EF context
                // For example, call a method from a service that uses the EF context
                var userService = new UserService(context);
                var user = await userService.GetUserById(1);

                // Assert
                Assert.NotNull(user);
                Assert.AreEqual("TestUser", user.UserName);
                Assert.AreEqual("test@example.com", user.Email);
            }
        }

        [Test]
        public async Task GetUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EntityFrameworkSqlServer.DataAccessLayer.EntityFrameworkSqlServerContext>()
             .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
             .Options;


            using (var context = new EntityFrameworkSqlServerContext(options))
            {
                // Add test data to the in-memory database
                context.Users.Add(new User { UserId = 2, UserName = "Testuser2", UserLastName = "TestLastName2", Email = "test2@example.com" });
                context.Users.Add(new User { UserId = 3, UserName = "TestUser3", UserLastName = "TestLastName3", Email = "test3@example.com" });
                context.SaveChanges();
            }

            using (var context = new EntityFrameworkSqlServerContext(options))
            {
                // Act - Your actual test logic using the EF context
                // For example, call a method from a service that uses the EF context
                var userService = new UserService(context);
                var users = await userService.GetUser();

                // Assert
                Assert.NotNull(users);
                Assert.IsTrue(users.Count() > 0);
                Assert.AreEqual("test3@example.com", users.FirstOrDefault(u => u.UserId == 3).Email);
            }
        }

        [Test]
        public async Task GetUserCountPerGroup()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EntityFrameworkSqlServer.DataAccessLayer.EntityFrameworkSqlServerContext>()
             .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
             .Options;
            
            using (var context = new EntityFrameworkSqlServerContext(options))
            {
                // Add test data to the in-memory database
                context.Groups.Add(new EntityFrameworkSqlServer.Entities.Group { GroupId = 3, GroupName = "Admin Group", IsActive = true });
                context.Groups.Add(new EntityFrameworkSqlServer.Entities.Group { GroupId = 4, GroupName = "Supervisor Group", IsActive = true });

                context.UserGroups.Add(new EntityFrameworkSqlServer.Entities.UserGroups { GroupId = 3, UserId = 1 });
                context.UserGroups.Add(new EntityFrameworkSqlServer.Entities.UserGroups { GroupId = 3, UserId = 2 });
                context.UserGroups.Add(new EntityFrameworkSqlServer.Entities.UserGroups { GroupId = 4, UserId = 3 });

                context.SaveChanges();
            }

            using (var context = new EntityFrameworkSqlServerContext(options))
            {
                var userService = new UserService(context);
                var userCount = await userService.GetUsersPerGroupId(3);

                // Assert
                Assert.NotNull(userCount);
                Assert.IsTrue(userCount == 2);
            }
        }
    }
}
