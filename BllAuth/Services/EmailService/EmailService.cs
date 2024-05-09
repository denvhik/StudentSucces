
using Dal.Auth.Model;
using Microsoft.AspNetCore.Identity;

namespace BllAuth.Services.EmailService;

public class EmailService(UserManager<User> userManager) : IEmailService
{
    public async Task<User> GetUserByEmail(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user;
    }
}
