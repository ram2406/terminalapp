using Logic;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        private TextBlock current_textblock = null;
        public Register()
        {
            InitializeComponent();
            this.virt.ResultChanged += virt_ResultChanged;
            this.ft.Menu.Content = "Завершить";
            this.ft.Menu.PreviewMouseDown += Menu_Click;
        }

        bool CheckInputs()
        {
            return CheckTextBlock(inputNumber)
                && CheckTextBlock(inputLastName)
                && CheckTextBlock(inputFirstName)
                && CheckTextBlock(inputPName); 
        }

        void Menu_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckInputs())
            {
                this.popup.IsOpen = true;
                this.popup_textblock.Text = string.Empty;
                if (!CheckTextBlock(inputNumber))       this.popup_textblock.Text += "Проверьте свой идентифиционный номер\n";
                if (!CheckTextBlock(inputLastName))     this.popup_textblock.Text += "Проверьте свою фамилию\n";
                if (!CheckTextBlock(inputFirstName))    this.popup_textblock.Text += "Проверьте свое имя\n";
                if (!CheckTextBlock(inputPName))        this.popup_textblock.Text += "Проверьте свое отчество\n";
                e.Handled = true;
                return;
            }
            try {
                MemberFactory.Create( uint.Parse(inputNumber.Text)
                                    , string.Format("{0} {1} {2}", inputLastName.Text, inputFirstName.Text, inputPName.Text));
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        void virt_ResultChanged(object sender, EventArgs e)
        {
            if (this.current_textblock == null) return;
            this.current_textblock.Text = this.FormattingText(virt.Result);
        }

        private void grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as Grid;
            if (grid == null && grid.Children.Count < 2) return;
            var maybe_rect = grid.Children[0] as Rectangle;
            if (maybe_rect == null) return;
            this.UnFocusedGrids();
            maybe_rect.Stroke = new SolidColorBrush(Color.FromRgb(0x94, 0xC1, 0x1E));
            var maybe_textblock = grid.Children[1] as TextBlock;
            if (maybe_textblock == null) return;
            if (!CheckTextBlock(maybe_textblock))
            {
                virt.ResetResult(string.Empty);
            }
            else
            {
                virt.ResetResult(maybe_textblock.Text);
            }
            this.current_textblock = maybe_textblock;
        }

        private bool CheckTextBlock(TextBlock maybe_textblock)
        {
            string text = maybe_textblock.Text;
            bool textValid =  !string.IsNullOrEmpty(text) && !text.EndsWith("...");
            if (maybe_textblock != this.inputNumber)
            {
                return textValid;
            }
            uint value = 0;
            uint.TryParse(maybe_textblock.Text, out value);
            return value > (10000 - 1);

            
        }
        private void UnFocusedGrids()
        {
            Rectangle[] rects = new Rectangle[4] {
                grid_number.Children[0]     as Rectangle,
                grid_first_name.Children[0] as Rectangle,
                grid_last_name.Children[0]  as Rectangle,
                grid_patronymic.Children[0] as Rectangle
            };
            foreach(var rect in rects) 
            {
                rect.Stroke = Brushes.Black;
            }
        }
        private string FormattingText(string text)
        {
            if (text.Length < 1) return string.Empty;
            return Char.ToUpper(text[0]) + text.ToLower().Substring(1);
        }
        private void popup_MouseDown(object sender, MouseButtonEventArgs e)
        {
            popup.IsOpen = false;
        }

        private void Button_Click_Menu(object sender, RoutedEventArgs e)
        {
            Footer.MoveTo("Menu");
        }
    }
}
