using Microsoft.AspNetCore.Http;

namespace NoteForge.Application.MarkdownFile.CreateMarkdownFile
{
    public class CreateMarkdownFileRequest
    {
        public required IFormFile File { get; set; }
    }
}
