using VehicleTracking.Models.Contracts;

namespace VehicleTracking.Models.Vehicles
{
    public interface IVehicleRepository : IAsyncRepository<Vehicle>
    {
    }
}
