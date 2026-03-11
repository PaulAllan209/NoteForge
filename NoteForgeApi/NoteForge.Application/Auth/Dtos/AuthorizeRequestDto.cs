namespace NoteForge.Application.Auth.Dtos
{
    public record AuthorizeRequestDto(
        string ClientId,
        string RedirectUri,
        string ResponseType,
        string Scope,
        string CodeChallenge,
        string CodeChallengeMethod,
        string? State = null,
        string? Nonce = null
    );
}
