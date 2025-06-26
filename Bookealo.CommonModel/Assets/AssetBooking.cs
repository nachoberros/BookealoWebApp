using Bookealo.CommonModel.Users;

namespace Bookealo.CommonModel
{
    public class AssetBooking
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
    }
}