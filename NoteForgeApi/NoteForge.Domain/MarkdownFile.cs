namespace NoteForge.Domain
{
    public class MarkdownFile : BaseAuditableEntity
    {
        public int Id { get; private set; }
        public Guid ExternalGuid { get; private set; } = Guid.NewGuid();
        public int AppUserId { get; private set; }
        public AppUser AppUser { get; private set; }
        public string BlobName { get; private set; }
        public string FileName { get; private set; }

        public MarkdownFile(int appUserId)
        {
            AppUserId = appUserId;
        }
    }
}
