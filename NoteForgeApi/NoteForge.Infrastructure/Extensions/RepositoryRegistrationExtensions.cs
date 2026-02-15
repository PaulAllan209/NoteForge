using Microsoft.Extensions.DependencyInjection;
using NoteForge.Domain.Interfaces.Repositories;
using NoteForge.Infrastructure.Repositories;

namespace NoteForge.Infrastructure.Extensions
{
    public static class RepositoryRegistrationExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBlobStorageRepository, LocalBlobStorageRepository>();

            return services;
        }
    }
}
