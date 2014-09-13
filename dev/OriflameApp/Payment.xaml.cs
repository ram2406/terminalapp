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
    /// Interaction logic for UserRoom.xaml
    /// </summary>
    public partial class Payment : Page
    {
        public Payment()
        {
            InitializeComponent();
            keyp.PropertyChanged += keyp_PropertyChanged;
        }

        void keyp_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.inputValue.Text = keyp.Result;
        }
        
    }
}
