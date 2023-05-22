using Telemetry.Service.Contracts;
using Telemetry.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<MqttClientWorker>();
builder.Services.AddSingleton<IInfluxDBService, InfluxDBService>();
builder.Services.AddSingleton<IMqttClientPublish, MqttClientPublish>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/telemetries", async (IInfluxDBService influxDBService) =>
{
    return await influxDBService.GetMeasurements();
});

app.MapGet("/latestTelemetry", async (IInfluxDBService influxDBService) =>
{
    return await influxDBService.GetLatestMeasurement();
});

app.MapGet("/telemetry/lastHour", async (IInfluxDBService influxDBService) =>
{
    return await influxDBService.GetMeasurementsLatestHour();
});

app.MapGet("/telemetry/lastDay", async (IInfluxDBService influxDBService) =>
{
    return await influxDBService.GetMeasurementsLatestDay();
});

app.MapGet("/telemetry/lastWeek", async (IInfluxDBService influxDBService) =>
{
    return await influxDBService.GetMeasurementsLatestWeek();
});

app.MapPost("/servo", async (IMqttClientPublish publishClient, string message, string location) =>
{
    await publishClient.PublishMessage(message, location);
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
