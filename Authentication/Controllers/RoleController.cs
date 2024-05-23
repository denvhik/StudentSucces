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
    /// <summary>
    /// Adds a new role to the system.
    /// </summary>
    /// <param name="model">The <see cref="RoleModel"/> containing the details of the role to be added.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> indicating the result of the add operation.
    /// If the role is added successfully, returns an OK response with a success message.
    /// If the add operation fails, returns a Bad Request response with the failure message.
    /// </returns>
    /// <remarks>
    /// This method performs the following steps:
    /// 1. Calls the `_roleService.AddRoleAsync` method with the provided role name to add the role to the system.
    /// 2. Checks the result of the add operation. If the result contains "successfully", returns an OK response with the result message.
    /// 3. If the result does not contain "successfully", returns a Bad Request response with the result message.
    /// </remarks>
    /// <response code="200">Returns an OK response if the role is added successfully.</response>
    /// <response code="400">Returns a Bad Request response if an error occurs during the add operation.</response>
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
