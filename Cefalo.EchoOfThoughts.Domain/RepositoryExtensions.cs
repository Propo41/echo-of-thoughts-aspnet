using Cefalo.EchoOfThoughts.Domain.Repositories;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Cefalo.EchoOfThoughts.Domain {
    public static class RepositoryExtensions {

        public static IServiceCollection RegisterRepositories(this IServiceCollection services) {
            services.AddScoped<IStoryRepository, StoryRepository>();

            return services;
        }


    }
}
