using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Cefalo.EchoOfThoughts.AppCore.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Cefalo.EchoOfThoughts.AppCore {
    public static class ServiceExtensions {
        public static IServiceCollection RegisterServices(this IServiceCollection services) {
            services.AddScoped<IStoryService, StoryService>();

            return services;
        }
    }
}
