using Cohabitation.Abstractions;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Cohabitation.Models
{
    public class Setting : TableData
    {
        #region DB保存
        private decimal _targetAmount;

        public decimal TargetAmount
        {
            get { return _targetAmount; }
            set
            {
                _targetAmount = value;
            }
        }

        private decimal _currentAmount;
        public decimal CurrentAmount 
        {
            get { return _currentAmount;  }
            set
            {
                _currentAmount = value;
            }
        }

        public string PersonName1 { get; set; }

        public string PersonName2 { get; set; }
        public decimal PersonRatio1 { get;set; }
        public decimal PersonRatio2 { get; set; }

        public decimal TargetIncome1 
        {
            get => _targetIncome1;
            set
            {
                _targetIncome1 = (TargetAmount - CurrentAmount) * (PersonRatio1 / 100);
            }
        }
        private decimal _targetIncome1 = 0;

        public decimal TargetIncome2 
        {
            get => _targetIncome2;
            set 
            {
               _targetIncome2 = (TargetAmount - CurrentAmount) * (PersonRatio2 / 100);
            }
        }
        private decimal _targetIncome2 = 0;

        #endregion


    }
}
 