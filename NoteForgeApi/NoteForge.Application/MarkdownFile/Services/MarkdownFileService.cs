using Microsoft.AspNetCore.Http;
using NoteForge.Application.Auth.Interfaces;
using NoteForge.Application.MarkdownFile.Dtos;
using NoteForge.Application.MarkdownFile.Interfaces;
using NoteForge.Domain.Interfaces;
using NoteForge.Domain.Interfaces.Repositories;
using System.Security.Claims;

namespace NoteForge.Application.MarkdownFile.Services
{
    internal class MarkdownFileService : IMarkdownFileService
    {
        private readonly IMarkdownFileRepository markdownFileRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserContext userContext;

        public MarkdownFileService(IMarkdownFileRepository markdownFileRepository, IUnitOfWork unitOfWork, IUserContext userContext)
        {
            this.markdownFileRepository = markdownFileRepository;
            this.unitOfWork = unitOfWork;
            this.userContext = userContext;
        }

        public async Task CreateMarkdownFile(CreateMarkdownFileRequest request, CancellationToken cancellationToken = default)
        {
            var markDownFile = new Domain.MarkdownFile(userContext.GetUserId, $"{request.FileName}.md", request.FileName, userContext.GetUserName);

            await markdownFileRepository.AddAsync(markDownFile);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
