using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using Telemetry.App.Extensions;
using Telemetry.App.Models;
using Telemetry.Service.Contracts;
using Telemetry.Service.Models;

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
        private MeasurementDTO _overviewMeasurement;

        [ObservableProperty]
        private DateTime _shownTime;

        [ObservableProperty]
        private bool _isToggled;

        [RelayCommand]
        public async void GetLatestReading()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) { return; }

            Measurement tempMeasurement = await _apiService.GetLatestMeasurement();
            OverviewMeasurement = tempMeasurement.ToDTO();
            ShownTime = OverviewMeasurement.Time.ToLocalTime();
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
