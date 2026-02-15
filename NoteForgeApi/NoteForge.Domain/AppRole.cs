using Microsoft.AspNetCore.Identity;
using NoteForge.Domain.Interfaces;

namespace NoteForge.Domain
{
    public class AppRole : IdentityRole<int>, IBaseAuditableEntity
    {
        public string? Description { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedAtUtc { get; set; }
        public string? LastModifiedBy { get; set; }
        private AppRole()
        {
        }
    }
}
