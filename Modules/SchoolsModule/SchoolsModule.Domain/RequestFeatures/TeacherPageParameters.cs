using ConectOne.Domain.RequestFeatures;

namespace SchoolsModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters used for paginated and sortable requests to retrieve teacher-related data.
    /// </summary>
    /// <remarks>This class extends <see cref="RequestParameters"/> to include additional filtering options
    /// specific to teachers,  such as search text, teacher ID, class ID, and grade ID. It is typically used to
    /// configure query parameters  for API endpoints or data retrieval methods that support pagination, sorting, and
    /// filtering.</remarks>
    public class TeacherPageParameters : RequestParameters
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherPageParameters"/> class.
        /// </summary>
        public TeacherPageParameters() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherPageParameters"/> class with the specified pagination
        /// and sorting options, as well as a search string.
        /// </summary>
        /// <param name="sortOrder">The order in which the results should be sorted. This can be a field name or a predefined sort key.</param>
        /// <param name="pageNr">The page number to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of items to include on each page. Defaults to 50.</param>
        /// <param name="searchString">The search text used to filter the results. Defaults to an empty string, which means no filtering is
        /// applied.</param>
        public TeacherPageParameters(string sortOrder, int pageNr = 1, int pageSize = 50, string searchString = "") : base(pageNr, pageSize, sortOrder)
        {
            SearchText = searchString;
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for the teacher.
        /// </summary>
        public string? TeacherId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the class.
        /// </summary>
        public string? ClassId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the grade.
        /// </summary>
        public string? GradeId { get; set; }

    }
}
