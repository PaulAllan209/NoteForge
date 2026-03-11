namespace NoteForge.Domain.Dtos
{
    public record TokenGrantRequest(
        string GrantType,
        string? Code = null,
        string? CodeVerifier = null,
        string? RefreshToken = null,
        string? ClientId = null,
        string? RedirectUri = null,
        string? Email = null,
        string? Password = null,
        string? UserName = null
    );
}
