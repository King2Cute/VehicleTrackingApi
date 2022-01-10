using System;

namespace VehicleTracking.Models.Vehicles
{
    public class Vehicle
    {
        public Guid? Id { get; set; }
        public string Registration { get; set; }
        public VehicleType VehicleType { get; set; }
    }
}
