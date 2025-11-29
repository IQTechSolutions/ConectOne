using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Enums;

namespace SchoolsModule.Domain.DataTransferObjects;

/// <summary>
/// Data transfer object for <see cref="AttendanceGroup"/>.
/// </summary>
public record AttendanceGroupDto
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AttendanceGroupDto"/> class.
    /// </summary>
    public AttendanceGroupDto() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AttendanceGroupDto"/> class using the specified attendance group.
    /// </summary>
    /// <param name="group">The attendance group from which to initialize the DTO. Cannot be null.</param>
    public AttendanceGroupDto(AttendanceGroup group)
    {
        AttendanceGroupId = group.Id;
        Date = group.Date;
        Name = group.Name;
        Type = group.Type;
        ParentGroupId = group.ParentGroupId;
    }

    #endregion

    /// <summary>
    /// Gets the identifier for the attendance group.
    /// </summary>
    public string? AttendanceGroupId { get; init; }

    /// <summary>
    /// Gets the date associated with the current instance.
    /// </summary>
    public DateTime Date { get; init; }

    /// <summary>
    /// Gets the name associated with the current instance.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Gets the type of attendance.
    /// </summary>
    public AttendanceType Type { get; init; }

    /// <summary>
    /// Gets the identifier of the parent group.
    /// </summary>
    public string ParentGroupId { get; init; } = null!;

    /// <summary>
    /// Creates a new instance of the <see cref="AttendanceGroup"/> class with the specified properties.
    /// </summary>
    /// <remarks>If the <see cref="AttendanceGroupId"/> is null or empty, a new GUID is generated for the <see
    /// cref="AttendanceGroup.Id"/>.</remarks>
    /// <returns>A new <see cref="AttendanceGroup"/> object initialized with the current properties.</returns>
    public AttendanceGroup CreateAttendanceGroup() => new()
    {
        Id = string.IsNullOrEmpty(AttendanceGroupId) ? Guid.NewGuid().ToString() : AttendanceGroupId,
        Date = Date,
        Name = Name,
        Type = Type,
        ParentGroupId = ParentGroupId
    };
}

