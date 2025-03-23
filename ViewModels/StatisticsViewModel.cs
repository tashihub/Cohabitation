using Cohabitation.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cohabitation.ViewModels
{
    public partial class StatisticsViewModel : BaseViewModel
    {

        //dotnet add package LiveChartsCore
        //dotnet add package LiveChartsCore.SkiaSharpView
        //dotnet add package LiveChartsCore.SkiaSharpView.Maui
        //dotnet add package SkiaSharp.Views.Maui.Controls
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
                "全出費",
            };
        [ObservableProperty]
        public string itemName;
        [ObservableProperty]
        public string date;
        [ObservableProperty]
        public ISeries[] seriesBar;
        [ObservableProperty]
        public Axis[] xAxes;
        [ObservableProperty]
        public Axis[] yAxes;
        [ObservableProperty]
        public int pickerSelectedIndex = 5;
        public ObservableCollection<Transaction> SpendingList { get; set; } =
            new ObservableCollection<Transaction>();

        public void Init()
        {
            Date = App.CurrentDateTime.ToString("yyyy");
            GetTransactionsSummary();
        }

        public void GetItemName()
        {

            switch (PickerSelectedIndex)
            {
                case 0:
                    ItemName = "入金";
                    break;
                case 1:
                    ItemName = "食費";
                    break;
                case 2:
                    ItemName = "家賃・水道・光熱費";
                    break;
                case 3:
                    ItemName = "交際費";
                    break;
                case 4:
                    ItemName = "雑費";
                    break;
                case 5:
                    ItemName = "全出費";
                    break;
                default:
                    itemName = "";
                    break;
            }
        }

        public void GetTransactionsSummary()
        {
            var year = (App.CurrentDateTime.Year % 100).ToString();
            var data = App.TransactionRepo.GetItems();
            var groupedTransactions = data
                .Where(x => x.Setting.Date.Substring(2, 2) == year &&
                       x.SelectedItemIndex == PickerSelectedIndex)
                .GroupBy(t => t.Setting.Date); //25/03とかでまとめる
            //全出費の場合
            if(PickerSelectedIndex == 5)
            {
                groupedTransactions = data
                    .Where(x => x.Setting.Date.Substring(2, 2) == year)
                    .GroupBy(t => t.Setting.Date); //25/03とかでまとめる  
            }

            #region グラフの作成データ
            //月
            List<double> totals = new List<double> { 0,0,0,0,0,0,0,0,0,0,0,0};
            //groupTransactionにある月だけ書き換える
            foreach ( var grouping in groupedTransactions)
            {
                //Keyには25/03で入ってくる
                int month = int.Parse(grouping.Key.Split('/')[1]);  //03だけ取得する
                totals[month - 1] = grouping.Where(x => !x.IsIncome).Sum(a => (double)a.Amount);
            }
            MakeGraph(totals);
            #endregion

            #region 今まで使った合計
            var spendingList = new ObservableCollection<Transaction>();
            groupedTransactions = groupedTransactions.OrderByDescending(x=>x.Key);
            SpendingList.Clear();
            foreach ( var grouping in groupedTransactions)
            {
                
                foreach (var transaction in grouping)
                {
                    if(transaction.SelectedItemIndex == PickerSelectedIndex || 
                        PickerSelectedIndex == 5)
                    {
                        SpendingList.Add(transaction);

                    }
                }
            }
            #endregion
        }

        /// <param name="values"> グラフに使用するデータ</param>
        private void MakeGraph(List<double> values)
        {
            
            SeriesBar = new ISeries[]
            {
                new ColumnSeries<double> //棒グラフ
                {
                    Values = values,
                    Name = "出金額"
                }
            };

            //1月～12月
            var labels = Enumerable.Range(1, 12)
                .Select(m => m.ToString()) 
                .ToArray();


            XAxes = new Axis[]
            {
                new Axis
                {
                   Labels = labels,
                   LabelsRotation = 0,
                   MinStep = 1
                }
            };

            YAxes = new Axis[]
            {
                new Axis
                {
                    Labeler = value => $"{value}",
                    MinLimit = 0,
                    MaxLimit = values.Max() +5000,
                }
            };
        }






        public StatisticsViewModel()
        {

        }
        [RelayCommand]
        public void PreviousMonth()
        {
            App.CurrentDateTime = App.CurrentDateTime.AddYears(-1);

            //Date = _currentDate.ToString("yyyy/MM");
            Date = App.CurrentDateTime.ToString("yyyy");
            Init();
        }
        [RelayCommand]
        public void NextMonth()
        {
            //_currentDate = _currentDate.AddMonths(1);
            App.CurrentDateTime = App.CurrentDateTime.AddYears(1);

            //Date = _currentDate.ToString("yyyy/MM");
            Date = App.CurrentDateTime.ToString("yyyy");
            Init();
        }
    }
}
