using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkSqlServer.Entities
{
    public class GroupPermission
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
