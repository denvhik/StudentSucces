using BllAuth.Models;
using Dal.Auth.Model;
using DalAuth.Model;
using Microsoft.AspNetCore.Identity;

namespace BllAuth.Services.AuthService;
public class AuthService(UserManager<User> userManager,
    RoleManager<Roles> roleManager) : IAuthService
{
    public async Task<bool> RegisterUser(RegisterUser user)
    {
        try
        {
            var identityUser = new User
            {
                UserName = user.UserName,
                Email = user.Email,
            };

            var result = await userManager.CreateAsync(identityUser, user.Password);
            if (result.Succeeded)
            {

                var roleResult = await userManager.AddToRoleAsync(identityUser, "Member");
                if (!roleResult.Succeeded)
                {

                    var errors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException("Failed to assign role: " + errors);
                }

                return true;
            }
            else
            {

                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException("User creation failed: " + errors);
            }
        }
        catch (Exception ex)
        {

            throw new Exception("An error occurred while creating the user: " + ex.Message);
        }

    }
    public async Task<bool> Login(LoginUser user)
    {
        var identityuser = await userManager.FindByEmailAsync(user.Email);
        if (identityuser == null)
        {
            return false;
        }
        return await userManager.CheckPasswordAsync(identityuser, user.Password);
    }


    public async Task<string> AddRoleAsync(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            return "Role name is required.";
        }

        if (await roleManager.RoleExistsAsync(roleName))
        {
            return "Role already exists.";
        }
        var role = new Roles { Name = roleName };
        IdentityResult result = await roleManager.CreateAsync(role);

        if (result.Succeeded)
        {
            return $"Role {roleName} created successfully.";
        }

        return string.Join("; ", result.Errors);
    }

    public async Task<bool> RegisterAdmin(RegisterUser user)
    {
        var identityUser = new User
        {
            UserName = user.UserName,
            Email = user.Email,
        };
        var result = await userManager.CreateAsync(identityUser, user.Password);
        if (!result.Succeeded)
        {

            throw new Exception("Registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        var roleResult = await userManager.AddToRoleAsync(identityUser, "Admin");
        if (!roleResult.Succeeded)
        {
            throw new Exception("Failed to add user to role: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
        }

        return true;
    }

    public  async Task<IdentityResult> ChangePasswordAsync(string email, string currentPassword, string newPassword)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null) 
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        return await userManager.ChangePasswordAsync(user, currentPassword,newPassword);
    }

    public  async Task<bool> RegisterMenaager(RegisterUser user)
    {
        var identityUser = new User
        {
            UserName = user.UserName,
            Email = user.Email,
        };
        var result = await userManager.CreateAsync(identityUser, user.Password);

        if (!result.Succeeded)
            throw new Exception("Registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

        var roleResult = await userManager.AddToRoleAsync(identityUser, "Menager");

        if (!roleResult.Succeeded)
            throw new Exception("Failed to add user to role: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));

        return true;
    }
}
