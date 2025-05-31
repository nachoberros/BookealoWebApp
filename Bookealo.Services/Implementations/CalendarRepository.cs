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

        public List<Calendar> GetCalendars(string userEmail)
        {
            return _mockingRepository.GetCalendars(userEmail);
        }

        public void UpdateCalendar(Calendar calendar)
        {
            _mockingRepository.UpdateCalendar(calendar);
        }

        public Calendar? GetCalendarById(string userEmail, int calendarID)
        {
            return _mockingRepository.GetCalendars(userEmail).FirstOrDefault(c => c.Id == calendarID);
        }

        public void RemoveCalendar(Calendar calendar)
        {
            _mockingRepository.RemoveCalendar(calendar);
        }

        public void AddCalendar(Calendar calendar)
        {
            _mockingRepository.AddCalendar(calendar);
        }
    }
}
