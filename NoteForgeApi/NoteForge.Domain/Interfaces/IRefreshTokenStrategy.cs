using NoteForge.Domain.Dtos;

namespace NoteForge.Domain.Interfaces
{
    public interface IRefreshTokenStrategy
    {
        Task<AuthResponseDto> GenerateFromRefreshTokenAsync(RefreshToken refreshToken, TokenGrantRequest request, CancellationToken cancellationToken);
    }
}
