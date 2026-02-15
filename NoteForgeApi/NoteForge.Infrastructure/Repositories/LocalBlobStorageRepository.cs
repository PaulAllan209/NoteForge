using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NoteForge.Domain.Dtos.StorageService;
using NoteForge.Domain.Interfaces.Repositories;

namespace NoteForge.Infrastructure.Repositories
{
    internal class LocalBlobStorageRepository : IBlobStorageRepository
    {
        private readonly string basePath;
        private readonly ILogger<LocalBlobStorageRepository> logger;

        public LocalBlobStorageRepository(IConfiguration configuration, ILogger<LocalBlobStorageRepository> logger)
        {
            this.basePath = configuration["LocalStorage:BasePath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "local-blobs");
            this.logger = logger;
        }

        public Task<bool> DeleteAsync(string containerName, string blobName)
        {
            var filePath = GetBlobPath(containerName, blobName);

            if (!File.Exists(filePath)) return Task.FromResult(false);

            File.Delete(filePath);
            return Task.FromResult(true);
        }

        public Task<Stream?> DownloadAsync(string containerName, string blobName)
        {
            var filePath = GetBlobPath(containerName, blobName);

            if (!File.Exists(filePath)) return Task.FromResult<Stream?>(null);

            Stream stream = File.OpenRead(filePath);
            return Task.FromResult<Stream?>(stream);
        }

        public Task<bool> ExistsAsync(string containerName, string blobName)
        {
            var filePath = GetBlobPath(containerName, blobName);
            return Task.FromResult(File.Exists(filePath));
        }

        public Task<IEnumerable<string>> ListAsync(string containerName)
        {
            var containerPath = GetContainerPath(containerName);

            if (!Directory.Exists(containerPath)) return Task.FromResult(Enumerable.Empty<string>());

            var files = Directory.GetFiles(containerPath)
                .Select(Path.GetFileName)
                .Where(f => f != null)
                .Cast<string>();

            return Task.FromResult(files);
        }

        public async Task<BlobUploadResultDto> UploadAsync(string containerName, string fileName, Stream content, string contentType)
        {
            var containerPath = GetContainerPath(containerName);
            Directory.CreateDirectory(containerPath);

            var blobName = $"{Guid.NewGuid()}_{SanitizeFileName(fileName)}";
            var filePath = Path.Combine(containerPath, blobName);

            await using var fileStream = File.Create(filePath);
            await content.CopyToAsync(fileStream);

            logger.LogInformation("Uploaded blob {blobName} to container {containerName}", blobName, containerName);

            return new BlobUploadResultDto(
                BlobName: blobName,
                ContainerName: containerName,
                Url: $"/blobs/{containerName}/{blobName}",
                SizeInBytes: new FileInfo(filePath).Length
            );
        }

        // Helpers
        private string GetContainerPath(string containerName) => Path.Combine(basePath, containerName);

        private string GetBlobPath(string containerName, string blobName) => Path.Combine(basePath, containerName, blobName);

        private static string SanitizeFileName(string fileName) =>
            Path.GetInvalidFileNameChars()
                .Aggregate(fileName, (f, c) => f.Replace(c, '_'));
    }
}