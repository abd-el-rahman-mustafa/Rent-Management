using API.Domain.Entities;
using API.Infrastructure.Data;
using API.Middleware;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register application services via extension method
builder.Services.AddApplicationServices(builder.Configuration);



builder.Services.AddDataProtection();

builder.Services.AddIdentityCore<AppUser>(opt =>
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

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
