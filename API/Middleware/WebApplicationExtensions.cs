using API.Domain.Entities;
using API.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Middleware;

public static class WebApplicationExtensions
{
    public static async Task InitializeDB(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        // Auto-migrate the database 
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        await dbContext.Database.MigrateAsync();


        // Seed roles and users
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        await DataSeeder.SeedAsync(userManager, roleManager);
    }
}
