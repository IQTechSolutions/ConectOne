using ConectOne.Domain.Interfaces;
using FilingModule.Domain.Entities;

namespace FilingModule.Domain.Interfaces
{
    /// <summary>
    /// Represents a collection of image files associated with a specific entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to which the image files are related. Must implement <see cref="IAuditableEntity{TId}"/>.</typeparam>
    /// <typeparam name="TId">The type of the identifier for the entity and image files.</typeparam>
    public interface IFileCollection<TEntity, TId> : IAuditableEntity<TId> where TEntity : IAuditableEntity<TId>
    {
        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        /// <remarks>The collection contains all images linked to the current entity instance. Modifying
        /// this collection will update the set of images related to the entity.</remarks>
        ICollection<EntityImage<TEntity, TId>> Images { get; set; }

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        /// <remarks>Each image in the collection is represented by an <see cref="EntityVideo{TEntity, TId}"/> object,
        /// which contains metadata and content related to the image. The collection can be modified to
        /// add,  remove, or update images associated with the entity.</remarks>
        ICollection<EntityVideo<TEntity, TId>> Videos { get; set; }

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        /// <remarks>Each image in the collection is represented by an <see cref="EntityDocument{TEntity, TId}"/> object,
        /// which contains metadata and content related to the image. The collection can be modified to
        /// add,  remove, or update images associated with the entity.</remarks>
        ICollection<EntityDocument<TEntity, TId>> Documents { get; set; }
    }
}
