using API.Domain.Entities;
using API.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace API.Middleware;

public static class WebApplicationExtensions
{
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        await DataSeeder.SeedAsync(userManager, roleManager);
    }
}
