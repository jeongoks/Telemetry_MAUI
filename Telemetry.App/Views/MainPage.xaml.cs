using Telemetry.App.ViewModels;

namespace Telemetry.App.Views;

public partial class MainPage : ContentPage
{
	public MainPage(OverviewViewModel overviewViewModel)
	{
		InitializeComponent();	
		BindingContext = overviewViewModel;
	}
}