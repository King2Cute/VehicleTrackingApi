using VehicleTracking.Models.Users;

namespace VehicleTracking.Core.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IMongoContext context) : base(context)
        {

        }
    }
}
