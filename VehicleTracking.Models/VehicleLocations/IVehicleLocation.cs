using System;
using System.Collections.Generic;
using System.Text;
using VehicleTracking.Models.Contracts;

namespace VehicleTracking.Models.VehicleLocations
{
    public interface IVehicleLocation : IAsyncRepository<VehicleLocation>
    {

    }
}
