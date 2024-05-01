using System.ComponentModel.DataAnnotations;

namespace AuthenticationWebApi.Models;
public class RegisterUserRequest
{

    [Required]public string UserName { get; set; }
    [Required]public string Password { get; set; }
    [Required]public string Email { get; set; }
}
