using Bookealo.CommonModel.Calendars;

namespace Bookealo.Services.Interfaces
{
    public interface ICalendarRepository
    {
        List<Calendar> GetCalendars(int accountId);
        void UpdateCalendar(int accountId, Calendar calendar);
        void RemoveCalendar(int accountId, Calendar calendar);
        void AddCalendar(int accountId, Calendar calendar);
        Calendar? GetCalendarById(int accountId, int calendarID);
    }
}
