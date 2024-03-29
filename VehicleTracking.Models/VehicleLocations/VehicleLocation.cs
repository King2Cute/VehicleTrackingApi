﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using VehicleTracking.Models.Contracts;

namespace VehicleTracking.Models.VehicleLocations
{
    public class VehicleLocation : IEquatable<VehicleLocation>, IEntity
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        [JsonIgnore]
        public Guid VehicleId { get; set; }
        public List<BaseLocation> Locations { get; set; }

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
                Locations == other.Locations;
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
                if (Locations != null)
                    hashCode = hashCode * 23 + Locations.GetHashCode();
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
