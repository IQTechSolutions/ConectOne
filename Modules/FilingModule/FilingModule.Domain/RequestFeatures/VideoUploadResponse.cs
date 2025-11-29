namespace FilingModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the response returned after a video upload operation.
    /// </summary>
    /// <remarks>This record encapsulates details about the uploaded video, including its unique identifier, 
    /// the original file name, the file size in bytes, and the storage path.</remarks>
    /// <param name="VideoId">The unique identifier assigned to the uploaded video.</param>
    /// <param name="FileName">The name of the uploaded video file, including its extension.</param>
    /// <param name="Length">The size of the uploaded video file, in bytes.</param>
    /// <param name="Path">The storage path where the uploaded video is saved.</param>
    public record VideoUploadResponse(string VideoId, string FileName, long Length, string Path);
}
