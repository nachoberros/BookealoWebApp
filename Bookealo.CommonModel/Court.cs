namespace Bookealo.CommonModel
{
    public class Court
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CourtBooking> Bookings { get; set; }
    }
}
