using System.Collections.Generic;

namespace EntityFrameworkSqlServer.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
