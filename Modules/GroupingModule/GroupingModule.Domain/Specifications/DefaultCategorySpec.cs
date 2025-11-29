using ConectOne.Domain.Interfaces;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using GroupingModule.Domain.Entities;
using GroupingModule.Domain.RequestFeatures;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace GroupingModule.Domain.Specifications
{
    /// <summary>
    /// Represents a specification for filtering and including categories based on the provided parameters.
    /// </summary>
    /// <remarks>This specification is designed to filter out deleted categories and match the specified
    /// active state and parent category ID. If the <see cref="CategoryPageParameters.Featured"/> property is provided,
    /// the filtering criteria are further refined to include only featured categories. Additionally, this specification
    /// configures the query to include related category images and their associated image details.</remarks>
    /// <typeparam name="T">The type of the entity associated with the category, which must implement <see cref="string"/>
    /// with a key of type <see cref="IAuditableEntity"/>.</typeparam>
    public sealed class DefaultCategorySpec<T> : Specification<Category<T>> where T : IAuditableEntity<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCategorySpec"/> class with the specified category page
        /// parameters.
        /// </summary>
        /// <remarks>This constructor sets up the filtering criteria to exclude deleted categories and
        /// match the specified active state  and parent category ID. If the <see
        /// cref="CategoryPageParameters.Featured"/> property is provided, the criteria  are further refined to include
        /// only featured categories. Additionally, it configures the query to include related  category images and
        /// their associated image details.</remarks>
        /// <param name="parameters">The parameters used to define the filtering and inclusion criteria for categories.  This includes properties
        /// such as <see cref="CategoryPageParameters.Active"/>, <see cref="CategoryPageParameters.ParentId"/>,  and
        /// optionally <see cref="CategoryPageParameters.Featured"/>.</param>
        public DefaultCategorySpec(CategoryPageParameters parameters)
        {
            Criteria = category => !category.IsDeleted && category.Active == parameters.Active && category.ParentCategoryId == parameters.ParentId;
            if (parameters.Featured is not null)
            {
                Criteria = Criteria.And(f => f.Featured == parameters.Featured);
            }
            
            AddInclude(c => c.Include(category => category.Images).ThenInclude(category => category.Image));
        }
    }
}
