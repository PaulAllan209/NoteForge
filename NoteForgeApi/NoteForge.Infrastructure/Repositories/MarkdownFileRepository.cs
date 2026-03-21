using NoteForge.Domain;
using NoteForge.Domain.Interfaces;

namespace NoteForge.Infrastructure.Repositories
{
    internal class MarkdownFileRepository : BaseRepository<MarkdownFile>, IMarkdownFileRepository
    {
        public MarkdownFileRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
