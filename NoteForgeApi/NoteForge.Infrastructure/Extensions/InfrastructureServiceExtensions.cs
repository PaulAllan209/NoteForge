using Microsoft.Extensions.DependencyInjection;
using NoteForge.Domain.Interfaces;
using NoteForge.Infrastructure.Services;

namespace NoteForge.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IOAuthStrategy, AuthorizationCodeStrategy>();
            services.AddScoped<IOAuthStrategy, PasswordStrategy>();
            services.AddScoped<IOAuthStrategy, RefreshTokenStrategy>();

            services.AddScoped<IRefreshTokenStrategy, AuthorizationCodeStrategy>();
            services.AddScoped<IRefreshTokenStrategy, PasswordStrategy>();

            return services;
        }
    }
}
