using Cohabitation.Models;
using Cohabitation.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cohabitation.ViewModels
{
    public partial class SettingViewModel : BaseViewModel
    {
        #region フィールド
        [ObservableProperty]
        public string lbl_targetAmount;
        [ObservableProperty]
        public string lbl_currentAmount;
        [ObservableProperty]
        public int personRatio1;
        [ObservableProperty]
        public int personRatio2;
        [ObservableProperty]
        public string targetAmountText;
        [ObservableProperty]
        public Color targetAmountTextColor;
        [ObservableProperty]
        public string currentAmountText;
        [ObservableProperty]
        public Color currentAmountTextColor;
        [ObservableProperty]
        public string personNameText1;
        [ObservableProperty]
        public Color personNameText1Color;
        [ObservableProperty]
        public string personNameText2;
        [ObservableProperty]
        public Color personNameText2Color;
        [ObservableProperty]
        public string personRatioText1;
        [ObservableProperty]
        public Color personRatioText1Color;
        [ObservableProperty]
        public string personRatioText2;
        [ObservableProperty]
        public Color personRatioText2Color;

        [ObservableProperty]
        public string date;
        
        [ObservableProperty]
        public string errorMessage;
        
        private Color _defaultLabelColor;
        private DateTime _currentDate;
        private Setting _prevSetting = new Setting();
        #endregion

        public SettingViewModel()
        {
            // 日本円の通貨記号（￥）を取得
            string CurrencySymbol = CultureInfo.GetCultureInfo("ja-JP").NumberFormat.CurrencySymbol;
            Lbl_targetAmount = $"目標金額({CurrencySymbol})";
            Lbl_currentAmount = $"残高({CurrencySymbol})";   
        }
        [RelayCommand]
        public void Update()
        {
            ErrorMessage = CheckSettingData();
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                //エラー発生！
                return;
            }
            //DBにセットするデータを集める
            var currentSetting = new Setting()
            {
                TargetAmount = decimal.Parse(TargetAmountText),
                CurrentAmount = decimal.Parse(CurrentAmountText),
                PersonName1 = PersonNameText1,
                PersonName2 = PersonNameText2,
                PersonRatio1 = int.Parse(PersonRatioText1),
                PersonRatio2 = int.Parse(PersonRatioText2),
                Date = App.CurrentDateTime.ToString("yyyy/MM"),
            };
                //すでにデータあるのでUpdate
            ErrorMessage = App.SettingRepo.SaveItemByDate(currentSetting,_prevSetting);

        }


        [RelayCommand]
        public void PreviousMonth()
        {
            //App.SettingRepo.TestDeleteAllItem();
            App.CurrentDateTime = App.CurrentDateTime.AddMonths(-1);
            Date = App.CurrentDateTime.ToString("yyyy/MM");
            ShowData();
        }
        [RelayCommand]
        public void NextMonth()
        {
             App.CurrentDateTime = App.CurrentDateTime.AddMonths(1);
            Date = App.CurrentDateTime.ToString("yyyy/MM");
            ShowData();
        }
        /// <summary>
        /// //現在のSetting情報を取得する
        /// </summary>
        public void SetData()
        {   
            SetDefaultColor();
            ErrorMessage = string.Empty;
            Date = App.CurrentDateTime.ToString("yyyy/MM");
            ShowData();
        }

        private void ShowData()
        {
            //データがある場合
            var settings = App.SettingRepo.GetItems();
            var setting = settings.OrderByDescending(x => x.Version).FirstOrDefault();
            if (setting != null)
            {
                TargetAmountText = setting.TargetAmount.ToString();
                CurrentAmountText =setting.CurrentAmount.ToString();
                PersonNameText1 =  setting.PersonName1;
                PersonNameText2 =  setting.PersonName2;
                PersonRatioText1 = setting.PersonRatio1.ToString();
                PersonRatioText2 = setting.PersonRatio2.ToString();
                ErrorMessage = string.Empty;
            }
            else
            {
                TargetAmountText = string.Empty;
                CurrentAmountText = string.Empty;
                PersonNameText1 = string.Empty;
                PersonNameText2 = string.Empty;
                PersonRatioText1 = string.Empty;
                PersonRatioText2 = string.Empty;
                ErrorMessage = string.Empty;
            }
        }

        private string CheckSettingData()
        {
            SetDefaultColor();
            if (string.IsNullOrEmpty(TargetAmountText))
            {
                TargetAmountTextColor = Colors.Red;
                return  $"目標金額を入力してください。";
            }
            else if(string.IsNullOrEmpty(CurrentAmountText))
            {
                CurrentAmountTextColor = Colors.Red;
                return $"2人の口座残高を入力してください。";
            }
            else if(string.IsNullOrEmpty(PersonNameText1))
            {
                PersonNameText1Color = Colors.Red;
                return $"名前（1人目）を入力してください。";
            }
            else if(string.IsNullOrEmpty(PersonNameText2))
            {
                PersonNameText2Color = Colors.Red;
                return $"名前（2人目）を入力してください。";
            }
            else if(string.IsNullOrEmpty(PersonRatioText1))
            {
                PersonRatioText1Color = Colors.Red;
                return $"1人目入金比率（%）を入力してください。";
            }
            else if (string.IsNullOrEmpty(PersonRatioText2))
            {
                PersonRatioText2Color= Colors.Red;
                return $"2人目入金比率（%）を入力してください。";
            }
            int personRatio1 = string.IsNullOrEmpty(PersonRatioText1) ? 0 : int.Parse(PersonRatioText1);
            int personRatio2 = string.IsNullOrEmpty(PersonRatioText2) ? 0 : int.Parse(PersonRatioText2);

            if (personRatio1 + personRatio2 != 100)
            {
                PersonRatioText1Color = Colors.Red;
                PersonRatioText2Color = Colors.Red;
                return "入金比率を合計100%に設定してください。";
            }

            var targetAmount = int.Parse(TargetAmountText);
            if(targetAmount == 0)
            {
                TargetAmountTextColor = Colors.Red;
                return $"目標金額を設定してください。";
            }
            return string.Empty;
        }
        
        private void SetDefaultColor()
        {
            _defaultLabelColor = Colors.Black;
            TargetAmountTextColor = _defaultLabelColor;
            CurrentAmountTextColor = _defaultLabelColor;
            PersonNameText1Color = _defaultLabelColor;
            PersonNameText2Color= _defaultLabelColor;
            PersonRatioText1Color= _defaultLabelColor;
            PersonRatioText2Color = _defaultLabelColor;
        }
    }
}
