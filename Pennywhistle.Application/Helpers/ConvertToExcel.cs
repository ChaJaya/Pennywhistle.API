using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace Pennywhistle.Application.Helpers
{
    /// <summary>
    /// Excel converter impelentation
    /// </summary>
    public class ConvertToExcel
    {
        #region Public Methods
        /// <summary>
        /// Convert work sheet to user list.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <typeparam name="T">Type of the value to be returned</typeparam>
        /// <returns>The data table.</returns>
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();
            foreach (var prop in typeof(T).GetProperties())
            {
                var array = prop.GetCustomAttributes(typeof(DisplayNameAttribute), false);
                if (array.Length > 0)
                {
                    table.Columns.Add((array[0] as DisplayNameAttribute).DisplayName);
                }
                else
                {
                    table.Columns.Add(prop.Name);
                }
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (var prop in typeof(T).GetProperties())
                {
                    var array = prop.GetCustomAttributes(typeof(DisplayNameAttribute), false);
                    if (array.Length > 0)
                    {
                        row[(array[0] as DisplayNameAttribute).DisplayName] = prop.GetValue(item) ?? DBNull.Value;
                    }
                    else
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }
                }

                table.Rows.Add(row);
            }

            for (int i = 0; i < table.Columns.Count; i++)
            {
                string newValue = Regex.Replace(table.Columns[i].ColumnName, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
                table.Columns[i].ColumnName = newValue;
                table.AcceptChanges();
            }


            return table;
        }

        /// <summary>
        /// Convert a data table to a work sheet.
        /// </summary>
        /// <param name="filePath">The excel file path.</param>
        /// <param name="dataTable">The list of unsuccessful users.</param>
        public static void ConvertDataTableToExcel(string filePath, DataTable dataTable)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };

                sheets.Append(sheet);

                Row headerRow = new Row();

                List<string> columns = new List<string>();
                foreach (System.Data.DataColumn column in dataTable.Columns)
                {
                    columns.Add(column.ColumnName);

                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(column.ColumnName);
                    headerRow.AppendChild(cell);
                }

                sheetData.AppendChild(headerRow);

                foreach (DataRow dsrow in dataTable.Rows)
                {
                    Row newRow = new Row();
                    foreach (string col in columns)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(dsrow[col].ToString());
                        newRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(newRow);
                }

                workbookPart.Workbook.Save();
            }
        }

        /// <summary>
        /// Convert work sheet to user list.
        /// </summary>
        /// <param name="filePath">The excel file path.</param>
        /// <returns>The data table.</returns>
        public static DataTable ConvertExcelToDataTable(string filePath)
        {
            DataTable dt = new DataTable();

            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filePath, false))
            {
                WorkbookPart workbookPart = doc.WorkbookPart;
                IEnumerable<Sheet> sheets = doc.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)doc.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                foreach (Cell cell in rows.ElementAt(0))
                {
                    if (cell.CellValue != null)
                    {
                        dt.Columns.Add(ConvertToExcel.GetCellValue(doc, cell));
                    }
                }

                foreach (Row row in rows)
                {
                    if (row.Descendants<Cell>().Count() > 0)
                    {
                        DataRow tempRow = dt.NewRow();
                        if (row.Descendants<Cell>().ElementAt(0).CellValue != null)
                        {
                            for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                            {
                                Cell cell = row.Descendants<Cell>().ElementAt(i);
                                if (cell.CellValue != null)
                                {
                                    int actualCellIndex = CellReferenceToIndex(cell);
                                    if (actualCellIndex < tempRow.ItemArray.Count())
                                    {
                                        tempRow[actualCellIndex] = GetCellValue(doc, cell);
                                    }
                                }
                            }
                        }

                        dt.Rows.Add(tempRow);
                    }
                }
            }

            return dt;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get cell value.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="cell">The cell.</param>
        /// <returns>The string.</returns>
        private static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {

            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[int.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }

        }

        /// <summary>
        /// Get cell reference index.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>The index.</returns>
        private static int CellReferenceToIndex(Cell cell)
        {

            int index = 0;
            string reference = cell.CellReference.ToString().ToUpper();
            foreach (char ch in reference)
            {
                if (char.IsLetter(ch))
                {
                    int value = (int)ch - (int)'A';
                    index = (index == 0) ? value : ((index + 1) * 26) + value;
                }
                else
                {
                    return index;
                }
            }

            return index;

        }
        #endregion
    }
}
