using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Navigation;

namespace OriflameApp
{
    internal class OriflameApplication
    {
        private OriflameApplication()
        {

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
    }
}
