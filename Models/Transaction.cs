using Cohabitation.Abstractions;
using CommunityToolkit.Mvvm.Input;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cohabitation.Models
{
    public class Transaction :TableData
    {
        public string Item { get; set; }

        /// <summary>
        ///  "0入金","1食費", "2家賃・水道・光熱費","3交際費","4雑費",
        /// </summary>
        public int SelectedItemIndex { get; set; }

        public decimal Amount { get; set; }
        public bool IsIncome 
        {
            get => _isIncome;
            set
            {
                if(SelectedItemIndex == 0)
                {
                    _isIncome = true;
                }
            }
        }
        private bool _isIncome;
        
        public string Name { get; set; }
        public bool Person1IncomeExpense { get; set; }

        public string SettingJson { get; set; }
        
        [Ignore]
        public Setting Setting
        {
            get => string.IsNullOrEmpty(SettingJson)? null : JsonSerializer.Deserialize<Setting>(SettingJson); 
            set => SettingJson = JsonSerializer.Serialize(value);
        }


    }
}
