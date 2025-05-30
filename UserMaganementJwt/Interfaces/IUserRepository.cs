using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMaganementJwt.Models;

namespace UserMaganementJwt.Interfaces
{

    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string username);
        Task AddAsync(User user);
    }
}
