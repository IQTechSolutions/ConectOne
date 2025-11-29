using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Enums;
using FilingModule.Domain.Entities;
using GroupingModule.Domain.Entities;
using IdentityModule.Domain.Entities;

namespace BusinessModule.Domain.Entities;

/// <summary>
/// Directory listing that groups company information with its tier and approval status.
/// </summary>
public class BusinessListing : FileCollection<BusinessListing, string>
{
    /// <summary>
    /// Gets the default number of years that a listing remains active for a single subscription period.
    /// </summary>
    public const int DefaultActivePeriodInYears = 1;

    /// <summary>
    /// Gets or sets the heading text to be displayed.
    /// </summary>
    public string? Heading { get; set; }

    /// <summary>
    /// Gets or sets the slogan associated with the entity.
    /// </summary>
    public string? Slogan { get; set; }

    /// <summary>
    /// Gets or sets a description providing additional details or context.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the date and time until which the entity remains active.
    /// </summary>
    public DateTime? ActiveUntil { get; set; }

    /// <summary>
    /// Gets or sets the address associated with the entity.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the email address associated with the user.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number associated with the entity.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the URL of the website associated with this entity.
    /// </summary>
    public string? WebsiteUrl { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the item is approved.
    /// </summary>
    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
    
    #region One-To-Many Relationships

    /// <summary>
    /// Gets or sets the identifier of the associated listing tier.
    /// </summary>
    [ForeignKey(nameof(ListingTier))]public string? ListingTierId { get; set; }

    /// <summary>
    /// Gets or sets the listing tier, which determines the level of features and benefits available for the listing.
    /// </summary>
    public ListingTier? ListingTier { get; set; }


    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the user associated with the current application context.
    /// </summary>
    public ApplicationUser? User { get; set; }

    #endregion

    #region Many-To-One Relationships

    /// <summary>
    /// Gets or sets the collection of categories associated with the business listing.
    /// </summary>
    public ICollection<EntityCategory<BusinessListing>> Categories { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of services associated with the current instance.
    /// </summary>
    public ICollection<ListingService> Services { get; set; } = new List<ListingService>();

    /// <summary>
    /// Gets or sets the collection of products associated with this instance.
    /// </summary>
    public ICollection<ListingProduct> Products { get; set; } = new List<ListingProduct>();

    #endregion

    /// <summary>
    /// Initializes the activation window for the listing to the default duration.
    /// </summary>
    /// <remarks>
    /// This method ensures that a listing remains active for exactly one year from the moment it is created.
    /// It also persists the creation timestamp when it has not yet been set.
    /// </remarks>
    /// <param name="reference">An optional reference date that defaults to <see cref="DateTime.UtcNow"/>.</param>
    public void ActivateForDefaultPeriod(DateTime? reference = null)
    {
        var now = reference ?? DateTime.UtcNow;
        CreatedOn ??= now;
        ActiveUntil = (CreatedOn ?? now).AddYears(DefaultActivePeriodInYears);
    }

    /// <summary>
    /// Extends the listing's active window by the default duration.
    /// </summary>
    /// <remarks>
    /// When a listing is renewed before it expires, the new expiration date is calculated from the current
    /// expiration date. If the listing has already expired, the renewal period begins at the time of renewal.
    /// </remarks>
    /// <param name="reference">An optional reference date that defaults to <see cref="DateTime.UtcNow"/>.</param>
    public void Renew(DateTime? reference = null)
    {
        var now = reference ?? DateTime.UtcNow;
        var baseline = ActiveUntil.HasValue && ActiveUntil.Value > now ? ActiveUntil.Value : now;
        ActiveUntil = baseline.AddYears(DefaultActivePeriodInYears);
    }

    /// <summary>
    /// Calculates the remaining active time for the listing.
    /// </summary>
    /// <param name="reference">An optional reference date that defaults to <see cref="DateTime.UtcNow"/>.</param>
    /// <returns>The remaining time until expiration, or <c>null</c> when no expiration date is configured.</returns>
    public TimeSpan? GetTimeRemaining(DateTime? reference = null)
    {
        if (!ActiveUntil.HasValue)
            return null;

        var now = reference ?? DateTime.UtcNow;
        return ActiveUntil.Value - now;
    }

    /// <summary>
    /// Determines whether the listing is expiring within the provided time window.
    /// </summary>
    /// <param name="window">The time window to evaluate.</param>
    /// <param name="reference">An optional reference date that defaults to <see cref="DateTime.UtcNow"/>.</param>
    /// <returns><c>true</c> if the listing expires within the window; otherwise, <c>false</c>.</returns>
    public bool IsExpiringWithin(TimeSpan window, DateTime? reference = null)
    {
        var remaining = GetTimeRemaining(reference);
        return remaining.HasValue && remaining.Value > TimeSpan.Zero && remaining.Value <= window;
    }
}
