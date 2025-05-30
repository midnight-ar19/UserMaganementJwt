using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMaganementJwt.Models;

namespace UserMaganementJwt.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterAsync(string username, string email, string password);
        Task<string> LoginAsync(string email, string password);
        Task<User> GetUserByIdAsync(string userId);
        Task<bool> ValidateTokenAsync(string token);
    }
}
