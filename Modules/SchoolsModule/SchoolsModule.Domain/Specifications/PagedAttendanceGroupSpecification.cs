using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using LinqKit;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Specifications;

/// <summary>
/// Specification for retrieving paged <see cref="AttendanceGroup"/> records.
/// </summary>
public sealed class PagedAttendanceGroupSpecification : Specification<AttendanceGroup>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedAttendanceGroupSpecification"/> class with the specified page
    /// parameters.
    /// </summary>
    /// <remarks>The specification is initialized with a base criterion that filters attendance groups by
    /// their parent group ID.  If the <paramref name="p"/> parameter includes an attendance type, an additional
    /// criterion is added to filter by the specified type.</remarks>
    /// <param name="p">The page parameters used to filter and configure the attendance group specification. Cannot be null.</param>
    public PagedAttendanceGroupSpecification(AttendanceGroupPageParameters p)
    {
        Criteria = PredicateBuilder.New<AttendanceGroup>(c => c.ParentGroupId == p.ParentGroupId);

        if (p.AttendanceType.HasValue)
            Criteria = Criteria.And(c => c.Type == p.AttendanceType);
    }
}

