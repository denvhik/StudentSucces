using Azure.Core;
using BllAuth.Models;
using Dal.Auth.Model;
using DalAuth.Model;
using Microsoft.AspNetCore.Identity;

namespace BllAuth.Services.AuthService;
public class AuthService(UserManager<User> userManager,
    RoleManager<Roles> roleManager) : IAuthService
{
    /// <summary>
    /// Registers a new user in the system and assigns the "Member" role to the user.
    /// </summary>
    /// <param name="user">An object containing the details of the user to be registered, such as UserName, Email, and Password.</param>
    /// <returns>A boolean value indicating whether the user registration and role assignment were successful.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the user creation fails or the role assignment fails. The exception message contains the specific errors encountered.
    /// </exception>
    /// <exception cref="Exception">
    /// Thrown when an unexpected error occurs during the user registration process. The exception message contains the details of the error.
    /// </exception>
    /// <remarks>
    /// This method creates a new user with the provided details and assigns the "Member" role to the user. If the user creation or role assignment fails,
    /// an appropriate exception is thrown with the details of the errors encountered.
    /// </remarks>
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
    /// <summary>
    /// Authenticates a user by checking their email and password.
    /// </summary>
    /// <param name="user">An object containing the login details of the user, such as Email and Password.</param>
    /// <returns>A boolean value indicating whether the login was successful. Returns true if the email exists and the password is correct; otherwise, false.</returns>
    /// <remarks>
    /// This method first checks if a user with the provided email exists in the system. If the user is found, it then verifies the password.
    /// If the email or password is incorrect, the method returns false.
    /// </remarks>
    public async Task<bool> Login(LoginUser user)
    {
        var identityuser = await userManager.FindByEmailAsync(user.Email);
        if (identityuser == null)
        {
            return false;
        }
        return await userManager.CheckPasswordAsync(identityuser, user.Password);
    }

    /// <summary>
    /// Adds a new role to the system.
    /// </summary>
    /// <param name="roleName">The name of the role to be added.</param>
    /// <returns>
    /// A string message indicating the result of the operation:
    /// - "Role name is required." if the role name is null or whitespace.
    /// - "Role already exists." if the role already exists in the system.
    /// - "Role {roleName} created successfully." if the role was created successfully.
    /// - A string containing the errors if the role creation failed.
    /// </returns>
    /// <remarks>
    /// This method checks if the role name is provided and not null or whitespace.
    /// It then checks if the role already exists in the system.
    /// If the role does not exist, it attempts to create the role and returns a success message.
    /// If the role creation fails, it returns a string containing the errors encountered during the creation process.
    /// </remarks>
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
    /// <summary>
    /// Registers a new user as an Admin.
    /// </summary>
    /// <param name="user">The user registration details.</param>
    /// <returns>
    /// A boolean indicating whether the registration was successful.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when:
    /// - The email already exists.
    /// - The user registration fails.
    /// - Adding the user to the Admin role fails.
    /// </exception>
    /// <remarks>
    /// This method performs the following steps:
    /// 1. Checks if the email already exists in the system. If it does, an exception is thrown.
    /// 2. Creates a new identity user with the provided username and email.
    /// 3. Attempts to register the user with the provided password. If registration fails, an exception is thrown with the error details.
    /// 4. Assigns the newly created user to the "Admin" role. If this step fails, an exception is thrown with the error details.
    /// </remarks>
    public async Task<bool> RegisterAdmin(RegisterUser user)
    {
        var emailexist = await userManager.FindByEmailAsync(user.Email);
        if (emailexist != null)
            throw new Exception ("Email already exist");
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
    /// <summary>
    /// Changes the password for the user with the specified email.
    /// </summary>
    /// <param name="email">Email of the user whose password needs to be changed.</param>
    /// <param name="currentPassword">Current password of the user.</param>
    /// <param name="newPassword">New password to be set.</param>
    /// <returns>Result of the password change operation.</returns>
    public async Task<IdentityResult> ChangePasswordAsync(string email, string currentPassword, string newPassword)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null) 
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        return await userManager.ChangePasswordAsync(user, currentPassword,newPassword);
    }
    /// <summary>
    /// Registers a new user manager in the system.
    /// </summary>
    /// <param name="user">Object containing data for registering a new user.</param>
    /// <returns>Boolean indicating the success of the registration operation.</returns>
    public async Task<bool> RegisterMenaager(RegisterUser user)
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
