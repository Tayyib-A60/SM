using System.Collections.Generic;
using System.Threading.Tasks;
using SkineroMotors.Core.Models;

namespace SkineroMotors.Core
{
    public interface IUserRepository
    {
         User Authenticate(string email, string password);
         Task<IEnumerable<User>> GetUsers();
         Task<User> GetUser(int id);
         Task<User> CreateUser(User user, string password);
         void UpdateUser(User user, string password = null);
         void DeleteUser(User user);
    }
}