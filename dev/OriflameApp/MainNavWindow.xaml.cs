﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Logic.SerialPort;

using Microsoft.PointOfService;
using System.Diagnostics;

namespace OriflameApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainNavWindow : NavigationWindow
    {
        public MainNavWindow()
        {
            
            
            this.Loaded += MainNavWindow_Loaded;
        }

        void MainNavWindow_Loaded(object sender, RoutedEventArgs e)
        {
            OriflameApplication.Instance.MainNavWindow = this;
            ShowCursor();
            
        }
        [Conditional("DEBUG")]
        void ShowCursor()
        {
            this.Cursor = Cursors.Cross;
            this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            this.ResizeMode = System.Windows.ResizeMode.CanResize;
            this.WindowState = System.Windows.WindowState.Normal;
            this.Height = 1024;
            this.Width = 1280;
        }
        
       
    }
    
}
