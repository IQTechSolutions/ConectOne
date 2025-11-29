using ConectOne.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Extensions;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Entities;
using FilingModule.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace FilingModule.Infrastucture.Implementation
{
    /// <summary>
    /// Provides functionality for managing and retrieving video-related data associated with a specific entity type.
    /// </summary>
    /// <remarks>This service is designed to handle operations related to videos, including retrieving
    /// paginated results, fetching all videos,  and retrieving specific video details. It relies on repositories for
    /// data access and supports asynchronous operations.</remarks>
    /// <typeparam name="TEntity">The type of the entity associated with the videos. Must implement <see cref="IAuditableEntity"/> with a
    /// key of type <see cref="string"/>.</typeparam>
    /// <param name="entityVideoRepository"></param>
    /// <param name="videoRepository"></param>
    public class VideoService<TEntity>(IRepository<EntityVideo<TEntity, string>, string> entityVideoRepository) : IVideoService<TEntity> where TEntity : IAuditableEntity<string>
    {
        /// <summary>
        /// Retrieves a paginated list of videos associated with the specified entity type.
        /// </summary>
        /// <remarks>This method retrieves videos associated with the entity type <typeparamref
        /// name="TEntity"/> and converts them into data transfer objects (<see cref="VideoDto"/>). The results are
        /// paginated based on the provided <paramref name="parameters"/>.</remarks>
        /// <param name="parameters">The pagination parameters, including the page number and page size.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="VideoDto"/> objects for the
        /// specified page. If the operation fails, the result will indicate failure with the corresponding error
        /// messages.</returns>
        public async Task<PaginatedResult<VideoDto>> PagedVideosAsync(RequestParameters parameters, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<EntityVideo<TEntity, string>>(c => true);
            spec.AddInclude(c => c.Include(i => i.Video)!);
            spec.AddInclude(c => (IIncludableQueryable<EntityVideo<TEntity, string>, object>)c.Include(i => i.Entity));

            var result = await entityVideoRepository.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return PaginatedResult<VideoDto>.Failure(result.Messages);

            var response = result.Data.Select(VideoDto.ToDto).ToList();

            return await response.ToPaginatedListAsync(parameters.PageNr, parameters.PageSize);
        }

        /// <summary>
        /// Retrieves a collection of videos associated with the specified entity.
        /// </summary>
        /// <remarks>This method retrieves all videos associated with the entity type specified by the
        /// generic parameter <c>TEntity</c>. The result includes additional related data, such as the entity and video
        /// details, based on the defined specifications.</remarks>
        /// <param name="parameters">The request parameters used to filter, sort, or paginate the results.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation. The default value is <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="VideoDto"/> objects. If the operation is
        /// successful, the result contains the collection of videos; otherwise, it contains an error message.</returns>
        public async Task<IBaseResult<IEnumerable<VideoDto>>> AllVideosAsync(RequestParameters parameters, CancellationToken cancellationToken = default)
        {
            try
            {
                var spec = new LambdaSpec<EntityVideo<TEntity, string>>(c => true);
                spec.AddInclude(c => c.Include(i => i.Video)!);
                spec.AddInclude(c => (IIncludableQueryable<EntityVideo<TEntity, string>, object>)c.Include(i => i.Entity));

                var result = await entityVideoRepository.ListAsync(spec, false, cancellationToken);

                var response = result.Data.Select(VideoDto.ToDto).ToList();

                return await Result<IEnumerable<VideoDto>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<VideoDto>>.FailAsync(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a video associated with the specified document identifier.
        /// </summary>
        /// <remarks>This method queries the database for a video associated with the specified document
        /// identifier. If no matching video is found, the result will indicate failure with an appropriate error
        /// message.</remarks>
        /// <param name="documentId">The unique identifier of the document for which the associated video is to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of type <see cref="VideoDto"/>. If the operation succeeds, the result contains the video data; otherwise, it
        /// contains error messages describing the failure.</returns>
        public async Task<IBaseResult<VideoDto>> VideoAsync(string documentId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<EntityVideo<TEntity, string>>(c => c.EntityId == documentId);
            spec.AddInclude(c => c.Include(i => i.Video)!);
            spec.AddInclude(c => (IIncludableQueryable<EntityVideo<TEntity, string>, object>)c.Include(i => i.Entity));


            var documentResult = await entityVideoRepository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!documentResult.Succeeded) return await Result<VideoDto>.FailAsync(documentResult.Messages);

            if (documentResult.Data is null) return await Result<VideoDto>.FailAsync($"No video with id matching : '{documentId}' was found in the database");

            var response = VideoDto.ToDto(documentResult.Data);

            return await Result<VideoDto>.SuccessAsync(response);
        }        
    }
}
