using Microsoft.AspNetCore.Http;

namespace BllAuth.Services.ImageUploadService;
public interface IPhotoService
{
    Task<string> UploadAvatarAsync(IFormFile file, string userId);
     Task<List<(byte[] imageData, string contentType, string fileName)>> GetAvatarsAsync(string userId);
}
