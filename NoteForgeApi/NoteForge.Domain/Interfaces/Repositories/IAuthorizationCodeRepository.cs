namespace NoteForge.Domain.Interfaces.Repositories
{
    public interface IAuthorizationCodeRepository
    {
        Task<AuthorizationCode?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
        Task AddAsync(AuthorizationCode authorizationCode, CancellationToken cancellationToken = default);
        void Update(AuthorizationCode authorizationCode);
    }
}
