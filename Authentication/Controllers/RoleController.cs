using AuthenticationWebApi.Models;
using BllAuth.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IAuthService _roleService;

    public RoleController(IAuthService roleService)
    {
        _roleService = roleService;
    }
    [HttpPost("add")]
    public async Task<IActionResult> AddRole([FromBody] RoleModel model)
    {
        var result = await _roleService.AddRoleAsync(model.RoleName);

        if (result.Contains("successfully"))
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}
