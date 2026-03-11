using Microsoft.EntityFrameworkCore;
using NoteForge.Domain;
using NoteForge.Domain.Interfaces.Repositories;

namespace NoteForge.Infrastructure.Repositories
{
    internal class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext context;

        public RefreshTokenRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return await context.RefreshToken
                .Include(x => x.AppUser)
                .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
        }

        public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            await context.RefreshToken.AddAsync(refreshToken, cancellationToken);
        }

        public void Update(RefreshToken refreshToken)
        {
            context.RefreshToken.Update(refreshToken);
        }
    }
}
