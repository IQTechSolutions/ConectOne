using ConectOne.Domain.Enums;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Application.ViewModels
{
    /// <summary>
    /// ViewModel for representing an Activity Group.
    /// </summary>
    public class ActivityGroupViewModel
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ActivityGroupViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityGroupViewModel"/> class from an <see cref="ActivityGroupDto"/>.
        /// </summary>
        /// <param name="dto">The data transfer object containing activity group information.</param>
        public ActivityGroupViewModel(ActivityGroupDto dto)
        {
            ActivityGroupId = dto.ActivityGroupId;
            Name = dto.Name;
            Gender = dto.Gender;
            AutoCreateChatGroup = dto.AutoCreateChatGroup;
            CoverImageUrl = dto.CoverImageUrl;

            if (dto.CategoryId is not null) CategoryId = dto.CategoryId;
            if (dto.CategoryName is not null) CategoryName = dto.CategoryName;

            if (dto.Teacher is not null) SelectedTeacher = new TeacherViewModel(dto.Teacher);

            if (dto.AgeGroup is not null) SelectedAgeGroup = new AgeGroupViewModel(dto.AgeGroup);

            SelectedTeamMembers = dto.TeamMembers.ToList();
            AnyAttendanceConsentRequired = dto.AnyAttendanceConsentRequired;
            AnyTransportConsentRequired = dto.AnyTransportConsentRequired;
        }

        #endregion

        /// <summary>
        /// Gets or sets the ID of the Activity Group.
        /// </summary>
        public string? ActivityGroupId { get; set; }

        /// <summary>
        /// Gets or sets the URL of the cover image for the Activity Group.
        /// </summary>
        public string CoverImageUrl { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the Activity Group.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether a chat group should be automatically created for the class.
        /// </summary>
        public bool AutoCreateChatGroup { get; set; }

        /// <summary>
        /// Gets or sets the gender associated with the Activity Group.
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Gets or sets the ID of the category to which the Activity Group belongs.
        /// </summary>
        public string CategoryId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the category to which the Activity Group belongs.
        /// </summary>
        public string CategoryName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the selected teacher for the Activity Group.
        /// </summary>
        public TeacherViewModel? SelectedTeacher { get; set; }

        /// <summary>
        /// Gets or sets the collection of available teachers for the Activity Group.
        /// </summary>
        public virtual ICollection<TeacherViewModel> AvailableTeachers { get; set; } = new List<TeacherViewModel>();

        /// <summary>
        /// Gets or sets the selected age group for the Activity Group.
        /// </summary>
        public AgeGroupViewModel? SelectedAgeGroup { get; set; }

        /// <summary>
        /// Gets or sets the collection of available age groups for the Activity Group.
        /// </summary>
        public virtual ICollection<AgeGroupViewModel> AvailableAgeGroups { get; set; } = new List<AgeGroupViewModel>();

        /// <summary>
        /// Gets or sets a value indicating whether to show team members.
        /// </summary>
        public bool ShowTeamMembers { get; set; } = false;

        /// <summary>
        /// Gets or sets the collection of selected team members for the Activity Group.
        /// </summary>
        public List<LearnerDto> SelectedTeamMembers { get; set; } = new List<LearnerDto>();

        /// <summary>
        /// Gets or sets a value indicating whether any attendance consent is required.
        /// </summary>
        public bool AnyAttendanceConsentRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether any transport consent is required.
        /// </summary>
        public bool AnyTransportConsentRequired { get; set; }

        #region Methods

        /// <summary>
        /// Converts the current instance of the activity group to its corresponding data transfer object (DTO).
        /// </summary>
        /// <remarks>This method creates an <see cref="ActivityGroupDto"/> representation of the current
        /// activity group,  including its properties such as ID, name, gender, category, and associated entities like
        /// age group,  teacher, and team members. If the <c>ActivityGroupId</c> is null or empty, a new GUID is
        /// generated.</remarks>
        /// <returns>An <see cref="ActivityGroupDto"/> object that represents the current activity group.</returns>
        public ActivityGroupDto ToDto()
        {
            return new ActivityGroupDto()
            {
                ActivityGroupId = string.IsNullOrEmpty(ActivityGroupId) ? Guid.NewGuid().ToString() : ActivityGroupId,
                Name = Name,
                Gender = Gender,
                CategoryId = CategoryId,
                AutoCreateChatGroup = AutoCreateChatGroup,
                CoverImageUrl = CoverImageUrl,
                AgeGroup = SelectedAgeGroup != null ? SelectedAgeGroup.ToDto() : null,
                Teacher = SelectedTeacher != null ? SelectedTeacher.ToDto() : null,
                TeamMembers = SelectedTeamMembers.ToList()
            };
        }

        #endregion
    }
}
