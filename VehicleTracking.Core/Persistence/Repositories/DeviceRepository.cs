using VehicleTracking.Models.Devices;

namespace VehicleTracking.Core.Persistence.Repositories
{
    public class DeviceRepository : BaseRepository<Device>, IDeviceRepository
    {
        public DeviceRepository(IMongoContext context) : base(context)
        {

        }
    }
}
