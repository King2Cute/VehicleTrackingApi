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
        public Location Location { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((VehicleLocation)obj);
        }

        public bool Equals(VehicleLocation other)
        {
            return other != null &&
                EqualityComparer<Guid?>.Default.Equals(Id, other.Id) &&
                EqualityComparer<Guid?>.Default.Equals(VehicleId, other.VehicleId) &&
                EqualityComparer<DateTime?>.Default.Equals(Time, other.Time) &&
                Location == other.Location;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 17;
                if (Id != null)
                    hashCode = hashCode * 23 + Id.GetHashCode();
                if (VehicleId != null)
                    hashCode = hashCode * 23 + VehicleId.GetHashCode();
                if (Time != null)
                    hashCode = hashCode * 23 + Time.GetHashCode();
                if (Location != null)
                    hashCode = hashCode * 23 + Location.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(VehicleLocation left, VehicleLocation right)
        {
            return EqualityComparer<VehicleLocation>.Default.Equals(left, right);
        }

        public static bool operator !=(VehicleLocation left, VehicleLocation right)
        {
            return !(left == right);
        }

    }
}
