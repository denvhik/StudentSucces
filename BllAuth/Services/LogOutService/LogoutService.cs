

using Dal.Auth.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BllAuth.Services.LogOutService;
public class LogoutService(SignInManager<User> signingMenager) : ILogoutService
{
    public  Task Logout()
    {
       return   signingMenager.SignOutAsync();
    }
}
