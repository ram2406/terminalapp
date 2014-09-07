using System;
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
//using CP = CSJWPOS;
using Microsoft.PointOfService;
using System.Diagnostics;

namespace OriflameApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CashCode cc= new CashCode();
            cc.NumberOfComPort = ushort.Parse(txb.Text);
            cc.OpenComPort();
           
            cc.DelegatePollingBill = PollingBill;
            cc.DelegateProcessMessage = ProcessMessage;
            cc.Reset();
            
            string name = string.Empty, id = string.Empty;
            cc.Identification(ref name, ref id);

            l_id.Content = name + " " + id;

            cc.Reset();
            var rubs = Enum.GetValues(typeof(CashCode.RUB));
            var lrubs = rubs.OfType<CashCode.RUB>().ToList();
            cc.EnableBillTypes(new List<CashCode.RUB> { CashCode.RUB.B100});
            
            var res = cc.PollingLoop(100, 20);
            cc.Reset();

            cc.CloseComPort();
        }
        void PollingBill(UInt16 nominal, ref bool canLoop)
        {
            lst.Items.Add(nominal);
        }
        void ProcessMessage(uint error, string message)
        {
            lst.Items.Add(error + message);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Citizen c = new Citizen();
            c.StringToPrint = "Origlame\n" + DateTime.Now + "\n\nСпасибо за покупку";
            c.Font = new System.Drawing.Font("Arial", 12);
            c.Print();
        }

        void printer_OutputCompleteEvent(object sender, OutputCompleteEventArgs e)
        {
            Debug.Assert(false);
        }
    }
    
}
