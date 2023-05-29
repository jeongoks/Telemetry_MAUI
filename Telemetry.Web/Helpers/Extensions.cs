using Telemetry.Service.Models;
using Telemetry.Web.Models;

namespace Telemetry.Web.Helpers
{
    public static class Extensions
    {
        public static MeasurementDTO MapToDTO(this Measurement measurement)
        {
            if (measurement != null)
            {
                return new MeasurementDTO
                {
                    Location = measurement.Location,
                    Temperature = measurement.Temperature,
                    Humidity = measurement.Humidity,
                    Time = measurement.Time,
                };
            }
            return null;
        }
    }
}
