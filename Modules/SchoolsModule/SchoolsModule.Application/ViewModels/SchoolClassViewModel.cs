using System.ComponentModel.DataAnnotations;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Application.ViewModels
{
    /// <summary>
    /// ViewModel representing a school class, including class name, grade details, and chat group settings.
    /// </summary>
    public class SchoolClassViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolClassViewModel"/> class.
        /// </summary>
        public SchoolClassViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolClassViewModel"/> class from a <see cref="SchoolClassDto"/>.
        /// </summary>
        /// <param name="dto">The DTO containing school class data.</param>
        public SchoolClassViewModel(SchoolClassDto dto)
        {
            SchoolClassId = dto.SchoolClassId;
            if (dto.SchoolClass != null) SchoolClass = dto.SchoolClass;
            LearnerCount = dto.LearnerCount;
            GradeId = dto.GradeId;
            GradeName = dto.GradeName;
            AutoCreateChatGroup = dto.AutoCreateChatGroup;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier of the school class.
        /// </summary>
        public string? SchoolClassId { get; set; }

        /// <summary>
        /// Gets or sets the display name or code of the school class.
        /// This field is required.
        /// </summary>
        [Required] public string SchoolClass { get; set; } = null!;

        /// <summary>
        /// Gets or sets the number of learners in the class.
        /// </summary>
        public int LearnerCount { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the grade to which this class belongs.
        /// </summary>
        public string GradeId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the grade associated with the class.
        /// </summary>
        public string GradeName { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether a chat group should be automatically created for the class.
        /// </summary>
        public bool AutoCreateChatGroup { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of the school class to a <see cref="SchoolClassDto"/>.
        /// </summary>
        /// <returns>A <see cref="SchoolClassDto"/> object containing the data from the current school class instance.</returns>
        public SchoolClassDto ToDto()
        {
            return new SchoolClassDto
            {
                SchoolClassId = SchoolClassId,
                SchoolClass = SchoolClass,
                LearnerCount = LearnerCount,
                GradeId = GradeId,
                GradeName = GradeName,
                AutoCreateChatGroup = AutoCreateChatGroup
            };
        }

        #endregion
    }
}
