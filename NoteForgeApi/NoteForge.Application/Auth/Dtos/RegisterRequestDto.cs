namespace NoteForge.Application.Auth.Dtos
{
    public record RegisterRequestDto(
        string Email,
        string Password,
        string UserName
    );
    
}
