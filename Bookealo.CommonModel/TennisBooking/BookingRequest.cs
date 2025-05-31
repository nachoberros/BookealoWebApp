namespace Bookealo.CommonModel.TennisBooking
{
    public class BookingRequest
    {
        public int CourtId { get; set; }
        public DateTime Date { get; set; }
        public int? UserId { get; set; }
    }
}
