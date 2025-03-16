using Cohabitation.Models;
using Cohabitation.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cohabitation.ViewModels
{
    public partial class TransactionViewModel :BaseViewModel
    {
        public Transaction Transaction { get; set; } = new Transaction();
        
        public ObservableCollection<string> Names { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// 0 : 入金　1:食費　2:雑費　3: 外食　4:交際費　5:家賃光熱費
        /// </summary>
        public ObservableCollection<string> Items { get; set; } =
            new ObservableCollection<string>
            {
                "入金",
                "食費",
                "家賃・水道・光熱費",
                "交際費",
                "雑費",
            };

        [ObservableProperty]
        public int selectedItemIndex;

        [ObservableProperty]
        public DateTime maximumDate;

        [ObservableProperty]
        public DateTime minimumDate;

        [ObservableProperty]
        public string personName1;
        [ObservableProperty]
        public string personName2;

        [ObservableProperty] 
        public string selectedName;

        [ObservableProperty]
        public string item;

        [ObservableProperty]
        public int amount;

        

        [ObservableProperty]
        public bool isIncome;
        [ObservableProperty]
        public bool isExpense;

        [ObservableProperty]
        public bool showError;

        internal void Init(Setting currentSetting)
        {
            Transaction.Setting = currentSetting;
            //2月だったら2月1日～28日選択のDatePickerを作成する。
            GetMaxMinDate();
            //名前も選択式
            PersonName1 = currentSetting.PersonName1;
            PersonName2 = currentSetting.PersonName2;
            Names.Add(PersonName1);
            Names.Add(PersonName2);
            ShowError = false;
        }
         
        public void GetMaxMinDate()
        {
            string[] parts = Transaction.Setting.Date.Split('/');
            // 選択された日付の年と月を取得
            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            // その月の最終日を取得
            int daysInMonth = DateTime.DaysInMonth(year, month);
            // DatePicker の選択可能範囲を更新
            MinimumDate = new DateTime(year, month, 1);
            MaximumDate = new DateTime(year, month, daysInMonth);
        }

        [RelayCommand]
        public void Cancel()
        {
            App.Current.MainPage = new AppContainer();
        }
        [RelayCommand]
        public void Save()
        {
            if (CheckItem())
            {

                var transaction = new Transaction()
                {
                    Item = Items[SelectedItemIndex],
                    SelectedItemIndex = SelectedItemIndex,
                    Amount = Amount,
                    Name = SelectedName,
                    Date = MinimumDate.ToString("yyyy/MM/dd"),
                    Person1IncomeExpense = PersonName1 == SelectedName,
                    Setting = Transaction.Setting,
                };
                App.TransactionRepo.SaveItem(transaction);
                var a =  App.TransactionRepo.GetItems();
                App.Current.MainPage = new AppContainer();
            }
            else
            {
                ShowError = true;
            }
        }

        private bool CheckItem()
        {
            if(SelectedItemIndex == -1 || Amount == 0 || SelectedName == null)
            {
                return false;
            }
            return true;
        }
    }
}
