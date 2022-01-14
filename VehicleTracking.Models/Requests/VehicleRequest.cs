using VehicleTracking.Models.VehicleLocations;
using VehicleTracking.Models.Vehicles;

namespace VehicleTracking.Models.Requests
{
    public class VehicleRequest
    {
        public Vehicle Vehicle { get; set; }
        public VehicleLocation VehicleLocation { get; set; }
    }
}
