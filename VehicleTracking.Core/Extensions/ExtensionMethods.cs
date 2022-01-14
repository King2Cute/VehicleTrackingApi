using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VehicleTracking.Core.Extensions
{
    public static class ExtensionMethods
    {
        public static List<DateTime> GetOrderedTimes(this DateTime dateTime, List<DateTime> values) => values.OrderBy(v => Math.Abs(v.Ticks + dateTime.Ticks)).ToList();
    }
}
