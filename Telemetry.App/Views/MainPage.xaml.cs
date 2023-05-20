using Telemetry.App.ViewModels;

namespace Telemetry.App.Views;

public partial class MainPage : ContentPage
{
	public MainPage(ChartViewModel chartViewModel)
	{
		InitializeComponent();
		BindingContext = chartViewModel;
	}
}