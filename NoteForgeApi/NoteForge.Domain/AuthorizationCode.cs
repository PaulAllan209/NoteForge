namespace NoteForge.Domain
{
    public class AuthorizationCode : BaseAuditableEntity
    {
        public int Id { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public string CodeChallenge { get; private set; } = string.Empty;
        public string CodeChallengeMethod { get; private set; } = "S256";
        public int AppUserId { get; private set; }
        public AppUser AppUser { get; private set; }
        public string? Nonce { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public bool IsUsed { get; set; }
        public string RedirectUri { get; private set; } = string.Empty;
        public string Scope { get; private set; } = "openid profile email";

        private AuthorizationCode() { }

        public AuthorizationCode(AppUser appUser, string code, string codeChallenge, string redirectUri, string? nonce = null)
        {
            AppUser = appUser;
            AppUserId = appUser.Id;
            Code = code;
            CodeChallenge = codeChallenge;
            RedirectUri = redirectUri;
            Nonce = nonce;
            ExpiryDate = DateTime.UtcNow.AddMinutes(10);
            IsUsed = false;
        }

        public void MarkAsUsed()
        {
            IsUsed = true;
        }

        public bool IsExpired() => DateTime.UtcNow > ExpiryDate;
        public bool IsValid() => !IsUsed && !IsExpired();
    }
}
