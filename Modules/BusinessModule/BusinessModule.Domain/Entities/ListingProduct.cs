using System.ComponentModel.DataAnnotations.Schema;
using FilingModule.Domain.Entities;

namespace BusinessModule.Domain.Entities;

/// <summary>
/// Represents a product offered by a company.
/// </summary>
public class ListingProduct : FileCollection<ListingProduct, string>
{
    /// <summary>
    /// Gets or sets the name associated with the object.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description associated with the object.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the price of the item.
    /// </summary>
    public decimal Price { get; set; }
    
    #region One-To-Many Relationship

    /// <summary>
    /// Gets or sets the unique identifier for the associated listing.
    /// </summary>
    [ForeignKey(nameof(Listing))] public string ListingId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the business listing associated with the current context.
    /// </summary>
    public BusinessListing Listing { get; set; } = null!;

    #endregion

}
