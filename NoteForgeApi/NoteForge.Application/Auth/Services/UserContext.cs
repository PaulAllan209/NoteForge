using Microsoft.AspNetCore.Http;
using NoteForge.Application.Auth.Interfaces;
using System.Security.Claims;

namespace NoteForge.Application.Auth.Services
{
    internal class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId
        {
            get
            {
                var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? httpContextAccessor.HttpContext?.User.FindFirstValue("user_id");

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    throw new UnauthorizedAccessException("User is not authenticated or user ID is invalid.");
                }

                return userId;
            }
        }

        public string GetUserName 
        { 
            get
            {
                var userName = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name)
                    ?? httpContextAccessor.HttpContext?.User.FindFirstValue("username");

                if (string.IsNullOrEmpty(userName))
                {
                    throw new UnauthorizedAccessException("User is not authenticated or username is invalid.");
                }

                return userName;
            }
        } 

        public IEnumerable<Claim> GetClaims()
        {
            return httpContextAccessor.HttpContext?.User.Claims ?? Enumerable.Empty<Claim>();
        }
    }
}
