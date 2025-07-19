
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WaterProducts.services.token;
using WebChatApi.data;
using WebChatApi.model;
using WebChatApi.services.authincation;
using WebChatApi.services.chat;
using WebChatApi.services.users;

namespace WebChatApi.extensions
{
    public static class SevicesExtensions
    {
        public static void IdentiyConfigure(this IServiceCollection services)
        {
            services.AddIdentity < ApplicationUser, IdentityRole>(
                     options =>
                     {
                         options.Password.RequireDigit = false;
                         options.Password.RequireLowercase = false;
                         options.Password.RequireUppercase = false;
                         options.Password.RequireNonAlphanumeric = false;
                         options.Password.RequiredLength = 8;
                         options.User.AllowedUserNameCharacters = null;
                         options.SignIn.RequireConfirmedAccount = false;
                         options.SignIn.RequireConfirmedEmail = false;


                     }).
                     AddEntityFrameworkStores<ApplicationData>();

        }

        public static void JWTConfigureAuthincation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(
                options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).
                AddJwtBearer(options => {

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                    };
                    options.Authority = "https://localhost:7182";
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived= context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            if (!string.IsNullOrEmpty(accessToken) && context.HttpContext.Request.Path.StartsWithSegments("/chatHub"))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };


                });
        }

        public static void ScopeServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthincationServices, AuthincationServices>();
            services.AddScoped<ITokenServices, TokenServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IMessageServices, MessageServices>();
        }
        public static void DataBaseConfigure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationData>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
        }


        public static void SetCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllPolicy", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }

    }
}
