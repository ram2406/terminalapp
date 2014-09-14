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
                     , databasePath;
        private OriflameApplication()
        {
            this.catalogPath    = ConfigurationManager.AppSettings.Get("PathToCatalogFolder");
            this.databasePath   = ConfigurationManager.AppSettings["PathToDataBase"];
        }
        static OriflameApplication instance;
        public static OriflameApplication Instance
        {
            get
            {
                return instance = instance ?? new OriflameApplication();
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
    }
}
