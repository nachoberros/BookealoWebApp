namespace Bookealo.CommonModel.Users
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Permission> Permissions { get; set; } = new();
    }

    public enum Permission
    {
        BookealoAdmin,
        Owner,
        Admin,
        Guest
    }
}
