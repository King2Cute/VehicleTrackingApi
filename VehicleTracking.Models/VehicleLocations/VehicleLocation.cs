using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using VehicleTracking.Models.Contracts;

namespace VehicleTracking.Models.VehicleLocations
{
    public class VehicleLocation : IEquatable<VehicleLocation>, IEntity
    {
        public Guid? Id { get; set; }
        public Guid VehicleId { get; set; }
        public DateTime Time { get; set; }
        public Location location { get; set; }

        public bool Equals([AllowNull] VehicleLocation other)
        {
            throw new NotImplementedException();
        }
    }
}
