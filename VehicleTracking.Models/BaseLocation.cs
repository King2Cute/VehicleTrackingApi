using System;

namespace VehicleTracking.Models
{
    public class BaseLocation : Location
    {
        public DateTime Time { get; set; }
        public string Name { get; set; }
    }
}
