using FilingModule.Domain.DataTransferObjects;

namespace FilingModule.Application.ViewModels
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for a document file.
    /// This DTO is used to transfer document file data between different layers of the application.
    /// </summary>
    public class DocumentFileDto
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the size of the file in bytes.
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Gets or sets the URL of the file.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Gets or sets the content type of the file.
        /// </summary>
        public string? ContentType { get; set; }

        /// <summary>
        /// Gets the display name of the file, which is a combination of the file name and content type.
        /// </summary>
        public string? DisplayName => FileName;

        #endregion
    }

    /// <summary>
    /// Represents the view model for creating a document link URL.
    /// This view model encapsulates the URL and a collection of document files.
    /// </summary>
    public class CreateDocumentLinkUrlViewModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the URL for the document link.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the collection of document files associated with the URL.
        /// </summary>
        public ICollection<DocumentDto> Documents { get; set; } = new List<DocumentDto>();

        #endregion
    }
}