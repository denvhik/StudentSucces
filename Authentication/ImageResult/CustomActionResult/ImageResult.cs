using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

public class ImageZipResult : ActionResult
{
    public List<(byte[] imageData, string contentType, string fileName)> ImagesData { get; }

    public ImageZipResult(List<(byte[] imageData, string contentType, string fileName)> imagesData)
    {
        ImagesData = imagesData;
    }
    /// <summary>
    /// Executes the result asynchronously, creating a zip file containing image data and writing it to the HTTP response.
    /// </summary>
    /// <param name="context">The context in which the result is executed. Provides information about the HTTP request and response.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    /// This method performs the following steps:
    /// 1. Checks if the `ImagesData` property is null or empty. If it is, sets the HTTP response status code to 404 (Not Found) and exits.
    /// 2. Creates a `MemoryStream` to temporarily hold the zip file in memory.
    /// 3. Creates a `ZipArchive` in the memory stream for writing files into the zip format.
    /// 4. Iterates over the `ImagesData` collection, which contains tuples of image data, content type, and file name:
    ///    - For each image, determines the appropriate file extension based on the content type.
    ///    - Creates an entry in the zip archive for each image file.
    ///    - Writes the image data into the zip archive entry.
    /// 5. Resets the memory stream's position to the beginning to prepare for reading.
    /// 6. Sets the HTTP response content type to "application/zip".
    /// 7. Adds a content disposition header to the HTTP response to indicate that the response is an attachment with the file name "Images.zip".
    /// 8. Copies the memory stream's data to the HTTP response body, sending the zip file to the client.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown if the `context` parameter is null.</exception>
    public override async Task ExecuteResultAsync(ActionContext context)
    {
        if (ImagesData == null || !ImagesData.Any())
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        using (var memoryStream = new MemoryStream())
        {
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var (imageData, contentType, fileName) in ImagesData)
                {
                    if (imageData == null)
                    {
                        // Skip if imageData is null
                        continue;
                    }
                    string extension = GetExtensionFromContentType(contentType);
                    string fullFileName = $"{fileName}{extension}";

                    var fileEntry = archive.CreateEntry(fullFileName);
                    using (var entryStream = fileEntry.Open())
                    {
                        await entryStream.WriteAsync(imageData, 0, imageData.Length);
                    }
                }
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            context.HttpContext.Response.ContentType = "application/zip";
            context.HttpContext.Response.Headers.ContentDisposition = new Microsoft.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = "Images.zip"
            }.ToString();
            await memoryStream.CopyToAsync(context.HttpContext.Response.Body);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="contentType"></param>
    /// <returns></returns>
    private string GetExtensionFromContentType(string contentType)
    {
        switch (contentType.ToLower())
        {
            case "image/jpeg":
                return ".jpeg";
            case "image/png":
                return ".png";
            case "image/gif":
                return ".gif";
            case "image/bmp":
                return ".bmp";
            case "image/svg+xml":
                return ".svg";
            default:
                return "";
        }
    }
}