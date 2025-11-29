using System.ComponentModel.DataAnnotations;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Application.ViewModels
{
    /// <summary>
    /// Represents the view model for a school grade.
    /// </summary>
    public class SchoolGradeViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolGradeViewModel"/> class.
        /// </summary>
        public SchoolGradeViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolGradeViewModel"/> class with the specified DTO.
        /// </summary>
        /// <param name="dto">The data transfer object containing school grade information.</param>
        public SchoolGradeViewModel(SchoolGradeDto dto)
        {
            SchoolGradeId = dto.SchoolGradeId;
            SchoolGrade = dto.SchoolGrade;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ID of the school grade.
        /// </summary>
        public string? SchoolGradeId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the school grade.
        /// </summary>
        [Required] public string SchoolGrade { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of the school grade to a <see cref="SchoolGradeDto"/>.
        /// </summary>
        /// <returns>A <see cref="SchoolGradeDto"/> representing the current school grade, including its identifier and name.</returns>
        public SchoolGradeDto ToDto()
        {
            return new SchoolGradeDto
            {
                SchoolGradeId = this.SchoolGradeId!,
                SchoolGrade = this.SchoolGrade
            };
        }

        #endregion
    }
}