using Microsoft.AspNetCore.Identity;
namespace Dal.Auth.Model;

public class User:IdentityUser<Guid>
{
    public string RefreshToken { get; set; }
}
