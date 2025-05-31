using Bookealo.CommonModel.Users;

namespace Bookealo.CommonModel.TennisBooking
{
    public class CourtBooking
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
    }
}