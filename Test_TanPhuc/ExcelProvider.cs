using ExcelDataReader;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_TanPhuc
{
    internal class ExcelProvider
    {
        private static DataTable _excelDataTable; 
        private static DataTable ReadExcel(string filePath)
        {               
            if (_excelDataTable != null)
                return _excelDataTable;

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using(var stream = File.Open(filePath,FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    DataSet dataSet = reader.AsDataSet();
                    _excelDataTable = dataSet.Tables[1]; // Test case for TanPhuc
                    return _excelDataTable;
                }
            }
        }
        public static int rowIndex = 0;
        private static int colIndexActual = 7;
        public static IEnumerable<TestCaseData> GetDataForAddRoom(int start,int end)
        {
            rowIndex=start;
            var testCases = new List<TestCaseData>();
            DataTable excelDataTable = ReadExcel("C:\\Users\\phuct\\OneDrive - Ho Chi Minh City University of Foreign Languages and Information Technology - HUFLIT\\TestCase_BDCLPM_HK2.xlsx");
            for(int i= start-1; i< end; i++)
            {
                var testData = excelDataTable.Rows[i][4];
                var exp = excelDataTable.Rows[i][5];
                testCases.Add(new TestCaseData(testData,exp));
            }
            return testCases;
        }

       

        public static void WriteResultToExcel(string filePath, string sheetName, string actual, string result)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet wordsheet = package.Workbook.Worksheets[sheetName] ?? package.Workbook.Worksheets[0];

                    //write value actual in position at rowIndex and colIndex
                    wordsheet.Cells[rowIndex, colIndexActual].Value = actual;
                    wordsheet.Cells[rowIndex, colIndexActual + 1].Value = result;
                    package.Save();
                    rowIndex++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
