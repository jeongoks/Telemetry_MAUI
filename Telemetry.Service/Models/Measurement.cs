using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Service.Models
{
    public class Measurement
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
