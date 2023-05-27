using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Telemetry.App.Extensions;
using Telemetry.App.Models;
using Telemetry.Service.Contracts;
using Telemetry.Service.Models;

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
        private List<MeasurementDTO> measurements;

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

            List<Measurement> tempMeasurements = await _apiService.GetMeasurementsLatestHour();
            Measurements.Clear();
            foreach (var measurement in tempMeasurements)
            {
                Measurements.Add(measurement.ToDTO());
            }
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

            List<Measurement> tempMeasurements = await _apiService.GetMeasurementsLatestDay();
            Measurements.Clear();
            foreach (var measurement in tempMeasurements)
            {
                Measurements.Add(measurement.ToDTO());
            }
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

            List<Measurement> tempMeasurements = await _apiService.GetMeasurementsLatestWeek();
            Measurements.Clear();
            foreach (var measurement in tempMeasurements)
            {
                Measurements.Add(measurement.ToDTO());
            }
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
            try
            {
                List<Measurement> tempMeasurements = await _apiService.GetAllMeasurements();
                Measurements = new List<MeasurementDTO>();
                foreach (var measurement in tempMeasurements)
                {
                    Measurements.Add(measurement.ToDTO());
                }
                double[] lastHumidity = Measurements.Select(x => x.Humidity).ToArray();
                Series[0].Values = lastHumidity;

                double[] lastTemperature = Measurements.Select(x => x.Temperature).ToArray();
                Series[1].Values = lastTemperature;

                string[] labels = Measurements.Select(x => x.Time.ToLocalTime().DayOfWeek.ToString()).ToArray();
                IsVisible = true;

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
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex}");
            }            
        }

        async partial void OnSelectedIndexChanged(int value)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) { return; }

            if (value == 0)
            {
                List<Measurement> tempMeasurements = await _apiService.GetAllMeasurements();
                Measurements.Clear();
                foreach (var measurement in tempMeasurements)
                {
                    Measurements.Add(measurement.ToDTO());
                }
                double[] roomHumidity = Measurements.Select(x => x.Humidity).ToArray();
                Series[0].Values = roomHumidity;

                double[] roomTemperature = Measurements.Select(x => x.Temperature).ToArray();
                Series[1].Values = roomTemperature;
                IsVisible = true;
            }
            else if(value == 1)
            {
                List<Measurement> tempMeasurements = await _apiService.GetKitchenMeasurements();
                Measurements.Clear();
                foreach (var measurement in tempMeasurements)
                {
                    Measurements.Add(measurement.ToDTO());
                };
                double[] roomHumidity = Measurements.Select(x => x.Humidity).ToArray();
                Series[0].Values = roomHumidity;

                double[] roomTemperature = Measurements.Select(x => x.Temperature).ToArray();
                Series[1].Values = roomTemperature;
                IsVisible = false;
            }
            else if( value == 2)
            {
                List<Measurement> tempMeasurements = await _apiService.GetLivingRoomMeasurements();
                Measurements.Clear();
                foreach (var measurement in tempMeasurements)
                {
                    Measurements.Add(measurement.ToDTO());
                }
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
