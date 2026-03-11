using Microsoft.EntityFrameworkCore;
using NoteForge.Domain.Dtos;
using NoteForge.Domain.Interfaces;

namespace NoteForge.Infrastructure.Services
{
    public class RefreshTokenStrategy : IOAuthStrategy
    {
        private readonly AppDbContext context;
        private readonly IEnumerable<IRefreshTokenStrategy> refreshStrategies;

        public string GrantType => "refresh_token";

        public RefreshTokenStrategy(
            AppDbContext context,
            IEnumerable<IRefreshTokenStrategy> refreshStrategies)
        {
            this.context = context;
            this.refreshStrategies = refreshStrategies;
        }

        public async Task<AuthResponseDto> GenerateTokenAsync(TokenGrantRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                throw new ArgumentException("Refresh token is required");
            }

            var storedToken = await context.RefreshToken
                .Include(x => x.AppUser)
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, cancellationToken);

            if (storedToken == null || !storedToken.IsValid())
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token");
            }

            var strategy = refreshStrategies
                .FirstOrDefault(x => x.GetType().GetProperty("GrantType")?.GetValue(x)?.ToString() == storedToken.GrantType);

            if (strategy == null)
            {
                throw new InvalidOperationException($"No strategy found for grant type: {storedToken.GrantType}");
            }

            return await strategy.GenerateFromRefreshTokenAsync(storedToken, request, cancellationToken);
        }
    }
}
