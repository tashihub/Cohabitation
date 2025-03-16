using Cohabitation.ViewModels;

namespace Cohabitation.Views;

public partial class SettingPage : ContentPage
{
	public SettingPage()
	{
		InitializeComponent();
		BindingContext = new SettingViewModel();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		var currentVM = (SettingViewModel)BindingContext;
		currentVM.SetData();
    }
}