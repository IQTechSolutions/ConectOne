using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Interfaces;
using BusinessModule.Domain.RequestFeatures;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace BusinessModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for querying business directory listings, including paginated results, active listings,  and
    /// specific listings by identifier.
    /// </summary>
    /// <remarks>This service acts as a REST client for interacting with the business directory API. It
    /// supports retrieving  paginated business listings, fetching all active listings, and retrieving a specific active
    /// listing by its unique identifier.</remarks>
    /// <param name="provider"></param>
    public class BusinessDirectoryQueryRestService(IBaseHttpProvider provider) : IBusinessDirectoryQueryService
    {
        /// <summary>
        /// Retrieves a paginated list of business listings based on the specified page parameters.
        /// </summary>
        /// <remarks>This method queries the "businessdirectory/paged" endpoint to retrieve the paginated
        /// data.  Ensure that the <paramref name="pageParameters"/> object is properly configured to specify the
        /// desired page size, page number, and any applicable filters.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering options for the business listings.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of <see cref="BusinessListingDto"/>
        /// objects.</returns>
        public async Task<PaginatedResult<BusinessListingDto>> PagedListingsAsync(BusinessListingPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<BusinessListingDto, BusinessListingPageParameters>("businessdirectory/paged", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a collection of active business listings.
        /// </summary>
        /// <remarks>This method fetches the active business listings from the underlying data provider.
        /// The returned collection may be empty if no active listings are available.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="BusinessListingDto"/> objects representing the active
        /// business listings.</returns>
        public async Task<IBaseResult<IEnumerable<BusinessListingDto>>> ActiveListingsAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<BusinessListingDto>>("businessdirectory/active");
            return result;
        }

        /// <summary>
        /// Retrieves the active business listing associated with the specified identifier.
        /// </summary>
        /// <remarks>This method retrieves the active business listing from the provider using the
        /// specified identifier. Ensure that the identifier corresponds to an active listing; otherwise, the result may
        /// indicate that no data was found.</remarks>
        /// <param name="id">The unique identifier of the business listing to retrieve. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="BusinessListingDto"/> for the specified business listing.</returns>
        public async Task<IBaseResult<BusinessListingDto>> ListingAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<BusinessListingDto>($"businessdirectory/{id}");
            return result;
        }
    }
}
