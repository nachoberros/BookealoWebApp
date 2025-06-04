using Bookealo.CommonModel.TennisBooking;
using Bookealo.Services.Interfaces;

namespace Bookealo.Services.Implementations
{
    public class InMemoryCourtRepository : ICourtRepository
    {
        private readonly IMockingRepository _mockingRepository;
        public InMemoryCourtRepository(IMockingRepository mockingRepository)
        {
            _mockingRepository = mockingRepository;
        }

        #region Public Methods        

        public void Save(BookingRequest booking)
        {
            _mockingRepository.AddBooking(booking);
        }

        public void SavePublicBooking(PublicBookingRequest booking)
        {
            _mockingRepository.AddPublicBooking(booking);
        }

        public void Cancel(BookingRequest booking)
        {
            _mockingRepository.CancelBooking(booking);
        }

        public void Block(BookingRequest booking)
        {
            _mockingRepository.BlockSlot(booking);
        }

        public void Unblock(BookingRequest booking)
        {
            _mockingRepository.UnblockSlot(booking);
        }

        public List<Court> Search(int accountId, int calendarId, DateTime? date)
        {
            return _mockingRepository.GetCourts(accountId, calendarId).Select(c => new Court
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Bookings = c.Bookings
                    .Where(b => !date.HasValue || b.Date.Date == date.Value.Date)
                    .ToList(),
                Blockings = c.Blockings
                    .Where(b => !date.HasValue || b.Date.Date == date.Value.Date)
                    .ToList()
            }).ToList();
        }

        #endregion
    }
}