using API.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace API.Infrastructure.Data;

public partial class DataContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<AppRole> AppRoles { get; set; }
    public DbSet<AppUserRole> AppUserRoles { get; set; }
    public DbSet<Location> Locations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Configure AppUserRole
        builder.Entity<AppUserRole>(userRole =>
        {
            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
        });

        // Configure AppUser
        builder.Entity<AppUser>(user =>
        {
            user.HasMany(u => u.UserRoles)
                .WithOne(u=> u.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure AppRole
        builder.Entity<AppRole>(role =>
        {
            role.HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                 .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Event
        builder.Entity<Event>(eventEntity =>
        {
            eventEntity.HasOne(e => e.Location)
                .WithMany()
                .HasForeignKey("LocationId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            eventEntity.HasMany(e => e.Attendees)
                .WithOne(ea => ea.Event)
                .HasForeignKey(ea => ea.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure EventAttendee
        builder.Entity<EventAttendee>(attendee =>
        {
            attendee.HasOne(ea => ea.User)
                .WithMany()
                .HasForeignKey(ea => ea.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });


    }

}
