using ConectOne.Domain.RequestFeatures;

namespace SchoolsModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters used to filter and retrieve paginated data for school classes.
    /// </summary>
    /// <remarks>This class provides optional filtering criteria such as grade, teacher ID, or teacher email
    /// to narrow down the results when querying school class data.</remarks>
    public class SchoolClassPageParameters : RequestParameters
    {
        /// <summary>
        /// Gets or sets the identifier for the grade.
        /// </summary>
        public string? GradeId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the teacher.
        /// </summary>
        public string? TeacherId { get; set; }

        /// <summary>
        /// Gets or sets the email address of the teacher.
        /// </summary>
        public string? TeacherEmail { get; set; }
    }
}
