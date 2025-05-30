﻿using Cohabitation.Models;
using Cohabitation.Repositories;
using Cohabitation.Views;

namespace Cohabitation
{
    public partial class App : Application
    {
        public static SettingRepository SettingRepo { get; private set; }
        public static TransactionRepository TransactionRepo { get; private set; }
        public static DateTime CurrentDateTime { get; set; }
        public App(
            SettingRepository settingRepo,
            TransactionRepository transactionRepository)
        {
            InitializeComponent();
            CurrentDateTime = DateTime.Now;
            SettingRepo = settingRepo;
            TransactionRepo = transactionRepository;
            MainPage = new AppContainer();
        }
    }
}
