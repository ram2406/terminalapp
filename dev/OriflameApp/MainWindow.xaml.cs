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
            var myExplorer = new PosExplorer();
            var devices = myExplorer.GetDevices("PosPrinter");
            var device = myExplorer.GetDevice("PosPrinter", "CT-S2000_1"); 
            var inst = myExplorer.CreateInstance(device);
            PosPrinter printer = (PosPrinter)inst;
            printer.OutputCompleteEvent += printer_OutputCompleteEvent;
            printer.Open();
            printer.Claim(1000);
            printer.DeviceEnabled = true;
            
            
            
            printer.PrintNormal(PrinterStation.Receipt, "test");
            //printer.EndInsertion();
            printer.DirectIO(
            printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
            printer.DeviceEnabled = false;
            printer.Release();
            printer.Close();
            //PosPrinter printer = (Pos) inst;
            //printer.PrintNormal(PrinterStation.None, "test");
            //DeviceInfo device = myExplorer.GetDevice("Msr", "MGTK_MSR");
           /*CP.CTCashDrawer2 drawer = (CP.CTCashDrawer2)inst;
            drawer.Claim(1000);
            drawer.Open();
            */
            /*
            CP.CTCashDrawer2 cc = new CP.CTCashDrawer2();
            cc.Open();
            
            try
            {
                var printer = new CP.CTS2000POSPrinter();
                printer.Open();
                printer.Claim(1000);
                printer.DeviceEnabled = true;
                printer.PrintNormal(Microsoft.PointOfService.PrinterStation.None, "test");    
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            */
           
         
            
        }

        void printer_OutputCompleteEvent(object sender, OutputCompleteEventArgs e)
        {
            Debug.Assert(false);
        }
    }
    
}
