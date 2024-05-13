using Dal.Auth.Context;
using Dal.Auth.Model;
using DalAuth.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BllAuth.Services.ImageUploadService;

public class PhotoService : IPhotoService
{
    private readonly UserManager<User> _userManager;
    private readonly AuthContext _applicationDbContext;
    
    public PhotoService(UserManager<User> userManager, AuthContext applicationDbContext)
    {
        _userManager = userManager;
        _applicationDbContext = applicationDbContext;
       
    }


    public async Task<List<(byte[] imageData, string contentType, string fileName)>> GetAvatarsAsync(string stringUserId)
    {
        if (string.IsNullOrEmpty(stringUserId))
            throw new Exception("User not authenticated");

        if (!Guid.TryParse(stringUserId, out Guid userId))
            throw new Exception("Invalid user ID format");

        var photos = await _applicationDbContext.Photos
                         .Where(p => p.UserId == userId)
                         .OrderByDescending(p => p.PhotoId)
                         .ToListAsync();
        if (!photos.Any())
            throw new Exception("No photos found for this user");

        var result = photos.Select(photo =>
            (photo.ImageData, photo.ContentType, photo.Title ?? "Unnamed")).ToList();

        return result;
    }

    public async Task<string> UploadAvatarAsync(IFormFile file, string userId, string url)
    {
        if (string.IsNullOrEmpty(userId))
            throw new Exception("User not authenticated");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        if (memoryStream.Length < 5097152)
        {
            var photo = new Photo
            {
                Title =file.FileName,
                ImageData = memoryStream.ToArray(),
                ContentType = file.ContentType,
                UserId = user.Id,
                Url = url
            };

            _applicationDbContext.Photos.Add(photo);
            await _applicationDbContext.SaveChangesAsync();

            return $"Avatar uploaded successfully with ID {photo.PhotoId}";
        }
        else
        {
            throw new Exception("File too large");
        }
    }
}
