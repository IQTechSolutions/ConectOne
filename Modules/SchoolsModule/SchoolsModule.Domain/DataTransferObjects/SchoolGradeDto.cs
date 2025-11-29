using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object for a school grade.
    /// </summary>
    public record SchoolGradeDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolGradeDto"/> class.
        /// </summary>
        public SchoolGradeDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolGradeDto"/> class with the specified school grade entity.
        /// </summary>
        /// <param name="schoolGrade">The school grade entity.</param>
        public SchoolGradeDto(SchoolGrade schoolGrade)
        {
            SchoolGradeId = schoolGrade.Id;
            SchoolGrade = schoolGrade.Name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ID of the school grade.
        /// </summary>
        public string? SchoolGradeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the school grade.
        /// </summary>
        public string SchoolGrade { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new <see cref="SchoolGrade"/> entity from the current DTO.
        /// </summary>
        /// <returns>A new <see cref="SchoolGrade"/> entity.</returns>
        public SchoolGrade CreateSchoolGrade()
        {
            return new SchoolGrade
            {
                Id = string.IsNullOrEmpty(SchoolGradeId) ? Guid.NewGuid().ToString() : SchoolGradeId,
                Name = SchoolGrade
            };
        }

        #endregion
    }
}


