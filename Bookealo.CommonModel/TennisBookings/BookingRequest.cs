namespace Bookealo.CommonModel.TennisBooking
{
    public class BookingRequest
    {
        public int AccountId { get; set; }
        public int AssetId { get; set; }
        public int CalendarId { get; set; }
        public DateTime Date { get; set; }
        public int? UserId { get; set; }
    }
}
