
namespace API.Domain.Entities;
public abstract class BaseEntity : SimpleBaseEntity
{
    public BaseEntity()
    {
        UpdatedAt = DateTime.UtcNow;
    }
    public int? CreatedById { get; set; }
    public AppUser? CreatedBy { get; set; }
    public int? UpdatedById { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
