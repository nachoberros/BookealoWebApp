using Bookealo.CommonModel.Calendars;
using Bookealo.CommonModel.TennisBooking;
using Bookealo.CommonModel.Users;

namespace Bookealo.Services.Interfaces
{
    public interface IMockingRepository
    {
        List<Court> GetBookingsByCourt();
        List<User> GetUsers();

        void AddBooking(int userId, int courtId, DateTime date);
        void CancelBooking(int userId, int courtId, DateTime date);
        void BlockSlot(int userId, int courtId, DateTime date);
        void UnblockSlot(int userId, int courtId, DateTime date);
        void AddCalendar(Calendar calendar);
        void RemoveCalendar(Calendar calendar);
        void UpdateCalendar(Calendar calendar);
        List<Calendar> GetCalendars(string userEmail);
    }
}
