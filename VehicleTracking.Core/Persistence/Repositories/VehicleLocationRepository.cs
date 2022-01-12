using VehicleTracking.Models.VehicleLocations;

namespace VehicleTracking.Core.Persistence.Repositories
{
    public class VehicleLocationRepository : BaseRepository<VehicleLocation>, IVehicleLocationRepository
    {
        public VehicleLocationRepository(IMongoContext context) : base(context)
        {

        }
    }
}
