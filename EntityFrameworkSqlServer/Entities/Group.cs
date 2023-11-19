using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EntityFrameworkSqlServer.Entities
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool IsActive { get; set; }

        [JsonIgnore]
        public List<Permission> Permissions { get; set; }
        
        [JsonIgnore]
        public List<UserGroups> UserGroups { get; set; }
        
        [JsonIgnore]
        public List<GroupPermission> GroupPermissions { get; set; }
    }
}
