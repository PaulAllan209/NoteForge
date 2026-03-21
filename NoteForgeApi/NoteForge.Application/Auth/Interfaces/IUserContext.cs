using System.Security.Claims;

namespace NoteForge.Application.Auth.Interfaces
{
    public interface IUserContext
    {
        int GetUserId { get; }
        IEnumerable<Claim> GetClaims();
        string GetUserName { get; }
    }
}
