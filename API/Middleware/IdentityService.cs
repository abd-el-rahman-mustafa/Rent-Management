
using System.Text;
using API.Domain.Entities;
using API.Infrastructure.Data;
using API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Middleware;

public static class IdentityService
{
    public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {


        // Configure Identity
        services.AddIdentityCore<AppUser>(opt =>
         {
             opt.Password.RequireNonAlphanumeric = false;
             opt.Password.RequireUppercase = false;

             // Lockout settings
             opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Lock for 5 minutes
             opt.Lockout.MaxFailedAccessAttempts = 10; // Lock after 10 failed attempts
             opt.Lockout.AllowedForNewUsers = true; // Enable lockout for new users

         })
          .AddRoles<AppRole>()
          .AddUserManager<UserManager<AppUser>>()
          .AddRoleManager<RoleManager<AppRole>>()
          .AddEntityFrameworkStores<DataContext>()
          .AddDefaultTokenProviders();


        // Configure authentication to use JWT Bearer tokens
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            // TODO: Consider removing clock skew tolerance after adding the refresh token mechanism
            // 1 minute clock skew to account for server time drift
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });
    }
}