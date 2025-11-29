using ConectOne.Domain.RequestFeatures;

namespace SchoolsModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters used for paginated requests to retrieve school grade data.
    /// </summary>
    /// <remarks>This class provides options for specifying the page number, page size, and sort order for
    /// paginated queries. It is typically used to configure requests for retrieving school grade information in a
    /// paginated format.</remarks>
    public class SchoolGradePageParameters : RequestParameters
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolGradePageParameters"/> class.
        /// </summary>
        public SchoolGradePageParameters() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolGradePageParameters"/> class with the specified sort
        /// order, page number, and page size.
        /// </summary>
        /// <param name="sortOrder">The sort order to apply to the results. Can be null to use the default sort order.</param>
        /// <param name="pageNr">The page number to retrieve. Defaults to 1. Must be greater than or equal to 1.</param>
        /// <param name="pageSize">The number of items per page. Defaults to 50. Must be greater than 0.</param>
        public SchoolGradePageParameters(string? sortOrder, int pageNr = 1, int pageSize = 50) : base(pageNr, pageSize, sortOrder)
        {
            
        }

        #endregion
    }
}
