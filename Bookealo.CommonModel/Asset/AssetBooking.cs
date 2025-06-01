using Bookealo.CommonModel.Users;

namespace Bookealo.CommonModel
{
    public class AssetBooking
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
    }
}