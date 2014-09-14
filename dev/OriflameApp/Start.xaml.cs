using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : Page
    {
        public Start()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if(b == null) {
                Debug.Assert(false, "Что то пошло не так");
                return;
            }
            string xaml = b.Tag as string;
            switch (xaml)
            {
                case "UserRoom.xaml": this.NavigationService.Navigate(new Payment()) ; break;
                case "Catalog.xaml": this.NavigationService.Navigate(new Catalog()); break;
            }
        }
    }
}
