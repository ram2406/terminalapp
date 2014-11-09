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
using System.ComponentModel;


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

            this.keyp.PropertyChanged += keyp_PropertyChanged;
            this.footer.Menu.Click += Menu_Click;
            this.keyp.OnOkClick += keyp_OnOkClick;

            this.tb_dt.Text = DateTime.Now.ToString();
            this.tb_id.Text = "-";
            this.tb_name.Text = "-";
            this.tb_cash.Text = "-";
            Canvas.SetZIndex(this.rect_green, 0);

            this.CurrentMember = null;
            this.sum = 0;
            this.bgwr = null;
        }

        void keyp_OnOkClick(object sender, EventArgs e)
        {
            try
            {
                this.Button_Click_Find(null, null);
                if (this.CurrentMember == null)
                {
                    this.tb_id.Text = "не найден";
                    this.tb_name.Text = "не найден";
                    var mv = OriflameApplication.Instance.MainNavWindow;
                    mv.NavigationService.Navigate(new Uri("Register.xaml", UriKind.Relative));
                }
                else
                {
                    this.tb_id.Text = this.CurrentMember.ID.ToString();
                    this.tb_name.Text = this.CurrentMember.Name.Split()[0];
                    Canvas.SetZIndex(this.rect_green, 1);
                    this.footer.Menu.Content = "Завершить";
                    this.Button_Click_Pay(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Menu_Click(object sender, RoutedEventArgs e)
        {
            
            if (this.CurrentMember != null && this.sum > 0)
            {
                this.Button_Click_Print(null, null);
            }
            if (bgwr != null && bgwr.IsBusy)
            {
                this.cc.CanPollingLoop = false;
                bgwr.CancelAsync();
            }
            this.footer.Menu.Content = "Меню";
        }
        
        

        void keyp_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.inputValue.Text = keyp.Result;
        }

        private void Button_Click_Print(object sender, RoutedEventArgs e)
        {
            if (bgwr == null) return;
            if (bgwr.IsBusy)
            {
                cc.CanPollingLoop = false;
                
            }
            //btmp.Save("check.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            Logic.SerialPort.Citizen cn = new Logic.SerialPort.Citizen();
            Logic.Payment p = new Logic.Payment();
            p.DateTime = DateTime.Now;
            p.Member = CurrentMember;
            p.Sum = this.sum;
            MemberFactory.SaveSum(p);

            cn.Sum = p.Sum.ToString();
            cn.Id = p.Member.ID.ToString();
            cn.Name = p.Member.Name.Split(' ')[0];
            cn.DateTime = p.DateTime.ToString();
            
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

        private void Button_Click_Find(object sender, RoutedEventArgs e)
        {
            uint value = 0;
            if (uint.TryParse(this.inputValue.Text, out value))
            {
                this.CurrentMember = MemberFactory.Find(value);
            }
            //var p = new Logic.Payment{Member = m, Sum = 40, DateTime = DateTime.Now} ;
            //MemberFactory.SaveSum(p);
        }

        private void Button_Click_Pay(object sender, RoutedEventArgs x)
        {
            bgwr = new BackgroundWorker();
            bgwr.WorkerSupportsCancellation = true;
            bgwr.WorkerReportsProgress = true;
            cc = new Logic.SerialPort.CashCode();
            bgwr.DoWork += (o, e) =>
            {
                cc.NumberOfComPort = (ushort) OriflameApplication.Instance.CashCodeNumberPort;
                cc.CloseComPort();
                cc.OpenComPort();
                ushort sum = 0;
                cc.DelegatePollingBill = (UInt16 nominal, ref bool canloop) => 
                {
                    File.AppendAllText(OriflameApplication.Instance.PaymentLog, string.Format("{0}\t{1}\t{2}\t{3}\t"+Environment.NewLine, DateTime.Now.ToString(), this.CurrentMember.ID, this.CurrentMember.Name, nominal.ToString()));
                    sum += nominal;
                    bgwr.ReportProgress(sum);
                };
                cc.DelegateProcessMessage = ProcessMessage;
                cc.Reset();
                var rubs = Enum.GetValues(typeof(Logic.SerialPort.CashCode.RUB));
                var lrubs = rubs.OfType<Logic.SerialPort.CashCode.RUB>().ToList();
                cc.EnableBillTypes(lrubs);
                var res = cc.PollingLoop(ushort.MaxValue, (uint)OriflameApplication.Instance.CashCodeInterval);
                cc.Reset();
                cc.CloseComPort();
            };
            bgwr.ProgressChanged += (o, e) =>
            {
                this.sum = e.ProgressPercentage;
                tb_cash.Text = ((double)e.ProgressPercentage).ToString();
            };
            
            bgwr.RunWorkerAsync();
        }

        
        void ProcessMessage(uint error, string message)
        {
            
        }

        public Logic.Member CurrentMember { get; set; }
        private BackgroundWorker bgwr;
        private Logic.SerialPort.CashCode cc;
        private decimal sum;
    }
}
