using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VehicleTracking.Core.Persistence.Repositories;
using VehicleTracking.Models.Contracts;
using VehicleTracking.Models.Users;
using VehicleTracking.Models.VehicleLocations;
using VehicleTracking.Models.Vehicles;

namespace VehicleTracking.Core.Persistence
{
    public static class ServiceRegistration
    {
        public static IServiceCollection ConfigureMongoServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ContextOptions>(configuration.GetSection(ContextOptions.Section));

            services.AddSingleton<IMongoContext, MongoContext>();
            services.AddSingleton(typeof(IAsyncRepository<>), typeof(BaseRepository<>));

            RegisterRepositories(services);

            services.AddSingleton<MongoDbService>();

            return services;
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IVehicleRepository, VehicleRepository>();
            services.AddSingleton<IVehicleLocationRepository, VehicleLocationRepository>();
            services.AddSingleton<IDeviceRepository, DeviceRepository>();
        }
    }
}
