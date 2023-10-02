using MessageChannel;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace RequestRep
{
    public class ChannelAdapter
    {
        private static int row = 1;
        Excel.Application oXL;
        Excel._Workbook oWB;
        Excel._Worksheet oSheet;
        Excel.Range oRng;

        public ChannelAdapter()
        {
            
        }
        public void WriteToExcel(string FlightNr, string Toa)
        {
            try
            {
                // Initialize Excel.
                oXL = new Excel.Application();
                oXL.Visible = true;

                // Create a new workbook and select the active worksheet.
                oWB = oXL.Workbooks.Add();
                oSheet = oWB.ActiveSheet;

                // Add table headers.
                oSheet.Cells[1, 1] = "Flight Number";
                oSheet.Cells[1, 2] = "Time of Arrival";

                // Format headers.
                oSheet.get_Range("A1", "B1").Font.Bold = true;
                oSheet.get_Range("A1", "B1").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                row++;
                oSheet.Cells[row, 1] = FlightNr;
                oSheet.Cells[row, 2] = Toa;
                

                // Added the following code to adjust the column widths to fit the content.
                oSheet.Columns[1].AutoFit();
                oSheet.Columns[2].AutoFit();

                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

            }


        }
        

    }
}
