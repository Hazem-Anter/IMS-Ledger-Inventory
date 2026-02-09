
namespace IMS.Domain.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; set; }

        public int? CreatedByUserId { get; set; }
        public string? CreatedByName { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedByUserId { get; set; }
        public string? UpdatedByName { get; set; }
    }
}
