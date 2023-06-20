using Cefalo.EchoOfThoughts.AppCore.Services;
using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;

namespace Cefalo.EchoOfThoughts.WebApi {
    public static class ServiceExtensions {
        public static IServiceCollection RegisterServices(this IServiceCollection services) {
            services.AddScoped<IStoryService, StoryService>();

            return services;
        }
    }
}
