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
        Task<ObservableCollection<Measurement>> GetAllMeasurements();
        Task<bool> TurnOnLedAsync(string isToggled);
        Task<ObservableCollection<Measurement>> GetMeasurementsLatestDay();
        Task<ObservableCollection<Measurement>> GetMeasurementsLatestHour();
        Task<ObservableCollection<Measurement>> GetMeasurementsLatestWeek();
        Task<ObservableCollection<Measurement>> GetLivingRoomMeasurements();
        Task<ObservableCollection<Measurement>> GetKitchenMeasurements();
    }
}
