using Cohabitation.Models;
using Cohabitation.Repositories;
using Cohabitation.ViewModels;

namespace Cohabitation.Views;

public partial class DashboardPage : ContentPage
{
	public DashboardPage()
	{
		InitializeComponent();
		BindingContext = new DashboardViewModel();
        var currentVM = (DashboardViewModel)BindingContext;
        currentSetting = currentVM.HasData();
        if (currentSetting != null) { _settingDone = true; }
        currentVM.Init();
    }

    bool _settingDone = false;
    Setting currentSetting;
    protected override void OnAppearing()
    {
        base.OnAppearing();
        var currentVM = (DashboardViewModel)BindingContext;
        currentSetting = currentVM.HasData();
        if (currentSetting != null) { _settingDone = true; }
        currentVM.Init();

    }

    private async void AddTransaction_Clicked(object sender, EventArgs e)
    {
        var currentVM = (DashboardViewModel)BindingContext;
        var date = currentVM.Date;
        if (_settingDone) 
        {
            App.Current.MainPage = new TransactionsPage(currentSetting);
        }
        else
        {

            await DisplayAlert("Alert", $"ê›íËâÊñ Ç©ÇÁ{date}ÇÃê›íËÇçsÇ¡ÇƒÇ≠ÇæÇ≥Ç¢ÅB", "OK");
        }
    }
}