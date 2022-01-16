using System;
using System.Text.Json.Serialization;

namespace VehicleTracking.Models
{
    public class BaseLocation : Location
    {
        public DateTime Time { get; set; }
        [JsonIgnore]
        public string Name { get; set; }
    }
}
