using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace FilingModule.Domain.Entities
{
    /// <summary>
    /// Represents an image associated with a specific entity, including metadata such as order and group selector.
    /// </summary>
    /// <remarks>This class provides a way to associate an image with an entity, including properties for
    /// managing the image's order and grouping within a collection of images. It supports one-to-many relationships
    /// between entities and their associated images.</remarks>
    /// <typeparam name="TEntity">The type of the entity to which the image is associated.</typeparam>
    /// <typeparam name="TId">The type of the identifier for the entity.</typeparam>
    public class EntityImage<TEntity, TId> : EntityBase<TId>
    {
        #region Constructors

        /// <summary>
        /// Default constructor for EntityImage.
        /// </summary>
        public EntityImage() { }

        /// <summary>
        /// Constructor for EntityImage that initializes the image ID and entity ID.
        /// </summary>
        /// <param name="imageId">The identity of the image being added</param>
        /// <param name="entityId">The identity of the entity the image is being added to</param>
        public EntityImage(string imageId, TId? entityId)
        {
            ImageId = imageId;
            EntityId = entityId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the group selector for ordering images that are supplied.
        /// </summary>
        public string? Selector { get; set; }

        /// <summary>
        /// Gets or sets the order in which the images should be supplied.
        /// </summary>
        public int Order { get; set; }

        #endregion

        #region One-To-Many Relationships

        /// <summary>
        /// Gets or sets the identifier of the image file.
        /// </summary>
        [ForeignKey(nameof(Image))] public string? ImageId { get; set; }

        /// <summary>
        /// Gets or sets the image file associated with this entity.
        /// </summary>
        public Image? Image { get; set; }


        /// <summary>
        /// Gets or sets the identifier of the associated entity.
        /// </summary>
        [ForeignKey(nameof(Entity))] public TId? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the associated entity.
        /// </summary>
        public TEntity Entity { get; set; } = default!;

        #endregion
    }
}
