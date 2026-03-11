using Microsoft.AspNetCore.Identity;
using NoteForge.Application.Auth.Dtos;
using NoteForge.Application.Auth.Interfaces;
using NoteForge.Domain;
using NoteForge.Domain.Dtos;
using NoteForge.Domain.Interfaces;
using NoteForge.Domain.Interfaces.Repositories;

namespace NoteForge.Application.Auth.Services
{
    internal class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IEnumerable<IOAuthStrategy> oAuthStrategies;
        private readonly ITokenService tokenService;
        private readonly IAuthorizationCodeRepository authorizationCodeRepository;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IUnitOfWork unitOfWork;

        public AuthService(
            UserManager<AppUser> userManager,
            IEnumerable<IOAuthStrategy> oAuthStrategies,
            ITokenService tokenService,
            IAuthorizationCodeRepository authorizationCodeRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.oAuthStrategies = oAuthStrategies;
            this.tokenService = tokenService;
            this.authorizationCodeRepository = authorizationCodeRepository;
            this.refreshTokenRepository = refreshTokenRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
        {
            var user = new AppUser(request.UserName, request.Email, "admin"); // TODO: add user context injection
            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description)); // TODO: create custom error models and exceptions
                throw new InvalidOperationException(errors);
            }

            var tokenRequest = new TokenGrantRequest("password", Email: request.Email, Password: request.Password);
            return await TokenAsync(tokenRequest, cancellationToken);

        }

        public async Task<AuthResponseDto> TokenAsync(TokenGrantRequest request, CancellationToken cancellationToken = default)
        {
            var strategy = oAuthStrategies.FirstOrDefault(x => x.GrantType == request.GrantType)
                ?? throw new NotSupportedException($"Grant type '{request.GrantType}' is not supported");

            return await strategy.GenerateTokenAsync(request, cancellationToken);
        }

        public async Task<AuthorizeResponseDto> AuthorizeAsync(AuthorizeRequestDto request, string userId, CancellationToken cancellationToken = default)
        {
            if (request.ResponseType != "code")
            {
                throw new ArgumentException("Only 'code' response type is supported");
            }

            if (request.CodeChallengeMethod != "S256")
            {
                throw new ArgumentException("Only S256 code challenge method is supported");
            }

            var user = await userManager.FindByIdAsync(userId)
                ?? throw new UnauthorizedAccessException("User not found");

            var code = tokenService.GenerateAuthorizationCode();
            var authCode = new AuthorizationCode(
                user,
                code,
                request.CodeChallenge,
                request.RedirectUri,
                request.Nonce
            );

            await authorizationCodeRepository.AddAsync(authCode, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthorizeResponseDto(request.RedirectUri, code, request.State);
        }

        public async Task RevokeAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var storedToken = await refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);
            if (storedToken is null)
            {
                return;
            }

            storedToken.Revoke();
            refreshTokenRepository.Update(storedToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<AuthResponseDto> ExternalCallbackAsync(string loginProvider, string providerKey, string? email, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByLoginAsync(loginProvider, providerKey);

            if (user is null)
            {
                user = await userManager.FindByEmailAsync(email!);

                if (user is null)
                {
                    user = new AppUser(email, email, email);
                    await userManager.CreateAsync(user);
                }

                await userManager.AddLoginAsync(user, new UserLoginInfo(loginProvider, providerKey, loginProvider));
            }

            var accessToken = tokenService.GenerateAccessToken(user);
            var idToken = tokenService.GenerateIdToken(user);
            var refreshTokenString = tokenService.GenerateRefreshToken();

            var refreshToken = new RefreshToken(user, refreshTokenString, DateTime.UtcNow.AddDays(7), "external");
            await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthResponseDto(accessToken, "Bearer", 900, refreshTokenString, idToken, "openid profile email");
        }
    }
}
