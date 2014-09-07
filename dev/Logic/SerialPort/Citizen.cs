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
        private PrintDocument printDocument1 = new PrintDocument();
        public string StringToPrint { get; set; }
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
            int charactersOnPage = 0;
            int linesPerPage = 0;
            SizeF bound = new SizeF(300, 300);
            // Sets the value of charactersOnPage to the number of characters  
            // of stringToPrint that will fit within the bounds of the page.
            e.Graphics.MeasureString(StringToPrint, this.Font,
                bound, StringFormat.GenericTypographic,
                out charactersOnPage, out linesPerPage);

            // Draws the string within the bounds of the page
            e.Graphics.DrawString(StringToPrint, this.Font, Brushes.Black,
                new Rectangle(0,0,300,300), StringFormat.GenericTypographic);

            // Remove the portion of the string that has been printed.
            StringToPrint = StringToPrint.Substring(charactersOnPage);

            // Check to see if more pages are to be printed.
            //e.HasMorePages = (StringToPrint.Length > 0);
        }

        public void Print()
        {
            //ReadFile();

            printDocument1.PrinterSettings.PrinterName = "CITIZEN CT-S2000";
            printDocument1.Print();
        }
    }
}
