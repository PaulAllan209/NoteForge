using NoteForge.Domain.Dtos.StorageService;

namespace NoteForge.Domain.Interfaces.Repositories
{
    public interface IBlobStorageRepository
    {
        Task<BlobUploadResultDto> UploadAsync(string containerName, string fileName, Stream content, string contentType);
        Task<Stream?> DownloadAsync(string containerName, string blobName);
        Task<bool> DeleteAsync(string containerName, string blobName);
        Task<IEnumerable<string>> ListAsync(string containerName);
        Task<bool> ExistsAsync(string containerName, string blobName);
    }
}
