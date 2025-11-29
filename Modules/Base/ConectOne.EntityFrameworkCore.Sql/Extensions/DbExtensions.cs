using System.Linq.Expressions;
using ConectOne.Domain.Interfaces;

namespace ConectOne.EntityFrameworkCore.Sql.Extensions
{
    /// <summary>
    /// Extension methods to support DbContext 
    /// </summary>
    public static class DbExtensions
    {
        /// <summary>
        /// Dynamically builds an expression tree for
        ///     e => !e.IsDeleted
        /// for any entity type that implements IAuditableEntity.
        /// </summary>
        public static LambdaExpression BuildIsDeletedFilterExpression(Type clrType)
        {
            var param = Expression.Parameter(clrType, "e");
            var prop = Expression.Property(param, nameof(IAuditableEntity.IsDeleted));
            var body = Expression.Not(prop);                  // !e.IsDeleted
            return Expression.Lambda(body, param);
        }
    }
}