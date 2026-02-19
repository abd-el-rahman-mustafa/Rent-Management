using API.Application.Services;
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

        // Register HttpClient for LocationService
        services.AddHttpClient<ILocationService, LocationService>()
            .ConfigureHttpClient(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(10);
            });

        // Register application services
        services.AddScoped<IEventService, EventService>();

        // Configure AutoMapper
        services.AddAutoMapper(typeof(EventMappingProfile));

        return services;
    }
}
