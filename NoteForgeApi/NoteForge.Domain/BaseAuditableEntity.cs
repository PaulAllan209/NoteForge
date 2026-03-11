using NoteForge.Domain.Interfaces;

namespace NoteForge.Domain
{
    public abstract class BaseAuditableEntity : IBaseAuditableEntity
    {
        public DateTime CreatedAtUtc { get; protected set; } = DateTime.UtcNow;
        public string CreatedBy { get; protected set; }
        public DateTime? LastModifiedAtUtc { get; protected set; }
        public string? LastModifiedBy { get; protected set; }
    }
}
