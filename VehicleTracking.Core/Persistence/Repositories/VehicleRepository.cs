using VehicleTracking.Models.Vehicles;

namespace VehicleTracking.Core.Persistence.Repositories
{
    public class VehicleRepository : BaseRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(IMongoContext context) : base(context)
        {

        }
    }
}
