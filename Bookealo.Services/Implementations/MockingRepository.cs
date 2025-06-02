using Bookealo.CommonModel.TennisBooking;
using Bookealo.CommonModel.Users;
using Bookealo.Services.Interfaces;
using Calendar = Bookealo.CommonModel.Calendars.Calendar;
using Bookealo.CommonModel.Account;
using Bookealo.CommonModel.Calendars.Enum;
using Bookealo.CommonModel.Assets;
using Bookealo.CommonModel;
using Bookealo.CommonModel.Users.Enum;

namespace Bookealo.Services.Implementations
{
    public class MockingRepository : IMockingRepository
    {
        private List<Account> _accounts = [];

        public MockingRepository()
        {
            _accounts = [InitializeMockedAccount()];
        }

        public List<Court> GetCourts(int accountId, int calendarId)
        {
            var courts = _accounts.FirstOrDefault(a => a.Id == accountId)?.Calendars.FirstOrDefault(c => c.Id == calendarId)?.Assets.Cast<Court>().ToList();
            if (courts == null || courts.Count < 1)
            {
                return new List<Court>() { };
            }

            return courts;
        }

        public void AddBooking(BookingRequest bookingRequest)
        {
            var court = GetCourts(bookingRequest.AccountId, bookingRequest.CalendarId).FirstOrDefault(c => c.Id == bookingRequest.AssetId);
            if (court == null)
            {
                throw new InvalidOperationException($"Court with ID {bookingRequest.AssetId} not found.");
            }

            var slot = court.Bookings.FirstOrDefault(b => b.Date == bookingRequest.Date);
            if (slot != null)
            {
                throw new InvalidOperationException("This time slot is already booked.");
            }

            var newBooking = new AssetBooking
            {
                Id = GenerateBookingId(),
                Date = bookingRequest.Date,
                User = GetUser(bookingRequest.UserId)
            };

            court.Bookings.Add(newBooking);
        }

        public void CancelBooking(BookingRequest bookingRequest)
        {
            var court = GetCourts(bookingRequest.AccountId, bookingRequest.CalendarId).FirstOrDefault(c => c.Id == bookingRequest.AssetId);
            if (court == null)
            {
                throw new InvalidOperationException($"Court with ID {bookingRequest.AssetId} not found.");
            }

            var slot = court.Bookings.FirstOrDefault(b => b.Date == bookingRequest.Date);
            if (slot == null)
            {
                throw new InvalidOperationException("This time slot is already empty.");
            }

            court.Bookings.Remove(slot);
        }

        public void BlockSlot(BookingRequest bookingRequest)
        {
            var court = GetCourts(bookingRequest.AccountId, bookingRequest.CalendarId).FirstOrDefault(c => c.Id == bookingRequest.AssetId);
            if (court == null)
            {
                throw new InvalidOperationException($"Court with ID {bookingRequest.AssetId} not found.");
            }

            bool isSlotTaken = court.Bookings.Any(b => b.Date == bookingRequest.Date);
            if (isSlotTaken)
            {
                throw new InvalidOperationException("This time slot is already booked.");
            }

            bool isAlreadyBlocked = court.Blockings.Any(b => b.Date == bookingRequest.Date);
            if (isAlreadyBlocked)
            {
                throw new InvalidOperationException("This time slot is already blocked.");
            }

            var newBlocking = new AssetBooking
            {
                Id = GenerateBlockingId(),
                Date = bookingRequest.Date,
                User = GetUser(bookingRequest.UserId)
            };

            court.Blockings.Add(newBlocking);
        }

        public void UnblockSlot(BookingRequest bookingRequest)
        {
            var court = GetCourts(bookingRequest.AccountId, bookingRequest.CalendarId).FirstOrDefault(c => c.Id == bookingRequest.AssetId);
            if (court == null)
            {
                throw new InvalidOperationException($"Court with ID {bookingRequest.AssetId} not found.");
            }

            bool isSlotTaken = court.Bookings.Any(b => b.Date == bookingRequest.Date);
            if (isSlotTaken)
            {
                throw new InvalidOperationException("This time slot is already booked.");
            }

            var newBooking = court.Blockings.FirstOrDefault(b => b.Date == bookingRequest.Date) ?? throw new InvalidOperationException("This time slot is already unblocked.");

            court.Blockings.Remove(newBooking);
        }

