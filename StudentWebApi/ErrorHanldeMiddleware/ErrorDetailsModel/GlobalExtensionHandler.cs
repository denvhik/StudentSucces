using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StudentWebApi.ErrorHanldeMiddleware.ErrorDetailsModel;

/// <summary>
/// Provides a global exception handling mechanism to standardize error responses across the application.
/// </summary>
public class GlobalExtensionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExtensionHandler> _logger;
    private readonly ErrorMessageLoader _errorMessageLoader;
    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalExtensionHandler"/> class with necessary dependencies.
    /// </summary>
    /// <param name="logger">The logger to log error details and other information.</param>
    /// <param name="errorMessageLoader">The loader to fetch custom error messages based on exception types.</param>
    public GlobalExtensionHandler(ILogger<GlobalExtensionHandler> logger, ErrorMessageLoader errorMessageLoader)
    {
        _logger = logger;
        _errorMessageLoader = errorMessageLoader;
    }
    /// <summary>
    /// Attempts to handle exceptions globally by providing a standardized JSON response based on the type of exception.
    /// </summary>
    /// <param name="httpContext">The HttpContext in which the exception occurred.</param>
    /// <param name="exception">The exception that was caught and needs to be handled.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation, containing a boolean value indicating whether the exception was handled.</returns>
    /// <remarks>
    /// This method logs the exception, loads a custom error message if available, and constructs a ProblemDetails object to return as a JSON response.
    /// If no custom error message is available, it defaults to a generic 500 Internal Server Error response.
    /// </remarks>
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
