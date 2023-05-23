﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.App.Models
{
    public class Measurement
    {
        public string Location { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }        
        public DateTime Time { get; set; }
    }
}
