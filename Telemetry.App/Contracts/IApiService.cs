using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.App.Models;

namespace Telemetry.App.Contracts
{
    public interface IApiService
    {
        Task<Measurement> GetLatestMeasurement();
        Task<ObservableCollection<Measurement>> GetAllMeasurements();
        Task<ObservableCollection<Measurement>> GetMeasurementsLatestDay();
        Task<ObservableCollection<Measurement>> GetMeasurementsLatestHour();
        Task<ObservableCollection<Measurement>> GetMeasurementsLatestWeek();
    }
}
