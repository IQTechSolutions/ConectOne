using FilingModule.Domain.Entities;
using FilingModule.Domain.RequestFeatures;

namespace FilingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for a document, used to transfer document data between layers of the application.
    /// </summary>
    public record DocumentDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDto"/> class.
        /// </summary>
        public DocumentDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDto"/> class using the specified document upload
        /// response.
        /// </summary>
        /// <remarks>This constructor maps the properties of the <see cref="DocumentUploadResponse"/> to
        /// the corresponding properties of the <see cref="DocumentDto"/>.</remarks>
        /// <param name="response">The response containing details about the uploaded document. Cannot be <see langword="null"/>.</param>
        public DocumentDto(DocumentUploadResponse response)
        {
            Id = response.DocumentId;
            FileName = response.FileName;
            Size = response.Length;
            RelativePath = response.Path;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier of the document.
        /// </summary>
        public string? Id { get; init; }

        /// <summary>
        /// Gets the unique identifier of the parent entity associated with the document.
        /// </summary>
        public string? ParentId { get; init; }

        /// <summary>
        /// Gets the title of the document.
        /// </summary>
        public string Title { get; init; } = null!;

        /// <summary>
        /// Gets the description of the document.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets a value indicating whether the document is public.
        /// </summary>
        public bool IsPublic { get; init; }

        /// <summary>
        /// Gets the user who created the document.
        /// </summary>
        public string? CreatedBy { get; init; }

        /// <summary>
        /// Gets the date and time when the document was created.
        /// </summary>
        public DateTime? CreatedOn { get; init; }

        /// <summary>
        /// Gets the URL of the document.
        /// </summary>
        public string? Url { get; init; }

        /// <summary>
        /// Gets or sets the file name of the document.
        /// </summary>
        public string FileName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the content type of the document.
        /// </summary>
        public string? ContentType { get; set; }

        /// <summary>
        /// Gets or sets the size of the document in bytes.
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the relative path of the document.
        /// </summary>
        public string? RelativePath { get; set; }

        /// <summary>
        /// Gets or sets the folder path where the document is stored.
        /// </summary>
        public string? FolderPath { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts an <see cref="EntityDocument{T, string}"/> instance to a <see cref="DocumentDto"/>.
        /// </summary>
        /// <typeparam name="T">The type of the entity associated with the document.</typeparam>
        /// <param name="c">The <see cref="EntityDocument{T, string}"/> instance to convert. Cannot be <c>null</c>.</param>
        /// <returns>A <see cref="DocumentDto"/> representing the converted document.</returns>
        public static DocumentDto ToDto<T>(EntityDocument<T, string> c)
        {
            return new DocumentDto
            {
                Id = c.Id,
                Title = c.Document.DisplayName,
                Description = c.Document.Description,
                IsPublic = c.Document.IsPublic,
                CreatedBy = c.CreatedBy,
                CreatedOn = c.CreatedOn,
                Url = c.Document.RelativePath,
                ParentId = c.EntityId

            };
        }

        #endregion
    }
}
