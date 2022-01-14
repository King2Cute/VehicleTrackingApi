using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleTracking.Models.Requests
{
    public class LocationUpdateRequest
    {
        public Guid VehicleId { get; set; }
        public Guid DeviceId { get; set; }
        public Location Location { get; set; }
    }
}
