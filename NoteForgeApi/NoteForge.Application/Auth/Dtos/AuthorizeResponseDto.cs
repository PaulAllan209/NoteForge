namespace NoteForge.Application.Auth.Dtos
{
    public record AuthorizeResponseDto(
        string RedirectUri,
        string Code,
        string? State
    );
}
