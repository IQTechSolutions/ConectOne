using IdentityModule.Domain.Entities;
using System.Linq.Expressions;
using System.Reflection;
using ConectOne.Domain.Entities;
using ConectOne.Domain.Interfaces;
using ConectOne.EntityFrameworkCore.Sql.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConectOne.EntityFrameworkCore.Sql
{
    public class GenericDbContext(DbContextOptions<GenericDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
        IdentityUserRole<string>, IdentityUserLogin<string>, ApplicationRoleClaim, IdentityUserToken<string>>(options)
    {
        /// <summary>
        /// Gets or sets the audit trail entries for tracking changes to entities.
        /// </summary>
        public DbSet<Audit> AuditTrails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationRoleClaim>().ToTable(name: "RoleClaims", "Identity");

            base.OnModelCreating(modelBuilder);

            // Apply entity configurations from assemblies so Owned types are known
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            }

            // Add RowVersion property to all non-owned entities for optimistic concurrency control
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Skip owned types (they may be configured by other code)
                if (entityType.IsOwned()) 
                    continue;

                modelBuilder.Entity(entityType.ClrType)
                    .Property<byte[]>("RowVersion")
                    .IsRowVersion()
                    .IsConcurrencyToken();
            }

            // Ensure consistent decimal precision/scale across all properties
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(t => t.GetProperties())
                         .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            // Apply nvarchar(128) to audit fields
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(t => t.GetProperties())
                         .Where(p => p.Name is "LastModifiedBy" or "CreatedBy"))
            {
                property.SetColumnType("nvarchar(128)");
            }
                        
            SoftDeleteFilter.Apply(modelBuilder);
        }

        /// <summary>
        /// Static utility class that applies a global query filter to exclude soft-deleted entities from queries.
        /// </summary>
        internal static class SoftDeleteFilter
        {
            /// <summary>
            /// Applies a global filter to all entities implementing <see cref="IAuditableEntity"/> to exclude records where <c>IsDeleted == true</c>.
            /// </summary>
            /// <param name="modelBuilder">The EF Core model builder instance.</param>
            public static void Apply(ModelBuilder modelBuilder)
            {
                var types = modelBuilder.Model.GetEntityTypes();

                foreach (var type in types)
                {
                    if (!ImplementsIAuditable(type.ClrType)) // Skip non-auditable entities
                        continue;

                    if (type.BaseType is not null) // Apply filter only to root entities
                        continue;

                    modelBuilder.Entity(type.ClrType)
                        .HasQueryFilter(BuildFilter(type.ClrType));
                }
            }

            /// <summary>
            /// Determines whether a given CLR type implements <see cref="IAuditableEntity{T}"/>.
            /// </summary>
            private static bool ImplementsIAuditable(Type clrType) => clrType.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAuditableEntity<>));

            /// <summary>
            /// Builds a lambda expression to filter out soft-deleted entities using the shared DbExtensions helper.
            /// </summary>
            /// <param name="clrType">The CLR type of the entity.</param>
            /// <returns>A lambda expression to be used as a query filter.</returns>
            private static LambdaExpression BuildFilter(Type clrType) => DbExtensions.BuildIsDeletedFilterExpression(clrType);
        }
    }
}
