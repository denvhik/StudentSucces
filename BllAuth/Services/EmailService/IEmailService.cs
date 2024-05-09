using Dal.Auth.Model;

namespace BllAuth.Services.EmailService
{
    public interface IEmailService
    {
        Task<User> GetUserByEmail(string email);
    }
}