namespace EntityFrameworkSqlServer.Entities
{
    public class UserGroup
    {
        public int Id { get; set; }
        public virtual SysUsers User { get; set; }
        public virtual Group Group { get; set; }

    }
}
