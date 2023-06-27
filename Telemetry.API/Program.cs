using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Telemetry.API.Auth0;
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
builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

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

app.MapGet("/telemetry/livingRoom", async (IInfluxDBService influxDBService) =>
{
    return await influxDBService.GetLivingRoomMeasurements();
});

app.MapGet("/telemetry/kitchen", async (IInfluxDBService influxDBService) =>
{
    return await influxDBService.GetKitchenMeasurements();
});

app.MapPost("/servo", async (IMqttClientPublish publishClient, [FromBody] string message) =>
{
    await publishClient.PublishMessage(message);
});

app.Run();