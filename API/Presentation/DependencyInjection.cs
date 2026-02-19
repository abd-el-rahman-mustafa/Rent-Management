using API.Application.Services;
using API.Application.Interfaces;
using API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Middleware;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Auth
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
