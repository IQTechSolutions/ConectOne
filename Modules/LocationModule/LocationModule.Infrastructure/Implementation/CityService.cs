using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using LocationModule.Domain.DataTransferObjects;
using LocationModule.Domain.Entities;
using LocationModule.Domain.Interfaces;
using LocationModule.Domain.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace LocationModule.Infrastructure.Implementation
{
    /// <summary>
    /// Provides CRUD functionality for cities.
    /// </summary>
    public class CityService(IRepository<City, string> repository) : ICityService
    {
        private readonly IRepository<City, string> _repository = repository;

        /// <summary>
        /// Retrieves a paginated list of cities, including their associated country information.
        /// </summary>
        /// <remarks>This method queries the data source for cities and includes their associated country
        /// information. If the query is successful, the result contains the paginated list of cities. Otherwise, it
        /// returns a failure result with error messages.</remarks>
        /// <param name="pageParameters">The pagination parameters, including the page number and page size, used to determine the subset of cities
        /// to retrieve.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a list of <see cref="CityDto"/> objects representing the
        /// cities in the requested page, along with pagination metadata such as the total count of cities.</returns>
        public async Task<PaginatedResult<CityDto>> PagedCitiesAsync(CityPageParameters pageParameters)
        {
            var result = _repository.FindAll();
            if (result.Succeeded)
            {
                var response = await result.Data.Include(c => c.Country).ToListAsync();
                return PaginatedResult<CityDto>.Success(response.Select(c => new CityDto(c)).ToList(), response.Count, pageParameters.PageNr, pageParameters.PageSize);
            }
            return PaginatedResult<CityDto>.Failure(result.Messages);
        }

        /// <summary>
        /// Retrieves a collection of all cities.
        /// </summary>
        /// <remarks>This method asynchronously fetches a list of cities and returns the result as a
        /// collection of <see cref="CityDto"/> objects. If the operation fails, the result will include error messages
        /// describing the failure.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="CityDto"/>. The result indicates whether the
        /// operation succeeded and provides the corresponding data or error messages.</returns>
        public async Task<IBaseResult<IEnumerable<CityDto>>> AllCitiesAsync()
        {
            var result = await _repository.ListAsync();
            return !result.Succeeded
                ? await Result<IEnumerable<CityDto>>.FailAsync(result.Messages)
                : await Result<IEnumerable<CityDto>>.SuccessAsync(result.Data.Select(c => new CityDto(c)));
        }

        /// <summary>
        /// Retrieves a city by its unique identifier, including its associated country information.
        /// </summary>
        /// <remarks>This method queries the database for a city with the specified identifier. If the
        /// city is found, its data is returned as a <see cref="CityDto"/> object, including its associated country
        /// information. If no city matches the provided identifier, the result will indicate failure with an
        /// appropriate error message.</remarks>
        /// <param name="cityId">The unique identifier of the city to retrieve. Cannot be <see langword="null"/> or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is <see cref="CityDto"/>. If the operation succeeds, the result contains the city data;
        /// otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<CityDto>> CityAsync(string cityId)
        {
            var spec = new LambdaSpec<City>(c => c.Id == cityId);
            spec.AddInclude(c => c.Include(x => x.Country));

            var result = await _repository.FirstOrDefaultAsync(spec);
            if (!result.Succeeded)
                return await Result<CityDto>.FailAsync(result.Messages);
            if (result.Data == null)
                return await Result<CityDto>.FailAsync($"No city with id matching '{cityId}' was found in the database");
            return await Result<CityDto>.SuccessAsync(new CityDto(result.Data));
        }

        /// <summary>
        /// Creates a new city record asynchronously based on the provided data transfer object (DTO).
        /// </summary>
        /// <remarks>This method performs the following steps: 1. Creates a new city entity using the data
        /// provided in the <paramref name="dto"/>. 2. Attempts to save the new city to the repository. If any step
        /// fails, the method returns a failure result with the corresponding error messages.</remarks>
        /// <param name="dto">The <see cref="CityDto"/> containing the details of the city to be created. The <see cref="CityDto.Name"/>
        /// and <see cref="CityDto.Code"/> properties must not be null or empty.</param>
        /// <returns>An <see cref="IBaseResult{T}"/> containing the ID of the newly created city as a string if the operation
        /// succeeds. If the operation fails, the result contains error messages describing the failure.</returns>
        public async Task<IBaseResult<string>> CreateCityAsync(CityDto dto)
        {
            var createResult = await _repository.CreateAsync(new City
            {
                Name = dto.Name!,
                Code = dto.Code!,
                ShortName = dto.ShortName,
                Description = dto.Description,
                CountryId = dto.CountryId
            });
            if (!createResult.Succeeded)
                return await Result<string>.FailAsync(createResult.Messages);
            var saveResult = await _repository.SaveAsync();
            if (!saveResult.Succeeded)
                return await Result<string>.FailAsync(saveResult.Messages);
            return await Result<string>.SuccessAsync(createResult.Data.Id);
        }

        /// <summary>
        /// Updates the details of an existing city in the database.
        /// </summary>
        /// <remarks>This method updates the city identified by the <see cref="CityDto.CityId"/> property
        /// of the <paramref name="dto"/> parameter.  If no city with the specified ID is found, the operation fails
        /// with an appropriate error message.</remarks>
        /// <param name="dto">An object containing the updated details of the city. The <see cref="CityDto.CityId"/> property must specify
        /// the ID of the city to update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If successful, the result includes a success message. If
        /// the operation fails, the result includes error messages describing the failure.</returns>
        public async Task<IBaseResult> UpdateCityAsync(CityDto dto)
        {
            var spec = new LambdaSpec<City>(c => c.Id == dto.CityId);
            var result = await _repository.FirstOrDefaultAsync(spec, true);
            if (!result.Succeeded)
                return await Result.FailAsync(result.Messages);
            var city = result.Data;
            if (city == null)
                return await Result.FailAsync($"No city with id matching '{dto.CityId}' was found in the database");
            city.Name = dto.Name!;
            city.Code = dto.Code!;
            city.ShortName = dto.ShortName;
            city.Description = dto.Description;
            city.CountryId = dto.CountryId;
            _repository.Update(city);
            var saveResult = await _repository.SaveAsync();
            if (!saveResult.Succeeded)
                return await Result.FailAsync(saveResult.Messages);
            return await Result.SuccessAsync($"City '{city.Name}' was updated successfully");
        }

        /// <inheritdoc />
        public async Task<IBaseResult> RemoveCityAsync(string cityId)
        {
            var deleteResult = await _repository.DeleteAsync(cityId);
            if (!deleteResult.Succeeded)
                return await Result.FailAsync(deleteResult.Messages);
            var saveResult = await _repository.SaveAsync();
            if (!saveResult.Succeeded)
                return await Result.FailAsync(saveResult.Messages);
            return await Result.SuccessAsync("City was successfully removed");
        }
    }
}
