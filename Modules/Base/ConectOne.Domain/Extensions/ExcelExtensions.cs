using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ConectOne.Domain.Extensions
{
    /// <summary>
    /// Provides extension methods for exporting data to Excel format using the EPPlus library.
    /// </summary>
    /// <remarks>This class contains static methods that facilitate the creation and export of Excel files
    /// from collections of data. The methods are designed to simplify the process of mapping data to Excel worksheets,
    /// including support for custom column mappings and sheet naming. All methods are intended for use with the EPPlus
    /// library and require appropriate licensing for commercial use.</remarks>
    public static class ExcelExtensions
    {
        /// <summary>
        /// Exports the specified data to an Excel worksheet and returns the result as a Base64-encoded string.
        /// </summary>
        /// <remarks>The exported Excel file will contain a header row with the specified column names and
        /// data rows generated from the provided mappers. The method uses the EPPlus library and sets the license
        /// context to non-commercial use. The output is suitable for direct download or storage as an Excel file after
        /// Base64 decoding.</remarks>
        /// <typeparam name="TData">The type of the data items to export.</typeparam>
        /// <param name="data">The collection of data items to be exported to the Excel worksheet. Cannot be null.</param>
        /// <param name="mappers">A dictionary that maps column headers to functions extracting values from each data item. Each key
        /// represents a column header, and the corresponding function provides the cell value for that column. Cannot
        /// be null and must contain at least one entry.</param>
        /// <param name="sheetName">The name to assign to the Excel worksheet. Defaults to "Sheet1" if not specified.</param>
        /// <returns>A Base64-encoded string representing the generated Excel file. The string can be decoded to obtain the
        /// binary contents of the Excel file.</returns>
        public static async Task<string> ExportAsync<TData>(IEnumerable<TData> data, Dictionary<string, Func<TData, object>> mappers, string sheetName = "Sheet1")
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using var p = new ExcelPackage();
            p.Workbook.Properties.Author = "Connect";
            p.Workbook.Worksheets.Add("Audit Trails");
            var ws = p.Workbook.Worksheets[0];
            ws.Name = sheetName;
            ws.Cells.Style.Font.Size = 11;
            ws.Cells.Style.Font.Name = "Calibri";

            var colIndex = 1;
            var rowIndex = 1;

            var headers = mappers.Keys.Select(x => x).ToList();

            foreach (var header in headers)
            {
                var cell = ws.Cells[rowIndex, colIndex];

                var fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightBlue);

                var border = cell.Style.Border;
                border.Bottom.Style =
                    border.Top.Style =
                        border.Left.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;

                cell.Value = header;

                colIndex++;
            }

            var dataList = data.ToList();
            foreach (var item in dataList)
            {
                colIndex = 1;
                rowIndex++;

                var result = headers.Select(header => mappers[header](item));

                foreach (var value in result)
                {
                    ws.Cells[rowIndex, colIndex++].Value = value;
                }
            }

            using (ExcelRange autoFilterCells = ws.Cells[1, 1, dataList.Count + 1, headers.Count])
            {
                autoFilterCells.AutoFilter = true;
                autoFilterCells.AutoFitColumns();
            }

            var byteArray = await p.GetAsByteArrayAsync();
            var stringToReturn = Convert.ToBase64String(byteArray);
            return stringToReturn;
        }
    }
}
