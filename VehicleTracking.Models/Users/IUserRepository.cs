using System;
using System.Collections.Generic;
using System.Text;
using VehicleTracking.Models.Contracts;

namespace VehicleTracking.Models.Users
{
    public interface IUserRepository : IAsyncRepository<User>
    {

    }
}
