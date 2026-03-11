using NoteForge.Domain;
using System.Security.Claims;

namespace NoteForge.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(AppUser user);
        string GenerateIdToken(AppUser user, string? nonce = null);
        string GenerateRefreshToken();
        string GenerateAuthorizationCode();
        string GenerateCodeChallenge(string codeVerifier);
        bool ValidateCodeChallenge(string codeVerifier, string codeChallenge);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
