using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.App.Contracts;
using Telemetry.App.Models;

namespace Telemetry.App.ViewModels
{
    public partial class OverviewViewModel : ObservableObject
    {
        private readonly IApiService _apiService;

        public OverviewViewModel(IApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private Measurement _overviewMeasurement;

        [RelayCommand]
        public async void GetLatestReading()
        {
            OverviewMeasurement = await _apiService.GetLatestMeasurement();
        }
    }
}
