using Telemetry.App.ViewModels;

namespace Telemetry.App.Views;

public partial class GraphPage : ContentPage
{
	public GraphPage(ChartViewModel chartViewModel)
	{
		InitializeComponent();
        BindingContext = chartViewModel;
    }
}