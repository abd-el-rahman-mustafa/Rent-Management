using API.Domain.Constants;
using API.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        await SeedRolesAsync(roleManager);
        await SeedUsersAsync(userManager);
    }

    // ─── Roles ───────────────────────────────────────────────────────────────

    private static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
    {
        var roles = new List<AppRole>
        {
            // Admin role with full permissions
            new AppRole
            {
                Name        = Roles.Admin,
                NameAr      = "مدير",
                DescriptionEn = "Full access to all system features.",
                DescriptionAr = "وصول كامل إلى جميع ميزات النظام."
            },
            // User role with limited permissions
            new AppRole
            {
                Name        = Roles.User,
                NameAr      = "مستخدم",
                DescriptionEn = "Access to basic features and functionalities.",
                DescriptionAr = "الوصول إلى الميزات والوظائف الأساسية."
            }
            // Other roles can be added here in the future, such as "Manager" or "Support"
            
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role.Name!))
                await roleManager.CreateAsync(role);
        }
    }

    // ─── Users ────────────────────────────────────────────────────────────────

    private static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    {
        var now = DateTimeOffset.UtcNow;

        var seedUsers = new List<(AppUser User, string Password, string Role)>
        {
            (
                new AppUser
                {
                    UserName  = "admin",
                    Email     = "aamus2024@gmail.com",
                    FirstName = "System",
                    LastName  = "Admin",
                    Gender    = Gender.Male,
                    CreatedAt = now,
                    UpdatedAt = now,
                    IsActive  = true
                },
                "Admin@1234",
                Roles.Admin
            ),
            (
                new AppUser
                {
                    UserName  = "user",
                    Email     = "user@example.com",
                    FirstName = "Regular",
                    LastName  = "User",
                    Gender    = Gender.Male,
                    CreatedAt = now,
                    UpdatedAt = now,
                    IsActive  = true
                },
                "User@1234",
                Roles.User

            ),
                (
                    new AppUser
                    {
                        UserName  = "Samir",
                        Email     = "Samir@example.com",
                        FirstName = "Samir",
                        LastName  = "Example",
                        Gender    = Gender.Male,
                        CreatedAt = now,
                        UpdatedAt = now,
                        IsActive  = true
                    },
                    "Samir@1234",
                    Roles.User
                ),
                (
                    new AppUser
                    {
                        UserName  = "Sara",
                        Email     = "Sara@example.com",
                        FirstName = "Sara",
                        LastName  = "Example",
                        Gender    = Gender.Female,
                        CreatedAt = now,
                        UpdatedAt = now,
                        IsActive  = true
                    },
                    "Sara@1234",
                    Roles.User
                )
                ,(
                    new AppUser
                    {
                        UserName  = "Heba",
                        Email     = "Heba@example.com",
                        FirstName = "Heba",
                        LastName  = "Example",
                        Gender    = Gender.Female,
                        CreatedAt = now,
                        UpdatedAt = now,
                        IsActive  = true
                    },
                    "Heba@1234",
                    Roles.User
                )

        };
        Console.WriteLine("Seeding users...");
        foreach (var (user, password, role) in seedUsers)
        {
            if (await userManager.FindByEmailAsync(user.Email!) is null)
            {
                var result = await userManager.CreateAsync(user, password);
                Console.WriteLine($"Creating user {user.Email}: {(result.Succeeded ? "Success" : "Failed")}");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, role);
                Console.WriteLine($"Assigning role '{role}' to user {user.Email}: Success");
            }
        }
        Console.WriteLine("User seeding completed.");
    }
}
