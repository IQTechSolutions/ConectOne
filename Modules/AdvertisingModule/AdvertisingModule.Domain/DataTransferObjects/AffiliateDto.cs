using AdvertisingModule.Domain.Entities;
using FilingModule.Domain.DataTransferObjects;

namespace AdvertisingModule.Domain.DataTransferObjects;

/// <summary>
/// Represents an affiliate with associated metadata, including its title, description, URL, and images.
/// </summary>
/// <remarks>This data transfer object (DTO) is used to encapsulate affiliate information for use in application
/// layers such as APIs or view models. It provides a lightweight representation of an affiliate entity.</remarks>
public record AffiliateDto
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AffiliateDto"/> class.
    /// </summary>
    public AffiliateDto() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AffiliateDto"/> class using the specified <see cref="Affiliate"/>
    /// object.
    /// </summary>
    /// <param name="affiliate">The <see cref="Affiliate"/> object containing the data to initialize the DTO. Cannot be <see langword="null"/>.</param>
    public AffiliateDto(Affiliate affiliate)
    {
        Id = affiliate.Id;
        Title = affiliate.Title;
        Description = affiliate.Description;
        Url = affiliate.Url;
        Featured = affiliate.Featured;
        DisplayOrder = affiliate.DisplayOrder;
        Images = affiliate.Images is null ? [] : affiliate.Images.Select(ImageDto.ToDto).ToList();
    }

    #endregion

    /// <summary>
    /// Gets the unique identifier for the entity.
    /// </summary>
    public string? Id { get; init; }

    /// <summary>
    /// Gets the title associated with the object.
    /// </summary>
    public string Title { get; init; } = null!;

    /// <summary>
    /// Gets the description associated with the object.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the URL associated with the current instance.
    /// </summary>
    public string? Url { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the affiliate is marked as featured.
    /// </summary>
    public bool Featured { get; init; }

    /// <summary>
    /// Gets or sets the display order of the item.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Gets the collection of images associated with the entity.
    /// </summary>
    public ICollection<ImageDto>? Images { get; init; } = [];
    
}

/// <summary>
/// Request object for updating vacation inclusion display type information.
/// </summary>
/// <param name="AffiliateId">The identity of the affiliate that the display orders are being updated for </param>
/// <param name="Items">The items being updated</param>
public record AffiliateOrderUpdateRequest(List<AffiliateDto> Items);
