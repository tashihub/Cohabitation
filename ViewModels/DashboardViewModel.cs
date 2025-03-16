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
    public partial class DashboardViewModel :BaseViewModel
    {
       
        public ObservableCollection<Transaction> Transactions { get; set; } = new ObservableCollection<Transaction>();

        [ObservableProperty]
        public decimal personRatio1;
        [ObservableProperty]
        public decimal personRatio2;
        [ObservableProperty]
        public decimal targetAmount;
        [ObservableProperty]
        public decimal currentAmount;
        [ObservableProperty]
        public string personName1;
        [ObservableProperty]
        public string personName2;

        [ObservableProperty]
        public decimal person1Expenses;
        [ObservableProperty]
        public decimal person1Income;
        /// <summary>
        /// 目標入金額1
        /// </summary>
        [ObservableProperty]
        public decimal person1Payment;

        [ObservableProperty]
        public decimal person2Expenses;
        [ObservableProperty]
        public decimal person2Income;
        /// <summary>
        /// 目標入金額2
        /// </summary>
        [ObservableProperty]
        public decimal person2Payment;



        [ObservableProperty]
        public decimal expenses;
        [ObservableProperty]
        public decimal income;
        [ObservableProperty]
        public string date;

        private DateTime _currentDate;
        public Setting CurrentSetting { get; set; }


        public void Init()
        {
            _currentDate = DateTime.Now;
            Date = _currentDate.ToString("yyyy/MM");
            SetData();
        }

        /// <summary>
        /// 初期値を設定する
        /// </summary>
        private void Initialize()
        {
            CurrentSetting = new Setting();
            TargetAmount = 0;
            CurrentAmount = 0;
            Person1Income = 0;
            Person1Expenses = 0;
            Person1Payment = 0;
            Person2Income = 0;
            Person2Expenses = 0;
            Person2Payment = 0;
            TargetAmount = 0;
            CurrentAmount = 0;
            PersonRatio1 = 0;
            PersonRatio2 = 0;
            PersonName1 = string.Empty;
            PersonName2 = string.Empty;

        }
        private void SetTransactionData()
        {
            var transactions = App.TransactionRepo.GetItems();
            transactions = transactions.ToList();
            var currentVersion = CurrentSetting.Version;
            var currentDate = CurrentSetting.Date;
            Transactions.Clear();
            foreach ( var transaction in transactions )
            {
                if (transaction.Date.Substring(0,7) == Date) 
                {
                    Transactions.Add(transaction);
                }
            }

            //Person1の入金額合計と出費額合計
            var person1TotalExpneseVer = Transactions
                .Where(x=>x.Person1IncomeExpense 
                && !x.IsIncome 
                && x.Setting.Version == currentVersion
                && x.Date.Substring(0,7) == currentDate)
                .Sum(x => x.Amount);

            var person1TotalIncomeVer = Transactions
                .Where(x => x.Person1IncomeExpense 
                && x.IsIncome 
                && x.Setting.Version == currentVersion
                && x.Date.Substring(0,7) == currentDate)
                .Sum(x => x.Amount);

            var person1TotalExpnese = Transactions
                .Where(x => x.Person1IncomeExpense 
                && !x.IsIncome
                && x.Date.Substring(0,7) == currentDate)
                .Sum(x => x.Amount);

            var person1TotalIncome = Transactions
                .Where(x => x.Person1IncomeExpense 
                && x.IsIncome
                && x.Date.Substring(0,7) == currentDate)
                .Sum(x => x.Amount);

            //Person2の入金額合計と出費額合計
            var person2TotalExpneseVer = Transactions
                .Where(x => !x.Person1IncomeExpense 
                && !x.IsIncome 
                && x.Setting.Version == currentVersion
                && x.Date.Substring(0,7) == currentDate)
                .Sum(x => x.Amount);

            var person2TotalIncomeVer = Transactions
                .Where(x => !x.Person1IncomeExpense 
                && x.IsIncome 
                && x.Setting.Version == currentVersion
                && x.Date.Substring(0,7) == currentDate)
                .Sum(x => x.Amount);

            var person2TotalExpnese = Transactions
                .Where(x => !x.Person1IncomeExpense
                && !x.IsIncome 
                && x.Date.Substring(0,7) == currentDate)
                .Sum(x => x.Amount);

            var person2TotalIncome = Transactions
                .Where(x => !x.Person1IncomeExpense
                && x.IsIncome
                && x.Date.Substring(0,7) == currentDate)
                .Sum(x => x.Amount);
            //目標入金金額の計算をする　目標金額　＝　目標金額　-　(入金金額　- 出費金額)
            //途中で現在高が設定変えられた場合を考慮する
            Person1Payment -= (person1TotalIncomeVer - person1TotalExpneseVer);
            Person2Payment -= (person2TotalIncomeVer - person2TotalExpneseVer);

            //その他値の代入
            Person1Expenses = person1TotalExpnese;
            Person1Income = person1TotalIncome;
            Person2Expenses = person2TotalExpnese;
            Person2Income = person2TotalIncome;

            //現在高の計算　Settingで設定した現在高 + 入金額合計　- 出金額合計
            CurrentAmount = CurrentSetting.CurrentAmount
                + (person1TotalIncomeVer + person2TotalIncomeVer)
                - (person1TotalExpneseVer + person2TotalExpneseVer);
        }
        internal void SetData()
        {
            Initialize();
            string date = _currentDate.ToString("yyyy/MM");
            var data = App.SettingRepo
                .GetAllData()
                .FirstOrDefault(x=>x.Date == date);
            //データがある場合
            if (data != null)
            {
                CurrentSetting = data;
                TargetAmount = data.TargetAmount;
                CurrentAmount = data.CurrentAmount;
                PersonRatio1 = data.PersonRatio1;
                PersonRatio2 = data.PersonRatio2;
                PersonName1 = data.PersonName1;
                PersonName2 = data.PersonName2;
                Date = data.Date;
                Person1Payment = data.TargetIncome1;
                Person2Payment = data.TargetIncome2;
            }
            SetTransactionData();
        }

        /// <summary>
        /// 現在の年月日にデータがあるかを確認
        /// </summary>
        /// <returns></returns>
        internal Setting HasData()
        {
            string date = _currentDate.ToString("yyyy/MM");
            var data = App.SettingRepo
                .GetAllData()
                .FirstOrDefault(x => x.Date == date);
            if(data == null) { return null; }
            CurrentSetting = data;
            return data;
        }
        [RelayCommand]
        public void PreviousMonth()
        {
            //App.SettingRepo.TestDeleteAllItem();
            _currentDate = _currentDate.AddMonths(-1);
            Date = _currentDate.ToString("yyyy/MM");
            SetData();
        }
        [RelayCommand]
        public void NextMonth()
        {
            _currentDate = _currentDate.AddMonths(1);
            Date = _currentDate.ToString("yyyy/MM");
            SetData();
        }

        [RelayCommand]
        public void Delete(Transaction transaction)
        {
            App.TransactionRepo.DeleteItem(transaction);
            SetData();
        }
    }
}
