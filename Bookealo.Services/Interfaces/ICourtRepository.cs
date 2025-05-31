using Bookealo.CommonModel;

namespace Bookealo.Services.Interfaces
{
    public interface ICourtRepository
    {
        List<Court> GetAll();
        List<Court> Search(DateTime? date);
        void Save(BookingRequest booking);
        void Cancel(BookingRequest booking);
        void Block(BookingRequest booking);
        void Unblock(BookingRequest booking);
    }
}
