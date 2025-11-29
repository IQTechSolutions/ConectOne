namespace FilingModule.Domain.Entities
{
    /// <summary>
    /// Represents a video entity with metadata and an optional hierarchical relationship to a parent entity.
    /// </summary>
    public class Video : FileBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Video"/> class.
        /// </summary>
        public Video() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Video"/> class with the specified name, caption, description,
        /// file path, and parent identifier.
        /// </summary>
        /// <param name="name">The name of the video. This value cannot be null or empty.</param>
        /// <param name="caption">The caption associated with the video. This value cannot be null or empty.</param>
        /// <param name="description">A brief description of the video. This value cannot be null or empty.</param>
        /// <param name="path">The file path where the video is stored. This value cannot be null or empty.</param>
        public Video(string name, string caption, string description, string path)
        {
            DisplayName = name;
            Caption = caption;
            Description = description;
            RelativePath = path;
        }

        #endregion

        /// <summary>
        /// Gets or sets the caption text associated with this instance.
        /// </summary>
        public string? Caption { get; set; } 

        /// <summary>
        /// Gets or sets the description associated with the object.
        /// </summary>
        public string? Description { get; set; } 
    }
}
