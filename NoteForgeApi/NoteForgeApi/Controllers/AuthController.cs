using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteForge.Application.Auth.Dtos;
using NoteForge.Application.Auth.Interfaces;
using NoteForge.Domain;
using NoteForge.Domain.Dtos;
using System.Security.Claims;

namespace NoteForgeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly SignInManager<AppUser> signInManager;

        public AuthController(IAuthService authService, SignInManager<AppUser> signInManager)
        {
            this.authService = authService;
            this.signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request, CancellationToken cancellationToken)
        {
            var response = await authService.RegisterAsync(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost("token")]
        public async Task<ActionResult<AuthResponseDto>> Token([FromBody] TokenGrantRequest request, CancellationToken cancellationToken)
        {
            var response = await authService.TokenAsync(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RefreshTokenRequestDto request, CancellationToken cancellationToken)
        {
            await authService.RevokeAsync(request.RefreshToken, cancellationToken);
            return Ok();
        }

        [HttpGet("external/{provider}")]
        public IActionResult ExternalLogin(string provider, [FromQuery] string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet("external/callback")]
        public async Task<IActionResult> ExternalLoginCallback(CancellationToken cancellationToken, [FromQuery] string? returnUrl = null)
        {
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return BadRequest(new { error = "external_login_failure" });
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var response = await authService.ExternalCallbackAsync(info.LoginProvider, info.ProviderKey, email, cancellationToken);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect($"{returnUrl}?access_token={response.AccessToken}&refresh_token={response.RefreshToken}");
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet("userinfo")]
        public IActionResult UserInfo()
        {
            return Ok(new
            {
                sub = User.FindFirstValue(ClaimTypes.NameIdentifier),
                email = User.FindFirstValue(ClaimTypes.Email),
                username = User.FindFirstValue(ClaimTypes.Name),
                email_verified = true
            });
        }
    }
}
