using System.ComponentModel.DataAnnotations;

namespace API.Domain.Entities;
    public abstract class SimpleBaseEntity
    {
        public SimpleBaseEntity()
        {
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }

        [Key]
        public int Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; } = true;
    }

