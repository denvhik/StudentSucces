using AuthenticationWebApi.Models;
using AutoMapper;
using BllAuth.Models;
using BllAuth.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public AuthenticationController(IAuthService authService, IMapper mapper)
        {

            _authService = authService;
            _mapper = mapper;
        }

        // POST /user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            try
            {
                var mappedRequest = _mapper.Map<RegisterUser>(request);
                return Ok(await _authService.RegisterUser(mappedRequest));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

     
       [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var mappedRequest = _mapper.Map<LoginUser>(request);
            if (await _authService.Login(mappedRequest))
            {
                var token = _authService.GenerateToken(mappedRequest);
                return Ok(token);
            }
                
            return BadRequest();
        }
    }
}
