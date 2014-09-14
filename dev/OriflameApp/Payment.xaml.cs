using System;
using System.Collections.Generic;
using System.IO;
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

using Logic;


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

        private void Button_Click_Print(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)check.ActualWidth, (int)check.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(check);
            System.Drawing.Bitmap btmp = BitmapFromSource(rtb);
            //btmp.Save("check.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            Logic.SerialPort.Citizen cn = new Logic.SerialPort.Citizen();
            cn.Bitmap = btmp;
            cn.Font = new System.Drawing.Font("Arial", 12);
            cn.StringToPrint = "test";
            cn.Print();
        }
        public static System.Drawing.Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            System.Drawing.Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }
            return bitmap;
        }
    }
}
