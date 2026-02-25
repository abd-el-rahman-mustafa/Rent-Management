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
    public DbSet<OtpRecord> OtpRecords { get; set; }
    // public DbSet<Location> Locations { get; set; }

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

        // Configure OtpRecord
        builder.Entity<OtpRecord>(otp =>
        {
            otp.HasKey(o => o.Id);
            otp.HasOne(o => o.User)
               .WithMany()
               .HasForeignKey(o => o.UserId)
               .OnDelete(DeleteBehavior.Cascade);
            otp.HasIndex(o => new { o.UserId, o.Type, o.IsConsumed });
        });

    }

}
