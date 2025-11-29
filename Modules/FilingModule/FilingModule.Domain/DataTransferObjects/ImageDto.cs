using FilingModule.Domain.Entities;
using FilingModule.Domain.Enums;

namespace FilingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data-transfer object (DTO) representing a single image that belongs to, or is otherwise associated with,
    /// a domain entity (e.g., accommodation, vacation, product, etc.).
    /// <para>
    /// The record is deliberately lightweight and serialisation-friendly so it can be sent over the wire
    /// (Blazor → API, API → Blazor, background services, etc.) without leaking persistence-layer concerns.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Use <see cref="ToDto{T}(EntityImage{T,string})"/> or <see cref="ToDto(Image)"/> helper methods to build an
    /// <see cref="ImageDto"/> from your domain / persistence entities.
    /// </remarks>
    public record ImageDto
    {
        #region Properties

        /// <summary>
        /// Primary key of the image in the backing store.
        /// </summary>
        public string? Id { get; init; }

        /// <summary>
        /// The identifier of the parent (owning) entity—  
        /// for example, a vacation, accommodation, franchisee, etc.
        /// </summary>
        public string? EntityId { get; init; }

        /// <summary>
        /// Human-readable name or title for display purposes (e.g., “Front View Banner”).
        /// </summary>
        public string? Name { get; init; } 

        /// <summary>
        /// Indicates whether this image should be treated as the <i>featured</i> image
        /// (e.g., the default hero/banner shown to end-users).
        /// </summary>
        public bool? Featured { get; init; }

        /// <summary>
        /// Logical category that describes what the image is used for
        /// (slider, banner, map, document scan, etc.).  
        /// See <see cref="UploadType"/>.
        /// </summary>
        public UploadType? ImageType { get; init; }

        /// <summary>
        /// Filename as stored on disk / object storage (may differ from <see cref="Name"/>).
        /// </summary>
        public string FileName { get; init; } = null!;

        /// <summary>
        /// MIME content-type (e.g., <c>image/jpeg</c>, <c>image/png</c>).
        /// </summary>
        public string? ContentType { get; init; }

        /// <summary>
        /// File size in bytes.
        /// </summary>
        public long Size { get; init; }

        /// <summary>
        /// Relative path from the root of your storage provider to the file
        /// (useful for generating fully qualified URLs).
        /// </summary>
        public string? RelativePath { get; init; }

        /// <summary>
        /// Optional CSS selector / DOM identifier used when
        /// dynamically replacing or targeting the image in a Blazor page.
        /// </summary>
        public string? Selector { get; init; }

        /// <summary>
        /// An explicit display order index; useful when images must appear
        /// in a specific sequence (e.g., sliders, step-by-step guides, etc.).
        /// </summary>
        public int? Order { get; init; }

        #endregion

        #region Factory Helpers

        /// <summary>
        /// Converts a <see cref="EntityImage{TEntity,TKey}"/> into a lightweight <see cref="ImageDto"/>.
        /// </summary>
        /// <typeparam name="T">
        /// CLR type of the entity that owns the image (e.g., <c>Vacation</c>, <c>Accommodation</c>).
        /// </typeparam>
        /// <param name="c">The fully populated <see cref="EntityImage{T,string}"/> instance.</param>
        /// <returns>A new <see cref="ImageDto"/> containing the transferable subset of properties.</returns>
        public static ImageDto ToDto<T>(EntityImage<T, string> c)
        {
            return new ImageDto
            {
                Id = c.Id,
                Name = c.Image.DisplayName,
                Featured = c.Image.Featured,
                ImageType = c.Image.ImageType,
                EntityId = c.EntityId,
                FileName = c.Image.FileName,
                ContentType = c.Image.ContentType,
                Size = c.Image.Size,
                RelativePath = c.Image.RelativePath,
                Selector = c.Selector,
                Order = c.Order
            };
        }

        public static ImageDto ToDto(Image c)
        {
            return new ImageDto
            {
                Id = c.Id,
                Name = c.DisplayName,
                Featured = c.Featured,
                ImageType = c.ImageType,
                FileName = c.FileName,
                ContentType = c.ContentType,
                Size = c.Size,
                RelativePath = c.RelativePath
            };
        }

        #endregion
    }
}
