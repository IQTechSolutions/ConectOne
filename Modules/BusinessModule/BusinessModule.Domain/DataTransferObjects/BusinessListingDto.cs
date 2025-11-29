using BusinessModule.Domain.Entities;
using ConectOne.Domain.Enums;
using FilingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.DataTransferObjects;

namespace BusinessModule.Domain.DataTransferObjects;

/// <summary>
/// Data transfer object for business listings.
/// </summary>
public record BusinessListingDto
{
    /// <summary>
    /// The default number of years that a listing remains active before renewal is required.
    /// </summary>
    public const int DefaultActivePeriodInYears = BusinessListing.DefaultActivePeriodInYears;

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessListingDto"/> class.
    /// </summary>
    public BusinessListingDto() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessListingDto"/> class using the specified business listing.
    /// </summary>
    /// <param name="listing">The <see cref="BusinessListing"/> object containing the data to populate the DTO.  This parameter cannot be <see
    /// langword="null"/>.</param>
    public BusinessListingDto(BusinessListing listing)
    {
        Id = listing.Id;
        UserId = listing.UserId;
        Heading = listing.Heading;
        Slogan = listing.Slogan;
        ActiveUntil = listing.ActiveUntil;
        Address = listing.Address;
        Email = listing.Email;
        PhoneNumber = listing.PhoneNumber;
        Description = listing.Description;
        WebsiteUrl = listing.WebsiteUrl;
        Categories = listing.Categories?.Select(c => CategoryDto.ToCategoryDto(c.Category)).ToList();
        Tier = listing.ListingTier is null ? null : new ListingTierDto(listing.ListingTier);
        Status = listing.Status;
        Images = listing?.Images == null ? [] : listing.Images.Select(c => ImageDto.ToDto(c)).ToList();
        Videos = listing?.Videos == null ? [] : listing.Videos.Select(c => VideoDto.ToDto(c)).ToList();
        Services = listing.Services?.Select(s => new ListingServiceDto(s)).ToList() ?? [];
        Products = listing.Products?.Select(p => new ListingProductDto(p)).ToList() ?? [];
    }

    #endregion

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the name of the company.
    /// </summary>
    public string Heading { get; set; } = null!;

    /// <summary>
    /// Gets or sets the slogan associated with the entity.
    /// </summary>
    public string? Slogan { get; init; }

    /// <summary>
    /// Gets or sets the date and time until which the entity remains active.
    /// </summary>
    public DateTime? ActiveUntil { get; init; }

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
    /// Gets or sets the description associated with the object.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the URL of the website associated with this entity.
    /// </summary>
    public string? WebsiteUrl { get; set; }

    /// <summary>
    /// Gets or sets the tier of the listing, indicating its level or category.
    /// </summary>
    public ListingTierDto? Tier { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is approved.
    /// </summary>
    public ReviewStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the collection of images associated with the entity.
    /// </summary>
    public List<ImageDto> Images { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of video files to be displayed.
    /// </summary>
    public List<VideoDto> Videos { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of categories associated with the entity.
    /// </summary>
    public IEnumerable<CategoryDto> Categories { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of services associated with the listing.
    /// </summary>
    public List<ListingServiceDto> Services { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of products associated with the listing.
    /// </summary>
    public List<ListingProductDto> Products { get; set; } = [];

    #region Helper Methods

    /// <summary>
    /// Gets the expiration date as a UTC value.
    /// </summary>
    public DateTime? ActiveUntilUtc => ActiveUntil.HasValue
        ? ActiveUntil.Value.Kind switch
        {
            DateTimeKind.Utc => ActiveUntil.Value,
            DateTimeKind.Local => ActiveUntil.Value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(ActiveUntil.Value, DateTimeKind.Utc)
        }
        : null;

    /// <summary>
    /// Calculates the remaining active time for the listing.
    /// </summary>
    /// <param name="reference">An optional reference date that defaults to <see cref="DateTime.UtcNow"/>.</param>
    /// <returns>The remaining time span, or <c>null</c> when no expiration date is defined.</returns>
    public TimeSpan? TimeRemaining(DateTime? reference = null)
    {
        if (ActiveUntilUtc is not { } expiry)
            return null;

        var now = reference ?? DateTime.UtcNow;
        return expiry - now;
    }

    /// <summary>
    /// Determines whether the listing will expire within the specified time threshold.
    /// </summary>
    /// <param name="threshold">The time threshold to evaluate.</param>
    /// <param name="reference">An optional reference date that defaults to <see cref="DateTime.UtcNow"/>.</param>
    /// <returns><c>true</c> if the listing expires within the threshold; otherwise, <c>false</c>.</returns>
    public bool IsExpiringWithin(TimeSpan threshold, DateTime? reference = null)
    {
        var remaining = TimeRemaining(reference);
        return remaining.HasValue && remaining.Value > TimeSpan.Zero && remaining.Value <= threshold;
    }

    /// <summary>
    /// Calculates the elapsed percentage of the active period.
    /// </summary>
    /// <param name="reference">An optional reference date that defaults to <see cref="DateTime.UtcNow"/>.</param>
    /// <returns>The percentage value in the range 0-100.</returns>
    public double GetActiveProgressPercentage(DateTime? reference = null)
    {
        if (ActiveUntilUtc is not { } expiry)
            return 0d;

        var periodStart = expiry.AddYears(-DefaultActivePeriodInYears);
        var now = reference ?? DateTime.UtcNow;

        var totalSeconds = (expiry - periodStart).TotalSeconds;
        if (totalSeconds <= 0)
            return expiry <= now ? 100d : 0d;

        var elapsedSeconds = (now - periodStart).TotalSeconds;
        var percentage = elapsedSeconds / totalSeconds * 100d;

        if (percentage < 0d)
            return 0d;

        if (percentage > 100d)
            return 100d;

        return percentage;
    }

    /// <summary>
    /// Builds a human-readable description of the remaining active time.
    /// </summary>
    /// <param name="reference">An optional reference date that defaults to <see cref="DateTime.UtcNow"/>.</param>
    /// <param name="maxComponents">The maximum number of time components to include in the description.</param>
    /// <returns>A formatted string describing the time remaining.</returns>
    public string BuildTimeRemainingDescription(DateTime? reference = null, int maxComponents = 4)
    {
        var remaining = TimeRemaining(reference);
        if (!remaining.HasValue)
            return "No expiration date";

        if (remaining.Value <= TimeSpan.Zero)
            return "Expired";

        if (ActiveUntilUtc is not { } expiry)
            return "No expiration date";

        var now = reference ?? DateTime.UtcNow;
        if (now >= expiry)
            return "Expired";

        var components = new List<string>(maxComponents);
        var workingDate = now;

        void TryAddComponent(int value, string singular)
        {
            if (value <= 0 || components.Count >= maxComponents)
                return;

            var label = value == 1 ? singular : $"{singular}s";
            components.Add($"{value} {label}");
        }

        var years = 0;
        while (workingDate.AddYears(1) <= expiry)
        {
            workingDate = workingDate.AddYears(1);
            years++;
        }
        TryAddComponent(years, "year");

        var months = 0;
        while (workingDate.AddMonths(1) <= expiry)
        {
            workingDate = workingDate.AddMonths(1);
            months++;
        }
        TryAddComponent(months, "month");

        var residual = expiry - workingDate;

        TryAddComponent(residual.Days, "day");
        TryAddComponent(residual.Hours, "hour");
        TryAddComponent(residual.Minutes, "minute");

        if (components.Count == 0)
            components.Add("less than a minute");

        return string.Join(", ", components);
    }

    #endregion
}