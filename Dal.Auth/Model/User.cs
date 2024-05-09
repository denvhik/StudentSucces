using DalAuth.Model;
using Microsoft.AspNetCore.Identity;
namespace Dal.Auth.Model;

public class User:IdentityUser<Guid>
{
    public string RefreshToken { get; set; }
    public DateTime ExpirationTimetoken { get; set; }
    public virtual List<Photo> Photo { get; set; }
}
