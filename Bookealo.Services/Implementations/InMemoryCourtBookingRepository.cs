using Bookealo.CommonModel;
using Bookealo.Services.Interfaces;

namespace Bookealo.Services.Implementations
{
    public class InMemoryCourtRepository : ICourtRepository
    {
        private readonly List<Court> _courts;

        public InMemoryCourtRepository()
        {
            _courts = MockedBookings;
        }

        public List<Court> GetAll() => _courts;

        public List<Court> Search(DateTime? date)
        {
            return _courts.Select(c => new Court
            {
                ID = c.ID,
                Name = c.Name,
                Description = c.Description,
                Bookings = c.Bookings
                    .Where(b => !date.HasValue || b.Date.Date == date.Value.Date)
                    .ToList()
            }).ToList();
        }

        private static List<Court> MockedBookings => [
            new Court
            {
                ID = 1,
                Name = "Court 1",
                Description = "Clay",
                Bookings =
                [
                     new() { ID = 1, Date = DateTime.Today.AddHours(18), UserName = "Alice" },
                     new() { ID = 2, Date = DateTime.Today.AddHours(19), UserName = "Bob" },
                     new() { ID = 3, Date = DateTime.Today.AddDays(1).AddHours(18), UserName = "Mike" },
                     new() { ID = 4, Date = DateTime.Today.AddDays(1).AddHours(19), UserName = "Alice" }
                ]
            },
            new Court
            {
                ID = 2,
                Name = "Court 2",
                Description = "Hard",
                Bookings =
                [
                    new() { ID = 5, Date = DateTime.Today.AddHours(18), UserName = "Charlie" },
                    new() { ID = 6, Date = DateTime.Today.AddHours(18), UserName = "Bob" }
                ]
            },
         new Court
            {
                ID = 3,
                Name = "Court 3",
                Description = "Grass",
                Bookings =
                [
                    new() { ID = 7, Date = DateTime.Today.AddHours(17), UserName = "Mike" },
                    new() { ID = 8, Date = DateTime.Today.AddHours(14), UserName = "Bob" }
                ]
            },
         new Court
            {
                ID = 4,
                Name = "Court 4",
                Description = "Clay",
                Bookings =
                [
                    new() { ID = 9, Date = DateTime.Today.AddHours(17), UserName = "Alice" },
                    new() { ID = 10, Date = DateTime.Today.AddHours(14), UserName = "Charlie" }
                ]
            }];
    }
}