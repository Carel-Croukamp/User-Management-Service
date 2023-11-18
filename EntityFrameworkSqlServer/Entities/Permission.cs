using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkSqlServer.Entities
{
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public List<Group> Groups { get; set; }

        public List<GroupPermission> GroupPermissions { get; set; }
    }
}
