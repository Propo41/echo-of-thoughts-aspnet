using Cefalo.EchoOfThoughts.Domain.Repositories;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;

namespace Cefalo.EchoOfThoughts.WebApi {
    public static class RepositoryExtensions {

        public static IServiceCollection RegisterRepositories(this IServiceCollection services) {
            services.AddScoped<IStoryRepository, StoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
