using Bookealo.CommonModel.Calendars;

namespace Bookealo.Services.Interfaces
{
    public interface ICalendarRepository
    {
        List<Calendar> GetCalendars(string userEmail);
        void UpdateCalendar(Calendar calendar);
        void RemoveCalendar(Calendar calendar);
        void AddCalendar(Calendar calendar);
        Calendar? GetCalendarById(string userEmail, int calendarID);
    }
}
