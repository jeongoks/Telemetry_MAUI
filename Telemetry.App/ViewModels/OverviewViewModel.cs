using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.App.ViewModels
{
    public partial class OverviewViewModel : ObservableObject
    {
        [ObservableProperty]
        private double _temperature;

        [ObservableProperty]
        private double _humidity;

        [ObservableProperty]
        private string _location;

        [ObservableProperty]
        private DateTime _date;
    }
}
