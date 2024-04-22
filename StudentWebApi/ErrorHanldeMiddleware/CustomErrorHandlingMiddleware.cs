using Handling;
using StudentWebApi.ErrorHanldeMiddleware.ErrorDetailsModel;
using System.Net;

namespace StudentWebApi.ErrorHanldeMiddleware
{
    public class CustomErrorHandlingMiddleware
    {
        private readonly   RequestDelegate _requestDelegate;
        private readonly ILogger<CustomErrorHandlingMiddleware> _logger;

        public CustomErrorHandlingMiddleware(RequestDelegate requestDelegate, ILogger<CustomErrorHandlingMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpcontext) 
        {
            try
            {
                
                await _requestDelegate(httpcontext);
            }
            catch (SystemExeptionHandle ex)
            {
                _logger.LogError(ex.Message,ex);
                await HandleCustomErrorAsync(httpcontext,ex);
            }
        }
        private async static Task HandleCustomErrorAsync(HttpContext httpcontext, SystemExeptionHandle exception) 
        {
            httpcontext.Response.ContentType = "aplication/json";
            httpcontext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var error = new ErrorDetails
            {
                StatusCode = (int)HttpStatusCode.Forbidden,
                Message = exception.ToString()
            };
            await httpcontext.Response.WriteAsync(error.ToString());
        }
    }
}
