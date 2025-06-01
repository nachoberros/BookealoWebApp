using Bookealo.CommonModel.Account;
using Bookealo.CommonModel.Users;
using Bookealo.Services.Interfaces;

namespace Bookealo.Services.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly IMockingRepository _mockingRepository;

        public UserRepository(IMockingRepository mockingRepository) 
        {
            _mockingRepository = mockingRepository;
        }

        public List<User> GetUsers(int accountId)
        {
            return _mockingRepository.GetUsers(accountId);
        }

        public void UpdateUser(int accountId, User user)
        {
            _mockingRepository.UpdateUser(accountId, user);
        }

        public User? GetUserById(int accountId, int userID)
        {
            return _mockingRepository.GetUsers(accountId).FirstOrDefault(u => u.Id == userID);
        }

        public void RemoveUser(int accountId, User user)
        {
            _mockingRepository.RemoveUser(accountId, user);
        }

        public void AddUser(int accountId, User user)
        {
            _mockingRepository.AddUser(accountId, user);
        }

        public bool IsValidAccount(int accountId, string email)
        {
           return _mockingRepository.IsValidAccount(accountId, email);
        }

        public Account GetDefaultAccount(string email)
        {
            return _mockingRepository.GetDefaultAccount(email);
        }
    }
}
