using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using System;
using System.Data;
using System.IO;

namespace Bitshare.DataDecision.Common
{
    public class ExportExcelWorkbook
    {
        public static  HSSFWorkbook hssfworkbook;

        /// <summary>
        /// 获取excle模板
        /// </summary>
        /// <param name="ExcelName"></param>
        public static void InitializeWorkbook(string ExcelName)
        {
            //E:\visual studio 2010\Projects\文本解析\AjaxExportExcel\AjaxExportExcel
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            //basePath = Environment.CurrentDirectory;
            FileStream file = new FileStream(@basePath + "Excel\\" + ExcelName + ".xls", FileMode.Open, FileAccess.Read);
            hssfworkbook = new HSSFWorkbook(file);
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "NPOI Team";
            hssfworkbook.DocumentSummaryInformation = dsi;
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "NPOI SDK Example";
            hssfworkbook.SummaryInformation = si;
        }

        public static string WriteToFile(string ExcelName,DataTable dt)
        {
            InitializeWorkbook(ExcelName);
            NPOI.SS.UserModel.ISheet sheet1 = hssfworkbook.GetSheetAt(0);

            int g = 3;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int cIndex = g + i;
                sheet1.CreateRow(cIndex);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sheet1.GetRow(cIndex).CreateCell(j).CellStyle = sheet1.GetRow(1).GetCell(0).CellStyle;
                    sheet1.GetRow(cIndex).GetCell(j).SetCellValue(Convert.ToString(dt.Rows[i][j]));
                }
            }
            sheet1.ForceFormulaRecalculation = true;
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dPath = "Excel\\" + ExcelName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            string tPath = @basePath + dPath;
            FileStream file = new FileStream(tPath, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            return tPath;
        }
    }
}
