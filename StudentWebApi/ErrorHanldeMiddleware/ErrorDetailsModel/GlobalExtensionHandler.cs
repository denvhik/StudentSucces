using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StudentWebApi.ErrorHanldeMiddleware.ErrorDetailsModel;

public class GlobalExtensionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExtensionHandler> _logger;
    private readonly ErrorMessageLoader _errorMessageLoader;

    public GlobalExtensionHandler(ILogger<GlobalExtensionHandler> logger, ErrorMessageLoader errorMessageLoader)
    {
        _logger = logger;
        _errorMessageLoader = errorMessageLoader;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var errorKey = exception.GetType().Name;
        var customError = _errorMessageLoader.GetErrorMessage(errorKey);

        var problemDetails = customError != null ? new ProblemDetails
        {
            Status = customError.Status,
            Title = customError.Error,
            Detail = string.Format(customError.Message, exception.Message)
        } : new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Unhandled Exception",
            Detail = exception.Message
        };

        _logger.LogError($"Exception occurred: {exception.Message}");
        httpContext.Response.StatusCode = (int)problemDetails.Status;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
