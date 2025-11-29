using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object (DTO) representing the structure of a school class used across application layers.
    /// </summary>
    public record SchoolClassDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolClassDto"/> class.
        /// </summary>
        public SchoolClassDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolClassDto"/> class from a <see cref="SchoolClass"/> domain model.
        /// </summary>
        /// <param name="schoolClass">The domain model containing class data.</param>
        public SchoolClassDto(SchoolClass schoolClass)
        {
            SchoolClassId = schoolClass.Id;
            SchoolClass = schoolClass.Name;
            GradeId = schoolClass.GradeId!;
            GradeName = schoolClass.Grade?.Name;
            AutoCreateChatGroup = schoolClass.AutoCreateChatGroup;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier of the school class.
        /// </summary>
        public string? SchoolClassId { get; init; }

        /// <summary>
        /// Gets the display name or code of the school class.
        /// </summary>
        public string? SchoolClass { get; init; }

        /// <summary>
        /// Gets the number of learners in the class.
        /// </summary>
        public int LearnerCount { get; init; }

        /// <summary>
        /// Gets the unique identifier of the grade associated with the class.
        /// </summary>
        public string GradeId { get; init; } = null!;

        /// <summary>
        /// Gets the name of the grade associated with the class.
        /// </summary>
        public string? GradeName { get; init; }

        /// <summary>
        /// Gets a value indicating whether a chat group should be automatically created for the class.
        /// </summary>
        public bool AutoCreateChatGroup { get; init; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the DTO to a domain <see cref="SchoolClass"/> object.
        /// </summary>
        /// <returns>A new <see cref="SchoolClass"/> instance populated from this DTO.</returns>
        public SchoolClass CreateSchoolClass()
        {
            return new SchoolClass
            {
                Id = string.IsNullOrEmpty(SchoolClassId) ? Guid.NewGuid().ToString() : SchoolClassId,
                Name = SchoolClass!,
                AutoCreateChatGroup = AutoCreateChatGroup,
                GradeId = GradeId
            };
        }

        #endregion
    }
}
