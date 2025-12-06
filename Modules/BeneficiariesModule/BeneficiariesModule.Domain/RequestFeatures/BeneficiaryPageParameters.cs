using ConectOne.Domain.RequestFeatures;

namespace BeneficiariesModule.Domain.RequestFeatures
{
	/// <summary>
	/// Represents the parameters used for paginated retrieval of beneficiary data.
	/// </summary>
	/// <remarks>This class provides options for filtering, sorting, and paginating beneficiary data. It is
	/// typically used in API requests to specify the desired subset of data.</remarks>
	public class BeneficiaryPageParameters : RequestParameters
	{
		#region Consturctors

		/// <summary>
		/// Initializes a new instance of the <see cref="BeneficiaryPageParameters"/> class with default values.
		/// </summary>
		/// <remarks>The default values are: <list type="bullet"> <item><description><c>OrderBy</c>: "Name
		/// asc"</description></item> <item><description><c>PageSize</c>: 25</description></item> </list> These defaults can
		/// be modified after initialization by setting the respective properties.</remarks>
		public BeneficiaryPageParameters()
		{
			OrderBy = "Name asc";
			PageSize = 25;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BeneficiaryPageParameters"/> class with parameters for paginated and
		/// filtered retrieval of beneficiaries.
		/// </summary>
		/// <param name="userId">The unique identifier of the user requesting the data. Cannot be null or empty.</param>
		/// <param name="sortOrder">The sort order for the results, specified as a string (e.g., "Name asc"). Defaults to "Name asc".</param>
		/// <param name="pageNr">The page number to retrieve. Must be greater than or equal to 1. Defaults to 1.</param>
		/// <param name="pageSize">The number of items per page. Must be greater than 0. Defaults to 25.</param>
		/// <param name="active">A value indicating whether to filter results to include only active beneficiaries. Defaults to <see
		/// langword="true"/>.</param>
		/// <param name="searchText">An optional search text used to filter results by matching beneficiary names or other relevant fields. Defaults to
		/// an empty string.</param>
		public BeneficiaryPageParameters(string userId, string sortOrder = "Name asc", int pageNr = 1, int pageSize = 25, bool active = true, string? searchText = "")
		{
			OrderBy = sortOrder;
			PageNr = pageNr;
			PageSize = pageSize;
			Active = active;
			SearchText = searchText;
			UserId = userId;
		}

		#endregion

		/// <summary>
		/// Gets or sets the unique identifier for the user.
		/// </summary>
		public string? UserId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the entity is active.
		/// </summary>
		public bool Active { get; set; } = true;
	}
}