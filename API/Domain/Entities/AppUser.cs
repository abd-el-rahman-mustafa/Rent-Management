

using Microsoft.AspNetCore.Identity;

namespace API.Domain.Entities;

public class AppUser : IdentityUser<int>
{

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required Gender Gender { get; set; }
    public DateTimeOffset? DateOfBirth { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;

    public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();


}

public enum Gender
{
    Male,
    Female
}
