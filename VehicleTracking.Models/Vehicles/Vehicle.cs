using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VehicleTracking.Models.Contracts;

namespace VehicleTracking.Models.Vehicles
{
    public class Vehicle : IEquatable<Vehicle>, IEntity
    {
        public Guid? Id { get; set; }
        public string Registration { get; set; }
        public VehicleType VehicleType { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Vehicle)obj);
        }

        public bool Equals(Vehicle other)
        {
            return other != null &&
                EqualityComparer<Guid?>.Default.Equals(Id, other.Id) &&
                Registration == other.Registration &&
                EqualityComparer<VehicleType>.Default.Equals(VehicleType, other.VehicleType);
        }

        public override int GetHashCode()
        {
           unchecked
            {
                var hashCode = 17;
                if (Id != null)
                    hashCode = hashCode * 23 + Id.GetHashCode();
                if (Registration != null)
                    hashCode = hashCode * 23 + Registration.GetHashCode();
                if (VehicleType != null)
                    hashCode = hashCode * 23 + VehicleType.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Vehicle left, Vehicle right)
        {
            return EqualityComparer<Vehicle>.Default.Equals(left, right);
        }

        public static bool operator !=(Vehicle left, Vehicle right)
        {
            return !(left == right);
        }
    }
}
