using AwsS3Service;
using BllAuth.Services.ImageUploadService;
using Dal.Auth.Model;
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
        /// <summary>
        /// Retrieves all avatar images for the currently authenticated user and returns them as a zipped file.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> that contains the zipped avatar images for the user.
        /// If successful, returns a zip file containing the avatars. Otherwise, returns a Bad Request response with an error message.
        /// </returns>
        /// <remarks>
        /// This method performs the following steps:
        /// 1. Retrieves the user ID of the currently authenticated user from the HTTP context.
        /// 2. Calls the `_photoService.GetAvatarsAsync` method with the user ID to retrieve the avatar images.
        /// 3. Wraps the avatar images in an `ImageZipResult` object to be returned as a zip file.
        /// 4. If an exception occurs during this process, catches the exception and returns a Bad Request response with the error message.
        /// </remarks>
        /// <exception cref="Exception">Returns a Bad Request response with the exception message if an error occurs.</exception>
        /// <response code="200">Returns a zip file containing the user's avatars.</response>
        /// <response code="400">Returns a Bad Request response if an error occurs.</response>
        // GET: api/Avatars
        [HttpGet("getavatar")]
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
        /// <summary>
        /// Uploads a new avatar image for the currently authenticated user.
        /// </summary>
        /// <param name="file">The avatar image file to be uploaded.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> indicating the result of the upload operation.
        /// If successful, returns an OK response. Otherwise, throws an exception with an error message.
        /// </returns>
        /// <remarks>
        /// This method performs the following steps:
        /// 1. Generates a new GUID and appends the file extension from the uploaded file to create a unique filename.
        /// 2. Opens a read stream from the uploaded file.
        /// 3. Calls the `_s3Service.UploadFileAsync` method with the file stream and the generated filename to upload the file to Amazon S3.
        /// 4. If the file is successfully uploaded and a URL is returned, retrieves the user ID of the currently authenticated user from the HTTP context.
        /// 5. Calls the `_photoService.UploadAvatarWithoutDataAsync` method with the uploaded file, user ID, and the result URL to save the avatar details.
        /// 6. Returns an OK response if the process completes successfully.
        /// 7. If an exception occurs during this process, catches the exception and throws a new exception with the error message.
        /// </remarks>
        /// <exception cref="Exception">Throws an exception with the error message if an error occurs.</exception>
        /// <response code="200">Returns an OK response if the avatar is uploaded successfully.</response>
        /// <response code="400">Returns a Bad Request response if the file is null or an error occurs during the upload process.</response>
       
        [HttpPost("UploadAvatar")]
        public async Task<IActionResult> UploadAvatar( IFormFile file)
        {
            try
            {   
                string guid = Guid.NewGuid() + Path.GetExtension(file.FileName);
                using  var stream = file.OpenReadStream();
                 var resultUrl = await _s3Service.UploadFileAsync(stream, guid);
                if (!string.IsNullOrEmpty(resultUrl))
                {
                    var userid = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = await _photoService.UploadAvatarWithoutDataAsync(file, userid, resultUrl);
                }

                return Ok();
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpDelete("DeleteAvatar/{fileName}")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
             var response = await _s3Service.DeleteFileAsync(fileName);
            return Ok(response);
        }
    }
}
