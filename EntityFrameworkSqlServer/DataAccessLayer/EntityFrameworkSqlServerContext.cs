// ***********************************************************************
// Assembly         : EntityFrameworkSqlServer
// Author           : Bassam Alugili
// ***********************************************************************
// <copyright file="EntityFrameworkSqlServerContext.cs" company="EntityFrameworkSqlServer">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using EntityFrameworkSqlServer.Entities;
using Microsoft.EntityFrameworkCore;


namespace EntityFrameworkSqlServer.DataAccessLayer
{
    /// <summary>
    /// Class EntityFrameworkSqlServerContext. This class cannot be inherited.
    /// Implements the <see cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public sealed class EntityFrameworkSqlServerContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkSqlServerContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public EntityFrameworkSqlServerContext(DbContextOptions<EntityFrameworkSqlServerContext> options)
          : base(options)
        {
            this.Database.EnsureCreated();
        }

        /// <summary>
        /// Gets or sets the products.
        /// </summary>
        /// <value>The products.</value>
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroups> UserGroups { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<GroupPermission> GroupPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroups>()
               .HasKey(ug => new { ug.UserId, ug.GroupId });

            modelBuilder.Entity<GroupPermission>()
                .HasKey(gp => new { gp.GroupId, gp.PermissionId });

            modelBuilder.Entity<UserGroups>()
               .HasOne(ug => ug.User)
               .WithMany(g => g.UserGroups)
               .HasForeignKey(gp => gp.UserId);

            modelBuilder.Entity<UserGroups>()
               .HasOne(ug => ug.Group)
               .WithMany(g => g.UserGroups)
               .HasForeignKey(gp => gp.GroupId);

            modelBuilder.Entity<GroupPermission>()
                .HasOne(gp => gp.Group)
                .WithMany(g => g.GroupPermissions)
                .HasForeignKey(gp => gp.GroupId);

            modelBuilder.Entity<GroupPermission>()
                .HasOne(gp => gp.Permission)
                .WithMany(p => p.GroupPermissions)
                .HasForeignKey(gp => gp.PermissionId);

            // Seed data for Group and Permission
            modelBuilder.Entity<Group>().HasData(
                new Group { GroupId = 1, GroupName = "Group1" },
                new Group { GroupId = 2, GroupName = "Group2" }
            );

            modelBuilder.Entity<Permission>().HasData(
                new Permission { PermissionId = 1, PermissionName = "Level 1" },
                new Permission { PermissionId = 2, PermissionName = "Level 2" },
                new Permission { PermissionId = 3, PermissionName = "Level 3" }
            );

            // Seed data for GroupPermissions
            modelBuilder.Entity<GroupPermission>().HasData(
                new GroupPermission { GroupId = 1, PermissionId = 1 },
                new GroupPermission { GroupId = 1, PermissionId = 2 },
                new GroupPermission { GroupId = 2, PermissionId = 1 }
            // Add more seed data as needed
            );

        }

    }
}