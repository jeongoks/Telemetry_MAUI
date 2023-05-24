using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Service.Models;

namespace Telemetry.Service.Contracts
{
    public interface IInfluxDBService
    {
        Task<List<Measurement>> GetMeasurements();
        Task<Measurement> GetLatestMeasurement();
        Task<List<Measurement>> GetMeasurementsLatestHour();
        Task<List<Measurement>> GetMeasurementsLatestDay();
        Task<List<Measurement>> GetMeasurementsLatestWeek();
        Task<List<Measurement>> GetLivingRoomMeasurements();
        Task<List<Measurement>> GetKitchenMeasurements();
        Task WriteToDB(Measurement measurements, string topic);
    }
}
