namespace Telemetry.Web.Models
{
    public class MeasurementDTO
    {
        public string Location { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public DateTime Time { get; set; }
    }
}
