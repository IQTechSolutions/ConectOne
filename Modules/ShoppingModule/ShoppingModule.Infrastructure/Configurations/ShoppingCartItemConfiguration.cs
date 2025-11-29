using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingModule.Domain.Entities;

namespace ShoppingModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type settings for <see cref="ShoppingCartItem"/> in the database context.
    /// </summary>
    /// <remarks>This configuration establishes a relationship where a <see cref="ShoppingCartItem"/> can have
    /// a parent-child hierarchy. Specifically, it defines a one-to-many relationship between a parent item and its
    /// child items, with a foreign key on the <c>ParentId</c> property. The delete behavior is set to <see
    /// cref="DeleteBehavior.NoAction"/>, meaning deleting a parent item will not automatically delete its child
    /// items.</remarks>
    public class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
    {
        /// <summary>
        /// Configures the entity type for <see cref="ShoppingCartItem"/> in the database model.
        /// </summary>
        /// <remarks>This configuration establishes a relationship where a <see cref="ShoppingCartItem"/>
        /// can have a parent-child hierarchy. The parent-child relationship is defined with a foreign key on
        /// <c>ParentId</c>, and the delete behavior is set to <see cref="DeleteBehavior.NoAction"/>.</remarks>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="ShoppingCartItem"/> entity.</param>
        public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
        {
            builder.HasOne(c => c.Parent).WithMany(c => c.Children).HasForeignKey(c => c.ParentId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
