using Bookealo.CommonModel;

namespace Bookealo.Services.Interfaces
{
    public interface ICourtRepository
    {
        List<Court> GetAll();
        List<Court> Search(DateTime? date);
        void Save(BookingRequest booking);
    }
}
