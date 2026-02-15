namespace NoteForge.Domain.Dtos.StorageService
{
    public record BlobUploadResultDto
    (
        string BlobName,
        string ContainerName,
        string Url,
        long SizeInBytes);
}
