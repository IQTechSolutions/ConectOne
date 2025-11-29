using ConectOne.Domain.RequestFeatures;
using IdentityModule.Domain.Enums;

namespace IdentityModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters used for paginated requests to retrieve user data.
    /// Includes options for sorting, filtering by active status, registration status, and role.
    /// </summary>
    public class UserPageParameters : RequestParameters
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPageParameters"/> class with default values.
        /// </summary>
        public UserPageParameters()
        {
            OrderBy = "FirstName asc";
            PageSize = 25;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPageParameters"/> class with specified values.
        /// </summary>
        /// <param name="sortOrder">The field and direction to sort by (e.g., "FirstName asc").</param>
        /// <param name="pageNr">The page number to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of items per page. Defaults to 25.</param>
        /// <param name="active">Indicates whether to filter by active users. Defaults to true.</param>
        /// <param name="searchText">The search text to filter results. Defaults to null.</param>
        public UserPageParameters(string sortOrder = "FirstName asc", int pageNr = 1, int pageSize = 25, bool active = true, string? searchText = null)
        {
            OrderBy = sortOrder;
            PageNr = pageNr;
            PageSize = pageSize;
            Active = active;
            SearchText = searchText;
        }

        #endregion        

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether to filter by active users.
        /// Defaults to true.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Gets or sets the registration status to filter users by.
        /// Can be null to include all registration statuses.
        /// </summary>
        public RegistrationStatus? RegistrationStatus { get; set; } = null;

        /// <summary>
        /// Gets or sets the role to filter users by.
        /// Can be null to include all roles.
        /// </summary>
        public string? Role { get; set; } 

        #endregion
    }
}
