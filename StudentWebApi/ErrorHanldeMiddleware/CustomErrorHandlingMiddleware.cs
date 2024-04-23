//using StudentWebApi.ErrorHanldeMiddleware.ErrorDetailsModel;

//namespace StudentWebApi.ErrorHanldeMiddleware;

//public class CustomErrorHandlingMiddleware
//{
//    private readonly   RequestDelegate _requestDelegate;
//    private readonly ILogger<CustomErrorHandlingMiddleware> _logger;

//    public CustomErrorHandlingMiddleware(RequestDelegate requestDelegate, ILogger<CustomErrorHandlingMiddleware> logger)
//    {
//        _requestDelegate = requestDelegate;
//        _logger = logger;
//    }
//    public async Task InvokeAsync(HttpContext httpcontext) 
//    {
//            await _requestDelegate(httpcontext);
//            if (httpcontext.Response.StatusCode == 500)
//            {
//                await  HandleCustomErrorAsync(httpcontext);
//            }
//    }
//    private async static Task HandleCustomErrorAsync(HttpContext httpcontext) 
//    {
//            var error = new ErrorDetails
//            {
//                Message = "Something went wrong"
//            };
//            await httpcontext.Response.WriteAsync(error.ToString());
//    }
//}
