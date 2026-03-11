using NoteForge.Application.Auth.Dtos;
using NoteForge.Domain.Dtos;

namespace NoteForge.Application.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> TokenAsync(TokenGrantRequest request, CancellationToken cancellationToken = default);
        Task<AuthorizeResponseDto> AuthorizeAsync(AuthorizeRequestDto request, string userId, CancellationToken cancellationToken = default);
        Task RevokeAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> ExternalCallbackAsync(string loginProvider, string providerKey, string? email, CancellationToken cancellationToken = default);
    }
}
