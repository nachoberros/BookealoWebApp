using Bookealo.CommonModel.Account;
using Bookealo.CommonModel.Assets;
using Bookealo.CommonModel.Calendars;
using Bookealo.CommonModel.TennisBooking;
using Bookealo.CommonModel.Users;

namespace Bookealo.Services.Interfaces
{
    public interface IMockingRepository
    {
        List<Court> GetCourts(int accountId, int calendarId);
        void AddBooking(BookingRequest bookingRequest);
        void CancelBooking(BookingRequest bookingRequest);
        void BlockSlot(BookingRequest bookingRequest);
        void UnblockSlot(BookingRequest bookingRequest);
        List<Calendar> GetCalendars(int accountId);
        List<User> GetUsers(int accountId);
        void AddCalendar(int accountId, Calendar calendar);
        void RemoveCalendar(int accountId, Calendar calendar);
        void UpdateCalendar(int accountId, Calendar calendar);
        void UpdateUser(int accountId, User user);
        void RemoveUser(int accountId, User user);
        void AddUser(int accountId, User user);
        List<Asset> GetAssets(int accountId);
        void UpdateAsset(int accountId, Asset asset);
        void RemoveAsset(int accountId, Asset asset);
        void AddAsset(int accountId, Asset asset);
        bool IsValidAccount(int accountId, string email);
        Account GetDefaultAccount(string email);
    }
}
