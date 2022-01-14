using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using VehicleTracking.Models.Contracts;

namespace VehicleTracking.Models.Vehicles
{
    public class Vehicle : IEquatable<Vehicle>, IEntity
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public Guid? DeviceId { get; set; }
        public string Registration { get; set; }
        public VehicleStats VehicleStats { get; set; }

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
                EqualityComparer<Guid?>.Default.Equals(DeviceId, other.DeviceId) &&
                Registration == other.Registration &&
                EqualityComparer<VehicleStats>.Default.Equals(VehicleStats, other.VehicleStats);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 17;
                if (Id != null)
                    hashCode = hashCode * 23 + Id.GetHashCode();
                if (DeviceId != null)
                    hashCode = hashCode * 23 + DeviceId.GetHashCode();
                if (Registration != null)
                    hashCode = hashCode * 23 + Registration.GetHashCode();
                if (VehicleStats != null)
                    hashCode = hashCode * 23 + VehicleStats.GetHashCode();
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
