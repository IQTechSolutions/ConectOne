using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Represents a specification for exporting attendance group data based on the provided request criteria.
    /// </summary>
    /// <remarks>This specification is used to filter and include related data when exporting attendance group
    /// information. It applies criteria based on the group ID and optionally the attendance type, as specified in the
    /// request. Additionally, it includes attendance records and their associated learners in the query.</remarks>
    public class ExportAttendanceGroupSpec : Specification<AttendanceGroup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportAttendanceGroupSpec"/> class with the specified export
        /// request.
        /// </summary>
        /// <remarks>This constructor sets up the filtering criteria for attendance groups based on the
        /// provided request. If <c>AttendanceType</c> is specified in the request, the criteria will filter by both
        /// <c>GroupId</c> and <c>AttendanceType</c>. Additionally, it includes related attendance records and learners
        /// in the query.</remarks>
        /// <param name="request">The export request containing the criteria for filtering attendance groups.  The <paramref name="request"/>
        /// must specify a valid <c>GroupId</c>, and optionally an <c>AttendanceType</c>.</param>
        public ExportAttendanceGroupSpec(ExportAttendanceRequest request)
        {
            Criteria = l => l.Id == request.GroupId;
            if (request.AttendanceType.HasValue)
            {
                Criteria = l => l.Id == request.GroupId && l.Type == request.AttendanceType;
            }

            AddInclude(l => l.Include(c => c.AttendanceRecords).ThenInclude(c => c.Learner));
        }
    }
}
