
using Microsoft.AspNetCore.Identity;

namespace API.Domain.Entities;
public class AppRole : IdentityRole<int>
{
    public required string DescriptionEn { get; set; }
    public required string DescriptionAr { get; set; }
    public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
}