using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VehicleTracking.Core.Persistence
{
    public static class ServiceRegistration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ContextOptions>(configuration.GetSection(ContextOptions.Section));

            services.AddSingleton<IMongoContext, MongoContext>();
            services.AddSingleton<typeof(IAsyncRepositoru)>

        }
    }
}
