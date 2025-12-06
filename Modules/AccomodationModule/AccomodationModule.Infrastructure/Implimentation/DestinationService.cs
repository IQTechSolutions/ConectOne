using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using FilingModule.Domain.RequestFeatures;
using Microsoft.EntityFrameworkCore;
namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Service implementation for managing destinations, including CRUD operations and related entities.
    /// </summary>
    public class DestinationService(IRepository<Destination, string> destinationRepo, IRepository<EntityVideo<Destination, string>, string> videoRepository, 
        IRepository<EntityImage<Destination, string>, string> imageRepository) : IDestinationService
    {
        #region Destinations

        /// <summary>
        /// Retrieves a paginated list of destinations based on the specified paging parameters.
        /// </summary>
        /// <remarks>This method queries the destination repository and applies the specified pagination
        /// settings. The returned data includes destination details along with associated images.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination settings, including page number and page size.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of destinations. If the
        /// operation fails, the result will include failure messages.</returns>
        public async Task<PaginatedResult<DestinationDto>> PagedDestinationsAsync(DestinationPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Destination>(c => true);
            spec.AddInclude(c => c.Include(c => c.Images).ThenInclude(c => c.Image));

            var result = await destinationRepo.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return PaginatedResult<DestinationDto>.Failure(result.Messages);
            return PaginatedResult<DestinationDto>.Success(result.Data.Select(c => new DestinationDto(c)).ToList(), result.Data.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves all available destinations asynchronously.
        /// </summary>
        /// <remarks>This method fetches all destinations from the underlying repository and maps them to
        /// <see cref="DestinationDto"/> objects. The operation is performed asynchronously and supports cancellation
        /// via the <paramref name="cancellationToken"/>.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// with a collection of <see cref="DestinationDto"/> objects representing the destinations.</returns>
        public async Task<IBaseResult<IEnumerable<DestinationDto>>> AllDestinationsAsync(CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Destination>(c => true);
            spec.AddInclude(c => c.Include(c => c.Images).ThenInclude(c => c.Image));

            var result = await destinationRepo.ListAsync(false, cancellationToken);
            return await Result<IEnumerable<DestinationDto>>.SuccessAsync(result.Data.Select(c => new DestinationDto(c)).ToList());
        }

        /// <summary>
        /// Retrieves a specific destination by its ID.
        /// </summary>
        /// <param name="destinationId">The ID of the destination.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result containing the destination DTO.</returns>
        public async Task<IBaseResult<DestinationDto>> DestinationAsync(string destinationId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Destination>(c => c.Id == destinationId);
            spec.AddInclude(c => c.Include(c => c.Images).ThenInclude(c => c.Image));
            spec.AddInclude(c => c.Include(c => c.Videos).ThenInclude(c => c.Video));

            var result = await destinationRepo.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<DestinationDto>.FailAsync(result.Messages);

            if (result.Data == null) return await Result<DestinationDto>.FailAsync($"No destination with id matching '{destinationId}' was found in the database");

            return await Result<DestinationDto>.SuccessAsync(new DestinationDto(result.Data));
        }

        /// <summary>
        /// Creates a new destination.
        /// </summary>
        /// <param name="dto">The destination DTO to create.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> CreateAsync(DestinationDto dto, CancellationToken cancellationToken = default)
        {
            var destination = dto.ToDestination();
            await destinationRepo.CreateAsync(destination, cancellationToken);

            var saveResult = await destinationRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Vacation '{destination.Name} with id '{destination.Id}' was created successfully");
        }

        /// <summary>
        /// Updates an existing destination.
        /// </summary>
        /// <param name="dto">The destination DTO to update.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> UpdateAsync(DestinationDto dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Destination>(c => c.Id == dto.DestinationId);
            spec.AddInclude(c => c.Include(c => c.Images));

            var vacationResult = await destinationRepo.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!vacationResult.Succeeded) return await Result.FailAsync(vacationResult.Messages);

            var vacation = vacationResult.Data;
            if (vacation == null) return await Result<VacationDto>.FailAsync($"No destination with id matching '{dto.VacationId}' was found in the database");

            vacation.Name = dto.Name;
            vacation.Description = dto.Description;
            vacation.OnlineDescription = dto.OnlineDescription;
            vacation.Lat = dto.Lat;
            vacation.Lng = dto.Lng;

            destinationRepo.Update(vacation);

            var saveResult = await destinationRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Vacation '{vacation.Name} with id '{vacation.Id}' was updated successfully");
        }

        /// <summary>
        /// Removes a destination by its ID.
        /// </summary>
        /// <param name="id">The ID of the destination to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> RemoveAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await destinationRepo.DeleteAsync(id, cancellationToken);
            if (!result.Succeeded) return result;

            var saveResult = await destinationRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Vacation with id '{id}' was successfully removed");
        }

        #endregion

        #region Videos

        /// <summary>
        /// Adds a video to the specified entity.
        /// </summary>
        /// <remarks>This method attempts to add a video to the specified entity and save the changes. If
        /// the operation fails at any step, it returns a failure result with the associated error messages.</remarks>
        /// <param name="request">The request containing the video and entity identifiers.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> AddVideo(AddEntityVideoRequest request, CancellationToken cancellationToken = default)
        {
            var video = new EntityVideo<Destination, string>() { VideoId = request.VideoId, EntityId = request.EntityId };

            var addResult = await videoRepository.CreateAsync(video, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Removes a video identified by the specified video ID.
        /// </summary>
        /// <remarks>This method attempts to delete the video from the repository and save changes. If any
        /// operation fails, it returns a failure result with the associated error messages.</remarks>
        /// <param name="videoId">The unique identifier of the video to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default)
        {
            var addResult = await videoRepository.DeleteAsync(videoId, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }
        
        #endregion

        #region Images

        /// <summary>
        /// Adds an image to the specified entity.
        /// </summary>
        /// <remarks>This method creates a new image entity and attempts to add it to the repository. It
        /// then saves the changes to the repository. If the operation fails at any step, the method returns a failure
        /// result with the associated error messages.</remarks>
        /// <param name="request">The request containing the image details and the entity to which the image will be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var image = new EntityImage<Destination, string>(request.ImageId, request.EntityId) { Selector = request.Selector, Order = request.Order };

            var addResult = await imageRepository.CreateAsync(image, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Removes an image identified by the specified image ID from the repository.
        /// </summary>
        /// <remarks>This method attempts to delete the image from the repository and then save the
        /// changes. If either operation fails, the method returns a failure result with the associated error
        /// messages.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var addResult = await imageRepository.DeleteAsync(imageId, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }
        
        #endregion
    }
}
