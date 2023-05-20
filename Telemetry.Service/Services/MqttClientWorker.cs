using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Telemetry.Service.Contracts;
using Telemetry.Service.Models;

namespace Telemetry.Service.Services
{
    public class MqttClientWorker : BackgroundService
    {
        private readonly IInfluxDBService _influxDBService;
        private readonly IConfiguration _configuration;
        IMqttClient _mqttClient;

        public MqttClientWorker(IInfluxDBService influxDBService, IConfiguration configuration)
        {
            _influxDBService = influxDBService;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var mqttFactory = new MqttFactory();
            _mqttClient = mqttFactory.CreateMqttClient();
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(_configuration["_BROKER"], 8883)
                .WithTls()
                .WithClientId(_configuration["_CLIENTID"])
                .WithCleanSession(false)
                .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .WithCredentials(_configuration["_USERNAME"], _configuration["_PASSWORD"])
                .Build();

            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                string message = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                Measurement measurement = JsonSerializer.Deserialize<Measurement>(message);
                await _influxDBService.WriteToDB(measurement, e.ApplicationMessage.Topic);
            };

            await _mqttClient.ConnectAsync(mqttClientOptions, stoppingToken);

            var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(
                f =>
                {
                    f.WithTopic("telemetry/home/#");
                })
                .Build();

            await _mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
        }
    }
}
