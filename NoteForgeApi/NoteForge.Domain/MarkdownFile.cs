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

        private MarkdownFile() {}
        public MarkdownFile(int appUserId, string blobName, string fileName, string createdByUserName)
        {
            AppUserId = appUserId;
            BlobName = blobName;
            FileName = fileName;
            CreatedBy = createdByUserName;
        }
        
    }
}
