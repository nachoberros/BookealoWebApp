using Bookealo.CommonModel;
using Bookealo.Services.Interfaces;

namespace Bookealo.Services.Implementations
{
    public class InMemoryCourtRepository : ICourtRepository
    {
        private readonly List<Court> _courts;

        private static readonly List<User> Users = new()
        {
            new() { ID = 1, Name = "Alice" },
            new() { ID = 2, Name = "Bob" },
            new() { ID = 3, Name = "Mike" },
            new() { ID = 4, Name = "Charlie" }
        };

        public InMemoryCourtRepository()
        {
            _courts = MockBookings();
        }

        #region Public Methods

        public List<Court> GetAll() => _courts;

        public void Save(BookingRequest booking)
        {
            var court = _courts.FirstOrDefault(c => c.ID == booking.CourtId);
            if (court == null)
            {
                throw new InvalidOperationException($"Court with ID {booking.CourtId} not found.");
            }

            var slot = court.Bookings.FirstOrDefault(b => b.Date == booking.Date);
            if (slot != null)
            {
                throw new InvalidOperationException("This time slot is already booked.");
            }

            var newBooking = new CourtBooking
            {
                ID = GenerateBookingId(),
                Date = booking.Date,
                User = GetUser(booking.UserId)
            };

            court.Bookings.Add(newBooking);
        }

        public void Cancel(BookingRequest booking)
        {
            var court = _courts.FirstOrDefault(c => c.ID == booking.CourtId);
            if (court == null)
            {
                throw new InvalidOperationException($"Court with ID {booking.CourtId} not found.");
            }

            var slot = court.Bookings.FirstOrDefault(b => b.Date == booking.Date);
            if (slot == null)
            {
                throw new InvalidOperationException("This time slot is already empty.");
            }

            court.Bookings.Remove(slot);
        }

        public void Block(BookingRequest booking)
        {
            var court = _courts.FirstOrDefault(c => c.ID == booking.CourtId);
            if (court == null)
            {
                throw new InvalidOperationException($"Court with ID {booking.CourtId} not found.");
            }

            bool isSlotTaken = court.Bookings.Any(b => b.Date == booking.Date);
            if (isSlotTaken)
            {
                throw new InvalidOperationException("This time slot is already booked.");
            }

            bool isAlreadyBlocked = court.Blockings.Any(b => b.Date == booking.Date);
            if (isAlreadyBlocked)
            {
                throw new InvalidOperationException("This time slot is already blocked.");
            }

            var newBlocking = new CourtBooking
            {
                ID = GenerateBlockingId(),
                Date = booking.Date,
                User = GetUser(booking.UserId)
            };

            court.Blockings.Add(newBlocking);
        }

        public void Unblock(BookingRequest booking)
        {
            var court = _courts.FirstOrDefault(c => c.ID == booking.CourtId);
            if (court == null)
            {
                throw new InvalidOperationException($"Court with ID {booking.CourtId} not found.");
            }

            bool isSlotTaken = court.Bookings.Any(b => b.Date == booking.Date);
            if (isSlotTaken)
            {
                throw new InvalidOperationException("This time slot is already booked.");
            }

            var newBooking = court.Blockings.FirstOrDefault(b => b.Date == booking.Date) ?? throw new InvalidOperationException("This time slot is already unblocked.");

            court.Blockings.Remove(newBooking);
        }

        public List<Court> Search(DateTime? date)
        {
            return _courts.Select(c => new Court
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

        #region Private Methods

        private User GetUser(int? userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null.");
            }

            var user = Users.FirstOrDefault(u => u.ID == userId.Value);
            if (user == null)
            {
                throw new InvalidOperationException($"User with ID {userId} not found.");
            }

            return user;
        }

        private int GenerateBookingId()
        {
            return _courts.SelectMany(c => c.Bookings).Max(b => b.ID) + 1;
        }

        private int GenerateBlockingId()
        {
            return _courts.SelectMany(c => c.Blockings).Max(b => b.ID) + 1;
        }

        private static List<Court> MockBookings()
        {
            var alice = Users.First(u => u.Name == "Alice");
            var bob = Users.First(u => u.Name == "Bob");
            var mike = Users.First(u => u.Name == "Mike");
            var charlie = Users.First(u => u.Name == "Charlie");

            List<Court> result = new()
            {
                new() {
                    ID = 1,
                    Name = "Court 1",
                    Description = "Clay",
                    Bookings =
                    [
                        new CourtBooking { ID = 1, Date = DateTime.Today.AddHours(18), User = alice },
                        new CourtBooking { ID = 2, Date = DateTime.Today.AddHours(19), User = bob },
                        new CourtBooking { ID = 3, Date = DateTime.Today.AddDays(1).AddHours(18), User = mike },
                        new CourtBooking { ID = 4, Date = DateTime.Today.AddDays(1).AddHours(19),  User = alice}
                    ],
                    Blockings =
                    [
                        new CourtBooking { ID = 1, Date = DateTime.Today.AddHours(14), User = alice },
                        new CourtBooking { ID = 2, Date = DateTime.Today.AddHours(15), User = bob },
                        new CourtBooking { ID = 3, Date = DateTime.Today.AddDays(1).AddHours(14), User = mike },
                        new CourtBooking { ID = 4, Date = DateTime.Today.AddDays(1).AddHours(15),  User = alice}
                    ]
                },
                new() {
                    ID = 2,
                    Name = "Court 2",
                    Description = "Hard",
                    Bookings =
                    [
                        new CourtBooking { ID = 5, Date = DateTime.Today.AddHours(18), User = charlie },
                        new CourtBooking { ID = 6, Date = DateTime.Today.AddHours(19), User = bob }
                    ],
                    Blockings =
                    [
                        new CourtBooking { ID = 5, Date = DateTime.Today.AddHours(11), User = charlie },
                        new CourtBooking { ID = 6, Date = DateTime.Today.AddHours(12), User = bob }
                    ]
                },
                new() {
                    ID = 3,
                    Name = "Court 3",
                    Description = "Grass",
                    Bookings =
                    [
                        new CourtBooking { ID = 7, Date = DateTime.Today.AddHours(17), User = mike },
                        new CourtBooking { ID = 8, Date = DateTime.Today.AddHours(16), User = bob }
                    ],
                    Blockings =
                    [
                        new CourtBooking { ID = 7, Date = DateTime.Today.AddHours(14), User = mike },
                        new CourtBooking { ID = 8, Date = DateTime.Today.AddHours(15), User = bob }
                    ]
                },
                new() {
                    ID = 4,
                    Name = "Court 4",
                    Description = "Clay",
                    Bookings =
                    [
                        new CourtBooking { ID = 9, Date = DateTime.Today.AddHours(17), User = alice },
                        new CourtBooking { ID = 10, Date = DateTime.Today.AddHours(14), User = charlie }
                    ],
                    Blockings =
                    [
                        new CourtBooking { ID = 9, Date = DateTime.Today.AddHours(11), User = alice },
                        new CourtBooking { ID = 10, Date = DateTime.Today.AddHours(12), User = charlie }
                    ]
                }
            };

            return result;
        }

        #endregion
    }
}