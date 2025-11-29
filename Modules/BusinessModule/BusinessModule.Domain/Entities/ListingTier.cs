using FilingModule.Domain.Entities;

namespace BusinessModule.Domain.Entities;

/// <summary>
/// Represents a tier or level for a business listing, including its name, description, price, and order.
/// </summary>
/// <remarks>A listing tier defines the characteristics and pricing of a specific level of service or offering 
/// for business listings. It includes details such as the tier's name, descriptions, price, and the  order in which it
/// appears relative to other tiers. Each tier can be associated with multiple  business listings.</remarks>
public class ListingTier : FileCollection<ListingTier, string>
{
    /// <summary>
    /// Gets or sets the name associated with the object.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a brief description of the object.
    /// </summary>
    public string? ShortDescription { get; set; }

    /// <summary>
    /// Gets or sets the description associated with the object.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the order in which this item is processed or displayed.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets the price of the item.
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether services and products can be listed.
    /// </summary>
    public bool AllowServiceAndProductListing { get; set; }

    /// <summary>
    /// Gets or sets the collection of business listings associated with this entity.
    /// </summary>
    public virtual ICollection<BusinessListing> BusinessListings { get; set; } = [];
}