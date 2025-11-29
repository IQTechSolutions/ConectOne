namespace FilingModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a request to associate a video with a specific entity.
    /// </summary>
    /// <remarks>This class is used to encapsulate the identifiers required to link a video to an entity. It
    /// provides constructors for initializing the request with the necessary identifiers.</remarks>
    public sealed class AddEntityVideoRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddEntityVideoRequest"/> class.
        /// </summary>
        public AddEntityVideoRequest() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddEntityVideoRequest"/> class with the specified entity and
        /// video identifiers.
        /// </summary>
        /// <param name="entityId">The unique identifier of the entity to which the video will be added. Cannot be null or empty.</param>
        /// <param name="videoId">The unique identifier of the video to be associated with the entity. Cannot be null or empty.</param>
        public AddEntityVideoRequest(string entityId, string videoId)
        {
            EntityId = entityId;
            VideoId = videoId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Identifier of the entity that will own the image.
        /// </summary>
        public string EntityId { get; set; } = null!;

        /// <summary>
        /// Identifier of the image being associated with the entity.
        /// </summary>
        public string VideoId { get; set; } = null!;

        #endregion
    }
}
