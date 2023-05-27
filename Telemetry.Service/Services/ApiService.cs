using Polly;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using Telemetry.Service.Contracts;
using Telemetry.Service.Models;

namespace Telemetry.Service.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            };
        }

        public async Task<Measurement> GetLatestMeasurement()
        {
            Uri uri = new Uri(string.Format(Constants.RestUrl, "latestTelemetry"));

            Measurement measurement = new Measurement();
            try
            {
                HttpResponseMessage response = await Policy.HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
                    .RetryAsync(10)
                    .ExecuteAsync(async () => await _httpClient.GetAsync(uri));

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    measurement = JsonSerializer.Deserialize<Measurement>(content, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                throw;
            }
            return measurement;
        }

        public async Task<List<Measurement>> GetAllMeasurements()
        {
            Uri uri = new Uri(string.Format(Constants.RestUrl, "telemetries"));

            List<Measurement> measurements = new List<Measurement>();
            try
            {
                HttpResponseMessage response = await Policy.HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
                    .RetryAsync(10)
                    .ExecuteAsync(async () => await _httpClient.GetAsync(uri));

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    measurements = JsonSerializer.Deserialize<List<Measurement>>(content, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                throw;
            }
            return measurements;
        }

        public async Task<List<Measurement>> GetMeasurementsLatestDay()
        {
            Uri uri = new Uri(string.Format(Constants.RestUrl, "telemetry/lastDay"));

            List<Measurement> measurements = new List<Measurement>();
            try
            {
                HttpResponseMessage response = await Policy.HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
                    .RetryAsync(10)
                    .ExecuteAsync(async () => await _httpClient.GetAsync(uri));

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    measurements = JsonSerializer.Deserialize<List<Measurement>>(content, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                throw;
            }
            return measurements;
        }

        public async Task<List<Measurement>> GetMeasurementsLatestHour()
        {
            Uri uri = new Uri(string.Format(Constants.RestUrl, "telemetry/lastHour"));

            List<Measurement> measurements = new List<Measurement>();
            try
            {
                HttpResponseMessage response = await Policy.HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
                    .RetryAsync(10)
                    .ExecuteAsync(async () => await _httpClient.GetAsync(uri));

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    measurements = JsonSerializer.Deserialize<List<Measurement>>(content, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                throw;
            }
            return measurements;
        }

        public async Task<List<Measurement>> GetMeasurementsLatestWeek()
        {
            Uri uri = new Uri(string.Format(Constants.RestUrl, "telemetry/lastWeek"));

            List<Measurement> measurements = new List<Measurement>();
            try
            {
                HttpResponseMessage response = await Policy.HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
                    .RetryAsync(10)
                    .ExecuteAsync(async () => await _httpClient.GetAsync(uri));

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    measurements = JsonSerializer.Deserialize<List<Measurement>>(content, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                throw;
            }
            return measurements;
        }

        public async Task<bool> TurnOnLedAsync(string isToggled)
        {
            Uri uri = new Uri(string.Format(Constants.RestUrl, "servo"));
            try
            {
                HttpResponseMessage response = await Policy.HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
                    .RetryAsync(10)
                    .ExecuteAsync(async () => await _httpClient.PostAsJsonAsync(uri, isToggled));

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"Successfully sent to device!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                throw;
            }
            return false;
        }

        public async Task<List<Measurement>> GetLivingRoomMeasurements()
        {
            Uri uri = new Uri(string.Format(Constants.RestUrl, "telemetry/livingRoom"));

            List<Measurement> measurements = new List<Measurement>();
            try
            {
                HttpResponseMessage response = await Policy.HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
                    .RetryAsync(10)
                    .ExecuteAsync(async () => await _httpClient.GetAsync(uri));

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    measurements = JsonSerializer.Deserialize<List<Measurement>>(content, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                throw;
            }
            return measurements;
        }

        public async Task<List<Measurement>> GetKitchenMeasurements()
        {
            Uri uri = new Uri(string.Format(Constants.RestUrl, "telemetry/kitchen"));

            List<Measurement> measurements = new List<Measurement>();
            try
            {
                HttpResponseMessage response = await Policy.HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
                    .RetryAsync(10)
                    .ExecuteAsync(async () => await _httpClient.GetAsync(uri));

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    measurements = JsonSerializer.Deserialize<List<Measurement>>(content, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                throw;
            }
            return measurements;
        }
    }
}
