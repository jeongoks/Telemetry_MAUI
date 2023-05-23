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

        public async Task<Measurement> GetLatestMeasurement()
        {
            using var client = new InfluxDBClient(_configuration["_INFLUXDB:_URL"], _configuration["_INFLUXDB:_TOKEN"]);
            var queryApi = client.GetQueryApiSync();

            var query = InfluxDBQueryable<Measurement>.Queryable(_configuration["_INFLUXDB:_BUCKET"], _configuration["_INFLUXDB:_ORGANIZATION"], queryApi)
                        .ToList()        
                        .OrderByDescending(i => i.Time)
                        .Take(1);

            Measurement measurement = query.FirstOrDefault(); 

            return measurement;
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

        public async Task<List<Measurement>> GetMeasurementsLatestDay()
        {
            using var client = new InfluxDBClient(_configuration["_INFLUXDB:_URL"], _configuration["_INFLUXDB:_TOKEN"]);
            var queryApi = client.GetQueryApiSync();

            var query = InfluxDBQueryable<Measurement>.Queryable(_configuration["_INFLUXDB:_BUCKET"], _configuration["_INFLUXDB:_ORGANIZATION"], queryApi)
                        .OrderByDescending(x => x.Time)
                        .ToList();

            List<Measurement> measurements = query.Where(i => i.Time.AddHours(2) > DateTime.Now.AddHours(-24)).ToList();

            return measurements;
        }

        public async Task<List<Measurement>> GetLivingRoomMeasurements()
        {
            using var client = new InfluxDBClient(_configuration["_INFLUXDB:_URL"], _configuration["_INFLUXDB:_TOKEN"]);
            var queryApi = client.GetQueryApiSync();

            var query = InfluxDBQueryable<Measurement>.Queryable(_configuration["_INFLUXDB:_BUCKET"], _configuration["_INFLUXDB:_ORGANIZATION"], queryApi)
                        .OrderByDescending(x => x.Time)
                        .ToList();

            List<Measurement> measurements = query.Where(i => i.Location == "living-room").ToList();

            return measurements;
        }

        public async Task<List<Measurement>> GetKitchenMeasurements()
        {
            using var client = new InfluxDBClient(_configuration["_INFLUXDB:_URL"], _configuration["_INFLUXDB:_TOKEN"]);
            var queryApi = client.GetQueryApiSync();

            var query = InfluxDBQueryable<Measurement>.Queryable(_configuration["_INFLUXDB:_BUCKET"], _configuration["_INFLUXDB:_ORGANIZATION"], queryApi)
                        .OrderByDescending(x => x.Time)
                        .ToList();

            List<Measurement> measurements = query.Where(i => i.Location == "kitchen").ToList();

            return measurements;
        }

        public async Task<List<Measurement>> GetMeasurementsLatestHour()
        {
            using var client = new InfluxDBClient(_configuration["_INFLUXDB:_URL"], _configuration["_INFLUXDB:_TOKEN"]);
            var queryApi = client.GetQueryApiSync();

            var query = InfluxDBQueryable<Measurement>.Queryable(_configuration["_INFLUXDB:_BUCKET"], _configuration["_INFLUXDB:_ORGANIZATION"], queryApi)
                        .OrderByDescending(x => x.Time)                          
                        .ToList();

            List<Measurement> measurements = query.Where(i => i.Time.AddHours(2) > DateTime.Now.AddMinutes(-60)).ToList();

            return measurements;
        }

        public async Task<List<Measurement>> GetMeasurementsLatestWeek()
        {
            using var client = new InfluxDBClient(_configuration["_INFLUXDB:_URL"], _configuration["_INFLUXDB:_TOKEN"]);
            var queryApi = client.GetQueryApiSync();

            var query = InfluxDBQueryable<Measurement>.Queryable(_configuration["_INFLUXDB:_BUCKET"], _configuration["_INFLUXDB:_ORGANIZATION"], queryApi)
                        .OrderByDescending(x => x.Time)
                        .ToList();

            List<Measurement> measurements = query.Where(i => i.Time.AddHours(2) > DateTime.Now.AddDays(-7)).ToList();

            return measurements;
        }

        public async Task WriteToDB(Measurement measurement, string topic)
        {
            string location = topic.Split("/")[2];

            using var client = new InfluxDBClient(_configuration["_INFLUXDB:_URL"], _configuration["_INFLUXDB:_TOKEN"]);            

            // Write Data
            var writeApi = client.GetWriteApiAsync();

            // Write by LineProtocol
            await writeApi.WriteRecordAsync($"{_configuration["_INFLUXDB:_MEASUREMENT"]},Location={location} Temperature={measurement.Temperature.ToString("F", new CultureInfo("en-US"))},Humidity={measurement.Humidity.ToString("F", new CultureInfo("en-US"))}", WritePrecision.Ns, _configuration["_INFLUXDB:_BUCKET"], _configuration["_INFLUXDB:_ORGANIZATION"]);
        }
    }
}
