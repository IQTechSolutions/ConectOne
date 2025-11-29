namespace FilingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents the result of an upload operation, including progress and total bytes uploaded.
    /// </summary>
    public class UploadResult
    {
        /// <summary>
        /// Gets or sets the progress of an operation as a percentage.
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// Gets or sets the total number of bytes processed or transferred.
        /// </summary>
        public long TotalBytes { get; set; }
    }
}