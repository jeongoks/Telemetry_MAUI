using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Service.Models;

namespace Telemetry.Service.Contracts
{
    public interface IApiService
    {
        Task<Measurement> GetLatestMeasurement();
        Task<List<Measurement>> GetAllMeasurements();
        Task<bool> TurnOnLedAsync(string isToggled);
        Task<List<Measurement>> GetMeasurementsLatestDay();
        Task<List<Measurement>> GetMeasurementsLatestHour();
        Task<List<Measurement>> GetMeasurementsLatestWeek();
        Task<List<Measurement>> GetLivingRoomMeasurements();
        Task<List<Measurement>> GetKitchenMeasurements();
    }
}
