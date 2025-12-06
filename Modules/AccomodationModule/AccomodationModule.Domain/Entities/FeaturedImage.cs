using ConectOne.Domain.Entities;
using FilingModule.Domain.Enums;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a featured image associated with a lodging or room.
    /// </summary>
    public class FeaturedImage : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeaturedImage"/> class.
        /// </summary>
        public FeaturedImage() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeaturedImage"/> class with specified parameters.
        /// </summary>
        /// <param name="lodgingId">The ID of the lodging associated with the image.</param>
        /// <param name="roomId">The ID of the room associated with the image.</param>
        /// <param name="imageUrl">The URL of the image.</param>
        /// <param name="imageType">The type of the image (e.g., cover, slider).</param>
        public FeaturedImage(string? lodgingId, int? roomId, string imageUrl, UploadType imageType)
        {
            LodgingId = lodgingId;
            RoomId = roomId;
            ImageUrl = imageUrl;
            ImageType = imageType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ID of the room associated with the image.
        /// </summary>
        public int? RoomId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the lodging associated with the image.
        /// </summary>
        public string? LodgingId { get; set; }

        /// <summary>
        /// Gets or sets the URL of the image.
        /// </summary>
        public string ImageUrl { get; set; } = null!;

        /// <summary>
        /// Gets or sets the type of the image (e.g., cover, slider).
        /// </summary>
        public UploadType ImageType { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the featured image.
        /// </summary>
        /// <returns>A string representing the featured image.</returns>
        public override string ToString()
        {
            return $"Featured Image";
        }

        #endregion
    }
}
