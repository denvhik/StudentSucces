using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

public class ImageZipResult : ActionResult
{
    public List<(byte[] imageData, string contentType, string fileName)> ImagesData { get; }

    public ImageZipResult(List<(byte[] imageData, string contentType, string fileName)> imagesData)
    {
        ImagesData = imagesData;
    }

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