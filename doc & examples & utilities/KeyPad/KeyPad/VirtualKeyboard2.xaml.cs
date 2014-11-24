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

namespace KeyPad
{
    /// <summary>
    /// Interaction logic for VirtualKeyboard2.xaml
    /// </summary>
    public partial class VirtualKeyboard2 : UserControl
    {
        private string result = string.Empty;
        public string Result 
        { 
            get { return this.result; }
            private set { this.result = value; this.ResultChanged(this, new EventArgs()); } 
        }
        public event EventHandler ResultChanged;
        public void ResetResult(String text) { this.result = text; }

        public VirtualKeyboard2()
        {
            InitializeComponent();
            this.ResultChanged = new EventHandler((o, e) => { });
            this.Result = string.Empty;
            this.ShowPanel();
        }

        private void ShowPanel(StackPanel panel = null)
        {
            if (panel == null)
            {
                panel = this.k_ABC;
            }
            if (this.k_ABC == panel)
            {
                this.k_123.Visibility = System.Windows.Visibility.Collapsed;
                this.k_ABC.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.k_ABC.Visibility = System.Windows.Visibility.Collapsed;
                this.k_123.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender    as  Button;
            if (btn == null)        return;
            if (TogglePanel(btn.Content.ToString()))   return;
            AppendResultText(btn.Content as String);
        }

        private void AppendResultText(String btnText)
        {
            if (btnText == null) return;
            this.Result += btnText;
        }

        private bool TogglePanel(String content)
        {
            if (content == "ABC")
            {
                ShowPanel(this.k_ABC);
                return true;
            }
            if (content == "123?")
            {
                ShowPanel(this.k_123);
                return true;
            }
            return false;
        }

        
        private void Button_Click_BACK(object sender, RoutedEventArgs e)
        {
            var len = this.Result.Length;
            if (len < 1) return;
            this.Result = this.Result.Substring(0, len - 1);
            e.Handled = true;
        }
    }
}
