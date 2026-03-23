using API.Application.Services;
using API.Application.Interfaces;
using API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using API.Application.DTOs;

namespace API.Middleware;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Configure email settings
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        // Configure JWT settings
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        
        // Service registrations
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IOtpService, OtpService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
