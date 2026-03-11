namespace NoteForge.Domain
{
    public class RefreshToken : BaseAuditableEntity
    {
        public int Id { get; private set; }
        public string Token { get; private set; } = string.Empty;
        public int AppUserId { get; private set; }
        public AppUser AppUser { get; private set; }
        public DateTime ExpiresAtUtc { get; private set; }
        public bool IsRevoked { get; private set; }
        public string GrantType { get; private set; } = string.Empty;

        private RefreshToken() { }

        public RefreshToken(AppUser appUser, string token, DateTime expiresAtUtc, string grantType = "authorization_code")
        {
            AppUser = appUser;
            AppUserId = appUser.Id;
            Token = token;
            ExpiresAtUtc = expiresAtUtc;
            GrantType = grantType;
            CreatedBy = appUser.UserName;
            CreatedAtUtc = DateTime.UtcNow;
            IsRevoked = false;

        }

        public void Revoke()
        {
            IsRevoked = true;
        }

        public bool IsExpired() => DateTime.UtcNow >= ExpiresAtUtc;
        public bool IsValid() => !IsRevoked && !IsExpired();
    }
}
