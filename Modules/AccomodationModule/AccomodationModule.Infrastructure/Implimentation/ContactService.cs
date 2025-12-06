using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace AccomodationModule.Infrastructure.Implimentation;

/// <summary>
///     Application‑layer façade responsible for CRUD and paging operations on <see cref="Contact"/> domain entities.
/// </summary>
/// <remarks>
///     <para>
///         This service aggregates three distinct infrastructural dependencies:
///     </para>
///     <list type="bullet">
///         <item>
///             <description>
///                 <see cref="IRepository{TEntity,TKey}"/> for <see cref="Contact"/> – the primary data‑access abstraction.
///             </description>
///         </item>
///         <item>
///             <description>
///                 <see cref="IRepository{TEntity,TKey}"/> for <see cref="ImageFile{TEntity,TKey}"/> – used when manipulating
///                 profile images attached to a contact.
///             </description>
///         </item>
///         <item>
///             <description>
///                 An <see cref="IImageProcessingService"/> that converts raw uploads / URLs into on‑disk <c>.jpg</c>/<c>.png</c>
///                 files and returns metadata for persistence.
///             </description>
///         </item>
///     </list>
///     <para>
///         All methods are fully asynchronous, accept <see cref="CancellationToken"/>s, and return either
///         <see cref="IBaseResult"/> or <see cref="PaginatedResult{T}"/> wrappers to provide a uniform error‑handling
///         contract throughout the application.
///     </para>
/// </remarks>
public class ContactService(IRepository<Contact, string> repository, IRepository<EntityImage<Contact, string>, string> reviewImageRepo, IRepository<EntityImage<Contact, string>, string> imageRepository) : IContactService
{
    #region Paging & Query operations

    /// <summary>
    ///     Retrieves a paged collection of contacts filtered by <paramref name="pageParameters"/>.
    /// </summary>
    /// <remarks>
    ///     <list type="number">
    ///         <item>Load all records (filtered by <see cref="ContactType"/> when specified).</item>
    ///         <item>Apply in‑memory search filtering (case‑insensitive, whitespace‑trimmed).</item>
    ///         <item>Project to DTOs.</item>
    ///         <item>Return a <see cref="PaginatedResult{T}"/> using the caller‑supplied page number/size.</item>
    ///     </list>
    ///     For large datasets consider moving the filtering and paging logic down into the repository for server‑side
    ///     efficiency. Current implementation assumes a manageable row count.
    /// </remarks>
    /// <param name="pageParameters">Paging, filtering, and search options.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>Paginated <see cref="ContactDto"/> list or failure object.</returns>
    public async Task<PaginatedResult<ContactDto>> PagedAsync(ContactsPageParams pageParameters, CancellationToken cancellationToken = default)
    {
        IBaseResult<List<Contact>> result;

        // Filter by contact type if supplied, otherwise load all contacts.
        if (pageParameters.ContactType is not null)
        {
            var spec = new LambdaSpec<Contact>(c => c.ContactType == (ContactType)pageParameters.ContactType);
            spec.AddInclude(c => c.Include(b => b.Images).ThenInclude(c => c.Image));

            result = await repository.ListAsync(spec, false, cancellationToken);
        }
        else
        {
            var spec = new LambdaSpec<Contact>(_ => true);
            result = await repository.ListAsync(spec, false, cancellationToken);
        }

        if (!result.Succeeded)
            return PaginatedResult<ContactDto>.Failure(result.Messages);

        var response = result.Data;

        // Early exit when no records exist.
        if (!response.Any())
            return PaginatedResult<ContactDto>.Success(
                new List<ContactDto>(), 0, pageParameters.PageNr, pageParameters.PageSize);

        // Apply client‑side search text filter (simple contains / ToLower for demo purposes).
        if (!string.IsNullOrWhiteSpace(pageParameters.SearchText))
        {
            var search = pageParameters.SearchText.Trim().ToLower();
            response = response
                .Where(c => c.Name.Trim().ToLower().Contains(search))
                .ToList();
        }

        var pagedList = response.Select(c => new ContactDto(c)).ToList();
        return PaginatedResult<ContactDto>.Success(
            pagedList,
            pagedList.Count,
            pageParameters.PageNr,
            pageParameters.PageSize);
    }

    /// <summary>
    ///     Retrieves all contacts including their images.
    /// </summary>
    public async Task<IBaseResult<List<ContactDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Contact>(_ => true);
        spec.AddInclude(c => c.Include(x => x.Images).ThenInclude(c => c.Image));

        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<List<ContactDto>>.FailAsync(result.Messages);

