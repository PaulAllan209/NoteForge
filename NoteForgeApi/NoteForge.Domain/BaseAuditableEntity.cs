using NoteForge.Domain.Interfaces;

namespace NoteForge.Domain
{
    public abstract class BaseAuditableEntity : IBaseAuditableEntity
    {
        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
        public string CreatedBy { get; private set; }
        public DateTime? LastModifiedAtUtc { get; private set; }
        public string? LastModifiedBy { get; private set; }
    }
}
