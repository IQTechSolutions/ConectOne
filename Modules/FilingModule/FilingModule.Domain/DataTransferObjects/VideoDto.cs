using FilingModule.Domain.Entities;

namespace FilingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a video, containing metadata and file information.
    /// </summary>
    /// <remarks>This class is used to encapsulate video-related data for transfer between application layers.
    /// It includes properties for identifying the video, describing its content, and providing file details such as
    /// size, content type, and storage paths.</remarks>
    public record VideoDto
    {
        #region Properties

        /// <summary>
        /// Gets the unique identifier for the entity.
        /// </summary>
        public string? Id { get; init; }

        /// <summary>
        /// Gets the unique identifier for the entity.
        /// </summary>
        public string? EntityId { get; init; }

        /// <summary>
        /// Gets the name associated with the current instance.
        /// </summary>
        public string Name { get; init; } = null!;

        /// <summary>
        /// Gets the caption or title associated with the object.
        /// </summary>
        public string? Caption { get; init; }

        /// <summary>
        /// Gets the description associated with the current object.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets the URL associated with the current instance.
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
        /// Converts an <see cref="EntityVideo{T, string}"/> instance to a <see cref="VideoDto"/>.
        /// </summary>
        /// <typeparam name="T">The type of the entity associated with the video.</typeparam>
        /// <param name="c">The <see cref="EntityVideo{T, string}"/> instance to convert. Cannot be null.</param>
        /// <returns>A <see cref="VideoDto"/> representing the video details of the specified entity.</returns>
        public static VideoDto ToDto<T>(EntityVideo<T, string> c)
        {
            return new VideoDto
            {
                Id = c.Id,
                Name = c.Video.DisplayName,
                Caption = c.Video.Caption,
                Description = c.Video.Description,
                Url = c.Video.RelativePath,
                FileName = c.Video.FileName,
                ContentType = c.Video.ContentType,
                Size = c.Video.Size,
                RelativePath = c.Video.RelativePath,
                EntityId = c.EntityId
            };
        }

        /// <summary>
        /// Converts a <see cref="Video"/> object to a <see cref="VideoDto"/> object.
        /// </summary>
        /// <typeparam name="T">The type parameter is not used in this method but may be required for compatibility with generic contexts.</typeparam>
        /// <param name="c">The <see cref="Video"/> instance to convert. Cannot be <see langword="null"/>.</param>
        /// <returns>A <see cref="VideoDto"/> object containing the mapped properties from the <see cref="Video"/> instance.</returns>
        public static VideoDto ToDto(Video c)
        {
            return new VideoDto
            {
                Id = c.Id,
                Name = c.DisplayName,
                Caption = c.Caption,
                Description = c.Description,
                Url = c.RelativePath,
                FileName = c.FileName,
                ContentType = c.ContentType,
                Size = c.Size,
                RelativePath = c.RelativePath
            };
        }

        #endregion
    }
}
