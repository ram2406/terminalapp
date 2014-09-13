using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OriflameApp
{
    /// <summary>
    /// Interaction logic for Border.xaml
    /// </summary>
    public partial class Footer : UserControl
    {
        public Footer()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b    = sender as Button;
            if (b == null)                  return;
            string text =  b.Tag as string;
            if(string.IsNullOrEmpty(text))  return;
            var mv = OriflameApplication.Instance.MainNavWindow;
            switch (text)
            {
                case "Menu"     : mv.NavigationService.Navigate(new Uri( "Start.xaml",   UriKind.Relative));   break;
                case "Previos"  : if(mv.CanGoBack)    mv.GoBack()   ; break;
                case "Next"     : if(mv.CanGoForward) mv.GoForward(); break;
            }
        }
    }
}
