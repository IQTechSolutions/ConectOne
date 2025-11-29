using System.ComponentModel.DataAnnotations;
using ConectOne.Domain.Enums;
using ConectOne.Domain.Extensions;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Application.ViewModels
{
    /// <summary>
    /// Represents the view model for a learner, encapsulating various details
    /// such as personal information, emergency medical info, and school-related data.
    /// </summary>
    public class LearnerViewModel
    {
        #region Constructors

        /// <summary>
        /// Default constructor for LearnerViewModel.
        /// </summary>
        public LearnerViewModel()
        {
        }

        /// <summary>
        /// Constructs a LearnerViewModel from a LearnerDto.
        /// Maps the properties from the DTO to the view model.
        /// </summary>
        /// <param name="dto">The data transfer object containing learner information.</param>
        public LearnerViewModel(LearnerDto dto)
        {
            ParticipatingTeamMemberId = dto.ParticipatingTeamMemberId;
            LearnerId = dto.LearnerId;
            FirstName = dto.FirstName;
            MiddleNames = dto.MiddleName;
            DisplayName = dto.DisplayName;
            LastName = dto.LastName;
            Description = dto.Description;
            CoverImageUrl = dto.CoverImageUrl;
            MedicalNotes = dto.MedicalNotes;
            MedicalAidParent = dto.MedicalAidParent;
            IdNumber = dto.IdNumber;
            Gender = dto.Gender;

            if (dto.Grade != null) SelectedGrade = dto.Grade;
            if (dto.SchoolClass != null) SelectedSchoolClass = dto.SchoolClass;
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for the learner.
        /// </summary>
        public string? LearnerId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets the unique identifier for the participating team member.
        /// </summary>
        public string? ParticipatingTeamMemberId { get; }

        /// <summary>
        /// Gets or sets the URL for the cover image of the learner.
        /// </summary>
        public string? CoverImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the first name of the learner.
        /// </summary>
        [Required]
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the middle names of the learner.
        /// </summary>
        public string? MiddleNames { get; set; }

        /// <summary>
        /// Gets or sets the display name of the learner.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the learner.
        /// </summary>
        [Required]
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description or additional notes about the learner.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets additional medical notes about the learner.
        /// </summary>
        public string? MedicalNotes { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the parent whose medical aid applies to the learner.
        /// </summary>
        public ParentDto? MedicalAidParent { get; set; }

        /// <summary>
        /// Gets or sets the mobile number of the learner.
        /// </summary>
        [MinLength(11)]public string? MobileNr { get; set; }

        /// <summary>
        /// Gets or sets the email address of the learner.
        /// </summary>
        public string? EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the identification number of the learner.
        /// </summary>
        [MinLength(13)] public string? IdNumber { get; set; }

        /// <summary>
        /// Gets the date of birth of the learner based on the identification number.
        /// If the IdNumber is null or empty, returns the current date.
        /// </summary>
        public DateTime DateOfBirth => string.IsNullOrEmpty(IdNumber) ? DateTime.Now : IdNumber.GetDateOfBirth();

        /// <summary>
        /// Gets the age of the learner based on the identification number.
        /// If the IdNumber is null or empty, returns 0.
        /// </summary>
        public int Age => string.IsNullOrEmpty(IdNumber) ? 0 : IdNumber.GetAge();

        /// <summary>
        /// Gets or sets the gender of the learner.
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Gets or sets the selected grade of the learner.
        /// </summary>
        public SchoolGradeDto? SelectedGrade { get; set; }

        /// <summary>
        /// Gets or sets the selected school class of the learner.
        /// </summary>
        public SchoolClassDto? SelectedSchoolClass { get; set; }

        #region Methods

        /// <summary>
        /// Converts the current learner entity to a <see cref="LearnerDto"/> object.
        /// </summary>
        /// <remarks>This method maps the properties of the current learner entity to a new <see
        /// cref="LearnerDto"/> instance. Nullable properties such as <c>SelectedGrade</c> and
        /// <c>SelectedSchoolClass</c> are included only if they are not null.</remarks>
        /// <returns>A <see cref="LearnerDto"/> object containing the mapped data from the current learner entity.</returns>
        public LearnerDto ToDto()
        {
            return new LearnerDto
            {
                LearnerId = LearnerId,
                CoverImageUrl = CoverImageUrl,
                FirstName = FirstName,
                LastName = LastName,
                DisplayName = DisplayName,
                MiddleName = MiddleNames,
                Description = Description,
                MedicalNotes = MedicalNotes,
                MedicalAidParent = MedicalAidParent,
                IdNumber = IdNumber,
                Gender = Gender,
                Grade = SelectedGrade is not null ? SelectedGrade : null,
                SchoolClass = SelectedSchoolClass is not null ? SelectedSchoolClass : null
            };
        }

        #endregion
        
    }
}
