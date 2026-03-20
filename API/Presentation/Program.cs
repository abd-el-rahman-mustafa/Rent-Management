using API.Domain.Entities;
using API.Infrastructure.Data;
using API.Middleware;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register application services via extension method
builder.Services.AddApplicationServices(builder.Configuration);

// AddIdentityServices 
builder.Services.AddIdentityServices(builder.Configuration);

// cors
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!);
    });
});

builder.Services.AddDataProtection();



builder.Services.AddOpenApi();

var app = builder.Build();

// Seed roles and users
await app.SeedDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
