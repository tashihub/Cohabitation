using Cohabitation.ViewModels;

namespace Cohabitation.Views;

public partial class StatisticsPage : ContentPage
{
	public StatisticsPage()
	{
		InitializeComponent();
		BindingContext = new StatisticsViewModel();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var currentVM = (StatisticsViewModel)BindingContext;
        currentVM.Init();
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        base.OnAppearing();
        var currentVM = (StatisticsViewModel)BindingContext;
        currentVM.Init();
        currentVM.GetItemName();
    }
}