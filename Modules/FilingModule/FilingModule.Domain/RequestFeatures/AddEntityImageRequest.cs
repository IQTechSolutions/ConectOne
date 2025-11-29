namespace FilingModule.Domain.RequestFeatures
{
    /// <summary>
    /// DTO sent by clients to associate an existing <c>Image</c> with a domain entity
    /// (e.g., accommodation, vacation, product).  
    /// The request only references identifiers—no binary data is included.
    /// </summary>
    /// <remarks>
    /// Typical API usage:
    /// <code>
    /// POST /api/vacations/123/images
    /// {
    ///   "entityId": "123",
    ///   "imageId":  "img_456",
    ///   "selector": ".hero-banner",
    ///   "order":    1
    /// }
    /// </code>
    /// </remarks>
    public sealed class AddEntityImageRequest
    {
        #region Constructors

        /// <summary>
        /// Parameter-less constructor required for JSON deserialization and model binding.
        /// </summary>
        public AddEntityImageRequest() { }

        /// <summary>
        /// Creates a populated request that links an image to an entity.
        /// </summary>
        /// <param name="entityId">Unique identifier of the parent entity.</param>
        /// <param name="imageId">Unique identifier of the image to link.</param>
        /// <param name="selector">
        /// Optional CSS selector / DOM identifier indicating where the image will render
        /// in the consuming UI. Defaults to an empty string when not relevant.
        /// </param>
        /// <param name="order">
        /// Display order index used when the entity has multiple images.  
        /// Lower numbers appear earlier; defaults to <c>0</c>.
        /// </param>
        public AddEntityImageRequest(string entityId, string imageId, string selector = "", int order = 0)
        {
            EntityId = entityId;
            ImageId = imageId;
            Selector = selector;
            Order = order;
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
        public string ImageId { get; set; } = null!;

        /// <summary>
        /// CSS selector or element ID used by the front-end to target where the image
        /// should be injected or replaced in the DOM.  
        /// Empty by default.
        /// </summary>
        public string Selector { get; set; } = string.Empty;

        /// <summary>
        /// Explicit sequence index that determines image ordering when multiple images
        /// are linked to the same entity. Defaults to <c>0</c>.
        /// </summary>
        public int Order { get; set; } = 0;

        #endregion
    }
}
