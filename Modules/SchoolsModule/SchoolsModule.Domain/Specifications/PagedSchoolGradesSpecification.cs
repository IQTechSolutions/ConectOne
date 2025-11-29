using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Specifications;

/// <summary>
/// Specification for retrieving a filtered and paginated list of <see cref="SchoolGrade"/> entities.
/// Supports text search, ordering and eager loading of related classes.
/// </summary>
public sealed class PagedSchoolGradesSpecification : Specification<SchoolGrade>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedSchoolGradesSpecification"/> class.
    /// </summary>
    /// <param name="parameters">Paging and filtering parameters.</param>
    public PagedSchoolGradesSpecification(SchoolGradePageParameters parameters)
    {
        Criteria = PredicateBuilder.New<SchoolGrade>(true);

        if (!string.IsNullOrEmpty(parameters.SearchText))
        {
            Criteria = Criteria.And(g => g.Name.ToLower().Contains(parameters.SearchText.ToLower()));
        }

        if (!string.IsNullOrEmpty(parameters.OrderBy))
        {
            var parts = parameters.OrderBy.Split(' ');
            if (parts.Length == 2 && parts[1].Equals("asc", StringComparison.OrdinalIgnoreCase))
            {
                OrderBy = q => q.OrderBy(g => EF.Property<object>(g, parts[0]));
            }
            else if (parts.Length == 2 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                OrderBy = q => q.OrderByDescending(g => EF.Property<object>(g, parts[0]));
            }
        }

        AddInclude(g => g.Include(g => g.Classes));

        Skip = (parameters.PageNr - 1) * parameters.PageSize;
        Take = parameters.PageSize;
    }
}
