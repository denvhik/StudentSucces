using System.ComponentModel.DataAnnotations;

namespace AuthenticationWebApi.Models;

public class LoginUserRequest
{
    [Required]public  string Email { get; set; }
    [Required]public  string Password { get; set; }
}
