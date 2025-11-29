using System.ComponentModel.DataAnnotations;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Enums;

namespace SchoolsModule.Application.ViewModels
{
    /// <summary>
    /// View model used to create or edit <see cref="AttendanceGroupDto"/> records.
    /// </summary>
    public class AttendanceGroupViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AttendanceGroupViewModel"/> class.
        /// </summary>
        public AttendanceGroupViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttendanceGroupViewModel"/> class using the specified data
        /// transfer object.
        /// </summary>
        /// <param name="dto">The data transfer object containing the attendance group information. Must not be <see langword="null"/>.</param>
        public AttendanceGroupViewModel(AttendanceGroupDto dto)
        {
            AttendanceGroupId = dto.AttendanceGroupId;
            Date = dto.Date;
            Name = dto.Name;
            Type = dto.Type;
            ParentGroupId = dto.ParentGroupId;
        }

        #endregion

        /// <summary>
        /// Gets or sets the identifier for the attendance group.
        /// </summary>
        public string? AttendanceGroupId { get; set; }

        /// <summary>
        /// Gets or sets the date value. This property is required and defaults to the current date.
        /// </summary>
        [Required] public DateTime Date { get; set; } = DateTime.Today;

        /// <summary>
        /// Gets or sets the name associated with the entity.
        /// </summary>
        [Required] public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of attendance.
        /// </summary>
        public AttendanceType Type { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the parent group.
        /// </summary>
        public string ParentGroupId { get; set; } = string.Empty;

        #region Methods

        /// <summary>
        /// Converts the current instance of the attendance group to its corresponding data transfer object (DTO).
        /// </summary>
        /// <returns>An <see cref="AttendanceGroupDto"/> representing the current attendance group.  If <see
        /// cref="AttendanceGroupId"/> is null, the <see cref="AttendanceGroupDto.AttendanceGroupId"/> will be an empty
        /// string.</returns>
        public AttendanceGroupDto ToDto()
        {
            return new AttendanceGroupDto
            {
                AttendanceGroupId = AttendanceGroupId ?? string.Empty,
                Date = Date,
                Name = Name,
                Type = Type,
                ParentGroupId = ParentGroupId
            };
        }

        #endregion
    }
}
