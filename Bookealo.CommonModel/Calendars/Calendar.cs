using Bookealo.CommonModel.Assets;
using Bookealo.CommonModel.Calendars.Enum;
using Bookealo.CommonModel.Users;

namespace Bookealo.CommonModel.Calendars
{
    public class Calendar
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public CalendarType Type { get; set; }
        public List<User> Users { get; set; } = new();
        public List<Asset> Assets { get; set; } = new();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsOnSaturday { get; set; }
        public bool IsOnSunday { get; set; }
        public TimeSpan? SaturdayStartTime { get; set; }
        public TimeSpan? SaturdayEndTime { get; set; }
        public TimeSpan? SundayStartTime { get; set; }
        public TimeSpan? SundayEndTime { get; set; }
    }
}
