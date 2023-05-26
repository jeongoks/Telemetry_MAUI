using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Telemetry.App.Extensions;
using Telemetry.App.ViewModels;
using Telemetry.App.Views;
using Telemetry.Service.Contracts;
using Telemetry.Service.Services;

namespace Telemetry.App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseSkiaSharp(true)
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<GraphPage>();
		builder.Services.AddSingleton<IApiService, ApiService>();

		builder.Services.AddTransient<ChartViewModel>();
		builder.Services.AddTransient<OverviewViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
