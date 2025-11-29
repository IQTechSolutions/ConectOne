using System.Globalization;
using ConectOne.Domain.Entities;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Interfaces;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityModule.Infrastructure.Implimentation
{
    /// <summary>
    /// The AuditTrialsService provides methods to query and export audit trails related to user actions.
    /// Audit trails track changes (old/new values) to database records, who made them, and when.
    /// This service can return audit history for a given user, and export those logs to Excel for review.
    /// </summary>
    public sealed class AuditTrialsService : IAuditTrailsService
    {
        private readonly IExcelService _excelService;
        private readonly IRepository<Audit, int> _repository;

        /// <summary>
        /// Constructs the AuditTrialsService with the required Excel exporting service and audit repository.
        /// </summary>
        /// <param name="excelService">Service for exporting data to Excel.</param>
        /// <param name="repository">Repository to access audit entries in the database.</param>
        public AuditTrialsService(IExcelService excelService, IRepository<Audit, int> repository)
        {
            _excelService = excelService;
            _repository = repository;
        }

        /// <summary>
        /// Exports audit trails of a specific user to an Excel file.
        /// Fetches all trails for the given user, orders them by DateTime (descending),
        /// and applies column mappings for the export. Returns a Base64 string of the Excel file.
        /// Optional parameters (searchString, searchInOldValues, searchInNewValues) are currently not applied.
        /// </summary>
        /// <param name="userId">The ID of the user whose trails are being exported.</param>
        /// <param name="searchString">An optional string to filter records, not currently implemented.</param>
        /// <param name="searchInOldValues">An optional bool to filter by old values, not currently implemented.</param>
        /// <param name="searchInNewValues">An optional bool to filter by new values, not currently implemented.</param>
        /// <returns>An IBaseResult containing either a Base64 Excel file or an error message.</returns>
        public async Task<IBaseResult<string>> ExportToExcelAsync(string userId, string searchString = "", bool searchInOldValues = false, bool searchInNewValues = false)
        {
            try
            {
                // Fetch all audit trails for the specified user, ordering by newest entries first.
                var trails = await _repository.FindByCondition(c => c.UserId.Equals(userId), false).Data
                    .OrderByDescending(a => a.DateTime)
                    .ToListAsync();

                // Define column mappings for the Excel export:
                // TableName, Type, DateTime (Local and UTC), Primary Key, Old Values, New Values
                var data = await _excelService.ExportAsync(trails, sheetName: "Audit trails",
                    mappers: new Dictionary<string, Func<Audit, object>>
                    {
                        { "Table Name", item => item.TableName },
                        { "Type", item => item.Type },
                        { "Date Time (Local)", item => DateTime.SpecifyKind(item.DateTime, DateTimeKind.Utc).ToLocalTime().ToString("G", CultureInfo.CurrentCulture) },
                        { "Date Time (UTC)", item => item.DateTime.ToString("G", CultureInfo.CurrentCulture) },
                        { "Primary Key", item => item.PrimaryKey },
                        { "Old Values", item => item.OldValues },
                        { "New Values", item => item.NewValues },
                    });

                return Result<string>.Success(data.Data);
            }
            catch (Exception ex)
            {
                return Result<string>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the latest 250 audit trail entries for the given user.
        /// Maps each Audit entity to an AuditEntryDto for easier consumption by clients.
        /// Useful for displaying a user's recent changes, allowing an admin or user to review activity history.
        /// </summary>
        /// <param name="userId">The ID of the user whose trails should be retrieved.</param>
        /// <returns>A result containing an enumerable of AuditEntryDto or an error message.</returns>
        public async Task<IBaseResult<IEnumerable<AuditEntryDto>>> GetCurrentUserTrailsAsync(string userId)
        {
            try
            {
                // Fetch up to 250 most recent entries for this user
                var response = await _repository.FindByCondition(a => a.UserId == userId, false)
                    .Data.OrderByDescending(a => a.DateTime)
                    .Take(250)
                    .ToListAsync();

                // Map Audit entities to AuditEntryDto
                var mappedLogs = response.Select(entry => new AuditEntryDto()
                {
                    Id = entry.Id,
                    UserId = entry.UserId,
                    Type = entry.Type,
                    TableName = entry.TableName,
                    DateTime = entry.DateTime,
                    OldValues = entry.OldValues,
                    NewValues = entry.NewValues,
                    AffectedColumns = entry.AffectedColumns,
                    PrimaryKey = entry.PrimaryKey,
                });

                return Result<IEnumerable<AuditEntryDto>>.Success(mappedLogs);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<AuditEntryDto>>.Fail(ex.Message);
            }
        }
    }
}
