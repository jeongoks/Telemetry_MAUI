using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Telemetry.API.Auth0;
using Telemetry.Service.Contracts;
using Telemetry.Service.Services;

var builder = WebApplication.CreateBuilder(args);

var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = domain;
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("read:telemetryData", policy => policy.Requirements.Add(new HasScopeRequirement("read:telemetryData", domain)));
    options.AddPolicy("write:LEDcontrol", policy => policy.Requirements.Add(new HasScopeRequirement("write:LEDcontrol", domain)));
});

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/telemetries", async (IInfluxDBService influxDBService) =>
{
    return await influxDBService.GetMeasurements();
}).RequireAuthorization();

app.MapGet("/latestTelemetry", async (IInfluxDBService influxDBService) =>
{
    return await influxDBService.GetLatestMeasurement();
}).RequireAuthorization("read:telemetryData");

app.MapGet("/telemetry/lastHour", async (IInfluxDBService influxDBService) =>
{
    return await influxDBService.GetMeasurementsLatestHour();
}).RequireAuthorization("read:telemetryData");

app.MapGet("/telemetry/lastDay", async (IInfluxDBService influxDBService) =>
{
    return await influxDBService.GetMeasurementsLatestDay();
}).RequireAuthorization("read:telemetryData");

app.MapGet("/telemetry/lastWeek", async (IInfluxDBService influxDBService) =>
{
    return await influxDBService.GetMeasurementsLatestWeek();
}).RequireAuthorization("read:telemetryData");

app.MapGet("/telemetry/livingRoom", async (IInfluxDBService influxDBService) =>
{
    return await influxDBService.GetLivingRoomMeasurements();
}).RequireAuthorization("read:telemetryData");

app.MapGet("/telemetry/kitchen", async (IInfluxDBService influxDBService) =>
{
    return await influxDBService.GetKitchenMeasurements();
}).RequireAuthorization("read:telemetryData");

app.MapPost("/servo", async (IMqttClientPublish publishClient, [FromBody] string message) =>
{
    await publishClient.PublishMessage(message);
}).RequireAuthorization("write:LEDcontrol");

app.Run();