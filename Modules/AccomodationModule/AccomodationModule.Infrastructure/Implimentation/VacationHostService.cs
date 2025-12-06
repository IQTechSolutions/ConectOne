using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using FilingModule.Domain.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Service implementation for managing vacation hosts, including CRUD operations and related entities.
    /// </summary>
    public class VacationHostService(IAccomodationRepositoryManager accomodationRepositoryManager, IRepository<EntityImage<VacationHost, string>, string> imageRepo) : IVacationHostService
    {
        #region Vacation Hosts

        /// <summary>
        /// Retrieves a paginated list of vacation hosts based on the specified request parameters.
        /// </summary>
        /// <remarks>This method retrieves vacation hosts along with their associated images. The returned
        /// data is transformed into <see cref="VacationHostDto"/> objects for consumption.</remarks>
        /// <param name="pageParameters">The pagination parameters, including the page number and page size.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of vacation hosts. If the
        /// operation fails, the result will indicate failure with appropriate error messages.</returns>
        public async Task<PaginatedResult<VacationHostDto>> PagedVacationHostsAsync(RequestParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationHost>(c => true);
            spec.AddInclude(c => c.Include(cg => cg.Images).ThenInclude(c => c.Image));
            spec.AddInclude(c => c.Include(cg => cg.Area));

            var result = await accomodationRepositoryManager.VacationHosts.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return PaginatedResult<VacationHostDto>.Failure(result.Messages);

            var response = result.Data;
            return PaginatedResult<VacationHostDto>.Success(response.Select(c => new VacationHostDto(c)).ToList(), response.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves a specific vacation host by its ID.
        /// </summary>
        /// <param name="vacationHostId">The ID of the vacation host.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result containing the vacation host DTO.</returns>
        public async Task<IBaseResult<VacationHostDto>> VacationHostAsync(string vacationHostId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationHost>(c => c.Id == vacationHostId);
            spec.AddInclude(c => c.Include(cg => cg.Images).ThenInclude(c => c.Image));
            spec.AddInclude(c => c.Include(cg => cg.Area));

            var result = await accomodationRepositoryManager.VacationHosts.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<VacationHostDto>.FailAsync(result.Messages);

            if (result.Data == null) return await Result<VacationHostDto>.FailAsync($"No vacation host with id matching '{vacationHostId}' was found in the database");

            return await Result<VacationHostDto>.SuccessAsync(new VacationHostDto(result.Data));
        }

        public async Task<IBaseResult<IEnumerable<VacationHostDto>>> AllVacationHostsAsync(CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationHost>(c => true);
            spec.AddInclude(c => c.Include(cg => cg.Images).ThenInclude(c => c.Image));
            spec.AddInclude(c => c.Include(cg => cg.Area));

            var result = await accomodationRepositoryManager.VacationHosts.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<VacationHostDto>>.FailAsync(result.Messages);
            return await Result<IEnumerable<VacationHostDto>>.SuccessAsync(result.Data.Select(c => new VacationHostDto(c)));
        }

        /// <summary>
        /// Retrieves a specific vacation host by its name.
        /// </summary>
        /// <param name="vacationHostName">The name of the vacation host.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result containing the vacation host DTO.</returns>
        public async Task<IBaseResult<VacationHostDto>> VacationHostFromNameAsync(string vacationHostName, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationHost>(c => c.Name.ToUpper() == vacationHostName);
            spec.AddInclude(c => c.Include(cg => cg.Images).ThenInclude(c => c.Image));
            spec.AddInclude(c => c.Include(cg => cg.Area));

            var result = await accomodationRepositoryManager.VacationHosts.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<VacationHostDto>.FailAsync(result.Messages);

            if (result.Data == null) return await Result<VacationHostDto>.FailAsync($"No vacation host with name matching '{vacationHostName}' was found in the database");

            return await Result<VacationHostDto>.SuccessAsync(new VacationHostDto(result.Data));
        }

        /// <summary>
        /// Creates a new vacation host.
        /// </summary>
        /// <param name="dto">The vacation host DTO to create.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> CreateAsync(VacationHostDto dto, CancellationToken cancellationToken = default)
        {
            var vacation = dto.ToVacationHost();
            await accomodationRepositoryManager.VacationHosts.CreateAsync(vacation, cancellationToken);

            var saveResult = await accomodationRepositoryManager.VacationHosts.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Vacation host '{vacation.Name} with id '{vacation.Id}' was created successfully");
        }

        /// <summary>
        /// Updates an existing vacation host.
        /// </summary>
        /// <param name="dto">The vacation host DTO to update.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> UpdateAsync(VacationHostDto dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationHost>(c => c.Id == dto.VacationHostId);
            spec.AddInclude(c => c.Include(cg => cg.Images));

            var vacationResult = await accomodationRepositoryManager.VacationHosts.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!vacationResult.Succeeded) return await Result.FailAsync(vacationResult.Messages);

            if (vacationResult.Data == null) return await Result<VacationHostDto>.FailAsync($"No vacation host with id matching '{dto.VacationHostId}' was found in the database");

            vacationResult.Data.Name = dto.Name;
            vacationResult.Data.Description = dto.Description;
            vacationResult.Data.Address = dto.Address;
            vacationResult.Data.Suburb = dto.Suburb;
            vacationResult.Data.AreaId = dto.Area.Id;
            vacationResult.Data.Lat = dto.Lat;
            vacationResult.Data.Lng = dto.Lng;
            vacationResult.Data.RepresentativeName2 = dto.RepresentativeName;
            vacationResult.Data.RepresentativeSurname2 = dto.RepresentativeSurname;
            vacationResult.Data.RepresentativePhone = dto.RepresentativePhone;
            vacationResult.Data.RepresentativeEmail = dto.RepresentativeEmail;
            vacationResult.Data.RepresentativeBio = dto.RepresentativeBio;

            accomodationRepositoryManager.VacationHosts.Update(vacationResult.Data);

            var saveResult = await accomodationRepositoryManager.VacationHosts.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Vacation host '{vacationResult.Data.Name} with id '{vacationResult.Data.Id}' was updated successfully");
        }

        /// <summary>
        /// Removes a vacation host by its ID.
        /// </summary>
        /// <param name="id">The ID of the vacation host to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> RemoveAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await accomodationRepositoryManager.VacationHosts.DeleteAsync(id, cancellationToken);
            if (!result.Succeeded) return result;

            var saveResult = await accomodationRepositoryManager.VacationHosts.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Vacation Host with id '{id}' was successfully removed");
        }

        #endregion

        #region Images

        /// <summary>
        /// Adds a new image record that links an existing file in the media store
        /// (<see cref="AddEntityImageRequest.ImageId"/>) with a specific entity
        /// (<see cref="AddEntityImageRequest.EntityId"/>).
        /// </summary>
        /// <param name="request">
        /// The DTO carrying the target entity identifier, the image identifier,
        /// a <c>Selector</c> flag describing the image’s purpose (e.g. <c>Main</c>,
        /// <c>Thumbnail</c>), and the desired display <c>Order</c>.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by the caller to abort the operation
        /// before it completes.
        /// </param>
        /// <returns>
        /// <see cref="IBaseResult"/> indicating:
        /// • <c>Success</c> – the link was written to the repository and persisted, or  
        /// • <c>Failure</c> – one of the repository operations failed (in which case
        ///   the aggregated error messages are bubbled back to the caller).
        /// </returns>
        /// <remarks>
        /// The method performs two repository calls—<c>CreateAsync</c> followed by
        /// <c>SaveAsync</c>.  It short-circuits on failure so that no partial state
        /// is persisted.
        /// </remarks>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var image = new EntityImage<VacationHost, string>(request.ImageId, request.EntityId)
            {
                Selector = request.Selector,
                Order = request.Order
            };

            var addResult = await imageRepo.CreateAsync(image, cancellationToken);
            if (!addResult.Succeeded)
                return await Result.FailAsync(addResult.Messages);

            var saveResult = await imageRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Removes the link between an entity and an image previously added via
        /// <see cref="AddImage"/>.
        /// </summary>
        /// <param name="imageId">
        /// The primary-key of the <see cref="EntityImage{TEntity,TKey}"/> record to delete.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by the caller to abort the operation
        /// before it completes.
        /// </param>
        /// <returns>
        /// <see cref="IBaseResult"/> indicating success or failure, mirroring the
        /// repository results.  On failure the aggregated repository error messages are
        /// returned to the caller.
        /// </returns>
        /// <remarks>
        /// Just like <see cref="AddImage"/>, this method executes two discrete repository
        /// calls—<c>DeleteAsync</c> followed by <c>SaveAsync</c>—and short-circuits on
        /// failure to avoid inconsistent state.
        /// </remarks>
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var deleteResult = await imageRepo.DeleteAsync(imageId, cancellationToken);
            if (!deleteResult.Succeeded)
                return await Result.FailAsync(deleteResult.Messages);

            var saveResult = await imageRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        #endregion
    }
}