        public List<Calendar> GetCalendars(int accountId)
        {
            var calendars = _accounts.FirstOrDefault(a => a.Id == accountId)?.Calendars;
            if (calendars == null)
            {
                return [];
            }

            return calendars;
        }

        public List<User> GetUsers(int accountId)
        {
            var users = _accounts.FirstOrDefault(a => a.Id.Equals(accountId))?.Users;
            if (users == null)
            {
                return [];
            }

            return users;
        }

        public void AddCalendar(int accountId, Calendar calendar)
        {
            var account = _accounts.FirstOrDefault(a => a.Id.Equals(accountId));
            if (account == null)
            {
                throw new InvalidOperationException("Account was not found");
            }

            var mockedCalendar = account.Calendars.FirstOrDefault(c => c.Name.ToLowerInvariant().Equals(calendar.Name.ToLowerInvariant()));
            if (mockedCalendar != null)
            {
                throw new InvalidOperationException($"Calendar with name {calendar.Name} is already in the list.");
            }

            account.Calendars.Add(calendar);
        }

        public void RemoveCalendar(int accountId, Calendar calendar)
        {
            var account = _accounts.FirstOrDefault(a => a.Id.Equals(accountId));
            if (account == null)
            {
                throw new InvalidOperationException("Account was not found");
            }

            var mockedCalendar = account.Calendars.FirstOrDefault(c => c.Name.ToLowerInvariant().Equals(calendar.Name.ToLowerInvariant()));
            if (mockedCalendar == null)
            {
                throw new InvalidOperationException($"Calendar with ID {calendar.Id} was not found.");
            }

            account.Calendars.Remove(calendar);
        }

        public void UpdateCalendar(int accountId, Calendar calendar)
        {
            var account = _accounts.FirstOrDefault(a => a.Id.Equals(accountId));
            if (account == null)
            {
                throw new InvalidOperationException("Account was not found");
            }

            var mockedCalendar = account.Calendars.FirstOrDefault(c => c.Name.ToLowerInvariant().Equals(calendar.Name.ToLowerInvariant()));
            if (mockedCalendar == null)
            {
                throw new InvalidOperationException($"Calendar with ID {calendar.Id} was not found.");
            }

            account.Calendars.Remove(mockedCalendar);
            account.Calendars.Add(calendar);
        }

        public void UpdateUser(int accountId, User user)
        {
            var account = _accounts.FirstOrDefault(a => a.Id.Equals(accountId));
            if (account == null)
            {
                throw new InvalidOperationException("Account was not found");
            }

            var mockedUser = account.Users.FirstOrDefault(c => c.Email.ToLowerInvariant().Equals(user.Email.ToLowerInvariant()));
            if (mockedUser == null)
            {
                throw new InvalidOperationException($"User with email {user.Email} was not found.");
            }

            account.Users.Remove(mockedUser);
            account.Users.Add(user);
        }

        public void RemoveUser(int accountId, User user)
        {
            var account = _accounts.FirstOrDefault(a => a.Id.Equals(accountId));
            if (account == null)
            {
                throw new InvalidOperationException("Account was not found");
            }

            var mockedUser = account.Users.FirstOrDefault(c => c.Email.ToLowerInvariant().Equals(user.Email.ToLowerInvariant()));
            if (mockedUser == null)
            {
                throw new InvalidOperationException($"User with name {user.Name} was not found.");
            }

            account.Users.Remove(user);
        }

        public void AddUser(int accountId, User user)
        {
            if (user == null)
            {
                throw new InvalidOperationException("User cannot be null");
            }

            var account = _accounts.FirstOrDefault(a => a.Id.Equals(accountId));
            if (account == null)
            {
                throw new InvalidOperationException("Account was not found");
            }

            var mockedUser = account.Users.FirstOrDefault(c => c.Email.ToLowerInvariant().Equals(user.Email.ToLowerInvariant()));
            if (mockedUser != null)
            {
                throw new InvalidOperationException($"User with email {user.Email} is already in the list.");
            }

            account.Users.Add(user);
        }

