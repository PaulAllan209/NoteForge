using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NoteForge.Domain;
using NoteForge.Domain.Dtos;
using NoteForge.Domain.Interfaces;

namespace NoteForge.Infrastructure.Services
{
    public class AuthorizationCodeStrategy : IOAuthStrategy, IRefreshTokenStrategy
    {
        private readonly AppDbContext context;
        private readonly ITokenService tokenService;
        private readonly IConfiguration configuration;

        public string GrantType => "authorization_code";

        public AuthorizationCodeStrategy(
            AppDbContext context,
            ITokenService tokenService,
            IConfiguration configuration
            )
        {
            this.context = context;
            this.tokenService = tokenService;
            this.configuration = configuration;
        }
        public async Task<AuthResponseDto> GenerateTokenAsync(TokenGrantRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(request.Code))
            {
                throw new ArgumentException("Authorization code is required");
            }

            if (string.IsNullOrEmpty(request.CodeVerifier))
            {
                throw new ArgumentException("Code verifier is required for PKCE");
            }

            var authCode = await context.AuthorizationCode
                .Include(x => x.AppUser)
                .FirstOrDefaultAsync(x => x.Code == request.Code, cancellationToken);

            if (authCode == null || !authCode.IsValid())
            {
                throw new UnauthorizedAccessException("Invalid or expired authorization code");
            }

            if(!tokenService.ValidateCodeChallenge(request.CodeVerifier, authCode.CodeChallenge))
            {
                throw new UnauthorizedAccessException("Invalid code verifier");
            }

            if (!string.IsNullOrEmpty(request.RedirectUri) && authCode.RedirectUri != request.RedirectUri)
            {
                throw new UnauthorizedAccessException("Redirect URI mismatch");
            }

            authCode.MarkAsUsed();
            context.AuthorizationCode.Update(authCode);

            var response = await CreateTokenResponse(authCode.AppUser, authCode.Nonce, cancellationToken);

            await context.SaveChangesAsync();

            return response;
        }

        public async Task<AuthResponseDto> GenerateFromRefreshTokenAsync(RefreshToken refreshToken, TokenGrantRequest request, CancellationToken cancellationToken)
        {
            if (refreshToken.GrantType != GrantType)
            {
                throw new UnauthorizedAccessException("Invalid grant type for this refresh token");
            }

            refreshToken.Revoke();
            context.RefreshToken.Update(refreshToken);

            var response = await CreateTokenResponse(refreshToken.AppUser, null, cancellationToken);
            await context.SaveChangesAsync();

            return response;
        }

        private async Task<AuthResponseDto> CreateTokenResponse(AppUser user, string? nonce, CancellationToken cancellationToken)
        {
            var accessToken = tokenService.GenerateAccessToken(user);
            var idToken = tokenService.GenerateIdToken(user, nonce);
            var refreshTokenString = tokenService.GenerateRefreshToken();

            var expiresInMinutes = int.Parse(configuration["Jwt:AccessTokenExpirationMinutes"]!);
            var refreshExpiryDays = int.Parse(configuration["Jwt:RefreshTokenExpirationDays"]!);

            var refreshToken = new RefreshToken(
                user,
                refreshTokenString,
                DateTime.UtcNow.AddDays(refreshExpiryDays),
                GrantType
            );

            context.RefreshToken.Add(refreshToken);
            await context.SaveChangesAsync();

            return new AuthResponseDto(
                accessToken,
                "Bearer",
                expiresInMinutes * 60,
                refreshTokenString,
                idToken,
                "openid profile email"
            );
        }
    }
}
