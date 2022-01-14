using VehicleTracking.Models.UserDevices;

namespace VehicleTracking.Core.Persistence.Repositories
{
    public class UserDeviceRepository : BaseRepository<UserDevice>, IUserDeviceRepository
    {
        public UserDeviceRepository(IMongoContext context) : base(context)
        {

        }
    }
}
