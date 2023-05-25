using Telemetry.App.ViewModels;

namespace Telemetry.App.Views;

public partial class GraphPage : ContentPage
{
	private readonly ChartViewModel _chartViewModel;
	public GraphPage(ChartViewModel chartViewModel)
	{
		InitializeComponent();
		this._chartViewModel = chartViewModel;
        BindingContext = chartViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _chartViewModel.AllMeasurementsCommand.Execute(null);
    }
}