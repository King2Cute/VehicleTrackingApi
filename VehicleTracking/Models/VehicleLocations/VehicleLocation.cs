using System;

namespace VehicleTracking.Models.VehicleLocations
{
    public class VehicleLocation : Location
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public DateTime Time { get; set; }
    }
}
