using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;

namespace Pennywhistle.Application.Helpers
{
    /// <summary>
    /// Excel genarator implementation
    /// </summary>
    public class ExcelGenerator
    {
        #region Variables

        /// <summary>
        /// sheet 1
        /// </summary>
        private static ExcelWorksheet ws;
        #endregion

        #region Public Methods
        /// <summary>
        /// generate excel from a list of entities
        /// </summary>
        /// <typeparam name="T">List of entities</typeparam>
        /// <param name="data">the Entity</param>
        /// <param name="sheetName">sheet name</param>
        /// <returns>a byte array of the content</returns>
        public static ByteArrayContent GenerateExcelFromList<T>(IList<T> data, string sheetName)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                ws = pck.Workbook.Worksheets.Add(sheetName);
                DataTable datatable = ConvertToExcel.ConvertToDataTable(data);
                ws.Cells["A1"].LoadFromDataTable(datatable, true, TableStyles.Dark10);
                ApplyStyling(datatable.Columns.Count);
                return new ByteArrayContent(pck.GetAsByteArray());
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Apply styling for the sheet
        /// </summary>
        /// <param name="columnCount">the column count</param>
        private static void ApplyStyling(int columnCount)
        {
            // apply auto formating for column size
            ws.Cells.AutoFitColumns();

            // apply header formatting

            // font bold
            ws.Row(1).Style.Font.Bold = true;

            // background colour - orange
            System.Drawing.Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#F28200");
            ws.Cells[1, 1, 1, columnCount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[1, 1, 1, columnCount].Style.Fill.BackgroundColor.SetColor(colFromHex);

            // font colour - white
            ws.Cells[1, 1, 1, columnCount].Style.Font.Color.SetColor(System.Drawing.Color.White);

            // freeze the topics row (first row)
            ws.View.FreezePanes(2, 1);
        } 
        #endregion
    }
}
