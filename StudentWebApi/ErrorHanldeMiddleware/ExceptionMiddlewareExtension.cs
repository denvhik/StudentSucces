//using System.Data.SqlClient;
//using System.Net;
//using System.Text.Json;

//namespace StudentWebApi.ErrorHanldeMiddleware;
//public  class ExceptionMiddlewareExtension
//{
//    private  readonly RequestDelegate _requestDelegate;
//    private readonly ILogger<ExceptionMiddlewareExtension> _logger;


//    public ExceptionMiddlewareExtension(RequestDelegate requestDelegate,
//    ILogger<ExceptionMiddlewareExtension> logger)
//    {
//        _requestDelegate = requestDelegate;
//        _logger = logger;
//    }

//    public async Task InvokeAsync(HttpContext httpContext) 
//    {
//        try 
//        {
//            await _requestDelegate(httpContext);
//        }
//        catch (SqlException ex) 
//        {
//            _logger.LogError(ex.Message);
//            await HandleExceptionAsync(httpContext, ex);
//        }  
//    }
//    private  Task HandleExceptionAsync(HttpContext context, SqlException exception)
//    {
//        context.Response.ContentType = "application/json";
//        context.Response.StatusCode = (int)HttpStatusCode.GatewayTimeout; 

//        var result = JsonSerializer.Serialize(new
//        {
//            error = exception.Message,
//            detail = exception.StackTrace 
//        });
//        return context.Response.WriteAsync(result);
//    }
//}
