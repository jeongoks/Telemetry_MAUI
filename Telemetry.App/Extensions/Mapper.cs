using Telemetry.App.Models;
using Telemetry.Service.Models;

namespace Telemetry.App.Extensions
{
    public static class Mapper
    {
        public static MeasurementDTO ToDTO(this Measurement measurement)
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
