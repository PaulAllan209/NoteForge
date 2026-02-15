using Microsoft.AspNetCore.Identity;
using NoteForge.Domain.Interfaces;

namespace NoteForge.Domain
{
    public class AppUser : IdentityUser<int>, IBaseAuditableEntity
    {
        public Guid ExternalGuid { get; private set; } = Guid.NewGuid();
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        private List<MarkdownFile> _MarkdownFiles;
        public IReadOnlyCollection<MarkdownFile> MarkdownFiles => _MarkdownFiles.AsReadOnly();
        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
        public string CreatedBy { get; private set; }
        public DateTime? LastModifiedAtUtc { get; private set; }
        public string? LastModifiedBy { get; private set; }

        private AppUser()
        {

        }
    }
}
