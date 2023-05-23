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

        [ObservableProperty]
        private bool _isToggled;

        [RelayCommand]
        public async void GetLatestReading()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) { return; }

            OverviewMeasurement = await _apiService.GetLatestMeasurement();
        }

        async partial void OnIsToggledChanged(bool value)
        {
            if(Connectivity.NetworkAccess != NetworkAccess.Internet) { return; }

            if (value)
            {
                await _apiService.TurnOnLedAsync("HIGH");
            }
            else if (!value)
            {
                await _apiService.TurnOnLedAsync("LOW");
            }
        }
    }
}
