using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using Telemetry.App.Contracts;
using Telemetry.App.Models;

namespace Telemetry.App.ViewModels
{
    public partial class ChartViewModel : ObservableObject
    {
        private readonly IApiService _apiService;

        public ChartViewModel(IApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private ISeries[] _series =
        {
            new LineSeries<double> 
            {
                Name = "Humidity",
                Values = new double[] { },
                Stroke = new SolidColorPaint(SKColors.DarkSlateBlue) { StrokeThickness = 4 },
                Fill = null,
                GeometryFill = null,
                GeometryStroke = null,
                LineSmoothness = 1,
                TooltipLabelFormatter = (chartPoint) => $"{chartPoint.Context.Series.Name}: {chartPoint.PrimaryValue}"
            },

            new LineSeries<double>
            {
                Name = "Temperature",
                Values = new double [] { },
                Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 4 },
                Fill = null,
                GeometryFill = null,
                GeometryStroke = null,
                LineSmoothness = 1,
                TooltipLabelFormatter = (chartPoint) => $"{chartPoint.Context.Series.Name}: {chartPoint.PrimaryValue}"
            }
        };

        [ObservableProperty]
        private Axis[] _yAxis = new Axis[]
        {
            new Axis
            {
                Name = "Reading values",
                NamePaint = new SolidColorPaint(SKColors.Black),
                NameTextSize = 40,
                LabelsPaint = new SolidColorPaint(SKColors.Coral),
                TextSize= 35
            }
        };

        [ObservableProperty]
        private Axis[] _xAxis = new Axis[]
        {
            new Axis
            {
                Name = "Time stamps - in days",
                NamePaint = new SolidColorPaint(SKColors.Black),
                NameTextSize = 40,
                LabelsPaint = new SolidColorPaint(SKColors.Coral),
                TextSize= 35
            }
        };

        [ObservableProperty]
        private ObservableCollection<Measurement> measurements;

        [ObservableProperty]
        private string[] _locations = new string[] { "All", "Kitchen", "Living room" };

        [ObservableProperty]
        private bool _isVisible = true;

        [ObservableProperty]
        private int _selectedIndex;

        public SolidColorPaint LegendTextPaint { get; set; } = new SolidColorPaint
        {
            Color = SKColors.Black  
        };

        [RelayCommand]
        public async Task LatestHour()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) { return; }

            Measurements = await _apiService.GetMeasurementsLatestHour();
            double[] lastHumidity = Measurements.Select(x => x.Humidity).ToArray();
            Series[0].Values = lastHumidity;

            double[] lastTemperature = Measurements.Select(x => x.Temperature).ToArray();
            Series[1].Values = lastTemperature;

            string[] labels = Measurements.Select(x => x.Time.ToLocalTime().ToString("HH:mm")).ToArray();

            XAxis = new Axis[]
            {
                new Axis
                {
                    Name = "Time stamps",
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    NameTextSize = 40,
                    Labels = labels,
                    LabelsRotation = 60,
                    LabelsPaint = new SolidColorPaint(SKColors.Coral),
                    TextSize= 35
                }
            };
        }

        [RelayCommand]
        public async Task LatestDay()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) { return; }

            Measurements = await _apiService.GetMeasurementsLatestDay();
            double[] lastHumidity = Measurements.Select(x => x.Humidity).ToArray();
            Series[0].Values = lastHumidity;

            double[] lastTemperature = Measurements.Select(x => x.Temperature).ToArray();
            Series[1].Values = lastTemperature;

            string[] labels = Measurements.Select(x => x.Time.ToLocalTime().ToString("HH:mm")).ToArray();

            XAxis = new Axis[]
            {
                new Axis
                {
                    Name = "Time stamps",
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    NameTextSize = 40,
                    Labels = labels,
                    LabelsRotation = 60,
                    LabelsPaint = new SolidColorPaint(SKColors.Coral),
                    TextSize= 35
                }
            };
        }

        [RelayCommand]
        public async Task LatestWeek()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) { return; }

            Measurements = await _apiService.GetMeasurementsLatestWeek();
            double[] lastHumidity = Measurements.Select(x => x.Humidity).ToArray();
            Series[0].Values = lastHumidity;

            double[] lastTemperature = Measurements.Select(x => x.Temperature).ToArray();
            Series[1].Values = lastTemperature;

            string[] labels = Measurements.Select(x => x.Time.ToLocalTime().DayOfWeek.ToString()).ToArray();

            XAxis = new Axis[]
            {
                new Axis
                {
                    Name = "Time stamps",
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    NameTextSize = 40,
                    Labels = labels,
                    LabelsRotation = 60,
                    LabelsPaint = new SolidColorPaint(SKColors.Coral),
                    TextSize= 35
                }
            };
        }

        [RelayCommand]
        public async Task AllMeasurements()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) { return; }

            Measurements = await _apiService.GetAllMeasurements();
            double[] lastHumidity = Measurements.Select(x => x.Humidity).ToArray();
            Series[0].Values = lastHumidity;

            double[] lastTemperature = Measurements.Select(x => x.Temperature).ToArray();
            Series[1].Values = lastTemperature;

            string[] labels = Measurements.Select(x => x.Time.ToLocalTime().DayOfWeek.ToString()).ToArray();

            XAxis = new Axis[]
            {
                new Axis
                {
                    Name = "Time stamps",
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    NameTextSize = 40,
                    Labels = labels,
                    LabelsRotation = 60,
                    LabelsPaint = new SolidColorPaint(SKColors.Coral),
                    TextSize= 35
                }
            };
        }

        async partial void OnSelectedIndexChanged(int value)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) { return; }

            if (value == 0)
            {                
                Measurements = await _apiService.GetAllMeasurements();
                double[] roomHumidity = Measurements.Select(x => x.Humidity).ToArray();
                Series[0].Values = roomHumidity;

                double[] roomTemperature = Measurements.Select(x => x.Temperature).ToArray();
                Series[1].Values = roomTemperature;
                IsVisible = true;
            }
            else if(value == 1)
            {
                Measurements = await _apiService.GetKitchenMeasurements();
                double[] roomHumidity = Measurements.Select(x => x.Humidity).ToArray();
                Series[0].Values = roomHumidity;

                double[] roomTemperature = measurements.Select(x => x.Temperature).ToArray();
                Series[1].Values = roomTemperature;
                IsVisible = false;
            }
            else if( value == 2)
            {
                Measurements = await _apiService.GetLivingRoomMeasurements();
                double[] roomHumidity = Measurements.Select(x => x.Humidity).ToArray();
                Series[0].Values = roomHumidity;

                double[] roomTemperature = Measurements.Select(x => x.Temperature).ToArray();
                Series[1].Values = roomTemperature;
                IsVisible = false;
            }

            string[] labels = Measurements.Select(x => x.Time.ToLocalTime().DayOfWeek.ToString()).ToArray();

            XAxis = new Axis[]
            {
                new Axis
                {
                    Name = "Time stamps",
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    NameTextSize = 40,
                    Labels = labels,
                    LabelsRotation = 60,
                    LabelsPaint = new SolidColorPaint(SKColors.Coral),
                    TextSize= 35
                }
            };
        }
    }
}
