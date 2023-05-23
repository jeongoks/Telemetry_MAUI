using Telemetry.App.ViewModels;

namespace Telemetry.App.Views;

public partial class MainPage : ContentPage
{
	private readonly OverviewViewModel _overviewViewModel;

	public MainPage(OverviewViewModel overviewViewModel)
	{
		InitializeComponent();	
		_overviewViewModel = overviewViewModel;
		BindingContext = overviewViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		_overviewViewModel.GetLatestReadingCommand.Execute(null);
    }
}