        var dtoList = result.Data.Select(c => new ContactDto(c)).ToList();
        return await Result<List<ContactDto>>.SuccessAsync(dtoList);
    }

    /// <summary>
    /// Asynchronously retrieves a list of featured contacts.
    /// </summary>
    /// <remarks>The method queries the repository for contacts marked as featured and includes their
    /// associated images. If the operation fails, the result will contain error messages.</remarks>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// a list of <see cref="ContactDto"/> objects representing the featured contacts.</returns>
    public async Task<IBaseResult<List<ContactDto>>> GetFeaturedAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Contact>(c => c.Featured);
        spec.AddInclude(c => c.Include(x => x.Images).ThenInclude(c => c.Image));

        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<List<ContactDto>>.FailAsync(result.Messages);

        var dtoList = result.Data.Select(c => new ContactDto(c)).ToList();
        return await Result<List<ContactDto>>.SuccessAsync(dtoList);
    }

    /// <summary>
    ///     Fetches a single contact by primary key, eagerly loading any related images.
    /// </summary>
    public async Task<IBaseResult<ContactDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Contact>(c => c.Id == id);
        spec.AddInclude(c => c.Include(x => x.Images).ThenInclude(c => c.Image));

        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<ContactDto>.FailAsync(
                result.Succeeded ? $"Entity with ID {id} not found." : string.Join(",", result.Messages));

        return await Result<ContactDto>.SuccessAsync(new ContactDto(result.Data));
    }

    #endregion

    #region CRUD operations

    /// <summary>
    ///     Creates a new contact and persists its profile image.
    /// </summary>
    public async Task<IBaseResult<ContactDto>> AddAsync(ContactDto dto, CancellationToken cancellationToken = default)
    {
        var entity = dto.ToGuide();

        // Generate image (download, resize, etc.).
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "StaticFiles", "reviews", "images");
        //var imageFile = imageProcessingService.CreateImage(path, dto.ProfileImageUrl);
        //entity.Images.Add(imageFile.ToImageFile<Contact, string>(entity.Id, UploadType.Cover));

        var createResult = await repository.CreateAsync(entity, cancellationToken);
        if (!createResult.Succeeded)
            return await Result<ContactDto>.FailAsync(createResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<ContactDto>.FailAsync(saveResult.Messages);

        return await Result<ContactDto>.SuccessAsync(dto);
    }

    /// <summary>
    ///     Updates scalar properties and profile image of the specified contact.
    /// </summary>
    public async Task<IBaseResult<ContactDto>> EditAsync(ContactDto dto, CancellationToken cancellationToken = default)
    {
        // Load entity with tracking because we intend to modify it.
        var spec = new LambdaSpec<Contact>(c => c.Id == dto.ContactId);
        spec.AddInclude(c => c.Include(x => x.Images));

        var result = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<ContactDto>.FailAsync(result.Messages);

        // Map scalar properties.
        result.Data.Name = dto.Name;
        result.Data.Surname = dto.Surname;
        result.Data.Phone = dto.Phone;
        result.Data.Email = dto.Email;
        result.Data.Bio = dto.Bio;
        result.Data.Featured = dto.Featured;
        result.Data.Order = dto.Order;
        result.Data.Selector = dto.Selector;
        result.Data.ContactType = dto.ContactType;

        // Update entity state.
        var updateResult = repository.Update(result.Data);
        if (!updateResult.Succeeded)
            return await Result<ContactDto>.FailAsync(updateResult.Messages);
        
        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<ContactDto>.FailAsync(saveResult.Messages);

        return await Result<ContactDto>.SuccessAsync(dto);
    }

    /// <summary>
    ///     Deletes a contact (and relies on cascade‑delete or relational hooks to remove dependent images).
    /// </summary>
    public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await repository.DeleteAsync(id, cancellationToken);
        if (!result.Succeeded)
            return await Result.FailAsync(result.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync();
    }

    #endregion

    #region Images

    /// <summary>
    /// Adds an image to the specified entity.
    /// </summary>
    /// <remarks>This method creates an image entity and attempts to save it to the repository. If the
    /// operation fails at any step, it returns a failure result with the associated error messages.</remarks>
    /// <param name="request">The request containing the image and entity details. Must not be null.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
    {
        var image = new EntityImage<Contact, string>(request.ImageId, request.EntityId) { Selector = request.Selector, Order = request.Order };

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

