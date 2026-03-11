using NoteForge.Domain.Dtos;

namespace NoteForge.Domain.Interfaces
{
    public interface IOAuthStrategy
    {
        string GrantType { get; }
        Task<AuthResponseDto> GenerateTokenAsync(TokenGrantRequest request, CancellationToken cancellationToken = default);
    }
}
