using AwsS3Service;
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
        private readonly IS3Service _s3Service;
        public ImageController(IPhotoService photoService, IHttpContextAccessor httpContextAccessor, IS3Service s3Service)
        {
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
            _s3Service = s3Service;
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
        [HttpPost("uploadAva")]
        public async Task<IActionResult> UploadAvatar( IFormFile file)
        {
            try
            {
                string guid = Guid.NewGuid() + Path.GetExtension(file.FileName);
                using  var stream = file.OpenReadStream();
                 var resultUrl = await _s3Service.UploadFileAsync(stream, guid);
                var userid = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var message = await _photoService.UploadAvatarAsync(file, userid, resultUrl);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
