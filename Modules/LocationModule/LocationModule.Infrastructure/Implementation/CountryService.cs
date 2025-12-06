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
    /// Provides CRUD functionality for countries.
    /// </summary>
    public class CountryService(IRepository<Country, string> repository) : ICountryService
    {
        /// <summary>
        /// Retrieves a paginated list of countries based on the specified page parameters.
        /// </summary>
        /// <remarks>This method queries the repository for all countries and applies pagination based on
        /// the provided parameters. The returned result indicates success or failure, and in the case of success,
        /// includes the paginated data.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination settings, including the page number and page size.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a list of <see cref="CountryDto"/> objects for the requested
        /// page, along with pagination metadata such as the total count of items. If the operation fails, the result
        /// contains error messages.</returns>
        public async Task<PaginatedResult<CountryDto>> PagedCountriesAsync(CountryPageParameters pageParameters)
        {
            var result = repository.FindAll();
            if (result.Succeeded)
            {
                var response = await result.Data.ToListAsync();
                return PaginatedResult<CountryDto>.Success(response.Select(c => new CountryDto(c)).ToList(), response.Count, pageParameters.PageNr, pageParameters.PageSize);
            }
            return PaginatedResult<CountryDto>.Failure(result.Messages);
        }

        /// <summary>
        /// Retrieves a list of all countries.
        /// </summary>
        /// <remarks>This method asynchronously fetches a collection of countries and returns the result
        /// as a strongly-typed object. The result indicates whether the operation succeeded and includes any associated
        /// messages or data.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="CountryDto"/>. If the operation succeeds,
        /// the result contains the list of countries; otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<IEnumerable<CountryDto>>> AllCountriesAsync()
        {
            var result = await repository.ListAsync();
            return !result.Succeeded
                ? await Result<IEnumerable<CountryDto>>.FailAsync(result.Messages)
                : await Result<IEnumerable<CountryDto>>.SuccessAsync(result.Data.Select(c => new CountryDto(c)));
        }

        /// <summary>
        /// Retrieves a country by its unique identifier.
        /// </summary>
        /// <remarks>This method attempts to retrieve a country from the database using the specified
        /// <paramref name="countryId"/>.  If no country is found with the given identifier, the result will indicate
        /// failure with an appropriate error message.</remarks>
        /// <param name="countryId">The unique identifier of the country to retrieve. Cannot be <see langword="null"/> or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// with a <see cref="CountryDto"/> representing the country if found, or an error message if the operation
        /// fails.</returns>
        public async Task<IBaseResult<CountryDto>> CountryAsync(string countryId)
        {
            var spec = new LambdaSpec<Country>(c => c.Id == countryId);
            var result = await repository.FirstOrDefaultAsync(spec);
            if (!result.Succeeded)
                return await Result<CountryDto>.FailAsync(result.Messages);
            if (result.Data == null)
                return await Result<CountryDto>.FailAsync($"No country with id matching '{countryId}' was found in the database");
            return await Result<CountryDto>.SuccessAsync(new CountryDto(result.Data));
        }

        /// <summary>
        /// Creates a new country record asynchronously based on the provided data transfer object (DTO).
        /// </summary>
        /// <remarks>This method performs two operations: creating the country record and saving the
        /// changes to the repository. If either operation fails, the method returns a failure result with the
        /// corresponding error messages.</remarks>
        /// <param name="dto">The <see cref="CountryDto"/> containing the details of the country to be created. The <see
        /// cref="CountryDto.Code"/>, <see cref="CountryDto.Name"/>, and <see cref="CountryDto.ShortName"/> must be
        /// provided and valid.</param>
        /// <returns>An <see cref="IBaseResult{T}"/> containing the ID of the newly created country if the operation succeeds. If
        /// the operation fails, the result will include error messages describing the failure.</returns>
        public async Task<IBaseResult<int>> CreateCountryAsync(CountryDto dto)
        {
            var createResult = await repository.CreateAsync(new Country
            {
                Code = dto.Code,
                Name = dto.Name,
                ShortName = dto.ShortName,
                Description = dto.Description
            });
            if (!createResult.Succeeded)
                return await Result<int>.FailAsync(createResult.Messages);
            var saveResult = await repository.SaveAsync();
            if (!saveResult.Succeeded)
                return await Result<int>.FailAsync(saveResult.Messages);
            return await Result<int>.SuccessAsync(createResult.Data.Id);
        }

        /// <summary>
        /// Updates the details of an existing country in the database.
        /// </summary>
        /// <remarks>This method retrieves the country specified by the <paramref name="dto"/> parameter
        /// from the database, updates its properties with the values provided in the DTO, and saves the changes. If the
        /// country does not exist, or if the save operation fails, the method returns a failure result with appropriate
        /// error messages.</remarks>
        /// <param name="dto">A <see cref="CountryDto"/> object containing the updated details of the country. The <see
        /// cref="CountryDto.CountryId"/> property specifies the ID of the country to update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If successful, the result contains a success message;
        /// otherwise, it contains error messages describing the failure.</returns>
        public async Task<IBaseResult> UpdateCountryAsync(CountryDto dto)
        {
            var spec = new LambdaSpec<Country>(c => c.Id == dto.CountryId);
            var result = await repository.FirstOrDefaultAsync(spec, true);
            if (!result.Succeeded)
                return await Result.FailAsync(result.Messages);
            var country = result.Data;
            if (country == null)
                return await Result.FailAsync($"No country with id matching '{dto.CountryId}' was found in the database");
            country.Code = dto.Code;
            country.Name = dto.Name;
            country.ShortName = dto.ShortName;
            country.Description = dto.Description;
            repository.Update(country);
            var saveResult = await repository.SaveAsync();
            if (!saveResult.Succeeded)
                return await Result.FailAsync(saveResult.Messages);
            return await Result.SuccessAsync($"Country '{country.Name}' was updated successfully");
        }

        /// <summary>
        /// Removes a country with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method attempts to delete the specified country and save the changes to the
        /// underlying data store.  If the deletion or save operation fails, the result will indicate the failure with
        /// appropriate error messages.</remarks>
        /// <param name="countryId">The unique identifier of the country to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation. If successful, the result contains a success message. Otherwise, it
        /// contains error messages describing the failure.</returns>
        public async Task<IBaseResult> RemoveCountryAsync(string countryId)
        {
            var result = await repository.DeleteAsync(countryId);
            if (!result.Succeeded)
                return await Result.FailAsync(result.Messages);
            var saveResult = await repository.SaveAsync();
            if (!saveResult.Succeeded)
                return await Result.FailAsync(saveResult.Messages);
            return await Result.SuccessAsync("Country was successfully removed");
        }
    }
}

