using BllAuth.Models;

namespace BllAuth.Services
{
    public interface IAuthService
    {
        public string GenerateToken(LoginUser user);
        Task<bool> RegisterUser(RegisterUser user);
        Task<bool> Login(LoginUser user);
        Task<string> AddRoleAsync(string roleName);
    }
}