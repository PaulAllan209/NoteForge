namespace NoteForge.Domain.Dtos
{
    public record AuthResponseDto(
        string AccessToken,
        string TokenType,
        int ExpiresIn,
        string RefreshToken,
        string IdToken,
        string? Scope
    );
}
