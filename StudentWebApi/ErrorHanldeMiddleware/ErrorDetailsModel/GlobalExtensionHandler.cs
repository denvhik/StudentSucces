using Handling;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace StudentWebApi.ErrorHanldeMiddleware.ErrorDetailsModel;

public class GlobalExtensionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExtensionHandler> _logger;

    public GlobalExtensionHandler(ILogger<GlobalExtensionHandler> logger)
    {
        _logger = logger;
    }

    public async  ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails;  
        _logger.LogError($"extension occured when proccesing your requst:{exception.Message}");
        switch (exception)
        {
            case BadHttpRequestException:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = exception.GetType().Name
                };
                break;
            case  KeyNotFoundException:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "5001",
                    Detail = exception.GetType().Name
                };
                break;
            case ArgumentException:
                problemDetails = new ProblemDetails
                
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Enter valid parameters"
                };
                break;
            case ValidationException:
                problemDetails = new ProblemDetails

                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Parametr"
                };
                break;
            case UnauthorizedAccessException:
                problemDetails = new ProblemDetails
                {
                   Status = (int)HttpStatusCode.Unauthorized,
                   Title = "Unauthorized",
                };
                break;
            case SystemExeptionHandle:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "5001",
                    Detail = exception.GetType().Name
                };
                break;
                case UserFriendlyException:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "5001",
                    Detail = exception.Message
                };
                break;
            default:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = exception.GetType().Name
                };
                break;
        }
        _logger.LogError($"extension occured when proccesing your requst:{exception.Message}");
        httpContext.Response.StatusCode = (int)problemDetails.Status;
        await httpContext
            .Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
