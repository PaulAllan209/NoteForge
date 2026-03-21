using Microsoft.Extensions.DependencyInjection;
using NoteForge.Application.Auth.Interfaces;
using NoteForge.Application.Auth.Services;
using NoteForge.Application.MarkdownFile.Interfaces;
using NoteForge.Application.MarkdownFile.Services;

namespace NoteForge.Application.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMarkdownFileService, MarkdownFileService>();

            return services;
        }
    }
}
