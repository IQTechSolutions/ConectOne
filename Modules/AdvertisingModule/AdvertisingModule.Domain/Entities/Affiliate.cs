using System.ComponentModel.DataAnnotations;
using FilingModule.Domain.Entities;

namespace AdvertisingModule.Domain.Entities
{
    /// <summary>
    /// Represents an affiliate advertisement, including details such as title, description, and an optional URL.
    /// </summary>
    /// <remarks>The <see cref="Affiliate"/> class is designed to manage information about an affiliate
    /// advertisement. The <see cref="Title"/> property is required and serves as the primary identifier for the
    /// advertisement. Optional properties such as <see cref="Description"/> and <see cref="Url"/> provide additional
    /// context and links related to the advertisement.</remarks>
    public class Affiliate : FileCollection<Affiliate, string>
    {
        /// <summary>
        /// Gets or sets the title of the entity. This property is required.
        /// </summary>
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = null!;

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
    }
}
