
using AccomodationModule.Domain.Entities;
using FilingModule.Domain.Enums;

namespace AccomodationModule.Domain.DataTransferObjects;

/// <summary>
/// Represents a Data Transfer Object (DTO) for a featured image associated with an entity (such as a lodging).
/// A "featured image" is often displayed prominently, for example as a cover photo on a listing.
/// </summary>
public record FeaturedImageDto
{
    #region Constructor

    /// <summary>
    /// Default parameterless constructor.
    /// Useful for scenarios where the DTO may need to be instantiated without immediate property assignments,
    /// often required by certain serialization frameworks.
    /// </summary>
    public FeaturedImageDto() { }

    /// <summary>
    /// Constructs a new instance of <see cref="FeaturedImageDto"/> from a <see cref="FeaturedImage"/> entity.
    /// This constructor is typically used to map database entities to a DTO that can be sent to the client or used elsewhere.
    /// </summary>
    /// <param name="image">A <see cref="FeaturedImage"/> instance from which to populate the DTO.</param>
    public FeaturedImageDto(FeaturedImage image)
    {
        FeaturedImageId = image.Id;
        ImageUrl = image.ImageUrl;
        IsCoverImage = image.ImageType == UploadType.Cover; // Determine if the image is set as a cover image.
    }

    /// <summary>
    /// Constructs a new instance of <see cref="FeaturedImageDto"/> directly from given parameters.
    /// Useful if you want to create a DTO from information that is not tied to a database entity or an image wrapper.
    /// </summary>
    /// <param name="entityId">The identifier of the entity this featured image belongs to (e.g., a lodging ID).</param>
    /// <param name="imageUrl">The URL or path to the featured image.</param>
    /// <param name="isCoverImage">Indicates whether this image should be treated as a cover image.</param>
    public FeaturedImageDto(string entityId, string imageUrl, bool isCoverImage)
    {
        EntityId = entityId;
        ImageUrl = imageUrl;
        IsCoverImage = isCoverImage;
    }

    #endregion

    /// <summary>
    /// Gets the unique identifier for the featured image.
    /// This ID may refer to the image's primary key in a database or another unique string identifier.
    /// </summary>
    public string? FeaturedImageId { get; init; }

    /// <summary>
    /// Gets the identifier of the entity (e.g., lodging) that this image is associated with.
    /// Typically, this would be something like a Lodging ID if the image is related to a particular accommodation listing.
    /// </summary>
    public string? EntityId { get; init; } 

    /// <summary>
    /// Gets the URL or relative path pointing to the location of the featured image.
    /// This could be a full URL to a CDN or a relative path within the application's content directory.
    /// </summary>
    public string ImageUrl { get; init; } = null!;

    /// <summary>
    /// Indicates whether the image is considered a "cover image".
    /// Cover images are often displayed more prominently, such as in listings or hero banners.
    /// </summary>
    public bool IsCoverImage { get; init; }
}