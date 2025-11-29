using ConectOne.Domain.Enums;
using FilingModule.Domain.Enums;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object for Activity Group.
    /// </summary>
    public record ActivityGroupDto
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ActivityGroupDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityGroupDto"/> class from an <see cref="ActivityGroup"/> entity.
        /// </summary>
        /// <param name="activityGroup">The activity group entity.</param>
        public ActivityGroupDto(ActivityGroup activityGroup)
        {
            ActivityGroupId = activityGroup.Id;
            Name = activityGroup.Name;
            Gender = activityGroup.Gender;
            CategoryId = activityGroup.Categories.FirstOrDefault()?.CategoryId;
            CategoryName = activityGroup.Categories.FirstOrDefault()?.Category.Name;
            AutoCreateChatGroup = activityGroup.AutoCreateChatGroup;

            CoverImageUrl = activityGroup.Images.FirstOrDefault(c => c.Image.ImageType == UploadType.Cover)?.Image.RelativePath ?? "_content/SchoolsModule.Blazor/images/NoImage.jpg";

            if (activityGroup.AgeGroup is not null)
            {
                AgeGroup = new AgeGroupDto(activityGroup.AgeGroup);
            }
            if (activityGroup.Teacher is not null)
            {
                Teacher = new TeacherDto(activityGroup.Teacher);
            }

            TeamMembers = activityGroup.TeamMembers.Select(c => new LearnerDto(c.Learner, false)).ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityGroupDto"/> class from a <see cref="ParticipatingActivityGroup"/> entity.
        /// </summary>
        /// <param name="activityGroup">The participating activity group entity.</param>
        public ActivityGroupDto(ParticipatingActivityGroup activityGroup)
        {
            ParticipatingActivityGroupId = activityGroup.Id;
            ActivityGroupId = activityGroup.ActivityGroupId;
            Name = activityGroup.ActivityGroup.Name;
            Gender = activityGroup.ActivityGroup.Gender;
            CategoryId = activityGroup.ActivityGroup.Categories.FirstOrDefault()?.CategoryId;
            CategoryName = activityGroup.ActivityGroup.Categories.FirstOrDefault()?.Category.Name;
            AutoCreateChatGroup = activityGroup.ActivityGroup.AutoCreateChatGroup;

            CoverImageUrl = activityGroup.ActivityGroup.Images.FirstOrDefault()?.Image.RelativePath ?? "_content/SchoolsModule.Blazor/images/NoImage.jpg";

            if (activityGroup.ActivityGroup.AgeGroup is not null)
            {
                AgeGroup = new AgeGroupDto(activityGroup.ActivityGroup.AgeGroup);
            }
            if (activityGroup.ActivityGroup.Teacher is not null)
            {
                Teacher = new TeacherDto(activityGroup.ActivityGroup.Teacher);
            }

            TeamMembers = activityGroup.ParticipatingTeamMembers.Select(c => new LearnerDto(c)).ToList();
        }

        #endregion

        /// <summary>
        /// Gets or sets the participating activity group ID.
        /// </summary>
        public string? ParticipatingActivityGroupId { get; set; }

        /// <summary>
        /// Gets or sets the activity group ID.
        /// </summary>
        public string? ActivityGroupId { get; set; }

        /// <summary>
        /// Gets or sets the URL of the cover image associated with the item.
        /// </summary>
        public string? CoverImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the activity group.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the gender associated with the activity group.
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Gets or sets the category ID of the activity group.
        /// </summary>
        public string? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category name of the activity group.
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the age group associated with the activity group.
        /// </summary>
        public AgeGroupDto? AgeGroup { get; set; }

        /// <summary>
        /// Gets or sets the teacher associated with the activity group.
        /// </summary>
        public TeacherDto? Teacher { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether any attendance consent is required.
        /// </summary>
        public bool AnyAttendanceConsentRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether any transport consent is required.
        /// </summary>
        public bool AnyTransportConsentRequired { get; set; }

        /// <summary>
        /// Gets or sets the collection of team members in the activity group.
        /// </summary>
        public ICollection<LearnerDto> TeamMembers { get; set; } = new List<LearnerDto>();

        /// <summary>
        /// Gets or sets a value indicating whether to show team members.
        /// </summary>
        public bool ShowTeamMembers { get; set; } = false;

        /// <summary>
        /// Gets a value indicating whether a chat group should be automatically created for the class.
        /// </summary>
        public bool AutoCreateChatGroup { get; init; }

        /// <summary>
        /// Creates an <see cref="ActivityGroup"/> entity from the current DTO.
        /// </summary>
        /// <returns>The created <see cref="ActivityGroup"/> entity.</returns>
        public ActivityGroup CreateActivityGroup()
        {
            return new ActivityGroup()
            {
                Id = string.IsNullOrEmpty(ActivityGroupId) ? Guid.NewGuid().ToString() : ActivityGroupId,
                Name = Name,
                Gender = Gender,
                AgeGroupId = AgeGroup?.AgeGroupId,
                TeacherId = Teacher?.TeacherId,
                AutoCreateChatGroup = AutoCreateChatGroup,
            };
        }
    }
}
