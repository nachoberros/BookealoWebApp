namespace Bookealo.CommonModel.Users
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Permission Permission { get; set; }
    }

    public enum Permission
    {
        BookealoAdmin,
        Owner,
        Admin,
        Guest
    }
}
