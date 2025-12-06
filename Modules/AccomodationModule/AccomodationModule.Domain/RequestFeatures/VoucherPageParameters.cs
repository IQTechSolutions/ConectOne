using ConectOne.Domain.RequestFeatures;

namespace AccomodationModule.Domain.RequestFeatures
{
	/// <summary>
	/// Represents the parameters used for paginated voucher requests, including sorting, filtering, and pagination
	/// options.
	/// </summary>
	/// <remarks>This class provides properties to configure the behavior of voucher queries, such as sorting by a
	/// specific field,  filtering by active status, and specifying pagination details like page number and page
	/// size.</remarks>
    public class VoucherPageParameters : RequestParameters
	{
		/// <summary>
		/// Gets or sets the unique identifier for the package.
		/// </summary>
		public string? PackageId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the entity is active.
		/// </summary>
		public bool Active { get; set; } = true;
    }
}