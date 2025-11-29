using System.ComponentModel.DataAnnotations;
using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Entities;
using FilingModule.Domain.DataTransferObjects;

namespace AdvertisingModule.Application.ViewModels;

/// <summary>
/// Represents an affiliate entity with associated metadata, including title, description, URL, and images.
/// </summary>
/// <remarks>This view model is typically used to transfer affiliate data between the application layers. It
/// includes optional fields such as <see cref="Description"/> and <see cref="Url"/>, as well as a collection of
/// images.</remarks>
public class AffiliateViewModel
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AffiliateViewModel"/> class.
    /// </summary>
    public AffiliateViewModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AffiliateViewModel"/> class using the specified data transfer
    /// object.
    /// </summary>
    /// <remarks>This constructor maps the properties of the provided <see cref="AffiliateDto"/> to the
    /// corresponding properties of the <see cref="AffiliateViewModel"/>. If the <c>Images</c> property of the <paramref
    /// name="dto"/> is null, it will be initialized to an empty collection.</remarks>
    /// <param name="dto">An <see cref="AffiliateDto"/> containing the data to populate the view model. The <see cref="AffiliateDto"/>
    /// must not be null.</param>
    public AffiliateViewModel(AffiliateDto dto)
    {
        Id = dto.Id;
        Title = dto.Title;
        Description = dto.Description;
        Url = dto.Url;
        Featured = dto.Featured;
        DisplayOrder = dto.DisplayOrder;
        Images = dto.Images ?? [];
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the entity. This property is required.
    /// </summary>
    [Required] public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description associated with the object.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the URL associated with the current instance.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the affiliate is marked as featured.
    /// </summary>
    public bool Featured { get; set; }

    /// <summary>
    /// Gets or sets the display order of the item.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Gets or sets the collection of images associated with the entity.
    /// </summary>
    public ICollection<ImageDto> Images { get; set; } = [];

    #endregion

    #region Methods

    /// <summary>
    /// Converts the current <see cref="Affiliate"/> instance to an <see cref="AffiliateDto"/> object.
    /// </summary>
    /// <returns>An <see cref="AffiliateDto"/> object containing the data from the current <see cref="Affiliate"/> instance.</returns>
    public AffiliateDto ToDto()
    {
        return new AffiliateDto()
        {
            Id = Id,
            Title = Title,
            Description = Description,
            Url = Url,
            Featured = Featured,
            DisplayOrder = DisplayOrder,
            Images = Images,
        };
    }

    #endregion
}
