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

namespace OriflameApp
{
    /// <summary>
    /// Interaction logic for Catalog.xaml
    /// </summary>
    public partial class Catalog : Page
    {
        public Catalog()
        {
            this.ipaths = new ImagePaths(OriflameApplication.Instance.Catalog);
            InitializeComponent();

            ft.Next.Visibility = System.Windows.Visibility.Visible;
            ft.Prev.Visibility = System.Windows.Visibility.Visible;

            ft.Next.Click += Next_Click;
            ft.Prev.Click += Prev_Click;

            ShowCurrentImage();
        }

        void Prev_Click(object sender, RoutedEventArgs e)
        {
            ShowImage(ipaths.GetPrev());
        }

        void Next_Click(object sender, RoutedEventArgs e)
        {
            ShowImage(ipaths.GetNext());
        }

        public void ShowCurrentImage()
        {
            ShowImage(ipaths.GetCurrent());
        }

        public void ShowImage(string path)
        {
            if (path == null) return;
            Image myImage = this.image;
            

            // Create source
            BitmapImage myBitmapImage = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(path);

            myBitmapImage.EndInit();
            //set image source
            myImage.Source = myBitmapImage;
        }
        private ImagePaths ipaths;

        public struct ImagePaths
        {
            public ImagePaths(string catalog)
            {
                paths = Directory.GetFiles(catalog, "*.jpg", SearchOption.AllDirectories);
                current = 0;
            }
            string[] paths;
            int current;
            public string GetCurrent() 
            {
                if (paths.Length == 0) return null;
                return System.IO.Path.Combine(  Directory.GetCurrentDirectory() , paths[current] );
            }
            public string GetNext() 
            {
                if (paths.Length == 0) return null;
                return System.IO.Path.Combine(Directory.GetCurrentDirectory(), paths[ current < paths.Length-1 ? ++current : paths.Length-1]); 
            }
            public string GetPrev() 
            {
                if (paths.Length == 0) return null;
                return System.IO.Path.Combine(Directory.GetCurrentDirectory(), paths[ current >0 ? --current:0]); 
            }
        }

    }
}
