//using AuthenticationWebApi.MappingProfiles;
//using Dal.Auth.Context;
//using Dal.Auth.Model;
//using DalAuth.Model;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using System.Reflection;
//using System.Text;

//namespace StudentWebApi;

//public  static class StudentWebApiExtensionConfiguration
//{
//    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
//    {
       
//        services.AddControllers();

    
//        var awsSettings = config.GetSection("AWS");
//        var credential = new Amazon.Runtime.BasicAWSCredentials(awsSettings["AccessKey"], awsSettings["SecretAccessKey"]);
//        var awsOptions = config.GetAWSOptions();
//        awsOptions.Credentials = credential;
//        awsOptions.Region = Amazon.RegionEndpoint.EUNorth1;
//        services.AddDefaultAWSOptions(awsOptions);

       
//        services.AddAutoMapper(typeof(UserProfiles));

       
//        services.AddIdentity<User, Roles>()
//                .AddEntityFrameworkStores<AuthContext>();

     
//        services.AddAuthentication(options =>
//        {
//            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//        })
//        .AddJwtBearer(options =>
//        {
//            options.SaveToken = true;
//            options.RequireHttpsMetadata = false;
//            options.TokenValidationParameters = new TokenValidationParameters()
//            {
//                ValidateIssuer = true,
//                ValidateAudience = true,
//                ValidateLifetime = true,
//                ValidateIssuerSigningKey = true,
//                ClockSkew = TimeSpan.Zero,
//                ValidAudience = config["JwtOptions:Audience"],
//                ValidIssuer = config["JwtOptions:Issuer"],
//                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtOptions:SecretKey"]!))
//            };
//        });

      
//        services.Configure<IdentityOptions>(options =>
//        {
//            options.Password.RequireDigit = false;
//            options.Password.RequireLowercase = false;
//            options.Password.RequireNonAlphanumeric = false;
//            options.Password.RequireUppercase = false;
//            options.Password.RequiredLength = 6;
//            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
//            options.Lockout.MaxFailedAccessAttempts = 3;
//            options.Lockout.AllowedForNewUsers = true;
//            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
//            options.User.RequireUniqueEmail = true;
//        });


//        services.AddEndpointsApiExplorer();
//        services.AddSwaggerGen(c =>
//        {
//            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//            var xmlPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, xmlFile);
//            c.IncludeXmlComments(xmlPath);

//            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//            {
//                BearerFormat = "JWT",
//                Name = "JWT Authentication",
//                In = ParameterLocation.Header,
//                Type = SecuritySchemeType.Http,
//                Scheme = JwtBearerDefaults.AuthenticationScheme,
//                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",
//            });
//            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
//                {
//                    new OpenApiSecurityScheme {
//                        Reference = new OpenApiReference {
//                            Type = ReferenceType.SecurityScheme,
//                            Id = "Bearer"
//                        }
//                    },
//                    new string[] {}
//                }
//            });
//        });
//        return services;
//    }
//}
