using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications;

/// <summary>
/// Specification for retrieving a single <see cref="SchoolGrade"/> with related classes.
/// </summary>
public sealed class SingleSchoolGradeSpecification : Specification<SchoolGrade>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SingleSchoolGradeSpecification"/> class.
    /// </summary>
    /// <param name="gradeId">The identifier of the grade to retrieve.</param>
    public SingleSchoolGradeSpecification(string gradeId)
    {
        Criteria = g => g.Id == gradeId;
        AddInclude(g => g.Include(gr => gr.Classes));
    }
}
