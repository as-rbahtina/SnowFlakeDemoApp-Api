using Allied.SnowFlakeDemoApp.Domain.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Allied.Configurations;
using Allied.DI;

namespace Allied.SnowFlakeDemoApp.Domain.DI
{
    public static class SnowFlakeDemoAppContainer
    {
        public static IServiceCollection AddDomainContainer(IServiceCollection services)
        {
            services.AddScopedConfiguration<SnowFlakeDemoAppConfiguration>();

            return services;
        }
    }
}
