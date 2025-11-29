using IdentityModule.Domain.RequestFeatures;

namespace IdentityModule.Domain.Interfaces
{
    /// <summary>
    /// Interface for exporting user information to various formats.
    /// </summary>
    public interface IExportService
    {
        /// <summary>
        /// Exports users matching the parameters to an Excel file.
        /// </summary>
        /// <param name="pageParameters">Filtering and paging parameters.</param>
        /// <returns>The exported file as a base64 string.</returns>
        Task<string> ExportToExcelAsync(UserPageParameters pageParameters);
    }
}
