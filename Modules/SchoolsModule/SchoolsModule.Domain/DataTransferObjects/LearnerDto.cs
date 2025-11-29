using ConectOne.Domain.Enums;
using FilingModule.Domain.Enums;
using IdentityModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object (DTO) for Learner entity.
    /// Used to transfer learner data between different layers of the application.
    /// </summary>
    public record LearnerDto
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LearnerDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LearnerDto"/> class from a <see cref="Learner"/> entity.
        /// </summary>
        /// <param name="learner">The learner entity.</param>
        /// <param name="loadParents">Flag to indicate if parents should be loaded</param>
        public LearnerDto(Learner? learner, bool loadParents = false)
        {
            if (learner is null) return;

            LearnerId = learner.Id;
            FirstName = learner.FirstName;
            LastName = learner.LastName;
            DisplayName = learner.DisplayName;
            MiddleName = learner.MiddleName;
            Description = learner.Description;
            MedicalNotes = learner.MedicalNotes;
            MedicalAidParent = learner?.MedicalAidParent is null ? new ParentDto() : new ParentDto(learner.MedicalAidParent);
            IdNumber = learner.IdNumber;
            Gender = learner.Gender;
            RecieveEmails = learner.RecieveEmails;
            ReceiveMessages = learner.ReceiveMessages;
            ReceiveNotifications = learner.ReceiveNotifications;
            RequireConsentFromAllParents = learner.RequireConsentFromAllParents;
            CoverImageUrl = learner.Images?.FirstOrDefault(c => c.Image.ImageType == UploadType.Cover)?.Image.RelativePath ?? "_content/FilingModule.Blazor/images/profileImage128x128.png";

            if (learner.SchoolGrade is not null)
            {
                Grade = new SchoolGradeDto(learner.SchoolGrade);
            }
            if (learner.SchoolClass is not null)
            {
                SchoolClass = new SchoolClassDto(learner.SchoolClass);
            }

            if (loadParents && learner.Parents.Any())
                SelectedParents = learner.Parents.Where(c => c.Parent != null).Select(c => new ParentDto(c.Parent!, false)).ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LearnerDto"/> class from a <see cref="ParticipatingActitivityGroupTeamMember"/> entity.
        /// </summary>
        /// <param name="learner">The participating activity group team member entity.</param>
        public LearnerDto(ParticipatingActitivityGroupTeamMember learner)
        {
            LearnerId = learner.TeamMember.Id;
            ParticipatingTeamMemberId = learner.Id;
            CoverImageUrl = learner.TeamMember.Images.FirstOrDefault(c => c.Image.ImageType == UploadType.Cover)?.Image.RelativePath ?? "_content/FilingModule.Blazor/images/profileImage128x128.png";
            FirstName = learner.TeamMember.FirstName;
            LastName = learner.TeamMember.LastName;
            DisplayName = learner.TeamMember.DisplayName;
            MiddleName = learner.TeamMember.MiddleName;
            Description = learner.TeamMember.Description;
            MedicalNotes = learner.TeamMember.MedicalNotes;
            MedicalAidParent = new ParentDto(learner.TeamMember.MedicalAidParent);
            IdNumber = learner.TeamMember.IdNumber;
            Gender = learner.TeamMember.Gender;
            RecieveEmails = learner.TeamMember.RecieveEmails;
            ReceiveMessages = learner.TeamMember.ReceiveMessages;
            ReceiveNotifications = learner.TeamMember.ReceiveNotifications;

            if (learner.TeamMember.SchoolGrade is not null)
            {
                Grade = new SchoolGradeDto(learner.TeamMember.SchoolGrade);
            }
            if (learner.TeamMember.SchoolClass is not null)
            {
                SchoolClass = new SchoolClassDto(learner.TeamMember.SchoolClass);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LearnerDto"/> class from a <see cref="UserInfoViewModel"/>.
        /// </summary>
        /// <param name="learner">The user info view model.</param>
        public LearnerDto(UserInfoDto learner)
        {
            LearnerId = learner.UserId;
            CoverImageUrl = learner.CoverImageUrl;
            FirstName = learner.FirstName;
            LastName = learner.LastName;
            DisplayName = learner.DisplayName;
            Description = learner.Description;
            RecieveEmails = learner.ReceiveEmails;
            ReceiveMessages = learner.ReceiveMessages;
            ReceiveNotifications = learner.ReceiveNotifications;
        }

        #endregion

        /// <summary>
        /// Gets or sets the learner ID.
        /// </summary>
        public string? LearnerId { get; init; }

        /// <summary>
        /// Gets or sets the participating team member ID.
        /// </summary>
        public string? ParticipatingTeamMemberId { get; init; }

        /// <summary>
        /// Gets or sets the cover image URL.
        /// </summary>
        public string? CoverImageUrl { get; init; } = "_content/FilingModule.Blazor/images/profileImage128x128.png";

        /// <summary>
        /// Gets or sets the first name of the learner.
        /// </summary>
        public string? FirstName { get; init; }

        /// <summary>
        /// Gets or sets the middle name of the learner.
        /// </summary>
        public string? MiddleName { get; init; }

        /// <summary>
        /// Gets or sets the display name of the learner.
        /// </summary>
        public string? DisplayName { get; init; }

        /// <summary>
        /// Gets or sets the last name of the learner.
        /// </summary>
        public string? LastName { get; init; }

        /// <summary>
        /// Gets or sets the phone number of the learner.
        /// </summary>
        public string? PhoneNr { get; set; }

        /// <summary>
        /// Gets or sets the email address of the learner.
        /// </summary>
        public string? EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the description of the learner.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets or sets additional medical notes for the learner.
        /// </summary>
        public string? MedicalNotes { get; init; }

        /// <summary>
        /// Gets or sets the identifier of the parent whose medical aid applies to this learner.
        /// </summary>
        public ParentDto? MedicalAidParent { get; init; }

        /// <summary>
        /// Gets or sets the ID number of the learner.
        /// </summary>
        public string? IdNumber { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether consent is required from all parents.
        /// </summary>
        public bool RequireConsentFromAllParents { get; set; }

        /// <summary>
        /// Indicates whether the parent should receive notifications (push, SMS, or other types).
        /// </summary>
        public bool ReceiveNotifications { get; init; } = true;

        /// <summary>
        /// Indicates whether the parent should receive messages related to school activities, announcements, etc.
        /// </summary>
        public bool ReceiveMessages { get; init; } = true;

        /// <summary>
        /// Indicates whether the parent should receive emails for communication from the school.
        /// </summary>
        public bool RecieveEmails { get; init; } = true;

        /// <summary>
        /// Gets or sets the gender of the learner.
        /// </summary>
        public Gender Gender { get; init; }

        /// <summary>
        /// Gets or sets the grade of the learner.
        /// </summary>
        public SchoolGradeDto? Grade { get; init; }

        /// <summary>
        /// Gets or sets the school class of the learner.
        /// </summary>
        public SchoolClassDto? SchoolClass { get; init; }

        /// <summary>
        /// Gets or sets the selected parents of the learner.
        /// </summary>
        public List<ParentDto> SelectedParents { get; init; } = [];

        /// <summary>
        /// Returns a string representation of the learner.
        /// </summary>
        /// <returns>A string that represents the learner.</returns>
        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        /// <summary>
        /// Creates and returns a <see cref="UserInfoDto"/> object populated with the current user's information.
        /// </summary>
        /// <remarks>The returned <see cref="UserInfoDto"/> object includes properties such as the user's
        /// name, description,  and communication preferences. All boolean properties related to notifications,
        /// messages, and emails  are set to <see langword="false"/> by default.</remarks>
        /// <returns>A <see cref="UserInfoDto"/> instance containing the user's details, such as identifiers, contact
        /// information,  and preferences for notifications, messages, and emails.</returns>
        public UserInfoDto GetUserInfoDto()
        {
            return new UserInfoDto()
            {
                UserId = LearnerId,
                CoverImageUrl = CoverImageUrl,
                FirstName = FirstName,
                MiddleName = MiddleName,
                DisplayName = DisplayName,
                LastName = LastName,
                Description = Description,
                ReceiveNotifications = ReceiveNotifications,
                ReceiveMessages = ReceiveMessages,
                ReceiveEmails = RecieveEmails,
                RequireConsent = RequireConsentFromAllParents,
                EmailAddress = EmailAddress,
                PhoneNr = PhoneNr
            };
        }
    }
}
