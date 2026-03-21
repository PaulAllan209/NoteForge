using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteForge.Application.MarkdownFile.Dtos;
using NoteForge.Application.MarkdownFile.Interfaces;

namespace NoteForgeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MarkdownFileController : Controller
    {
        IMarkdownFileService markdownFileService;

        public MarkdownFileController(IMarkdownFileService markdownFileService)
        {
            this.markdownFileService = markdownFileService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateMarkdownFile(CreateMarkdownFileRequest request, CancellationToken cancellationToken)
        {
            await markdownFileService.CreateMarkdownFile(request, cancellationToken);
            return NoContent();
        }
    }
}
