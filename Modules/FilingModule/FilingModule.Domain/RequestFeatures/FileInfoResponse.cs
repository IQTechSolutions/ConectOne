namespace FilingModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents information about a file, including its name, size, and the date and time it was last modified.
    /// </summary>
    /// <remarks>This record is typically used to encapsulate metadata about a file for use in file management
    /// or processing operations.</remarks>
    public record FileInfoResponse(string FileName, long Length, DateTime DateTime);
}
