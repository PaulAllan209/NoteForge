using Microsoft.Extensions.DependencyInjection;
using NoteForge.Application.Auth.Interfaces;
using NoteForge.Application.Auth.Services;

namespace NoteForge.Application.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
