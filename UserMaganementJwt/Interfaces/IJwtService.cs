using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMaganementJwt.Models;

namespace UserMaganementJwt.Interfaces
{
    public interface IJwtService
    {
        public string GenerateJwtToken(User user);
        Task<bool> ValidateTokenAsync(string token);
    }
}
