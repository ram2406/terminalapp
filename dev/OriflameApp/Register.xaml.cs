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
            string text = maybe_textblock.Text;
            if (text.EndsWith("..."))
            {
                virt.ResetResult(string.Empty);
            }
            else
            {
                virt.ResetResult(text);
            }
            this.current_textblock = maybe_textblock;
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
    }
}
