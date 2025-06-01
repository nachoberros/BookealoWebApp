using Bookealo.CommonModel.Calendars;
using Bookealo.Services.Interfaces;

namespace Bookealo.Services.Implementations
{
    public class CalendarRepository : ICalendarRepository
    {
        private readonly IMockingRepository _mockingRepository;

        public CalendarRepository(IMockingRepository mockingRepository) 
        {
            _mockingRepository = mockingRepository;
        }

        public List<Calendar> GetCalendars(int accountId)
        {
            return _mockingRepository.GetCalendars(accountId);
        }

        public void UpdateCalendar(int accountId, Calendar calendar)
        {
            _mockingRepository.UpdateCalendar(accountId, calendar);
        }

        public Calendar? GetCalendarById(int accountId, int calendarID)
        {
            return _mockingRepository.GetCalendars(accountId).FirstOrDefault(c => c.Id == calendarID);
        }

        public void RemoveCalendar(int accountId, Calendar calendar)
        {
            _mockingRepository.RemoveCalendar(accountId, calendar);
        }

        public void AddCalendar(int accountId, Calendar calendar)
        {
            _mockingRepository.AddCalendar(accountId, calendar);
        }
    }
}
