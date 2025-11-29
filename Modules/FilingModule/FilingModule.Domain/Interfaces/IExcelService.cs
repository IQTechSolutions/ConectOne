using System.Data;
using ConectOne.Domain.ResultWrappers;

namespace FilingModule.Domain.Interfaces
{
    /// <summary>
    /// Defines operations for exporting data to and importing data from Excel files.
    /// This service abstraction allows business logic to handle Excel-related tasks without
    /// depending on a specific Excel library or implementation.
    /// </summary>
    public interface IExcelService
    {
        /// <summary>
        /// Exports a collection of data into a single Excel sheet.
        /// </summary>
        /// <typeparam name="TData">The type of the data items to be exported.</typeparam>
        /// <param name="data">An enumerable of data items to export into the Excel sheet.</param>
        /// <param name="mappers">
        /// A dictionary where keys are column headers (strings) and values are functions that
        /// map each data item (TData) to the corresponding column value (object). This provides
        /// a flexible mechanism to determine which properties of TData go into which columns.
        /// </param>
        /// <param name="sheetName">The name of the worksheet. Defaults to "Sheet1".</param>
        /// <returns>
        /// An IBaseResult<string> containing the file path or a reference to the generated Excel file if successful,
        /// or error messages if the operation fails.
        /// </returns>
        Task<IBaseResult<string>> ExportAsync<TData>(IEnumerable<TData> data, Dictionary<string, Func<TData, object>> mappers, string sheetName = "Sheet1");

        /// <summary>
        /// Exports multiple lists of data into a single Excel file, with each list placed on a separate sheet.
        /// </summary>
        /// <typeparam name="TData">The type of the data items to be exported.</typeparam>
        /// <param name="data">
        /// A dictionary where each key is a sheet name and the value is a list of items to be placed in that sheet.
        /// This allows generating complex Excel files with multiple worksheets representing different data sets.
        /// </param>
        /// <param name="mappers">
        /// A dictionary mapping column headers to functions that extract values from the data items.
        /// The same mapping is applied to each sheet, so the data sets should be compatible with these mappings.
        /// </param>
        /// <returns>
        /// An IBaseResult<string> containing the file path or identifier of the generated Excel file if successful,
        /// or error messages if the operation fails.
        /// </returns>
        Task<IBaseResult<string>> ExportMultipleSheetsAsync<TData>(Dictionary<string, List<TData>> data, Dictionary<string, Func<TData, object>> mappers);

        /// <summary>
        /// Imports data from an Excel file into an enumerable of TEntity objects.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to populate from the Excel rows.</typeparam>
        /// <param name="data">A stream representing the uploaded Excel file's content.</param>
        /// <param name="mappers">
        /// A dictionary mapping column headers or indices to functions that take a DataRow and an entity instance,
        /// returning the mapped value. This allows customized reading and parsing from each row into TEntity properties.
        /// </param>
        /// <param name="sheetName">The worksheet name from which to import data. Defaults to "Sheet1".</param>
        /// <returns>
        /// An IBaseResult<IEnumerable<TEntity>> containing the resulting entities if successful,
        /// or error messages if the import fails.
        /// </returns>
        Task<IBaseResult<IEnumerable<TEntity>>> ImportAsync<TEntity>(Stream data, Dictionary<string, Func<DataRow, TEntity, object>> mappers, string sheetName = "Sheet1");
    }
}
