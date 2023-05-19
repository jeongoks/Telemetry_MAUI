using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Linq;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Service.Contracts;
using Telemetry.Service.Models;

namespace Telemetry.Service.Services
{
    public class InfluxDBService : IInfluxDBService
    {
        private readonly IConfiguration _configuration;

        public InfluxDBService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Measurement>> GetMeasurements()
        {
            using var client = new InfluxDBClient(_configuration["_INFLUXDB:_URL"], _configuration["_INFLUXDB:_TOKEN"]);
            var queryApi = client.GetQueryApiSync();

            var query = from s in InfluxDBQueryable<Measurement>.Queryable(_configuration["_INFLUXDB:_BUCKET"], _configuration["_INFLUXDB:_ORGANIZATION"], queryApi)
                        select s;

            List<Measurement> measurements = query.ToList();

            return measurements;
        }

        public async Task WriteToDB(Measurement measurement)
        {
            using var client = new InfluxDBClient(_configuration["_INFLUXDB:_URL"], _configuration["_INFLUXDB:_TOKEN"]);

            // Write Data
            var writeApi = client.GetWriteApiAsync();

            // Write by LineProtocol
            await writeApi.WriteRecordAsync($"{_configuration["_INFLUXDB:_MEASUREMENT"]},Sensor={_configuration["_INFLUXDB:_TAG"]} Temperature={measurement.Temperature.ToString("F", new CultureInfo("en-US"))},Humidity={measurement.Humidity.ToString("F", new CultureInfo("en-US"))},TimeStamp={measurement.TimeStamp}", WritePrecision.Ns, _configuration["_INFLUXDB:_BUCKET"], _configuration["_INFLUXDB:_ORGANIZATION"]);
        }
    }
}
