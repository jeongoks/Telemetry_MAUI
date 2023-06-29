using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Radzen;
using Telemetry.Service.Contracts;
using Telemetry.Service.Services;

var builder = WebApplication.CreateBuilder(args);

//builder.Services
//    .AddAuth0WebAppAuthentication(options => {
//        options.Domain = builder.Configuration["Auth0:Domain"];
//        options.ClientId = builder.Configuration["Auth0:ClientId"];
//    });

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<IApiService, ApiService>();
builder.Services.AddHttpClient("Telemetry_Web", client =>
{
    client.BaseAddress = new Uri("http://10.135.16.160:32678/");
});

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseCookiePolicy(new CookiePolicyOptions()
//{
//    MinimumSameSitePolicy = SameSiteMode.None
//});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

//app.UseAuthentication(); 
//app.UseAuthorization();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
