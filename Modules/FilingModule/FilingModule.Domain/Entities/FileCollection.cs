using ConectOne.Domain.Entities;
using ConectOne.Domain.Interfaces;
using FilingModule.Domain.Interfaces;

namespace FilingModule.Domain.Entities
{
    /// <summary>
    /// Represents a collection of files associated with a specific entity.
    /// Inherits from <see cref="EntityBase{TId}"/> and implements <see cref="IFileCollection{TEntity, TId}"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the associated entity.</typeparam>
    /// <typeparam name="TId">The type of the entity's identifier.</typeparam>
    public class FileCollection<TEntity, TId> : EntityBase<TId>, IFileCollection<TEntity, TId> where TEntity : IAuditableEntity<TId>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the collection of image files associated with the entity.
        /// </summary>
        public virtual ICollection<EntityImage<TEntity, TId>> Images { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of image files associated with the entity.
        /// </summary>
        public virtual ICollection<EntityVideo<TEntity, TId>> Videos { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of image files associated with the entity.
        /// </summary>
        public virtual ICollection<EntityDocument<TEntity, TId>> Documents { get; set; } = [];

        #endregion
    }
}
