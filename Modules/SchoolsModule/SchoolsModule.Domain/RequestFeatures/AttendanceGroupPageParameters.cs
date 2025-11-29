using ConectOne.Domain.RequestFeatures;
using SchoolsModule.Domain.Enums;

namespace SchoolsModule.Domain.RequestFeatures;

/// <summary>
/// Represents the parameters used for paginating attendance group requests.
/// </summary>
/// <remarks>This class is used to specify the criteria for retrieving attendance groups, including filtering by
/// attendance type and parent group identifier.</remarks>
public class AttendanceGroupPageParameters : RequestParameters
{
    /// <summary>
    /// Gets or sets the type of attendance for an event or session.
    /// </summary>
    public AttendanceType? AttendanceType { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the parent group.
    /// </summary>
    public string? ParentGroupId { get; set; }
}

