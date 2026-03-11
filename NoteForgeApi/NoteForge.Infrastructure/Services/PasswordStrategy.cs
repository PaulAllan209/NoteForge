using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NoteForge.Domain;
using NoteForge.Domain.Dtos;
using NoteForge.Domain.Interfaces;

namespace NoteForge.Infrastructure.Services
{
    public class PasswordStrategy : IOAuthStrategy, IRefreshTokenStrategy
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly AppDbContext context;
        private readonly ITokenService tokenService;
        private readonly IConfiguration configuration;

        public string GrantType => "password";

        public PasswordStrategy(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            AppDbContext context,
            ITokenService tokenService,
            IConfiguration configuration
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.tokenService = tokenService;
            this.configuration = configuration;
        }

        public async Task<AuthResponseDto> GenerateTokenAsync(TokenGrantRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                throw new ArgumentException("Email and password are required");
            }

            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            return await CreateTokenResponse(user, cancellationToken);
        }

        public async Task<AuthResponseDto> GenerateFromRefreshTokenAsync(RefreshToken refreshToken, TokenGrantRequest request, CancellationToken cancellationToken)
        {
            if (refreshToken.GrantType != GrantType)
            {
                throw new UnauthorizedAccessException("Invalid grant type for this refresh token");
            }

            refreshToken.Revoke();
            context.RefreshToken.Update(refreshToken);

            var response = await CreateTokenResponse(refreshToken.AppUser, cancellationToken);
            await context.SaveChangesAsync();
            return response;
        }

        private async Task<AuthResponseDto> CreateTokenResponse(AppUser user, CancellationToken cancellationToken)
        {
            var accessToken = tokenService.GenerateAccessToken(user);
            var idToken = tokenService.GenerateIdToken(user);
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
            await context.SaveChangesAsync(cancellationToken);

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
