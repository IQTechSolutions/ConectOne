using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace FilingModule.Domain.Entities
{
    /// <summary>
    /// Represents a document that is associated with an entity, supporting one-to-many relationships between entities
    /// and documents.
    /// </summary>
    /// <remarks>This class is typically used to link a document to a specific entity instance, enabling
    /// scenarios where entities can have related documents such as images or files. It provides navigation properties
    /// for both the document and the associated entity, facilitating data access and relationship management in
    /// object-relational mapping scenarios.</remarks>
    /// <typeparam name="TEntity">The type of the associated entity.</typeparam>
    /// <typeparam name="TId">The type of the identifier for the associated entity.</typeparam>
    public class EntityDocument<TEntity, TId> : EntityBase<TId>
    {
        #region One-To-Many Relationships

        /// <summary>
        /// Gets or sets the identifier of the image file.
        /// </summary>
        [ForeignKey(nameof(Document))] public string? DocumentId { get; set; }

        /// <summary>
        /// Gets or sets the image file associated with this entity.
        /// </summary>
        public Document? Document { get; set; }


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
