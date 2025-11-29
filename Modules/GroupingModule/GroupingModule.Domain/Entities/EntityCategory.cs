using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using ConectOne.Domain.Interfaces;

namespace GroupingModule.Domain.Entities
{
    /// <summary>
    /// Represents the association between an entity and a category, where the entity is of a specified type.
    /// </summary>
    /// <remarks>This class provides a way to establish a many-to-many relationship between entities and
    /// categories. It includes properties for the entity, the category, and their respective identifiers.</remarks>
    /// <typeparam name="T">The type of the entity associated with the category. The type must implement <see
    /// cref="IAuditableEntity"/> with a key of type <see cref="string"/>.</typeparam>
    public class EntityCategory<T> : EntityBase<string> where T : IAuditableEntity<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCategory"/> class.
        /// </summary>
        public EntityCategory(){ }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCategory"/> class with the specified entity and category
        /// identifiers.
        /// </summary>
        /// <param name="entityId">The unique identifier of the entity. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="categoryId">The unique identifier of the category. Cannot be <see langword="null"/> or empty.</param>
        public EntityCategory(string entityId, string categoryId)
        {
            EntityId = entityId;
            CategoryId = categoryId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCategory{T}"/> class, associating an entity with a
        /// category.
        /// </summary>
        /// <remarks>This constructor sets the <see cref="EntityId"/> and <see cref="CategoryId"/>
        /// properties based on the provided <paramref name="entity"/> and <paramref name="category"/> objects,
        /// respectively.</remarks>
        /// <param name="entity">The entity to associate with the category. Must not be <c>null</c>.</param>
        /// <param name="category">The category to associate with the entity. Must not be <c>null</c>.</param>
        public EntityCategory(T entity, Category<T> category)
        {
            EntityId = entity.Id;
            Entity = entity;
            CategoryId = category.Id;
            Category = category;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the associated entity.
        /// </summary>
        [ForeignKey(nameof(Entity))] public string EntityId { get; set; }

        /// <summary>
        /// Gets or sets the entity associated with the current operation.
        /// </summary>
        public T Entity { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated category.
        /// </summary>
        [ForeignKey(nameof(Category))] public string CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category associated with the current instance.
        /// </summary>
        public Category<T> Category { get; set; }

        /// <summary>
        /// Returns a string representation of the category associated with the specified type.
        /// </summary>
        /// <returns>A string in the format "Category for {TypeName}", where {TypeName} is the name of the type parameter
        /// <typeparamref name="T"/>.</returns>
        public override string ToString()
        {
            return $"Category for {typeof(T).Name}";
        }
    }
}