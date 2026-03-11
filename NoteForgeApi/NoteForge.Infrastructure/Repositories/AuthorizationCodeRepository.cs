using Microsoft.EntityFrameworkCore;
using NoteForge.Domain;
using NoteForge.Domain.Interfaces.Repositories;

namespace NoteForge.Infrastructure.Repositories
{
    internal class AuthorizationCodeRepository : IAuthorizationCodeRepository
    {
        private readonly AppDbContext context; // TODO: Create base repostory and classname tagwith

        public AuthorizationCodeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Task<AuthorizationCode?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return context.AuthorizationCode
                .Include(x => x.AppUser)
                .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
        }


        public async Task AddAsync(AuthorizationCode authorizationCode, CancellationToken cancellationToken = default)
        {
            await context.AuthorizationCode.AddAsync(authorizationCode, cancellationToken);
        }

        

        public void Update(AuthorizationCode authorizationCode)
        {
            context.AuthorizationCode.Update(authorizationCode);
        }
    }
}
