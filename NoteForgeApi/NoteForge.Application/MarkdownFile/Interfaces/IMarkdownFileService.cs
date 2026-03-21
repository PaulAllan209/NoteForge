using NoteForge.Application.MarkdownFile.Dtos;

namespace NoteForge.Application.MarkdownFile.Interfaces
{
    public interface IMarkdownFileService
    {
        Task CreateMarkdownFile(CreateMarkdownFileRequest request, CancellationToken cancellationToken = default);
    }
}
