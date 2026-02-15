namespace NoteForge.Domain.Interfaces
{
    public interface IBaseAuditableEntity
    {
        DateTime CreatedAtUtc { get; }
        string CreatedBy { get; }
        DateTime? LastModifiedAtUtc { get; }
        string? LastModifiedBy { get; }
    }
}
