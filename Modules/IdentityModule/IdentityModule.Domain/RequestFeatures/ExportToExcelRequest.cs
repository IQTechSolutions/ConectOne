namespace IdentityModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters for an export to Excel operation, including user context and search criteria.
    /// </summary>
    public class ExportToExcelRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ExportToExcelRequest class.
        /// </summary>
        public ExportToExcelRequest() { }

        /// <summary>
        /// Initializes a new instance of the ExportToExcelRequest class with the specified user and search parameters.
        /// </summary>
        /// <param name="userId">The unique identifier of the user requesting the export. Cannot be null or empty.</param>
        /// <param name="searchString">The text to search for within the export data. Can be null or empty to match all records.</param>
        /// <param name="searchInOldValues">true to include old values in the search; otherwise, false.</param>
        /// <param name="searchInNewValues">true to include new values in the search; otherwise, false.</param>
        public ExportToExcelRequest(string userId, string searchString, bool searchInOldValues, bool searchInNewValues)
        {
            UserId=userId;
            SearchString=searchString;
            SearchInOldValues=searchInOldValues;
            SearchInNewValues=searchInNewValues;
        }
        
        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the search query used to filter results.
        /// </summary>
        public string SearchString { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether searches should include previous values in addition to current
        /// values.
        /// </summary>
        public bool SearchInOldValues { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether searches should include newly added values.
        /// </summary>
        public bool SearchInNewValues { get; set; }
    }
}
