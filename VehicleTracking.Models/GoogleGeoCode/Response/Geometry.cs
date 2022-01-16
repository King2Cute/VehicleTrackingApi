using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleTracking.Models.GoogleGeoCode
{
    public class Geometry
    {
        public Location Location { get; set; }
        public string LocationType { get; set; }
        public Viewport Viewport { get; set; }
    }
}
