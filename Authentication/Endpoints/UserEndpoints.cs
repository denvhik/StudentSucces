using AuthenticationWebApi.Models;
using BLL.UserServices;

namespace AuthenticationWebApi.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app) 
    {
        app.MapPost("register", Register);
        app.MapPost("login", Login);
        return app;
    }
    private static async Task<IResult> Register(RegisterUserRequest request, UserService userService) 
    {
        await userService.Register(request.UserName, request.Email, request.Password);
        return Results.Ok();
    }
    private static async Task<IResult> Login(LoginUserRequest request,UserService userService) 
    {
        var token = await userService.Login(request.Email, request.Password);
        return Results.Ok(token);
    }
}
