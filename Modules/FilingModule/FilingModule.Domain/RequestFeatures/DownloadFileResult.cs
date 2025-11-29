namespace FilingModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the result of a file download operation, including success status and error information.
    /// </summary>
    /// <remarks>Use this class to determine whether a file download operation completed successfully and to
    /// access error details if the operation failed. The properties provide information about the outcome and any
    /// associated error messages.</remarks>
    public class DownloadFileResult
    {
        /// <summary>
        /// Gets or sets the error message associated with the current operation or validation result.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the name that identifies the error condition.
        /// </summary>
        public string ErrorName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the operation completed successfully.
        /// </summary>
        public bool Succeeded { get; set; }
    }
}
