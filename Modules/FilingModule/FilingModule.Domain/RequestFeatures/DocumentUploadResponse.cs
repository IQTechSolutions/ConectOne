namespace FilingModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the response returned after a document upload operation.
    /// </summary>
    /// <remarks>This record encapsulates details about the uploaded document, including its unique
    /// identifier, original file name, size, and storage path.</remarks>
    /// <param name="DocumentId">The unique identifier assigned to the uploaded document.</param>
    /// <param name="FileName">The original name of the uploaded file.</param>
    /// <param name="Length">The size of the uploaded document in bytes.</param>
    /// <param name="Path">The storage path where the uploaded document is saved.</param>
    public record DocumentUploadResponse(string DocumentId, string FileName, long Length, string Path);
}
