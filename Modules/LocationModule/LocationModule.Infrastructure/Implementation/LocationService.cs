using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using LocationModule.Domain.DataTransferObjects;
using LocationModule.Domain.Entities;
using LocationModule.Domain.Interfaces;
using LocationModule.Domain.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace LocationModule.Infrastructure.Implementation
{
    /// <summary>
    /// Provides methods for managing and retrieving location data, including paginated and full lists of locations.
    /// </summary>
    /// <remarks>This service interacts with a repository to fetch location data and transform it into DTOs
    /// for client consumption. It supports retrieving all locations or a paginated subset of locations based on the
    /// provided parameters.</remarks>
    public class LocationService : ILocationService
    {
        /// <summary>
        /// Gets or sets the repository context for managing <see cref="Location"/> entities.
        /// </summary>
        protected IRepository<Location, int> Context { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationService"/> class with the specified repository context.
        /// </summary>
        /// <param name="context">The repository context used to manage <see cref="Location"/> entities.
        /// This parameter cannot be <see langword="null"/>.</param>
        public LocationService(IRepository<Location, int> context)
        {
            Context = context;
        }

        /// <summary>
        /// Retrieves a paginated list of locations based on the specified page parameters.
        /// </summary>
        /// <remarks>This method queries the data source for locations, includes related destination data,
        /// and maps the results to <see cref="LocationDto"/> objects. The method returns a paginated result that
        /// encapsulates both the data and pagination details.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination settings, including the page number and page size.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a list of <see cref="LocationDto"/> objects for the specified
        /// page, along with pagination metadata such as total count, page size, and current page number. If the
        /// operation fails, the result will indicate failure with associated error messages.</returns>
        public async Task<PaginatedResult<LocationDto>> PagedLocationsAsync(LocationPageParameters pageParameters)
        {
            var result = Context.FindAll();
            if (result.Succeeded)
            {
                var response = await result.Data.ToListAsync();
                return PaginatedResult<LocationDto>.Success(response.Select(c => new LocationDto(c)).ToList(), response.Count, pageParameters.PageSize, pageParameters.PageNr);
            }
            return PaginatedResult<LocationDto>.Failure(result.Messages);  
        }

        /// <summary>
        /// Retrieves a list of all locations asynchronously.
        /// </summary>
        /// <remarks>This method fetches all locations from the data source, including their associated
        /// destination details, and maps them to a list of <see cref="LocationDto"/> objects. If the operation fails,
        /// the result will contain error messages describing the failure.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object, where <c>T</c> is a list of <see cref="LocationDto"/>. If successful, the result contains the list
        /// of locations; otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<List<LocationDto>>> AllLocationsAsync()
        {
            var result = Context.FindAll();
            if (result.Succeeded)
            {
                var response = await result.Data.Select(c => new LocationDto(c)).ToListAsync();
                return Result<List<LocationDto>>.Success(response);
            }
            return Result<List<LocationDto>>.Fail(result.Messages);
        }
    }
}
