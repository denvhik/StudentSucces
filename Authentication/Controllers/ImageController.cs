using BllAuth.Services.ImageUploadService;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthenticationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ImageController(IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/Avatars
        [HttpGet]
        public async Task<IActionResult> GetAllAvatars()
        {
            try
            {
                var userid = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var avatars = await _photoService.GetAvatarsAsync(userid);
                return new ImageZipResult(avatars);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Avatars
        [HttpPost]
        public async Task<IActionResult> UploadAvatar( IFormFile file)
        {
            try
            {
                var userid = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var message = await _photoService.UploadAvatarAsync(file, userid);
                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
