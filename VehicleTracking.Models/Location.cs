using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTracking.Models
{
    public class Location
    {
        public DateTime Time { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
    }
}
