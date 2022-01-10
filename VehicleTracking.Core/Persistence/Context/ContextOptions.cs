namespace VehicleTracking.Core.Persistence
{
    public class ContextOptions
    {
        public string DatabaseName { get; set; }
        public string VehicleTrackingDb { get; set; }

        public static readonly string Section = "ConnectionStrings";
    }
}
