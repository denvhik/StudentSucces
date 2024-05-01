using Asp.Versioning;
using BLL;
using DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Serilog;
using Sieve.Services;
using StudentWebApi.Autommaper;
using StudentWebApi.ErrorHanldeMiddleware.ErrorDetailsModel;

var builder = WebApplication.CreateBuilder(args);
// AddAsync services to the container.
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{

});
builder.Services.AddDalService();   
builder.Services.AddBllService();
builder.Services.AddMemoryCache();
builder.Services.AddLogging();
builder.Services.AddExceptionHandler<GlobalExtensionHandler>();

builder.Services.AddSingleton<ErrorMessageLoader>();
builder.Services.AddSingleton<SieveProcessor>();
builder.Services.AddAutoMapper(typeof(MapperProvider));
builder.Services.AddProblemDetails();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddApiVersioning(x =>
{
x.DefaultApiVersion = new ApiVersion(1, 0);
x.AssumeDefaultVersionWhenUnspecified = true;
x.ReportApiVersions = true;
x.ApiVersionReader = ApiVersionReader.Combine(
new UrlSegmentApiVersionReader(),
new HeaderApiVersionReader());
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});
Log.Logger = new LoggerConfiguration()
.ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseMiddleware<ExceptionMiddlewareExtension>();
//app.UseMiddleware<CustomErrorHandlingMiddleware>();
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
