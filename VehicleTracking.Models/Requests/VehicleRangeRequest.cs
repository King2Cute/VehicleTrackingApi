using System;

namespace VehicleTracking.Models
{
    public class VehicleRangeRequest
    {
        public Guid VehicleId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
