using MQTTnet.Client;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MQTTnet.Protocol;
using Telemetry.Service.Contracts;

namespace Telemetry.Service.Services
{
    public class MqttClientPublish : IMqttClientPublish
    {
        IMqttClient _mqttClient;
        private readonly IConfiguration _configuration;

        public MqttClientPublish(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task PublishMessage(string message)
        {
            var mqttFactory = new MqttFactory();
            _mqttClient = mqttFactory.CreateMqttClient();
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(_configuration["_BROKER"], 1883)
                .WithClientId(_configuration["_CLIENTID"])
                .WithCleanSession(true)
                .WithCredentials(_configuration["_USERNAME"], _configuration["_PASSWORD"])
                .Build();

            using (var timeout = new CancellationTokenSource(5000))
            {
                await _mqttClient.ConnectAsync(mqttClientOptions, timeout.Token);
            }

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic($"telemetry/home/led")
                .WithPayload(message)
                .Build();

            await _mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

            await _mqttClient.DisconnectAsync();

            Console.WriteLine("MQTT application message is published.");
        }
    }
}
