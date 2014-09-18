using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Navigation;

namespace OriflameApp
{
    internal class OriflameApplication
    {
        private string catalogPath
                     , databasePath
                     , paymentLogPath;
        private int cashCodePort
                  , cashCodeInterval;
        private OriflameApplication()
        {
            this.catalogPath    = ConfigurationManager.AppSettings.Get("PathToCatalogFolder");
            this.databasePath   = ConfigurationManager.AppSettings["PathToDataBase"];
            this.paymentLogPath = ConfigurationManager.AppSettings["PathToPaymentLog"];
            this.cashCodePort       = int.Parse( ConfigurationManager.AppSettings["CashCodePortNumber"] ?? "0");
            this.cashCodeInterval   = int.Parse(ConfigurationManager.AppSettings["CashCodeInterval"] ?? "0");
            Logic.DataBase.BasePath = this.databasePath;
        }
        static OriflameApplication instance;
        public static OriflameApplication Instance
        {
            get
            {
                return instance ?? (instance = new OriflameApplication());
            }
        }
        MainNavWindow window;
        public MainNavWindow MainNavWindow
        {
            get { return window; }
            set
            {
                if (window != null || value == null) return;
                this.window = value;
            }
        }
        public NavigationService NavigationService { get { return this.MainNavWindow.NavigationService; } }

        public string DataBase { get { return databasePath; } }
        public string Catalog { get { return catalogPath; } }
        public string PaymentLog { get { return paymentLogPath; } }
        public int CashCodeNumberPort { get { return cashCodePort; } }
        public int CashCodeInterval { get { return cashCodeInterval; } }
    }
}

