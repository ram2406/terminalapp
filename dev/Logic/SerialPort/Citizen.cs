using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Printing;
using System.Drawing.Printing;
using System.IO;
using System.Drawing;

namespace Logic.SerialPort
{
    public class Citizen
    {
        public Citizen()
        {
            // Associate the PrintPage event handler with the PrintPage event.
            printDocument1.PrintPage +=
                new PrintPageEventHandler(printDocument1_PrintPage);
        }
        public Font Font { get; set; }
        public Bitmap Bitmap { get; set; }
        private PrintDocument printDocument1 = new PrintDocument();
        public string StringToPrint { get; set; }

        public string DateTime { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Sum { get; set; }

        private void ReadFile()
        {
            StringToPrint = "test";
            return;
            string docName = "testPage.txt";
            string docPath = @"c:\";
            printDocument1.DocumentName = docName;
            using (FileStream stream = new FileStream(docPath + docName, FileMode.Open))
            using (StreamReader reader = new StreamReader(stream))
            {
                StringToPrint = reader.ReadToEnd();
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            string[][] strs = new string[5][];

            strs[0] = new string[] { "", "Oriflame" };
            strs[1] = new string[] { "Дата и время:", DateTime };
            strs[2] = new string[] { "Номер", Id };
            strs[3] = new string[] { "ФИО", Name };
            strs[4] = new string[] { "Сумма платежа, р.:", Sum };



            Size bound = new Size(250, 400);

            Font font1 = new System.Drawing.Font("Arial", 8);
            Font font2 = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            Pen linePen = new Pen(Brushes.Black);
            linePen.DashPattern = new float[] { 2, 4 };
            Point left = new Point(0, 5);
            Point right = new Point(bound.Width, 5);



            foreach (string[] item in strs)
            {
                e.Graphics.DrawLine(linePen, left, right);

                e.Graphics.DrawString(item[0], font1, Brushes.Black, new Point(left.X, left.Y + 10));

                int charactersOnPage = 0, linesPerPage = 0;

                Size currentBound = new System.Drawing.Size(150, 40);
                var stringSize = e.Graphics.MeasureString(item[1], font2,
                                            currentBound, StringFormat.GenericTypographic,
                                            out charactersOnPage, out linesPerPage);
                e.Graphics.DrawString(item[0], font1, Brushes.Black, new Point(left.X, left.Y + 10));
                e.Graphics.DrawString(item[1], font2, Brushes.Black, new Point(left.X + bound.Width - (int)stringSize.Width, left.Y + 10));


                left.Y = right.Y += 40;
            }

            

        }

        public void Print()
        {
            //ReadFile();

            printDocument1.PrinterSettings.PrinterName = "CITIZEN CT-S2000";
            printDocument1.PrintController = new StandardPrintController();
            printDocument1.Print();
        }
    }
}
