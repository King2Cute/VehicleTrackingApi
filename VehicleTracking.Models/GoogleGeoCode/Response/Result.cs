using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleTracking.Models.GoogleGeoCode
{
    public class Result
    {
        public AddressComponent[] Address_Components { get; set; }
        public string Formatted_Address { get; set; }
        public Geometry Geometry { get; set; }
        public string Place_Id { get; set; }
        public string[] Types { get; set; }
    }
}
