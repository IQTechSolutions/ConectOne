using System.Data;
using System.Drawing;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FilingModule.Domain.Implementation
{
    /// <summary>
    /// Provides functionality to export and import Excel files using EPPlus,
    /// supporting both single and multiple sheets with dynamic column mapping.
    /// </summary>
    public class ExcelService : IExcelService
    {
        /// <summary>
        /// Exports data to an Excel file with multiple worksheets.
        /// </summary>
        /// <typeparam name="TData">The type of data to export.</typeparam>
        /// <param name="data">A dictionary where keys are sheet names and values are the data to write to those sheets.</param>
        /// <param name="mappers">A mapping of column headers to property selectors for <typeparamref name="TData"/>.</param>
        public async Task<IBaseResult<string>> ExportMultipleSheetsAsync<TData>(Dictionary<string, List<TData>> data, Dictionary<string, Func<TData, object>> mappers)
        {
            try
            {
                using var package = CreateExcelPackage();
                foreach (var (sheetName, items) in data)
                {
                    var worksheet = package.Workbook.Worksheets.Add(sheetName);
                    SetupWorksheetStyles(worksheet);
                    WriteHeaders(worksheet, mappers.Keys.ToList());
                    WriteDataRows(worksheet, items, mappers);
                }

                return await ConvertPackageToBase64Result(package);
            }
            catch (Exception ex)
            {
                return await Result<string>.FailAsync(ex.Message);
            }
        }

        /// <summary>
        /// Exports a list of data into a single Excel worksheet.
        /// </summary>
        /// <typeparam name="TData">The type of data to export.</typeparam>
        /// <param name="data">The list of items to export.</param>
        /// <param name="mappers">A mapping of column headers to property selectors.</param>
        /// <param name="sheetName">The name of the worksheet to create.</param>
        public async Task<IBaseResult<string>> ExportAsync<TData>(IEnumerable<TData> data, Dictionary<string, Func<TData, object>> mappers, string sheetName = "Sheet1")
        {
            try
            {
                using var package = CreateExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add(sheetName);
                SetupWorksheetStyles(worksheet);
                WriteHeaders(worksheet, mappers.Keys.ToList());
                WriteDataRows(worksheet, data.ToList(), mappers);

                return await ConvertPackageToBase64Result(package);
            }
            catch (Exception ex)
            {
                return await Result<string>.FailAsync(ex.Message);
            }
        }

        /// <summary>
        /// Imports data from an Excel stream into a list of entities using a column-to-property mapping.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to populate.</typeparam>
        /// <param name="stream">The input Excel file stream.</param>
        /// <param name="mappers">A dictionary mapping column headers to data row processing actions.</param>
        /// <param name="sheetName">The worksheet name to read from.</param>
        public async Task<IBaseResult<IEnumerable<TEntity>>> ImportAsync<TEntity>(Stream stream, Dictionary<string, Func<DataRow, TEntity, object>> mappers, string sheetName = "Sheet1")
        {
            try
            {
                using var package = new ExcelPackage();
                stream.Position = 0;
                await package.LoadAsync(stream);
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null)
                    return Result<IEnumerable<TEntity>>.Fail($"Sheet '{sheetName}' does not exist!");

                var dataTable = WorksheetToDataTable(worksheet);
                var result = new List<TEntity>();
                var errors = ValidateHeaders(dataTable, mappers.Keys.ToList());

                if (errors.Any()) return Result<IEnumerable<TEntity>>.Fail(errors);

                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    var entity = Activator.CreateInstance<TEntity>();
                    var dataRow = dataTable.Rows[row];
                    foreach (var key in mappers.Keys)
                        mappers[key](dataRow, entity);
                    result.Add(entity);
                }

                return Result<IEnumerable<TEntity>>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<TEntity>>.Fail(ex.Message);
            }
        }

        #region Helpers

        /// <summary>
        /// Creates a pre-configured <see cref="ExcelPackage"/> instance with author metadata.
        /// </summary>
        private static ExcelPackage CreateExcelPackage()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            package.Workbook.Properties.Author = "NeuralTech";
            return package;
        }

        /// <summary>
        /// Applies default styles to a worksheet.
        /// </summary>
        private static void SetupWorksheetStyles(ExcelWorksheet ws)
        {
            ws.Cells.Style.Font.Size = 11;
            ws.Cells.Style.Font.Name = "Calibri";
        }

        /// <summary>
        /// Writes column headers to the worksheet.
        /// </summary>
        private static void WriteHeaders(ExcelWorksheet ws, List<string> headers)
        {
            for (int col = 1; col <= headers.Count; col++)
            {
                var cell = ws.Cells[1, col];
                cell.Value = headers[col - 1];
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                var border = cell.Style.Border;
                border.Bottom.Style = border.Top.Style =
                    border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            }
        }

        /// <summary>
        /// Writes data rows to the worksheet using provided mappers.
        /// </summary>
        private static void WriteDataRows<TData>(ExcelWorksheet ws, List<TData> data, Dictionary<string, Func<TData, object>> mappers)
        {
            var headers = mappers.Keys.ToList();
            for (int row = 0; row < data.Count; row++)
            {
                for (int col = 0; col < headers.Count; col++)
                {
                    var value = mappers[headers[col]](data[row]);
                    ws.Cells[row + 2, col + 1].Value = value;
                }
            }

            ws.Cells[1, 1, data.Count + 1, headers.Count].AutoFilter = true;
            ws.Cells[1, 1, data.Count + 1, headers.Count].AutoFitColumns();
        }

        /// <summary>
        /// Converts a worksheet to a <see cref="DataTable"/>.
        /// </summary>
        private static DataTable WorksheetToDataTable(ExcelWorksheet worksheet)
        {
            var dt = new DataTable();
            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
            {
                dt.Columns.Add(worksheet.Cells[1, col].Text);
            }

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var newRow = dt.NewRow();
                for (int col = 1; col <= dt.Columns.Count; col++)
                {
                    newRow[col - 1] = worksheet.Cells[row, col].Text;
                }

                dt.Rows.Add(newRow);
            }

            return dt;
        }

        /// <summary>
        /// Validates that all required headers are present in a <see cref="DataTable"/>.
        /// </summary>
        private static List<string> ValidateHeaders(DataTable dt, List<string> requiredHeaders)
        {
            return requiredHeaders
                .Where(header => !dt.Columns.Contains(header))
                .Select(header => $"Missing header: {header}")
                .ToList();
        }

        /// <summary>
        /// Converts an ExcelPackage to a base64-encoded result.
        /// </summary>
        private static async Task<IBaseResult<string>> ConvertPackageToBase64Result(ExcelPackage package)
        {
            var byteArray = await package.GetAsByteArrayAsync();
            var base64String = Convert.ToBase64String(byteArray);
            return await Result<string>.SuccessAsync(data: base64String);
        }

        #endregion
    }
}
