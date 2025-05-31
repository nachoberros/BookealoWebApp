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
            _mockingRepository.AddBooking(booking.UserId ?? 0, booking.CourtId, booking.Date);
        }

        public void Cancel(BookingRequest booking)
        {
            _mockingRepository.CancelBooking(booking.UserId ?? 0, booking.CourtId, booking.Date);
        }

        public void Block(BookingRequest booking)
        {
            _mockingRepository.BlockSlot(booking.UserId ?? 0, booking.CourtId, booking.Date);
        }

        public void Unblock(BookingRequest booking)
        {
            _mockingRepository.UnblockSlot(booking.UserId ?? 0, booking.CourtId, booking.Date);
        }

        public List<Court> Search(DateTime? date)
        {
            return _mockingRepository.GetBookingsByCourt().Select(c => new Court
            {
                ID = c.ID,
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