        public List<Asset> GetAssets(int accountId)
        {
            var assets = _accounts.FirstOrDefault(a => a.Id == accountId)?.Assets;
            if (assets == null)
            {
                return [];
            }

            return assets;
        }

        public void UpdateAsset(int accountId, Asset asset)
        {
            var account = _accounts.FirstOrDefault(a => a.Id.Equals(accountId));
            if (account == null)
            {
                throw new InvalidOperationException("Account was not found");
            }

            var mockedAsset = account.Assets.FirstOrDefault(c => c.Id == asset.Id);
            if (mockedAsset == null)
            {
                throw new InvalidOperationException($"Asset with name {asset.Name} was not found.");
            }

            asset.Blockings = mockedAsset.Blockings;
            asset.Bookings = mockedAsset.Bookings;

            account.Assets.Remove(mockedAsset);
            account.Assets.Add(asset);
        }

        public void RemoveAsset(int accountId, Asset asset)
        {
            var account = _accounts.FirstOrDefault(a => a.Id.Equals(accountId));
            if (account == null)
            {
                throw new InvalidOperationException("Account was not found");
            }

            var mockedAsset = account.Assets.FirstOrDefault(c => c.Id == asset.Id);
            if (mockedAsset == null)
            {
                throw new InvalidOperationException($"Asset with name {asset.Name} was not found.");
            }

            account.Assets.Remove(asset);
        }

        public void AddAsset(int accountId, Asset asset)
        {
            var account = _accounts.FirstOrDefault(a => a.Id.Equals(accountId));
            if (account == null)
            {
                throw new InvalidOperationException("Account was not found");
            }

            var mockedAsset = account.Assets.FirstOrDefault(c => c.Id == asset.Id);
            if (mockedAsset != null)
            {
                throw new InvalidOperationException($"Asset with name {asset.Name} is already found.");
            }

            account.Assets.Add(asset);
        }

        public bool IsValidAccount(int accountId, string email)
        {
            var account = _accounts.FirstOrDefault(c => c.Id.Equals(accountId));
            if (account == null)
            {
                throw new Exception("Account does not exist");
            }

            return account.Users.Any(u => u.Email == email);
        }

        public Account GetDefaultAccount(string email)
        {
            var account = _accounts.FirstOrDefault(a => a.Users.Any(u => u.Email.ToLowerInvariant().Equals(email.ToLowerInvariant())));
            if (account == null)
            {
                return new Account();
            }

            return account;
        }

        private Account InitializeMockedAccount()
        {
            var users = GetMockedUsers();
            var courts = GetMockedTennisCourts(users);
            var calendars = GetMockedCalendars(users, courts.Cast<Asset>().ToList());

            var charlie = users.FirstOrDefault(u => u.Name.ToLowerInvariant().Equals("charlie"));
            if (charlie == null)
            {
                throw new InvalidOperationException("charlie needs to exist");
            }

            return new Account() { Id = 1, Name = "Charlies Tennis Club", Owner = charlie, Assets = courts.Cast<Asset>().ToList(), Calendars = calendars, Users = users };
        }

        private User GetUser(int? userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null.");
            }

            var user = _accounts.SelectMany(a => a.Users).FirstOrDefault(u => u.Id == userId.Value);
            if (user == null)
            {
                throw new InvalidOperationException($"User with ID {userId} not found.");
            }

            return user;
        }

