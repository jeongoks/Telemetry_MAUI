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
        Task WriteToDB(Measurement measurements, string topic);
    }
}
