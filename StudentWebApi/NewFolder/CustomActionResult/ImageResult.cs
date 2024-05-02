using Microsoft.AspNetCore.Mvc;

namespace StudentWebApi.NewFolder.CustomActionResult;
public class ImageResult : ActionResult
{
    public string ImageFilePath { get; }
    public string ContentType { get; }

    public ImageResult(string imageFilePath, string contentType)
    {
        ImageFilePath = imageFilePath;
        ContentType = contentType;
    }
    public override async Task ExecuteResultAsync(ActionContext context)
    {
        if (File.Exists(ImageFilePath))
        {
            var buffer = await File.ReadAllBytesAsync(ImageFilePath);
            var response = context.HttpContext.Response;
            response.ContentType = ContentType;
            await response.Body.WriteAsync(buffer, 0, buffer.Length);
        }
        else
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        }
    }
}
