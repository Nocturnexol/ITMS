using Bitshare.Common;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace Bitshare.DataDecision.Common
{
    public class DoExport
    {
        /// <summary>
        /// 由DataTable导出Excel[web]
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <param name="sheetName">指定Excel工作表名称</param>
        /// <param name="xlsName">指定Excel工作表名称</param>
        /// <returns>Excel工作表</returns>
        public static string ExportDataTableToExcel(DataTable sourceTable, string sheetName, string xlsName)
        {
           
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(sheetName);
            IRow headerRow = sheet.CreateRow(0);
           
            foreach (DataColumn column in sourceTable.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                //headerRow.Cells[column.Ordinal].CellStyle
               
            }
            int rowIndex = 1;

            foreach (DataRow row in sourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in sourceTable.Columns)
                {
                    if (row[column].GetType() == typeof(DateTime))
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(Convert.ToDateTime(row[column]).ToString("yyyy-MM-dd"));
                    }
                    else if (row[column].GetType() == typeof(Decimal))
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(Convert.ToDecimal(row[column]).ToString("f2"));
                    }
                    else
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                }

                rowIndex++;
            }
            //for (int i = 0; i < sourceTable.Columns.Count; i++)
            //{
            //    sheet.AutoSizeColumn(i);   
            //}
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(basePath + "DownLoadFile"))
            {
                Directory.CreateDirectory(basePath + "DownLoadFile");
            }
            string dPath = "DownLoadFile\\" + xlsName + ".xls";
            string tPath = @basePath + dPath;

            FileStream filestream = new FileStream(tPath, FileMode.Create);
            workbook.Write(filestream);

            filestream.Close();

            sheet = null;
            headerRow = null;
            workbook = null;

            return tPath;
        }


        public static string ExportDataTableToExcel(DataTable sourceTable, string xlsName, HSSFWorkbook workbook, ISheet sheet, int rowIndex)
        {
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            foreach (DataRow row in sourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in sourceTable.Columns)
                {
                    ICell cell = dataRow.CreateCell(column.Ordinal);
                    cell.CellStyle = style;
                    cell.SetCellValue(row[column].ToString());
                }

                rowIndex++;
            }
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(basePath + "DownLoadFile"))
            {
                Directory.CreateDirectory(basePath + "DownLoadFile");
            }
            string dPath = "DownLoadFile\\" + xlsName + ".xls";
            string tPath = @basePath + dPath;

            FileStream filestream = new FileStream(tPath, FileMode.Create);
            workbook.Write(filestream);

            filestream.Close();

            sheet = null;
            workbook = null;

            return tPath;
        }
    }
}
