using Bookealo.CommonModel.Calendars.Enum;
using Bookealo.CommonModel.Calendars;
using Bookealo.CommonModel.TennisBooking;
using Bookealo.CommonModel.Users;
using Bookealo.Services.Interfaces;
using Calendar = Bookealo.CommonModel.Calendars.Calendar;

namespace Bookealo.Services.Implementations
{
    public class MockingRepository : IMockingRepository
    {

        public MockingRepository()
        {
            _users = GetMockedUsers();
            _courts = GetMockedTennisCourtBookings();
            _calendars = GetMockedCalendars();
        }

        private List<User> _users = [];
        private List<Court> _courts = [];
        private List<Calendar> _calendars = [];

        public List<Court> GetBookingsByCourt()
        {
            return _courts;
        }

        public void AddBooking(int userId, int courtId, DateTime date)
        {
            var court = GetBookingsByCourt().FirstOrDefault(c => c.ID == courtId);
            if (court == null)
            {
                throw new InvalidOperationException($"Court with ID {courtId} not found.");
            }

            var slot = court.Bookings.FirstOrDefault(b => b.Date == date);
            if (slot != null)
            {
                throw new InvalidOperationException("This time slot is already booked.");
            }

            var newBooking = new CourtBooking
            {
                ID = GenerateBookingId(),
                Date = date,
                User = GetUser(userId)
            };

            court.Bookings.Add(newBooking);
        }

        public void CancelBooking(int userId, int courtId, DateTime date)
        {
            var court = GetBookingsByCourt().FirstOrDefault(c => c.ID == courtId);
            if (court == null)
            {
                throw new InvalidOperationException($"Court with ID {courtId} not found.");
            }

            var slot = court.Bookings.FirstOrDefault(b => b.Date == date);
            if (slot == null)
            {
                throw new InvalidOperationException("This time slot is already empty.");
            }

            court.Bookings.Remove(slot);
        }

        public void BlockSlot(int userId, int courtId, DateTime date)
        {
            var court = GetBookingsByCourt().FirstOrDefault(c => c.ID == courtId);
            if (court == null)
            {
                throw new InvalidOperationException($"Court with ID {courtId} not found.");
            }

            bool isSlotTaken = court.Bookings.Any(b => b.Date == date);
            if (isSlotTaken)
            {
                throw new InvalidOperationException("This time slot is already booked.");
            }

            bool isAlreadyBlocked = court.Blockings.Any(b => b.Date == date);
            if (isAlreadyBlocked)
            {
                throw new InvalidOperationException("This time slot is already blocked.");
            }

            var newBlocking = new CourtBooking
            {
                ID = GenerateBlockingId(),
                Date = date,
                User = GetUser(userId)
            };

            court.Blockings.Add(newBlocking);
        }

        public void UnblockSlot(int userId, int courtId, DateTime date)
        {
            var court = GetBookingsByCourt().FirstOrDefault(c => c.ID == courtId);
            if (court == null)
            {
                throw new InvalidOperationException($"Court with ID {courtId} not found.");
            }

            bool isSlotTaken = court.Bookings.Any(b => b.Date == date);
            if (isSlotTaken)
            {
                throw new InvalidOperationException("This time slot is already booked.");
            }

            var newBooking = court.Blockings.FirstOrDefault(b => b.Date == date) ?? throw new InvalidOperationException("This time slot is already unblocked.");

            court.Blockings.Remove(newBooking);
        }

        public List<Calendar> GetCalendars(string userEmail)
        {
            return _calendars.Where(c => c.Users.Any(u => u.Email.ToLowerInvariant().Equals(userEmail.ToLowerInvariant()))).ToList();
        }

        public List<User> GetUsers()
        {
            return GetMockedUsers();
        }

        public void AddCalendar(Calendar calendar)
        {
            var mockedCalendar = GetMockedCalendars().FirstOrDefault(c => c.Name.Equals(calendar.Name));
            if (mockedCalendar != null)
            {
                throw new InvalidOperationException($"Calendar with name {calendar.Name} is already in the list.");
            }

            _calendars.Add(calendar);
        }

        public void RemoveCalendar(Calendar calendar)
        {
            var mockedCalendar = GetMockedCalendars().FirstOrDefault(c => c.Name.Equals(calendar.Name));
            if (mockedCalendar == null)
            {
                throw new InvalidOperationException($"Calendar with ID {calendar.Id} was not found.");
            }

            _calendars.Remove(calendar);
        }

        public void UpdateCalendar(Calendar calendar)
        {
            var mockedCalendar = GetMockedCalendars().FirstOrDefault(c => c.Name.Equals(calendar.Name)) ?? throw new InvalidOperationException($"Calendar with ID {calendar.Id} was not found.");

            _calendars.Remove(calendar);
            _calendars.Add(calendar);
        }

        private List<User> GetMockedUsers()
        {
            return [new()
            {
                ID = 1,
                Name = "Alice",
                Email = "alice@gmail.com"
            },
            new()
            {
                ID = 2,
                Name = "Bob",
                Email = "bob@gmail.com"
            },
            new()
            {
                ID = 3,
                Name = "Mike",
                Email = "mike@gmail.com"
            },
            new()
            {
                ID = 4,
                Name = "Charlie",
                Email = "andresberros@gmail.com"
            }];
        }

        private User GetUser(int? userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null.");
            }

            var user = GetUsers().FirstOrDefault(u => u.ID == userId.Value);
            if (user == null)
            {
                throw new InvalidOperationException($"User with ID {userId} not found.");
            }

            return user;
        }

        private List<Court> GetMockedTennisCourtBookings()
        {
            var users = GetMockedUsers();
            var alice = users.First(u => u.Name == "Alice");
            var bob = users.First(u => u.Name == "Bob");
            var mike = users.First(u => u.Name == "Mike");
            var charlie = users.First(u => u.Name == "Charlie");

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

        private List<Calendar> GetMockedCalendars()
        {
            var user = GetMockedUsers().FirstOrDefault(u => u.Name == "Charlie");
            if (user == null) return new List<Calendar>();

            List<Asset> assets = GetCourts();

            return
            [
                new Calendar
                {
                    Id = 101,
                    Name = "Tennis Courts Calendar",
                    Type = CalendarType.Tennis,
                    Users = new List<User> { user },
                    Assets = assets,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddMonths(1),
                    StartTime = TimeSpan.FromHours(8),
                    EndTime = TimeSpan.FromHours(20),
                    IsOnSaturday = true,
                    IsOnSunday = false,
                    SaturdayStartTime = TimeSpan.FromHours(9),
                    SaturdayEndTime = TimeSpan.FromHours(18),
                    SundayStartTime = null,
                    SundayEndTime = null
                }
            ];
        }

        private List<Asset> GetCourts()
        {
            var courts = GetMockedUsers();
            var assets = courts.Select(c => new Asset
            {
                Id = c.ID,
                Name = c.Name,
                Type = AssetType.TennisCourt
            }).Cast<Asset>().ToList();
            return assets;
        }

        private int GenerateBookingId()
        {
            return GetBookingsByCourt().SelectMany(c => c.Bookings).Max(b => b.ID) + 1;
        }

        private int GenerateBlockingId()
        {
            return GetBookingsByCourt().SelectMany(c => c.Blockings).Max(b => b.ID) + 1;
        }
    }
}
