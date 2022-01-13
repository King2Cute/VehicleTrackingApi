using System;
using System.Collections.Generic;
using System.Text;
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
