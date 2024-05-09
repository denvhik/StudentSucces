using Dal.Auth.Model;
using Microsoft.AspNetCore.Identity;
namespace DalAuth.Model;

public class Photo
{
    public int PhotoId { get; set; }
    public string Title { get; set; }
    public byte[] ImageData { get; set; }
    public string ContentType { get; set; }
    public Guid UserId { get; set; } 
    public virtual User User { get; set; }
}
