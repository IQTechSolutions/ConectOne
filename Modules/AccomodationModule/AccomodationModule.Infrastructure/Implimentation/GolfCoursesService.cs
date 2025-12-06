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
    /// Service implementation for managing golf courses, including CRUD operations and related entities.
    /// </summary>
    public class GolfCoursesService(IRepository<GolfCourse, string> golfCourseRepo, IRepository<GolfCourseDestination, string> golfCourseDestinationRepo, 
        IRepository<EntityVideo<GolfCourse, string>, string> videoRepository, IRepository<EntityImage<GolfCourse, string>, string> imageRepository) : IGolfCoursesService
    {
        #region Golf Courses

        /// <summary>
        /// Retrieves a paginated list of golf courses based on the specified request parameters.
        /// </summary>
        /// <remarks>This method fetches golf courses along with their associated images and maps them to
        /// DTOs. If the operation fails, the result will indicate failure with appropriate error messages.</remarks>
        /// <param name="pageParameters">The pagination parameters, including the page number and page size.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a list of <see cref="GolfCourseDto"/> objects, the total count
        /// of items, and pagination metadata.</returns>
        public async Task<PaginatedResult<GolfCourseDto>> PagedGolfCoursesAsync(RequestParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<GolfCourse>(c => true);
            spec.AddInclude(c => c.Include(g => g.Images).ThenInclude(c => c.Image));

            var result = await golfCourseRepo.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return PaginatedResult<GolfCourseDto>.Failure(result.Messages);

            return PaginatedResult<GolfCourseDto>.Success(result.Data.Select(c => new GolfCourseDto(c)).ToList(), result.Data.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves a specific golf course by its ID.
        /// </summary>
        /// <param name="golfCourseId">The ID of the golf course.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result containing the golf course DTO.</returns>
        public async Task<IBaseResult<GolfCourseDto>> GolfCourseAsync(string golfCourseId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<GolfCourse>(c => c.Id == golfCourseId);
            spec.AddInclude(c => c.Include(g => g.Images).ThenInclude(c => c.Image));

            var result = await golfCourseRepo.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<GolfCourseDto>.FailAsync(result.Messages);

            if (result.Data == null) return await Result<GolfCourseDto>.FailAsync($"No golf course with id matching '{golfCourseId}' was found in the database");

            return await Result<GolfCourseDto>.SuccessAsync(new GolfCourseDto(result.Data));
        }

        /// <summary>
        /// Creates a new golf course.
        /// </summary>
        /// <param name="dto">The golf course DTO to create.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> CreateAsync(GolfCourseDto dto, CancellationToken cancellationToken = default)
        {
            var golfCourse = dto.ToGolfCourse();

            foreach (var destination in dto.Destinations)
            {
                golfCourse.Destinations.Add(new GolfCourseDestination()
                {
                    Id = Guid.NewGuid().ToString(),
                    DestinationId = destination.DestinationId,
                    GolfCourseId = golfCourse.Id
                });
            }

            await golfCourseRepo.CreateAsync(golfCourse, cancellationToken);

            var saveResult = await golfCourseRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Golf Course '{golfCourse.Name} with id '{golfCourse.Id}' was created successfully");
        }

        /// <summary>
        /// Updates an existing golf course.
        /// </summary>
        /// <param name="dto">The golf course DTO to update.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> UpdateAsync(GolfCourseDto dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<GolfCourse>(c => c.Id == dto.GolfCourseId);
            spec.AddInclude(c => c.Include(g => g.Images));
            spec.AddInclude(c => c.Include(c => c.Videos));
            spec.AddInclude(c => c.Include(c => c.Destinations));

            var golfCourseResult = await golfCourseRepo.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!golfCourseResult.Succeeded) return await Result.FailAsync(golfCourseResult.Messages);

            if (golfCourseResult.Data == null) return await Result<GolfCourseDto>.FailAsync($"No golf course with id matching '{dto.VacationId}' was found in the database");


            golfCourseResult.Data.Name = dto.Name;
            golfCourseResult.Data.Description = dto.Description;
            golfCourseResult.Data.OnlineDescription = dto.OnlineDescription;
            golfCourseResult.Data.Lat = dto.Lat;
            golfCourseResult.Data.Lng = dto.Lng;
            golfCourseResult.Data.CourseType = dto.CourseType;
            golfCourseResult.Data.Ranking = dto.Ranking;
            golfCourseResult.Data.Carts = dto.Carts;
            golfCourseResult.Data.Caddies = dto.Caddies;
            golfCourseResult.Data.GolfClubs = dto.GolfClubs;
            golfCourseResult.Data.DesignedBy = dto.DesignedBy;

            golfCourseRepo.Update(golfCourseResult.Data);

            var newIds = new HashSet<string>(dto.Destinations?.Select(m => m.DestinationId));
            var currentIds = golfCourseResult.Data.Destinations == null ? new HashSet<string>() : new HashSet<string>(golfCourseResult.Data.Destinations.Select(m => m.DestinationId));

            var toAdd = newIds.Except(currentIds);
            var toRemove = currentIds.Except(newIds);

            foreach (var destinationId in toAdd)
            {
                await golfCourseDestinationRepo.CreateAsync(new GolfCourseDestination
                {
                    Id = Guid.NewGuid().ToString(),
                    DestinationId = destinationId,
                    GolfCourseId = golfCourseResult.Data.Id
                }, cancellationToken);
            }

            foreach (var destinationId in toRemove)
            {
                var memberToRemove = golfCourseResult.Data.Destinations.FirstOrDefault(m => m.DestinationId == destinationId);
                if (memberToRemove != null)
                {
                    await golfCourseDestinationRepo.DeleteAsync(memberToRemove.Id, cancellationToken);
                }
            }
            
            var saveResult = await golfCourseRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Golf Course '{golfCourseResult.Data.Name} with id '{golfCourseResult.Data.Id}' was updated successfully");
        }

        /// <summary>
        /// Removes a golf course by its ID.
        /// </summary>
        /// <param name="id">The ID of the golf course to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> RemoveAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await golfCourseRepo.DeleteAsync(id, cancellationToken);
            if (!result.Succeeded) return result;

            var saveResult = await golfCourseRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Golf Course with id '{id}' was successfully removed");
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
            var video = new EntityVideo<GolfCourse, string>() { VideoId = request.VideoId, EntityId = request.EntityId };

            var addResult = await videoRepository.CreateAsync(video, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Removes a video identified by the specified video ID.
        /// </summary>
        /// <remarks>This method attempts to delete the video from the repository and save changes. If
        /// either operation fails, the method returns a failure result with the associated error messages.</remarks>
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
        /// returns a success result if the image is added and saved successfully.</remarks>
        /// <param name="request">The request containing the image details and the entity to which the image will be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var image = new EntityImage<GolfCourse, string>(request.ImageId, request.EntityId) { Selector = request.Selector, Order = request.Order };

            var addResult = await imageRepository.CreateAsync(image, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Removes an image from the repository based on the specified image identifier.
        /// </summary>
        /// <remarks>This method attempts to delete the image with the given identifier from the
        /// repository.  If the deletion is successful, it commits the changes. If any step fails, the operation is
        /// rolled back, and an error result is returned.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
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
