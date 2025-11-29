using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace FilingModule.Domain.Entities
{
    /// <summary>
    /// Represents an image file associated with a specific entity.
    /// Inherits from <see cref="Video"/> and includes properties for entity association.
    /// </summary>
    /// <typeparam name="TEntity">The type of the associated entity.</typeparam>
    /// <typeparam name="TId">The type of the entity's identifier.</typeparam>
    public class EntityVideo<TEntity, TId> : EntityBase<TId>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier of the image file.
        /// </summary>
        [ForeignKey(nameof(Video))] public string? VideoId { get; set; }

        /// <summary>
        /// Gets or sets the image file associated with this entity.
        /// </summary>
        public Video? Video { get; set; }


        /// <summary>
        /// Gets or sets the identifier of the associated entity.
        /// </summary>
        [ForeignKey(nameof(Entity))] public TId? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the associated entity.
        /// </summary>
        public TEntity? Entity { get; set; } = default!;

        #endregion
    }
}
