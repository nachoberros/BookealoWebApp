using Bookealo.CommonModel.Account;
using Bookealo.CommonModel.Users;

namespace Bookealo.Services.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetUsers(int accountId);
        void UpdateUser(int accountId, User user);
        User? GetUserById(int accountId, int userID);
        void RemoveUser(int accountId, User user);
        void AddUser(int accountId, User user);
        bool IsValidAccount(int accountId, string email);
        Account GetDefaultAccount(string email);
    }
}
