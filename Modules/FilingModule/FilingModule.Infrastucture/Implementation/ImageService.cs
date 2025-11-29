using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Extensions;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Entities;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;

namespace FilingModule.Infrastucture.Implementation
{
    /// <summary>
    /// Provides functionality for managing and retrieving images associated with entities.
    /// </summary>
    /// <remarks>This service is designed to handle operations related to images, such as retrieving paginated
    /// lists of images, fetching all images,  or retrieving a specific image by its associated entity identifier. It
    /// relies on a repository pattern for data access.</remarks>
    /// <typeparam name="TEntity">The type of the entity associated with the images. Must implement <see cref="IAuditableEntity"/> with a
    /// key of type <see cref="string"/>.</typeparam>
    /// <param name="entityImageRepository"></param>
    public class ImageService<TEntity>(IRepository<EntityImage<TEntity, string>, string> entityImageRepository)
        : IImageService<TEntity> where TEntity : IAuditableEntity<string>
    {
        public async Task<PaginatedResult<ImageDto>> PagedImagesAsync(ImagePageParameters parameters, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<EntityImage<TEntity, string>>(c => true);
            spec.AddInclude(c => c.Include(i => i.Image)!);
            spec.AddInclude(c => (IIncludableQueryable<EntityImage<TEntity, string>, object>)c.Include(i => i.Entity));

            var result = await entityImageRepository.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return PaginatedResult<ImageDto>.Failure(result.Messages);

            var response = result.Data.Select(ImageDto.ToDto).ToList();

            return await response.ToPaginatedListAsync(parameters.PageNr, parameters.PageSize);
        }

        /// <summary>
        /// Retrieves a collection of images associated with the specified entity, based on the provided pagination
        /// parameters.
        /// </summary>
        /// <remarks>This method retrieves images associated with the entity type specified by the generic
        /// parameter <c>TEntity</c>. The images are returned as data transfer objects (<see cref="ImageDto"/>), which
        /// include relevant metadata.</remarks>
        /// <param name="parameters">The pagination parameters used to filter and limit the results. This includes page size and page number.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will terminate early if the token is canceled.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="ImageDto"/> objects. If the operation
        /// succeeds, the result contains the collection of images; otherwise, it contains an error message.</returns>
        public async Task<IBaseResult<IEnumerable<ImageDto>>> AllImagesAsync(ImagePageParameters parameters, CancellationToken cancellationToken = default)
        {
            try
            {
                var spec = new LambdaSpec<EntityImage<TEntity, string>>(c => true);
                spec.AddInclude(c => c.Include(i => i.Image)!);
                spec.AddInclude(c =>
                    (IIncludableQueryable<EntityImage<TEntity, string>, object>)c.Include(i => i.Entity));

                var result = await entityImageRepository.ListAsync(spec, false, cancellationToken);

                var response = result.Data.Select(ImageDto.ToDto).ToList();

                return await Result<IEnumerable<ImageDto>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<ImageDto>>.FailAsync(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the image associated with the specified document identifier.
        /// </summary>
        /// <remarks>This method attempts to retrieve the image associated with the specified document
        /// identifier from the database. If no matching image is found, the result will indicate failure with an
        /// appropriate error message.</remarks>
        /// <param name="documentId">The unique identifier of the document for which the associated image is to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of type <see cref="ImageDto"/>. If the operation succeeds, the result contains the image data; otherwise, it
        /// contains error messages.</returns>
        public async Task<IBaseResult<ImageDto>> ImageAsync(string documentId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<EntityImage<TEntity, string>>(c => c.EntityId == documentId);
            spec.AddInclude(c => c.Include(i => i.Image)!);
            spec.AddInclude(c => (IIncludableQueryable<EntityImage<TEntity, string>, object>)c.Include(i => i.Entity));


            var documentResult = await entityImageRepository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!documentResult.Succeeded) return await Result<ImageDto>.FailAsync(documentResult.Messages);

            if (documentResult.Data is null)
                return await Result<ImageDto>.FailAsync(
                    $"No image with id matching : '{documentId}' was found in the database");

            var response = ImageDto.ToDto(documentResult.Data);

            return await Result<ImageDto>.SuccessAsync(response);
        }

    }
}
