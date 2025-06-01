using Bookealo.CommonModel.Assets;
using Bookealo.CommonModel.Calendars;
using Bookealo.CommonModel.Users;

namespace Bookealo.CommonModel.Account
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User Owner { get; set; }
        public List<User> Users { get; set; }
        public List<Calendar> Calendars { get; set; }
        public List<Asset> Assets { get; set; }
    }
}
