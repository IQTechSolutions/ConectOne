using System.ComponentModel.DataAnnotations.Schema;
using AdvertisingModule.Domain.Enums;
using ConectOne.Domain.Attributes;
using ConectOne.Domain.Enums;
using FilingModule.Domain.Entities;
using IdentityModule.Domain.Entities;

namespace AdvertisingModule.Domain.Entities
{
    /// <summary>
    /// Represents an advertisement entity that can hold images (via ImageFileCollection),
    /// along with a required Title, optional Description, and optional Url.
    /// </summary>
    public class Advertisement : FileCollection<Advertisement, string>
    {
        #region Properties

        /// <summary>
        /// The title of the advertisement, required for meaningful identification.
        /// </summary>
        public string? Title { get; set; } = null!;

        /// <summary>
        /// Optional descriptive text providing more context about the advertisement.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// An optional URL associated with the advertisement, potentially a landing page or product link.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Gets or sets the start date of the event or process.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the event or period. 
        /// </summary>
        [DateGreaterThan("StartDate", ErrorMessage = "End Date must be after Start Date.")]
        [RequiredIfOtherPropertyHasValueAttribute(nameof(StartDate), ErrorMessage = "This field is required if start date has a value")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Indicates whether the advertisement is active and should be shown.
        /// </summary>
        public bool Active  => StartDate is null || StartDate >= DateTime.Now.Date && EndDate?.Date <= DateTime.Now.Date || Status == ReviewStatus.Approved;

        /// <summary>
        /// Indicates whether the advertisement has been approved for display.
        /// </summary>
        public ReviewStatus Status { get; set; } = ReviewStatus.Pending;

        /// <summary>
        /// Gets or sets the type of advertisement.
        /// </summary>
        public AdvertisementType AdvertisementType { get; set; } 

        /// <summary>
        /// Gets or sets a value indicating whether the setup process has been completed.
        /// </summary>
        public bool SetupCompleted { get; set; } = false;

        /// <summary>
        /// Gets or sets the number of available items.
        /// </summary>
        public int Available { get; set; }

        #endregion

        #region One-To-Many Relationships

        /// <summary>
        /// Gets or sets the identifier for the tier.
        /// </summary>
        [ForeignKey(nameof(AdvertisementTier))] public string? AdvertisementTierId { get; set; }

        /// <summary>
        /// Determines the display order of the advertisement. Lower tiers are shown first.
        /// </summary>
        public AdvertisementTier? AdvertisementTier { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the user associated with the current application context.
        /// </summary>
        public ApplicationUser? User { get; set; }

        #endregion
    }
}
