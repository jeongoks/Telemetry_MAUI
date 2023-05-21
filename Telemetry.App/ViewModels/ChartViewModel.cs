using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using CommunityToolkit.Mvvm.Input;

namespace Telemetry.App.ViewModels
{
    public partial class ChartViewModel : ObservableObject
    {
        [ObservableProperty]
        public ISeries[] _series =
        {
            new LineSeries<double>
            {
                Name = "Humidity",
                Values = new double[] { 42, 10, 50, 23, 16, 40, 49 },
                Stroke = new SolidColorPaint(SKColors.DarkSlateBlue) { StrokeThickness = 4 },
                Fill = null,
                GeometryFill = null,
                GeometryStroke = null,
                LineSmoothness = 0,
                TooltipLabelFormatter = (chartPoint) => $"{chartPoint.Context.Series.Name}: {chartPoint.PrimaryValue}"
            },

            new LineSeries<double>
            {
                Name = "Temperature",
                Values = new double[] { 19.5, 20.4, 24.7, 16.5, 20.7, 30, 20.3 },
                Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 4 },
                Fill = null,
                GeometryFill = null,
                GeometryStroke = null,
                LineSmoothness = 0,
                TooltipLabelFormatter = (chartPoint) => $"{chartPoint.Context.Series.Name}: {chartPoint.PrimaryValue}"
            }
        };

        [ObservableProperty]
        public Axis[] _yAxis = new Axis[]
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
        public Axis[] _xAxis = new Axis[]
        {
            new Axis
            {
                Name = "Time stamps - in days",
                NamePaint = new SolidColorPaint(SKColors.Black),
                NameTextSize = 40,
                Labels = new string[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" },
                LabelsPaint = new SolidColorPaint(SKColors.Coral),
                TextSize= 35
            }
        };

        public SolidColorPaint LegendTextPaint { get; set; } = new SolidColorPaint
        {
            Color = SKColors.Black  
        };

        [RelayCommand]
        public async Task LatestHour()
        {
            XAxis = new Axis[]
            {
                new Axis
                {
                    Name = "Time stamps - in minutes",
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    NameTextSize = 40,
                    Labels = new string[] { "10", "20", "30", "40", "50", "60" },
                    LabelsPaint = new SolidColorPaint(SKColors.Coral),
                    TextSize= 35
                }
            };
        }

        [RelayCommand]
        public async Task LatestDay()
        {
            XAxis = new Axis[]
            {
                new Axis
                {
                    Name = "Time stamps - in hours",
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    NameTextSize = 40,
                    Labels = new string[] { "4", "8", "12", "16", "20", "24" },
                    LabelsPaint = new SolidColorPaint(SKColors.Coral),
                    TextSize= 35
                }
            };
        }

        [RelayCommand]
        public async Task LatestWeek()
        {
            XAxis = new Axis[]
            {
                new Axis
                {
                    Name = "Time stamps - in days",
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    NameTextSize = 40,
                    Labels = new string[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" },
                    LabelsPaint = new SolidColorPaint(SKColors.Coral),
                    TextSize= 35
                }
            };
        }
    }
}