        private static List<Court> GetMockedTennisCourts(List<User> users)
        {
            var alice = users.First(u => u.Name == "Alice");
            var bob = users.First(u => u.Name == "Bob");
            var mike = users.First(u => u.Name == "Mike");
            var charlie = users.First(u => u.Name == "Charlie");

            List<Court> tennisCourts = new()
            {
                new() {
                    Id = 1,
                    Name = "Court 1",
                    Description = "Clay",
                    Bookings =
                    [
                        new AssetBooking { Id = 1, Date = DateTime.Today.AddHours(18), User = alice },
                        new AssetBooking { Id = 2, Date = DateTime.Today.AddHours(19), User = bob },
                        new AssetBooking { Id = 3, Date = DateTime.Today.AddDays(1).AddHours(18), User = mike },
                        new AssetBooking { Id = 4, Date = DateTime.Today.AddDays(1).AddHours(19),  User = alice}
                    ],
                    Blockings =
                    [
                        new AssetBooking { Id = 1, Date = DateTime.Today.AddHours(14), User = alice },
                        new AssetBooking { Id = 2, Date = DateTime.Today.AddHours(15), User = bob },
                        new AssetBooking { Id = 3, Date = DateTime.Today.AddDays(1).AddHours(14), User = mike },
                        new AssetBooking { Id = 4, Date = DateTime.Today.AddDays(1).AddHours(15),  User = alice}
                    ]
                },
                new() {
                    Id = 2,
                    Name = "Court 2",
                    Description = "Hard",
                    Bookings =
                    [
                        new AssetBooking { Id = 5, Date = DateTime.Today.AddHours(18), User = charlie },
                        new AssetBooking { Id = 6, Date = DateTime.Today.AddHours(19), User = bob }
                    ],
                    Blockings =
                    [
                        new AssetBooking { Id = 5, Date = DateTime.Today.AddHours(11), User = charlie },
                        new AssetBooking { Id = 6, Date = DateTime.Today.AddHours(12), User = bob }
                    ]
                },
                new() {
                    Id = 3,
                    Name = "Court 3",
                    Description = "Grass",
                    Bookings =
                    [
                        new AssetBooking { Id = 7, Date = DateTime.Today.AddHours(17), User = mike },
                        new AssetBooking { Id = 8, Date = DateTime.Today.AddHours(16), User = bob }
                    ],
                    Blockings =
                    [
                        new AssetBooking { Id = 7, Date = DateTime.Today.AddHours(14), User = mike },
                        new AssetBooking { Id = 8, Date = DateTime.Today.AddHours(15), User = bob }
                    ]
                },
                new() {
                    Id = 4,
                    Name = "Court 4",
                    Description = "Clay",
                    Bookings =
                    [
                        new AssetBooking { Id = 9, Date = DateTime.Today.AddHours(17), User = alice },
                        new AssetBooking { Id = 10, Date = DateTime.Today.AddHours(14), User = charlie }
                    ],
                    Blockings =
                    [
                        new AssetBooking { Id = 9, Date = DateTime.Today.AddHours(11), User = alice },
                        new AssetBooking { Id = 10, Date = DateTime.Today.AddHours(12), User = charlie }
                    ]
                }
            };

            return tennisCourts;
        }

        private List<User> GetMockedUsers()
        {
            return [new()
            {
                Id = 1,
                Name = "Alice",
                Email = "alice@gmail.com",
                Role = Role.Admin
            },
            new()
            {
                Id = 2,
                Name = "Bob",
                Email = "bob@gmail.com",
                Role = Role.Admin
            },
            new()
            {
                Id = 3,
                Name = "Mike",
                Email = "mike@gmail.com",
                Role = Role.Owner
            },
            new()
            {
                Id = 4,
                Name = "Charlie",
                Email = "andresberros@gmail.com",
                Role = Role.BookealoAdmin
            }];
        }

        private List<Calendar> GetMockedCalendars(List<User> users, List<Asset> assets)
        {
            return
            [
                new Calendar
                {
                    Id = 101,
                    Name = "Tennis Courts Calendar",
                    Type = CalendarType.Tennis,
                    Users = users,
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

        private int GenerateBookingId()
        {
            return _accounts.SelectMany(a => a.Assets).SelectMany(c => c.Bookings).Max(b => b.Id) + 1;
        }

        private int GenerateBlockingId()
        {
            return _accounts.SelectMany(a => a.Assets).SelectMany(c => c.Blockings).Max(b => b.Id) + 1;
        }

        private int GenerateUserId()
        {
            return _accounts.SelectMany(a => a.Users).Max(b => b.Id) + 1;
        }

        private int GenerateAssetId()
        {
            return _accounts.SelectMany(a => a.Assets).Max(b => b.Id) + 1;
        }
    }
}
