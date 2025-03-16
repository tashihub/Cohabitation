using Cohabitation.Models;
using Cohabitation.ViewModels;

namespace Cohabitation.Views;

public partial class TransactionsPage : ContentPage
{
    private Setting _currentSetting { get; set; }
	public TransactionsPage(Setting setting)
	{
		InitializeComponent();
        BindingContext = new TransactionViewModel();
        _currentSetting = setting;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var currentVM = (TransactionViewModel)BindingContext;
        currentVM.Init(_currentSetting);
    }

    private void Save_Clicked(object sender, EventArgs e)
    {
        //åªç›ÇÃê›íËÇ‡ï€ë∂Ç∑ÇÈ

    }

    private void Cancel_Clicked(object sender, EventArgs e)
    {

    }
}