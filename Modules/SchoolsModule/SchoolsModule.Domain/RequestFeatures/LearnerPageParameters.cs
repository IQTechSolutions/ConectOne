using ConectOne.Domain.Enums;
using ConectOne.Domain.RequestFeatures;

namespace SchoolsModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters used for paginated and filtered requests to retrieve learner-related data.
    /// </summary>
    /// <remarks>This class provides a flexible way to specify filtering, sorting, and pagination options for
    /// learner-related queries. It includes properties for filtering by learner-specific attributes such as <see
    /// cref="LearnerId"/>, <see cref="Gender"/>,  and age range, as well as additional contextual filters like <see
    /// cref="ParentId"/>, <see cref="CategoryId"/>, and <see cref="EventId"/>.</remarks>
    public class LearnerPageParameters : RequestParameters
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LearnerPageParameters"/> class.
        /// </summary>
        public LearnerPageParameters() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LearnerPageParameters"/> class with the specified pagination,
        /// sorting, and filtering options.
        /// </summary>
        /// <param name="sortOrder">The sort order to apply to the results. This can be a field name or a predefined sort key.</param>
        /// <param name="pageNr">The page number to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of items per page. Defaults to 50.</param>
        /// <param name="searchString">The search text used to filter the results. Defaults to an empty string, which means no filtering is
        /// applied.</param>
        /// <param name="parentId">An optional identifier for the parent entity to filter results by. Can be <see langword="null"/> if no
        /// parent filtering is required.</param>
        public LearnerPageParameters(string sortOrder, int pageNr = 1, int pageSize = 50, string searchString = "", string? parentId = null) : base(pageNr, pageSize, sortOrder)
        {
            SearchText = searchString;
            ParentId = parentId;
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for the learner.
        /// </summary>
        public string? LearnerId { get; set; }

        /// <summary>
        /// Gets or sets the search text used to filter or query data.
        /// </summary>
        public string? SearchText { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the parent entity.
        /// </summary>
        public string? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the category.
        /// </summary>
		public string? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the activity group.
        /// </summary>
		public string? ActivityGroupId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the event.
        /// </summary>
        public string? EventId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the school class.
        /// </summary>
        public string? SchoolClassId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the grade.
        /// </summary>
        public string? GradeId { get; set; }

        /// <summary>
        /// Gets or sets the gender of the individual.
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// Gets or sets the minimum age required for eligibility.
        /// </summary>
        public int MinAge { get; set; } = 0;

        /// <summary>
        /// Gets or sets the maximum allowable age.
        /// </summary>
        public int MaxAge { get; set; } = 100;

        /// <summary>
        /// Gets or sets a value indicating whether learners should be linked.
        /// </summary>
		public bool LinkLearners { get; set; }
    }
}
