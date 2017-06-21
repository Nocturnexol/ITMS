using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;

namespace Bitshare.DataDecision.Common
{
    /// <summary>
    /// Excle文件读写操作类
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 把DataTable转换成Excel文件流
        /// </summary>
        /// <param name="dataSource">DataTable数据源</param>
        /// <returns></returns>
        public static MemoryStream RenderDataToExcel(DataTable dataSource)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();
            HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
            // handling header.        
            foreach (DataColumn column in dataSource.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            }
            // handling value.        
            int rowIndex = 1;
            foreach (DataRow row in dataSource.Rows)
            {
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dataSource.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());

                    //设置超链接
                    //HSSFCell cell = (HSSFCell)dataRow.CreateCell(column.Ordinal);
                    //cell.SetCellValue("value");
                    //HSSFHyperlink link = new HSSFHyperlink(NPOI.SS.UserModel.HyperlinkType.URL);
                    //link.Address = "url";
                    //cell.Hyperlink = link;
                }
                rowIndex++;
            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            headerRow = null;
            workbook = null;
            return ms;
        }
        /// <summary>
        /// Excel文件导成Datatable
        /// </summary>
        /// <param name="strFilePath">Excel文件目录地址</param>
        /// <param name="strTableName">Datatable表名</param>
        /// <param name="iSheetIndex">Excel sheet index</param>
        /// <returns></returns>
        public static System.Data.DataTable XlSToDataTable2003(string strFilePath, string strTableName = "", int iSheetIndex = 0)
        {

            string strExtName = Path.GetExtension(strFilePath);

            System.Data.DataTable dt = new System.Data.DataTable();
            if (!string.IsNullOrEmpty(strTableName))
            {
                dt.TableName = strTableName;
            }

            if (strExtName.Equals(".xls"))
            {
                using (FileStream file = new FileStream(strFilePath, FileMode.Open, FileAccess.Read))
                {
                    HSSFWorkbook wk = new HSSFWorkbook(file);
                    ISheet sheet = wk.GetSheetAt(iSheetIndex);
                    if (sheet == null || sheet.LastRowNum < 1)
                    {
                        return null;
                    }
                    //列头
                    foreach (ICell item in sheet.GetRow(sheet.FirstRowNum).Cells)
                    {
                        dt.Columns.Add(item.ToString(), typeof(string));
                    }

                    //写入内容
                    System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                    while (rows.MoveNext())
                    {
                        IRow row = (HSSFRow)rows.Current;
                        if (row.RowNum == sheet.FirstRowNum)
                        {
                            continue;
                        }

                        DataRow dr = dt.NewRow();
                        foreach (ICell item in row.Cells)
                        {
                            switch (item.CellType)
                            {
                                case CellType.Boolean:
                                    dr[item.ColumnIndex] = item.BooleanCellValue;
                                    break;
                                case CellType.Error:
                                    dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                    break;
                                case CellType.Formula:
                                    switch (item.CachedFormulaResultType)
                                    {
                                        case CellType.Boolean:
                                            dr[item.ColumnIndex] = item.BooleanCellValue;
                                            break;
                                        case CellType.Error:
                                            dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                            break;
                                        case CellType.Numeric:
                                            if (DateUtil.IsCellDateFormatted(item))
                                            {
                                                dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd");
                                            }
                                            else
                                            {
                                                dr[item.ColumnIndex] = item.NumericCellValue;
                                            }
                                            break;
                                        case CellType.String:
                                            string str = item.StringCellValue;
                                            if (!string.IsNullOrEmpty(str))
                                            {
                                                dr[item.ColumnIndex] = str.ToString();
                                            }
                                            else
                                            {
                                                dr[item.ColumnIndex] = null;
                                            }
                                            break;
                                        case CellType.Unknown:
                                        case CellType.Blank:
                                        default:
                                            dr[item.ColumnIndex] = string.Empty;
                                            break;
                                    }
                                    break;
                                case CellType.Numeric:
                                    if (DateUtil.IsCellDateFormatted(item))
                                    {
                                        dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        dr[item.ColumnIndex] = item.NumericCellValue;
                                    }
                                    break;
                                case CellType.String:
                                    string strValue = item.StringCellValue;
                                    if (!string.IsNullOrEmpty(strValue))
                                    {
                                        dr[item.ColumnIndex] = strValue.ToString();
                                    }
                                    else
                                    {
                                        dr[item.ColumnIndex] = null;
                                    }
                                    break;
                                case CellType.Unknown:
                                case CellType.Blank:
                                default:
                                    dr[item.ColumnIndex] = string.Empty;
                                    break;
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }

            return dt;
        }


        /// <summary>
        /// Excel文件导成Datatable
        /// </summary>
        /// <param name="strFilePath">Excel文件目录地址</param>
        /// <param name="strTableName">Datatable表名</param>
        /// <param name="iSheetIndex">Excel sheet index</param>
        /// <returns></returns>
        public static System.Data.DataTable XlSToDataTable2007(string strFilePath, string strTableName = "", int iSheetIndex = 0)
        {

            string strExtName = Path.GetExtension(strFilePath);

            System.Data.DataTable dt = new System.Data.DataTable();
            if (!string.IsNullOrEmpty(strTableName))
            {
                dt.TableName = strTableName;
            }

            if (strExtName.Equals(".xlsx"))
            {
                using (FileStream file = new FileStream(strFilePath, FileMode.Open, FileAccess.Read))
                {
                    XSSFWorkbook wk = new XSSFWorkbook(file);
                    ISheet sheet = wk.GetSheetAt(iSheetIndex);

                    if (sheet == null || sheet.LastRowNum < 1)
                    {
                        return null;
                    }


                    //列头
                    foreach (ICell item in sheet.GetRow(sheet.FirstRowNum).Cells)
                    {
                        dt.Columns.Add(item.ToString(), typeof(string));
                    }

                    //写入内容
                    System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                    while (rows.MoveNext())
                    {
                        IRow row = (XSSFRow)rows.Current;
                        if (row.RowNum == sheet.FirstRowNum)
                        {
                            continue;
                        }

                        DataRow dr = dt.NewRow();
                        foreach (ICell item in row.Cells)
                        {
                            switch (item.CellType)
                            {
                                case CellType.Boolean:
                                    dr[item.ColumnIndex] = item.BooleanCellValue;
                                    break;
                                case CellType.Error:
                                    dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                    break;
                                case CellType.Formula:
                                    switch (item.CachedFormulaResultType)
                                    {
                                        case CellType.Boolean:
                                            dr[item.ColumnIndex] = item.BooleanCellValue;
                                            break;
                                        case CellType.Error:
                                            dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                            break;
                                        case CellType.Numeric:
                                            if (DateUtil.IsCellDateFormatted(item))
                                            {
                                                dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd");
                                            }
                                            else
                                            {
                                                dr[item.ColumnIndex] = item.NumericCellValue;
                                            }
                                            break;
                                        case CellType.String:
                                            string str = item.StringCellValue;
                                            if (!string.IsNullOrEmpty(str))
                                            {
                                                dr[item.ColumnIndex] = str.ToString();
                                            }
                                            else
                                            {
                                                dr[item.ColumnIndex] = null;
                                            }
                                            break;
                                        case CellType.Unknown:
                                        case CellType.Blank:
                                        default:
                                            dr[item.ColumnIndex] = string.Empty;
                                            break;
                                    }
                                    break;
                                case CellType.Numeric:
                                    if (DateUtil.IsCellDateFormatted(item))
                                    {
                                        dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        dr[item.ColumnIndex] = item.NumericCellValue;
                                    }
                                    break;
                                case CellType.String:
                                    string strValue = item.StringCellValue;
                                    if (!string.IsNullOrEmpty(strValue))
                                    {
                                        dr[item.ColumnIndex] = strValue.ToString();
                                    }
                                    else
                                    {
                                        dr[item.ColumnIndex] = null;
                                    }
                                    break;
                                case CellType.Unknown:
                                case CellType.Blank:
                                default:
                                    dr[item.ColumnIndex] = string.Empty;
                                    break;
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }

            return dt;
        }
        /// <summary>
        /// Excel文件导成Datatable
        /// </summary>
        /// <param name="strFilePath">Excel文件目录地址</param>
        /// <param name="strTableName">Datatable表名</param>
        /// <param name="iSheetIndex">Excel sheet index</param>
        /// <returns></returns>
        public static System.Data.DataTable XlSToDataTable(string strFilePath, string strTableName = "", int iSheetIndex = 0)
        {
            string strExtName = Path.GetExtension(strFilePath);
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                if (strExtName.Equals(".xls"))
                {
                    dt = XlSToDataTable2003(strFilePath);
                }
                if (strExtName.Equals(".xlsx"))
                {
                    dt = XlSToDataTable2007(strFilePath);
                }
            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }

        /// <summary>
        /// 把DataSet转换成Excel文件流
        /// </summary>
        /// <param name="dataSource">Da源taTable数据</param>
        /// <returns></returns>
        public static MemoryStream RenderDataToExcel(DataSet dataSource)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = null;
            HSSFRow headerRow = null;
            string tableName = string.Empty;
            foreach (DataTable table in dataSource.Tables)
            {
                if (string.IsNullOrWhiteSpace(table.TableName))
                {
                    sheet = (HSSFSheet)workbook.CreateSheet();
                }
                else
                {
                    sheet = (HSSFSheet)workbook.CreateSheet(table.TableName);
                }
                headerRow = (HSSFRow)sheet.CreateRow(0);
                // handling header.        
                foreach (DataColumn column in table.Columns)
                {
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                }
                // handling value.        
                int rowIndex = 1;
                foreach (DataRow row in table.Rows)
                {
                    HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in table.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                    rowIndex++;
                }
            }

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            headerRow = null;
            workbook = null;
            return ms;
        }

        /// <summary>
        /// 把DataTable数据写入到Excle文件
        /// </summary>
        /// <param name="dataSource">DataTable数据源</param>
        /// <param name="FileName">Excel文件全路径</param>
        public static void ExportDataToExcel(DataTable dataSource, string FileName)
        {
            MemoryStream ms = RenderDataToExcel(dataSource);
            FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            byte[] data = ms.ToArray();
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
            data = null;
            ms = null;
            fs = null;
        }

        /// <summary>
        /// 把DataSet数据写入到Excle文件
        /// </summary>
        /// <param name="dataSource">DataSet数据源</param>
        /// <param name="FileName">Excel文件全路径</param>
        public static void RenderDataToExcel(DataSet dataSource, string FileName)
        {
            MemoryStream ms = RenderDataToExcel(dataSource) as MemoryStream;
            FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            byte[] data = ms.ToArray();
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
            data = null;
            ms = null;
            fs = null;
        }

        /// <summary>
        /// 读取Excle文件数据
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="HeaderRowIndex">表头所在的行索引</param>
        /// <returns></returns>
        public static DataSet RenderDataFromExcel(string filePath, int HeaderRowIndex = 0)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !(filePath.EndsWith(".xls") || filePath.EndsWith(".xlsx")) || !File.Exists(filePath))
            {
                return null;
            }
            DataSet ds = new DataSet();
            Stream ExcelFileStream = File.OpenRead(filePath);
            HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            HSSFSheet sheet = null;
            DataTable table = null;
            int sheetCount = workbook.NumberOfSheets;
            for (int s = 0; s < sheetCount; s++)
            {
                sheet = (HSSFSheet)workbook.GetSheetAt(s);
                table = new DataTable(sheet.SheetName);
                HSSFRow headerRow = (HSSFRow)sheet.GetRow(HeaderRowIndex);
                int cellCount = headerRow.LastCellNum;
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }
                int rowCount = sheet.LastRowNum;
                for (int i = (sheet.FirstRowNum + 1); i < sheet.LastRowNum; i++)
                {
                    HSSFRow row = (HSSFRow)sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        dataRow[j] = row.GetCell(j).ToString();
                    }
                }
                ds.Tables.Add(table);
            }
            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            table = null;
            return ds;
        }

        /// <summary>
        /// 读取Excle文件数据
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="SheetName">Sheet名称</param>
        /// <param name="HeaderRowIndex">表头所在的行索引</param>
        /// <returns></returns>
        public static DataTable RenderDataFromExcel(string filePath, string SheetName, int HeaderRowIndex = 0)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !(filePath.EndsWith(".xls") || filePath.EndsWith(".xlsx")) || !File.Exists(filePath))
            {
                return null;
            }
            Stream ExcelFileStream = File.OpenRead(filePath);
            HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            HSSFSheet sheet = (HSSFSheet)workbook.GetSheet(SheetName);
            DataTable table = new DataTable();
            HSSFRow headerRow = (HSSFRow)sheet.GetRow(HeaderRowIndex);
            int cellCount = headerRow.LastCellNum;
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            int rowCount = sheet.LastRowNum;
            for (int i = (sheet.FirstRowNum + 1); i < sheet.LastRowNum; i++)
            {
                HSSFRow row = (HSSFRow)sheet.GetRow(i);
                DataRow dataRow = table.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    dataRow[j] = row.GetCell(j).ToString();
                }
            }
            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }

        /// <summary>
        /// 读取Excle文件数据
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="SheetIndex">Sheet索引</param>
        /// <param name="HeaderRowIndex">表头所在的行索引</param>
        /// <returns></returns>
        public static DataTable RenderDataFromExcel(string filePath, int SheetIndex = 0, int HeaderRowIndex = 0)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !(filePath.EndsWith(".xls") || filePath.EndsWith(".xlsx")) || !File.Exists(filePath))
            {
                return null;
            }
            Stream ExcelFileStream = File.OpenRead(filePath);
            HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            HSSFSheet sheet = (HSSFSheet)workbook.GetSheetAt(SheetIndex);
            DataTable table = new DataTable();
            HSSFRow headerRow = (HSSFRow)sheet.GetRow(HeaderRowIndex);
            int cellCount = headerRow.LastCellNum;
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            int rowCount = sheet.LastRowNum;
            for (int i = (sheet.FirstRowNum + 1); i < sheet.LastRowNum; i++)
            {
                HSSFRow row = (HSSFRow)sheet.GetRow(i);
                DataRow dataRow = table.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }
                table.Rows.Add(dataRow);
            }
            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }

        #region 采用OleDb方式读取Excle文件
        /// <summary>
        ///  Excel转换为DataTable
        /// </summary>
        /// <param name="filename">Excel文件全路径</param>
        public static System.Data.DataTable ReadExcel(string filename)
        {
            try
            {
                //通过Imex=1来把混合型作为文本型读取,避免null值
                string strConn = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + filename + ";Extended Properties='Excel 8.0; HDR=NO; IMEX=1'";
                string filenames = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));
                if (filenames == ".xlsx")
                {
                    strConn = "Provider=Microsoft.Ace.OleDb.12.0;Data Source=" + filename + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";
                }
                System.Data.DataTable dt_excel = new System.Data.DataTable();
                using (OleDbConnection conn = new OleDbConnection(strConn))
                {
                    conn.Open();
                    string strExcel = "";
                    OleDbDataAdapter myCommand = null;
                    System.Data.DataTable dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
                    //包含excel中表名的字符串数组
                    string[] strTableNames = new string[dtSheetName.Rows.Count];
                    for (int k = 0; k < dtSheetName.Rows.Count; k++)
                    {
                        strTableNames[k] = dtSheetName.Rows[k]["TABLE_NAME"].ToString();
                    }
                    strExcel = "select * from [" + strTableNames[0] + "]";
                    myCommand = new OleDbDataAdapter(strExcel, strConn);
                    myCommand.Fill(dt_excel);
                    conn.Close();
                }
                System.Data.DataTable dt = new System.Data.DataTable();
                if (filenames == ".xls")
                {
                    int colnum = 0;
                    for (int j = 0; j < dt_excel.Columns.Count; j++)
                    {
                        string colname = Convert.ToString(dt_excel.Rows[0][j]).Trim();
                        if (colname.Length == 0)
                        {
                            continue;
                        }
                        colnum = colnum + 1;
                        dt.Columns.Add(colname, typeof(string));
                    }
                    for (int k = 0 + 1; k < dt_excel.Rows.Count; k++)
                    {
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < colnum; j++)
                        {
                            dr[j] = dt_excel.Rows[k][j];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    dt = dt_excel;
                }
                return dt;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 读取Excel文件数据
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static System.Data.DataSet ReadExcelToDataSet(string filename)
        {
            DataSet ds = null;
            try
            {
                //通过Imex=1来把混合型作为文本型读取,避免null值
                string strConn = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + filename + ";Extended Properties='Excel 8.0; HDR=NO; IMEX=1'";
                string filenames = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));
                if (filenames == ".xlsx")
                {
                    strConn = "Provider=Microsoft.Ace.OleDb.12.0;Data Source=" + filename + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";
                }
                using (OleDbConnection conn = new OleDbConnection(strConn))
                {
                    conn.Open();
                    ds = new DataSet();
                    string strExcel = null;
                    OleDbDataAdapter myCommand = null;
                    System.Data.DataTable dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
                    string tableName = null;
                    System.Data.DataTable dt = null;
                    foreach (DataRow row in dtSheetName.Rows)
                    {
                        System.Data.DataTable dt_excel = new System.Data.DataTable();
                        tableName = row["TABLE_NAME"].ToString();
                        strExcel = "select * from [" + tableName + "]";
                        myCommand = new OleDbDataAdapter(strExcel, strConn);
                        myCommand.Fill(dt_excel);

                        #region 把第一行作为表头获取表结构
                        dt = new System.Data.DataTable();
                        if (filenames == ".xls")
                        {
                            int colnum = 0;
                            for (int j = 0; j < dt_excel.Columns.Count; j++)
                            {
                                string colname = Convert.ToString(dt_excel.Rows[0][j]).Trim();
                                if (string.IsNullOrWhiteSpace(colname))
                                {
                                    continue;
                                }
                                colnum++;
                                dt.Columns.Add(colname, typeof(string));
                            }
                            for (int k = 1; k < dt_excel.Rows.Count; k++)
                            {
                                DataRow dr = dt.NewRow();
                                for (int j = 0; j < colnum; j++)
                                {
                                    dr[j] = dt_excel.Rows[k][j];
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            dt = dt_excel;
                        }
                        #endregion

                        dt.TableName = tableName.TrimEnd('$');
                        ds.Tables.Add(dt);
                    }
                    conn.Close();
                }
            }
            catch
            {
                return null;
            }
            return ds;
        }
        #endregion

        #region 导出Excel文件
        /// <summary>
        /// 导出Excel文件
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="fileName">文件名(默认加“.xls”后缀)</param>
        public static void ExportExcel(DataSet dataSource, string fileName)
        {
            MemoryStream ms = RenderDataToExcel(dataSource);

            #region 检查文件名
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = string.Format("{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
            }
            else
            {
                fileName = fileName.ToLower().Trim();
                if (!(fileName.EndsWith(".xls") || fileName.EndsWith(".xlsx") || fileName.EndsWith(".csv")))
                {
                    fileName = fileName + ".xls";
                }
                if (fileName.Equals(".xls") || fileName.Equals(".xlsx") || fileName.Equals(".csv"))
                {
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fileName;
                }
            }
            #endregion

            ExportFile(ms, fileName, "application/ms-excel");
        }

        /// <summary>
        /// 导出Excel文件
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="fileName">文件名(默认加“.xls”后缀)</param>
        public static void ExportExcel(DataTable dataSource, string fileName)
        {
            MemoryStream ms = RenderDataToExcel(dataSource);

            #region 检查文件名
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = string.Format("{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
            }
            else
            {
                fileName = fileName.ToLower().Trim();
                if (!(fileName.EndsWith(".xls") || fileName.EndsWith(".xlsx") || fileName.EndsWith(".csv")))
                {
                    fileName = fileName + ".xls";
                }
                if (fileName.Equals(".xls") || fileName.Equals(".xlsx") || fileName.Equals(".csv"))
                {
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fileName;
                }
            }
            #endregion

            ExportFile(ms, fileName, "application/ms-excel");
        }

        /// <summary>
        /// 导出文件
        /// </summary>
        /// <param name="ms">文件流</param>
        /// <param name="fileName">保存时的默认文件名</param>
        /// <param name="ContentType">输出流的HTTP MIME类型</param>
        public static void ExportFile(MemoryStream ms, string fileName, string ContentType = "application/ms-excel")
        {
            if (string.IsNullOrWhiteSpace(ContentType))
            {
                ContentType = "application/ms-excel";
            }

            #region 输出可供下载的 Excle 文件
            // 注意：对文件名进行编码，防止出现中文乱码
            fileName = HttpUtility.UrlEncode(System.Text.Encoding.UTF8.GetBytes(fileName));
            // 添加头信息，为"文件下载/另存为"对话框指定默认文件名
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            // 添加头信息，指定文件大小，让浏览器能够显示下载进度 
            //response.AddHeader("Content-Length", file.Length.ToString()); 
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            // 指定返回的是一个不能被客户端读取的流，必须被下载 
            System.Web.HttpContext.Current.Response.ContentType = ContentType;
            //System.Web.HttpContext.Current.Response.Charset = "utf-8";

            //向HTTP输出流中写入取得的数据信息
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            System.Web.HttpContext.Current.Response.End();
            #endregion
        }
        #endregion
    }
}
