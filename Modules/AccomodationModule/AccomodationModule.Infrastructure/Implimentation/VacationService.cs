using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.Arguments.Requests;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using FilingModule.Domain.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    ///     Concrete implementation of <see cref="IVacationService"/> that provides a rich API for querying and modifying
    ///     <see cref="Vacation"/> domain entities—including related media, pricing, reviews, hosts, and value‑added
    ///     extensions.
    ///     <para>
    ///         <strong>Responsibilities</strong>
    ///         <list type="bullet">
    ///             <item>Expose <em>read</em> operations that return fully‑hydrated <see cref="VacationDto"/> objects.</item>
    ///             <item>Expose <em>write</em> operations (Create/Update/Duplicate/Remove) with server‑side validation.</item>
    ///             <item>Manage complex object graphs (images, intervals, flights, gifts, payment rules, etc.) in an
    ///             aggregate‑root‑friendly manner.</item>
    ///             <item>Serve as the single orchestrator that sits between higher‑level application code (Blazor/API
    ///             handlers) and the <see cref="IAccomodationRepositoryManager"/> persistence layer.</item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         <strong>Thread‑safety</strong>
    ///         <br/>This type is <em>stateless</em>; however, the injected <see cref="IAccomodationRepositoryManager"/> is
    ///         scoped per request. Therefore, <see cref="VacationService"/> itself is <b>thread‑safe</b> to use with the
    ///         standard ASP.NET DI container lifetime (scoped).
    ///     </para>
    ///     <para>
    ///         <strong>Error propagation</strong>
    ///         <br/>All failures are returned as <see cref="IBaseResult"/> or <see cref="PaginatedResult{T}"/>, never via
    ///         thrown exceptions, keeping consumers free from try/catch noise.
    ///     </para>
    /// </summary>
    public class VacationService(IAccomodationRepositoryManager accomodationRepositoryManager, IRepository<EntityImage<Vacation, string>, string> vacationImageRepository, 
        IRepository<EntityVideo<Lodging, string>, string> vacationVideoRepository) : IVacationService
    {
        #region Vacation Operations

        /// <summary>
        ///     Retrieves a paginated slice of <see cref="Vacation"/> records, optionally filtered by a specific
        ///     <see cref="VacationHost"/>.
        /// </summary>
        /// <param name="pageParameters">
        ///     Paging and filtering meta‑data. <see cref="VacationPageParameters.VacationHostId"/> is treated as an optional
        ///     filter; when omitted, all vacations are included.
        /// </param>
        /// <param name="cancellationToken">Co‑operative cancellation token propagated from the caller.</param>
        /// <returns>
        ///     A <see cref="PaginatedResult{T}"/> whose <see cref="PaginatedResult{T}.Data"/> collection contains
        ///     <see cref="VacationDto"/> projections. Failure is indicated by
        ///     <see cref="IBaseResult{T}.Succeeded"/> = <c>false</c> and a populated <c>Messages</c> collection.
        /// </returns>
        /// <remarks>
        ///     <para>This overload <b>eager‑loads</b> vacation images to avoid the N+1 select problem.</para>
        ///     <para>The method is intentionally <c>read‑only</c>; EF change‑tracking is disabled (<c>trackChanges=false</c>)
        ///     to maximise performance for list‑style UI views.</para>
        /// </remarks>
        public async Task<PaginatedResult<VacationDto>> PagedAsync(VacationPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var spec = string.IsNullOrEmpty(pageParameters.VacationHostId)
                ? new LambdaSpec<Vacation>(_ => true)
                : new LambdaSpec<Vacation>(v => v.VacationHostId == pageParameters.VacationHostId);

            spec.AddInclude(s => s.Include(v => v.Images).ThenInclude(i => i.Image));
            spec.AddInclude(i => i.Include(c => c.VacationTitleTemplate));

            var result = await accomodationRepositoryManager.Vacations.ListAsync(spec, trackChanges: false, cancellationToken);
            if (!result.Succeeded)
            {
                return PaginatedResult<VacationDto>.Failure(result.Messages);
            }

            var page = result.Data.Select(v => new VacationDto(v)).ToList();
            return PaginatedResult<VacationDto>.Success(page, result.Data.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        ///     Returns <em>every</em> <see cref="Vacation"/> that meets the supplied criteria—without paging—mainly used for
        ///     export scenarios and admin grids.
        /// </summary>
        /// <inheritdoc cref="PagedAsync"/>
        public async Task<IBaseResult<IEnumerable<VacationDto>>> AllVacationsAsync(VacationPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var spec = string.IsNullOrEmpty(pageParameters.VacationHostId)
                ? new LambdaSpec<Vacation>(_ => true)
                : new LambdaSpec<Vacation>(v => v.VacationHostId == pageParameters.VacationHostId);

            spec.AddInclude(c => c.Include(g => g.Prices));
            spec.AddInclude(c => c.Include(g => g.VacationPriceGroups));
            spec.AddInclude(s => s.Include(v => v.Images).ThenInclude(i => i.Image));
            spec.AddInclude(i => i.Include(c => c.VacationTitleTemplate));
            spec.AddInclude(i => i.Include(iv => iv.Intervals).ThenInclude(l => l.Lodging).ThenInclude(l => l.Country));
            spec.AddInclude(i => i.Include(iv => iv.Intervals).ThenInclude(l => l.Lodging).ThenInclude(l => l.Destinations).ThenInclude(c => c.Destination));


            var result = await accomodationRepositoryManager.Vacations.ListAsync(spec, trackChanges: false, cancellationToken);
            if (!result.Succeeded)
            {
                return await Result<IEnumerable<VacationDto>>.FailAsync(result.Messages);
            }

            return await Result<IEnumerable<VacationDto>>.SuccessAsync(result.Data.Select(v => new VacationDto(v)));
        }
        
        /// <summary>
        ///     Retrieves a fully‑hydrated <see cref="VacationDto"/> given its unique identifier.
        /// </summary>
        /// <param name="vacationId">Primary key of the target <see cref="Vacation"/>.</param>
        /// <param name="cancellationToken">Co‑operative cancellation token.</param>
        /// <returns>
        ///     A success result with a populated DTO when found; otherwise, a failure result with a descriptive message.
        /// </returns>
        /// <remarks>
        ///     All child collections (images, intervals, flights, golfer packages, etc.) are eagerly loaded to provide a
        ///     single call that powers detailed read pages.
        /// </remarks>
        public async Task<IBaseResult<VacationDto>> VacationAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            // --- 1. Base entity -------------------------------------------------------------
            var spec = new LambdaSpec<Vacation>(v => v.Id == vacationId);
            spec.AddInclude(i => i.Include(v => v.Images).ThenInclude(ii => ii.Image));
            spec.AddInclude(i => i.Include(v => v.Prices));
            spec.AddInclude(i => i.Include(v => v.VacationHost).ThenInclude(h => h!.Images).ThenInclude(ii => ii.Image));
            spec.AddInclude(i => i.Include(v => v.DayTourActivities).ThenInclude(c => c.DayTourActivityTemplate));
            spec.AddInclude(i => i.Include(c => c.VacationPriceGroups));
            spec.AddInclude(i => i.Include(c => c.VacationTitleTemplate));
            spec.AddInclude(i => i.Include(c => c.ShortDescriptionTemplate));
            spec.AddInclude(i => i.Include(c => c.PaymentExclusionTemplate));
            spec.AddInclude(i => i.Include(c => c.CancellationTermsTemplate));
            spec.AddInclude(i => i.Include(c => c.TermsAndConditionsTemplate));
            spec.AddInclude(i => i.Include(c => c.GeneralInformationTemplate));
            spec.AddInclude(i => i.Include(c => c.MeetAndGreetTemplate));
            spec.AddInclude(i => i.Include(c => c.ItineraryEntryItemTemplates));

            var result = await accomodationRepositoryManager.Vacations.FirstOrDefaultAsync(spec, trackChanges: false, cancellationToken);
            if (!result.Succeeded)
            {
                return await Result<VacationDto>.FailAsync(result.Messages);
            }
            if (result.Data is null)
            {
                return await Result<VacationDto>.FailAsync($"No vacation with id '{vacationId}' was found.");
            }

            // --- 2. Satellite entities ------------------------------------------------------
            await EnrichVacationAsync(result.Data, cancellationToken);

            return await Result<VacationDto>.SuccessAsync(new VacationDto(result.Data));
        }


        /// <summary>
        ///     Same as <see cref="VacationAsync"/> but resolves the entity by <see cref="Vacation.Name"/> (case‑insensitive).
        /// </summary>
        public async Task<IBaseResult<VacationDto>> VacationFromNameAsync(string vacationName, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Vacation>(v => v.VacationTitleTemplate.VacationTitle.ToUpper() == vacationName.ToUpper());
            spec.AddInclude(i => i.Include(v => v.Images).ThenInclude(ii => ii.Image));
            spec.AddInclude(i => i.Include(v => v.Prices));
            spec.AddInclude(i => i.Include(v => v.VacationHost).ThenInclude(h => h!.Images).ThenInclude(ii => ii.Image));
            spec.AddInclude(i => i.Include(v => v.DayTourActivities).ThenInclude(c => c.DayTourActivityTemplate));
            spec.AddInclude(i => i.Include(c => c.VacationTitleTemplate));
            spec.AddInclude(i => i.Include(c => c.ShortDescriptionTemplate));
            spec.AddInclude(i => i.Include(c => c.PaymentExclusionTemplate));
            spec.AddInclude(i => i.Include(c => c.CancellationTermsTemplate));
            spec.AddInclude(i => i.Include(c => c.TermsAndConditionsTemplate));
            spec.AddInclude(i => i.Include(c => c.GeneralInformationTemplate));


            var result = await accomodationRepositoryManager.Vacations.FirstOrDefaultAsync(spec, trackChanges: false, cancellationToken);
            if (!result.Succeeded)
            {
                return await Result<VacationDto>.FailAsync(result.Messages);
            }
            if (result.Data is null)
            {
                return await Result<VacationDto>.FailAsync($"No vacation with name '{vacationName}' was found.");
            }

            await EnrichVacationAsync(result.Data, cancellationToken);
            return await Result<VacationDto>.SuccessAsync(new VacationDto(result.Data));
        }

        /// <summary>
        ///     Resolves a <see cref="Vacation"/> by its SEO‑friendly <see cref="Vacation.Url"/> slug.
        /// </summary>
        public async Task<IBaseResult<VacationDto>> VacationFromUrlAsync(string vacationUrl, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Vacation>(v => v.VacationTitleTemplate.VacationTitle.Trim().Replace(" ","-").ToLower() == vacationUrl.ToLower());
            spec.AddInclude(i => i.Include(v => v.Images).ThenInclude(ii => ii.Image));
            spec.AddInclude(i => i.Include(v => v.Prices));
            spec.AddInclude(i => i.Include(v => v.VacationHost).ThenInclude(h => h!.Images).ThenInclude(c => c.Image));
            spec.AddInclude(i => i.Include(v => v.DayTourActivities).ThenInclude(c => c.DayTourActivityTemplate));
            spec.AddInclude(i => i.Include(c => c.VacationTitleTemplate));
            spec.AddInclude(i => i.Include(c => c.ShortDescriptionTemplate));
            spec.AddInclude(i => i.Include(c => c.PaymentExclusionTemplate));
            spec.AddInclude(i => i.Include(c => c.CancellationTermsTemplate));
            spec.AddInclude(i => i.Include(c => c.TermsAndConditionsTemplate));
            spec.AddInclude(i => i.Include(c => c.GeneralInformationTemplate));


            var result = await accomodationRepositoryManager.Vacations.FirstOrDefaultAsync(spec, trackChanges: false, cancellationToken);
            if (!result.Succeeded)
            {
                return await Result<VacationDto>.FailAsync(result.Messages);
            }
            if (result.Data is null)
            {
                return await Result<VacationDto>.FailAsync($"No vacation with url '{vacationUrl}' was found.");
            }

            await EnrichVacationAsync(result.Data, cancellationToken);
            return await Result<VacationDto>.SuccessAsync(new VacationDto(result.Data));
        }

        /// <summary>
        ///     Lightweight <em>summary</em> projection by Id—omits heavyweight navigation properties for faster rendering in
        ///     listing grids.
        /// </summary>
        public async Task<IBaseResult<VacationDto>> VacationSummaryAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Vacation>(v => v.Id == vacationId);
            spec.AddInclude(i => i.Include(v => v.Images).ThenInclude(ii => ii.Image));
            spec.AddInclude(i => i.Include(v => v.Prices));
            spec.AddInclude(i => i.Include(v => v.VacationHost).ThenInclude(h => h!.Images).ThenInclude(i => i.Image));
            spec.AddInclude(i => i.Include(v => v.Reviews).ThenInclude(r => r.Review).ThenInclude(i => i.Images).ThenInclude(i => i.Image));

            var result = await accomodationRepositoryManager.Vacations.FirstOrDefaultAsync(spec, trackChanges: false, cancellationToken);
            if (!result.Succeeded)
            {
                return await Result<VacationDto>.FailAsync(result.Messages);
            }
            if (result.Data is null)
            {
                return await Result<VacationDto>.FailAsync($"No vacation with id '{vacationId}' was found.");
            }

            return await Result<VacationDto>.SuccessAsync(new VacationDto(result.Data));
        }

        /// <summary>
        ///     Summary‑only lookup by case‑insensitive <see cref="Vacation.Name"/>.
        /// </summary>
        public async Task<IBaseResult<VacationDto>> VacationSummaryFromNameAsync(string vacationName, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Vacation>(v => v.VacationTitleTemplate.VacationTitle.ToLower() == vacationName.ToLower());
            spec.AddInclude(i => i.Include(v => v.Images).ThenInclude(ii => ii.Image));
            spec.AddInclude(i => i.Include(v => v.Prices));
            spec.AddInclude(i => i.Include(v => v.VacationHost).ThenInclude(h => h!.Images).ThenInclude(i => i.Image));
            spec.AddInclude(i => i.Include(v => v.Reviews).ThenInclude(r => r.Review).ThenInclude(i => i.Images).ThenInclude(i => i.Image));

            var result = await accomodationRepositoryManager.Vacations.FirstOrDefaultAsync(spec, trackChanges: false, cancellationToken);
            if (!result.Succeeded)
            {
                return await Result<VacationDto>.FailAsync(result.Messages);
            }
            if (result.Data is null)
            {
                return await Result<VacationDto>.FailAsync($"No vacation with name '{vacationName}' was found.");
            }

            return await Result<VacationDto>.SuccessAsync(new VacationDto(result.Data));
        }

        /// <summary>
        /// Retrieves a vacation by its unique slug identifier.
        /// </summary>
        /// <remarks>This method queries the repository for a vacation that matches the specified slug. If
        /// no matching vacation is found, the result will indicate failure with an appropriate error message.</remarks>
        /// <param name="slug">The unique slug identifier of the vacation. This value is case-insensitive.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is <see cref="VacationDto"/>. If the operation succeeds, the result contains the vacation
        /// data; otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<VacationDto>> VacationFromSlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Vacation>(v => v.Slug.ToLower() == slug.ToLower());
            spec.AddInclude(i => i.Include(v => v.Images).ThenInclude(ii => ii.Image));
            spec.AddInclude(i => i.Include(v => v.Prices));
            spec.AddInclude(i => i.Include(v => v.VacationHost).ThenInclude(h => h!.Images).ThenInclude(ii => ii.Image));
            spec.AddInclude(i => i.Include(v => v.DayTourActivities).ThenInclude(c => c.DayTourActivityTemplate));
            spec.AddInclude(i => i.Include(c => c.VacationPriceGroups));
            spec.AddInclude(i => i.Include(c => c.VacationTitleTemplate));
            spec.AddInclude(i => i.Include(c => c.ShortDescriptionTemplate));
            spec.AddInclude(i => i.Include(c => c.PaymentExclusionTemplate));
            spec.AddInclude(i => i.Include(c => c.CancellationTermsTemplate));
            spec.AddInclude(i => i.Include(c => c.TermsAndConditionsTemplate));
            spec.AddInclude(i => i.Include(c => c.GeneralInformationTemplate));
            spec.AddInclude(i => i.Include(c => c.MeetAndGreetTemplate));
            spec.AddInclude(i => i.Include(c => c.ItineraryEntryItemTemplates));

            var result = await accomodationRepositoryManager.Vacations.FirstOrDefaultAsync(spec, trackChanges: false, cancellationToken);
            if (!result.Succeeded)
            {
                return await Result<VacationDto>.FailAsync(result.Messages);
            }
            if (result.Data is null)
            {
                return await Result<VacationDto>.FailAsync($"No vacation with slug '{slug}' was found.");
            }

            return await Result<VacationDto>.SuccessAsync(new VacationDto(result.Data));
        }
        
        /// <summary>
        ///     Creates a full <strong>clone</strong> of an existing <see cref="Vacation"/> (deep‑copies collections) and
        ///     persists it with a new identity.
        /// </summary>
        /// <param name="vacationId">The source vacation’s identifier.</param>
        /// <param name="name">The display name for the new duplicate.</param>
        /// <param name="cancellationToken">Co‑operative cancellation token.</param>
        /// <returns>A success or failure <see cref="IBaseResult"/>.</returns>
        public async Task<IBaseResult> DuplicateAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Vacation>(v => v.Id == vacationId);
            spec.AddInclude(i => i.Include(v => v.Images).ThenInclude(ii => ii.Image));
            spec.AddInclude(i => i.Include(v => v.Contacts));
            spec.AddInclude(i => i.Include(v => v.VacationPriceGroups));
            spec.AddInclude(i => i.Include(v => v.Prices));
            spec.AddInclude(i => i.Include(v => v.VacationHost).ThenInclude(h => h!.Images).ThenInclude(ii => ii.Image));
            spec.AddInclude(i => i.Include(v => v.DayTourActivities).ThenInclude(c => c.DayTourActivityTemplate));
            spec.AddInclude(i => i.Include(c => c.VacationTitleTemplate));
            spec.AddInclude(i => i.Include(c => c.ShortDescriptionTemplate));
            spec.AddInclude(i => i.Include(c => c.PaymentExclusionTemplate));
            spec.AddInclude(i => i.Include(c => c.CancellationTermsTemplate));
            spec.AddInclude(i => i.Include(c => c.TermsAndConditionsTemplate));
            spec.AddInclude(i => i.Include(c => c.GeneralInformationTemplate));
            spec.AddInclude(i => i.Include(c => c.ItineraryEntryItemTemplates));

            var sourceResult = await accomodationRepositoryManager.Vacations.FirstOrDefaultAsync(spec, trackChanges: false, cancellationToken);
            if (!sourceResult.Succeeded)
            {
                return await Result.FailAsync(sourceResult.Messages);
            }
            if (sourceResult.Data is null)
            {
                return await Result.FailAsync($"Vacation '{vacationId}' not found for duplication.");
            }

            await EnrichVacationAsync(sourceResult.Data, cancellationToken);

            var clone = sourceResult.Data.Clone();

            await accomodationRepositoryManager.Vacations.CreateAsync(clone, cancellationToken);
            var saveResult = await accomodationRepositoryManager.Vacations.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
            {
                return await Result.FailAsync(saveResult.Messages);
            }

            return await Result.SuccessAsync($"Vacation '{sourceResult.Data.VacationTitleTemplate.VacationTitle}' (clone of '{vacationId}') was created successfully.");
        }

        /// <summary>
        ///     Persists a new <see cref="Vacation"/> aggregate together with its associated images.
        /// </summary>
        /// <param name="dto">A fully‑populated <see cref="VacationDto"/> coming from UI/front‑end.</param>
        /// <param name="uploadPath">Root path used for image resize &amp; persistence.</param>
        /// <param name="cancellationToken">Co‑operative cancellation token.</param>
        public async Task<IBaseResult> CreateAsync(VacationDto dto, string uploadPath, CancellationToken cancellationToken = default)
        {
            var entity = dto.ToVacation(uploadPath);
            if (entity is null)
            {
                return await Result.FailAsync($"Transformation from DTO returned null for dto id '{dto.VacationId}'.");
            }

            var result = await accomodationRepositoryManager.Vacations.CreateAsync(entity, cancellationToken);
            if(!result.Succeeded) return await Result.FailAsync(result.Messages);

            foreach (var extension in dto.VacationExtensions)
            {
                await accomodationRepositoryManager.VacationExtensionAdditions.CreateAsync(new VacationExtensionAddition(){ParentVacationId = dto.VacationId, ExtensioId = extension.VacationId}, cancellationToken);
            }

            foreach (var image in dto.Images)
            {
                await accomodationRepositoryManager.VacationImages.CreateAsync(new EntityImage<Vacation, string>(image.Id, dto.VacationId), cancellationToken);
            }

            foreach (var itineraryEntryItem in dto.ItineraryEntryItemTemplates)
            {
                await accomodationRepositoryManager.ItineraryEntryItemTemplates.CreateAsync(new ItineraryEntryItemTemplate() { VacationId = dto.VacationId, DayNr = itineraryEntryItem.DayNr, Content = itineraryEntryItem.Content}, cancellationToken);
            }

            var saveResult = await accomodationRepositoryManager.Vacations.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Vacation '{dto.VacationTitle?.VacationTitle}' ({entity.Id}) created successfully.");
        }
        
        /// <summary>
        ///     Updates an existing <see cref="Vacation"/> aggregate. The implementation intentionally omits heavy image
        ///     replacement logic (commented‑out) but keeps the structure for future reinstatement.
        /// </summary>
        /// <param name="dto">DTO containing the updated values.</param>
        /// <param name="uploadPath">Local storage path used for potential image processing.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task<IBaseResult> UpdateAsync(VacationDto dto, string uploadPath, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Vacation>(v => v.Id == dto.VacationId);
            var vacationResult = await accomodationRepositoryManager.Vacations.FirstOrDefaultAsync(spec, trackChanges: true, cancellationToken);
            if (!vacationResult.Succeeded)
            {
                return await Result.FailAsync(vacationResult.Messages);
            }
            if (vacationResult.Data is null)
            {
                return await Result.FailAsync($"Vacation '{dto.VacationId}' not found.");
            }

            dto.UpdateVacation(vacationResult.Data);
            accomodationRepositoryManager.Vacations.Update(vacationResult.Data);

            var saveResult = await accomodationRepositoryManager.Vacations.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
            {
                return await Result.FailAsync(saveResult.Messages);
            }

            return await Result.SuccessAsync($"Vacation '{dto.VacationTitle.VacationTitle}' ({dto.VacationId}) updated successfully.");
        }

        /// <summary>
        ///     Permanently removes a <see cref="Vacation"/> aggregate from the data‑store.
        /// </summary>
        /// <param name="id">Primary key of the vacation to delete.</param>
        public async Task<IBaseResult> RemoveAsync(string id, CancellationToken cancellationToken = default)
        {
            var deleteResult = await accomodationRepositoryManager.Vacations.DeleteAsync(id, cancellationToken);
            if (!deleteResult.Succeeded) return await Result.FailAsync(deleteResult.Messages);

            var saveResult = await accomodationRepositoryManager.Vacations.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Vacation '{id}' was removed.");
        }

        #endregion

        #region Extensions

        /// <summary>
        /// Asynchronously retrieves all vacation extensions.
        /// </summary>
        /// <remarks>This method queries the repository for vacations marked as extensions and includes
        /// related images and title templates.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{VacationDto}"/> representing the collection of vacation extensions.</returns>
        public async Task<IBaseResult<IEnumerable<VacationDto>>> AllExtensionsAsync(CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Vacation>(v => v.IsExtension);
            spec.AddInclude(s => s.Include(v => v.Images).ThenInclude(i => i.Image));
            spec.AddInclude(i => i.Include(c => c.VacationTitleTemplate));

            var result = await accomodationRepositoryManager.Vacations.ListAsync(spec, trackChanges: false, cancellationToken);
            if (!result.Succeeded)
            {
                return await Result<IEnumerable<VacationDto>>.FailAsync(result.Messages);
            }

            return await Result<IEnumerable<VacationDto>>.SuccessAsync(result.Data.Select(v => new VacationDto(v)));
        }

        /// <summary>
        /// Retrieves a collection of vacation extensions associated with a specified vacation identifier.
        /// </summary>
        /// <remarks>This method queries the repository for vacation extensions linked to the specified
        /// vacation ID.  It includes related entities such as images and title templates in the result.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation for which extensions are to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{VacationDto}"/> representing the vacation extensions.</returns>
        public async Task<IBaseResult<IEnumerable<VacationDto>>> VacationExtensionsAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationExtensionAddition>(v => v.ParentVacationId == vacationId);
            spec.AddInclude(s => s.Include(v => v.Extension).ThenInclude(v => v.Images).ThenInclude(i => i.Image));
            spec.AddInclude(i => i.Include(v => v.Extension).ThenInclude(c => c.VacationTitleTemplate));
            spec.AddInclude(i => i.Include(v => v.Extension).ThenInclude(c => c.ShortDescriptionTemplate));

            var result = await accomodationRepositoryManager.VacationExtensionAdditions.ListAsync(spec, trackChanges: false, cancellationToken);
            if (!result.Succeeded)
            {
                return await Result<IEnumerable<VacationDto>>.FailAsync(result.Messages);
            }

            return await Result<IEnumerable<VacationDto>>.SuccessAsync(result.Data.Select(v => new VacationDto(v.Extension)));
        }

        /// <summary>
        /// Asynchronously creates a vacation extension for a specified vacation.
        /// </summary>
        /// <param name="request">The request containing the vacation ID and extension ID for the vacation extension to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation, along with any relevant messages.</returns>
        public async Task<IBaseResult> CreateExtensionAsync(CreateVacationExtensionForVacationRequest request, CancellationToken cancellationToken = default)
        {
            var result = await accomodationRepositoryManager.VacationExtensionAdditions.CreateAsync(new VacationExtensionAddition() { ParentVacationId = request.VacationId, ExtensioId = request.ExtensionId });
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);

            var saveResult = await accomodationRepositoryManager.Vacations.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
            return await Result.SuccessAsync($"Vacation extension created successfully.");
        }

        /// <summary>
        /// Asynchronously removes a vacation extension identified by the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the vacation extension to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation, along with any relevant messages.</returns>
        public async Task<IBaseResult> RemoveExtensionAsync(string id, CancellationToken cancellationToken = default)
        {
            var deleteResult = await accomodationRepositoryManager.VacationExtensionAdditions.DeleteAsync(id, cancellationToken);
            if (!deleteResult.Succeeded) return await Result.FailAsync(deleteResult.Messages);
            return await Result.SuccessAsync($"Vacation '{id}' was removed.");
        }

        #endregion

        #region Vacation Inclusion Display Info

        /// <summary>
        ///     Creates a <see cref="VacationInclusionDisplayTypeInformation"/> section for the given vacation, assigning a
        ///     <see cref="VacationInclusionDisplayTypeInformation.DisplayOrder"/> equal to <c>existingCount + 1</c>.
        /// </summary>
        public async Task<IBaseResult> CreateVacationInclusionDisplaySectionAsync(VacationInclusionDisplayTypeInformationDto dto, CancellationToken cancellationToken = default)
        {
            var currentCount = await accomodationRepositoryManager.VacationInclusionDisplayTypeInfos
                .ListAsync(new LambdaSpec<VacationInclusionDisplayTypeInformation>(i => i.VacationId == dto.VacationId),
                           trackChanges: false,
                           cancellationToken);

            var entity = dto.ToVacationInclusionDisplayTypeInformation();
            entity.DisplayOrder = currentCount.Data.Count();

            await accomodationRepositoryManager.VacationInclusionDisplayTypeInfos.CreateAsync(entity, cancellationToken);
            var saveResult = await accomodationRepositoryManager.VacationInclusionDisplayTypeInfos.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
            {
                return await Result.FailAsync(saveResult.Messages);
            }

            return await Result.SuccessAsync("Vacation inclusion display section created.");
        }

        /// <summary>
        ///     Updates the textual/content fields—but not the <see cref="VacationInclusionDisplayTypeInformation.DisplayOrder"/>
        ///     —of an existing inclusion‑info section.
        /// </summary>
        public async Task<IBaseResult> UpdateVacationInclusionDisplaySectionAsync(VacationInclusionDisplayTypeInformationDto dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationInclusionDisplayTypeInformation>(i => i.Id == dto.VacationInclusionDisplayTypeInformationId);
            var fetch = await accomodationRepositoryManager.VacationInclusionDisplayTypeInfos.FirstOrDefaultAsync(spec, trackChanges: true, cancellationToken);
            if (!fetch.Succeeded)
            {
                return await Result.FailAsync(fetch.Messages);
            }
            if (fetch.Data is null)
            {
                return await Result.FailAsync($"Inclusion section '{dto.VacationInclusionDisplayTypeInformationId}' was not found.");
            }

            dto.UpdateVacationInclusionDisplayTypeInformationValues(fetch.Data);
            accomodationRepositoryManager.VacationInclusionDisplayTypeInfos.Update(fetch.Data);

            var saveResult = await accomodationRepositoryManager.VacationInclusionDisplayTypeInfos.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
            {
                return await Result.FailAsync(saveResult.Messages);
            }

            return await Result.SuccessAsync("Vacation inclusion display section updated.");
        }

        /// <summary>
        ///     Bulk‑updates the visual order (and column selection) of multiple inclusion display sections in a single
        ///     database round‑trip.
        /// </summary>
        public async Task<IBaseResult> UpdateVacationInclusionDisplaySectionDisplayOrderAsync(VacationInclusionDisplayTypeInformationGroupUpdateRequest dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationInclusionDisplayTypeInformation>(i => i.VacationId == dto.VacationId);
            var collection = await accomodationRepositoryManager.VacationInclusionDisplayTypeInfos.ListAsync(spec, trackChanges: false, cancellationToken);

            foreach (var entity in collection.Data)
            {
                var match = dto.Items.FirstOrDefault(i => i.VacationInclusionDisplayTypeInformationId == entity.Id);
                if (match is null) continue;
                entity.DisplayOrder = match.DisplayOrder;
                entity.ColumnSelection = match.ColumnSelection;
                accomodationRepositoryManager.VacationInclusionDisplayTypeInfos.Update(entity);
            }

            var saveResult = await accomodationRepositoryManager.VacationInclusionDisplayTypeInfos.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
            {
                return await Result.FailAsync(saveResult.Messages);
            }

            return await Result.SuccessAsync("Display order updated.");
        }

        /// <summary>
        ///     Deletes a single <see cref="VacationInclusionDisplayTypeInformation"/> row.
        /// </summary>
        public async Task<IBaseResult> RemoveVacationInclusionDisplaySectionAsync(string vacationInclusionDisplayTypeInformationId, CancellationToken cancellationToken = default)
        {
            var deleteResult = await accomodationRepositoryManager.VacationInclusionDisplayTypeInfos.DeleteAsync(vacationInclusionDisplayTypeInformationId, cancellationToken);
            if (!deleteResult.Succeeded)
            {
                return await Result.FailAsync(deleteResult.Messages);
            }

            return await Result.SuccessAsync($"Inclusion display section '{vacationInclusionDisplayTypeInformationId}' removed.");
        }

        #endregion

        #region Images (Gallery/Cover/Map/Banner)

        /// <summary>
        /// Adds an image to a vacation entity with the specified details.
        /// </summary>
        /// <remarks>This method associates an image with a vacation entity by creating a new image record
        /// in the repository and saving the changes. If the operation fails at any step, the result will contain the
        /// corresponding error messages.</remarks>
        /// <param name="request">The request containing the image details, including the image ID, entity ID, selector, and order.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> AddVacationImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var image = new EntityImage<Vacation, string>(request.ImageId, request.EntityId) { Selector = request.Selector, Order = request.Order};

            var addResult = await vacationImageRepository.CreateAsync(image, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await vacationImageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Removes a vacation image identified by the specified image ID.
        /// </summary>
        /// <remarks>This method attempts to delete the specified image from the repository and save the
        /// changes. If either operation fails, the method returns a failure result with the associated error
        /// messages.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If the operation fails, the result includes error
        /// messages.</returns>
        public async Task<IBaseResult> RemoveVacationImage(string imageId, CancellationToken cancellationToken = default)
        {
            var addResult = await vacationImageRepository.DeleteAsync(imageId, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await vacationImageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        #endregion

        #region Videos

        /// <summary>
        /// Adds a video to the lodging entity.
        /// </summary>
        /// <remarks>This method attempts to add a video to the specified lodging entity. It first creates
        /// the video entity and then saves it to the repository. If either the creation or saving operation fails, the
        /// method returns a failure result with the associated error messages.</remarks>
        /// <param name="request">The request containing the video details to be added, including the video ID and entity ID.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> AddVideo(AddEntityVideoRequest request, CancellationToken cancellationToken = default)
        {
            var video = new EntityVideo<Lodging, string>() { VideoId = request.VideoId, EntityId = request.EntityId };

            var addResult = await vacationVideoRepository.CreateAsync(video, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await vacationVideoRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Removes a video identified by the specified video ID.
        /// </summary>
        /// <remarks>This method attempts to delete the video from the repository and save the changes. If
        /// either operation fails, the method returns a failure result with the associated error messages.</remarks>
        /// <param name="videoId">The unique identifier of the video to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default)
        {
            var addResult = await vacationVideoRepository.DeleteAsync(videoId, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await vacationVideoRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }


        #endregion

        #region Reviews

        /// <summary>
        /// Creates a new vacation review and saves it to the repository.
        /// </summary>
        /// <remarks>This method creates a vacation review using the provided <paramref name="dto"/> and
        /// saves it to the repository.  If the creation or saving process fails, the returned result will contain the
        /// corresponding error messages.</remarks>
        /// <param name="dto">The data transfer object containing the details of the vacation review to create. Must not be <see
        /// langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If successful, the result includes a success message. If
        /// unsuccessful, the result contains error messages.</returns>
        public async Task<IBaseResult> CreateVacationReviewAsync(VacationReviewDto dto, CancellationToken cancellationToken = default)
        {
            var createResult = await accomodationRepositoryManager.VacationReviews.CreateAsync(new VacationReview(dto.EntityId, dto.Review.Id), cancellationToken);
            if (!createResult.Succeeded)
                return await Result.FailAsync(createResult.Messages);

            var saveReview = await accomodationRepositoryManager.VacationReviews.SaveAsync(cancellationToken);
            if (!saveReview.Succeeded)
                return await Result.FailAsync(saveReview.Messages);

            return await Result.SuccessAsync("Vacation review was created successfully");
        }

        /// <summary>
        /// Updates an existing vacation review with the provided data.
        /// </summary>
        /// <remarks>This method updates the vacation review identified by the <see
        /// cref="VacationReviewDto.Id"/> property in the provided <paramref name="dto"/>. If the specified review does
        /// not exist, or if the update or save operation fails, the method returns a failure result with appropriate
        /// error messages.</remarks>
        /// <param name="dto">The data transfer object containing the updated vacation review information. The <see
        /// cref="VacationReviewDto.Id"/> must match an existing review.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If successful, the result includes a success message. If
        /// unsuccessful, the result includes error messages describing the failure.</returns>
        public async Task<IBaseResult> UpdateVacationReviewAsync(VacationReviewDto dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationReview>(r => r.Id == dto.Id);

            var result = await accomodationRepositoryManager.VacationReviews.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!result.Succeeded || result.Data == null)
                return await Result.FailAsync(result.Messages);

            var review = result.Data;
            review.ReviewId = dto.EntityId;

            var updateResult = accomodationRepositoryManager.VacationReviews.Update(review);
            if (!updateResult.Succeeded)
                return await Result.FailAsync(updateResult.Messages);

            var saveReview = await accomodationRepositoryManager.VacationReviews.SaveAsync(cancellationToken);
            if (!saveReview.Succeeded)
                return await Result.FailAsync(saveReview.Messages);

            return await Result.SuccessAsync("Vacation review updated successfully");
        }

        /// <summary>
        /// Removes a vacation review with the specified identifier.
        /// </summary>
        /// <remarks>This method attempts to locate the vacation review by its identifier and remove it
        /// from the repository. If the review is not found, the operation is considered successful without any
        /// changes.</remarks>
        /// <param name="id">The unique identifier of the vacation review to remove. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If successful, the result includes a success message.</returns>
        public async Task<IBaseResult> RemoveVacationReviewAsync(string id, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationReview>(r => r.Id == id);
            var linkResult = await accomodationRepositoryManager.VacationReviews.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (linkResult.Succeeded && linkResult.Data != null)
            {
                accomodationRepositoryManager.VacationReviews.Delete(linkResult.Data);
                var saveLink = await accomodationRepositoryManager.VacationReviews.SaveAsync(cancellationToken);
                if (!saveLink.Succeeded)
                    return await Result.FailAsync(saveLink.Messages);
            }

            return await Result.SuccessAsync($"Vacation review with id '{id}' was successfully removed");
        }

        #endregion

        #region Helper utilities

        /// <summary>
        ///     Populates the non‑lazy‑loaded navigation properties for detailed vacation views.
        /// </summary>
        private async Task EnrichVacationAsync(Vacation vacation, CancellationToken cancellationToken)
        {
            // Host (redundant include guard)
            if (vacation.VacationHost is null)
            {
                var hostSpec = new LambdaSpec<VacationHost>(h => h.Id == vacation.VacationHostId);
                hostSpec.AddInclude(i => i.Include(h => h.Images).ThenInclude(ii => ii.Image));
                var host = await accomodationRepositoryManager.VacationHosts.FirstOrDefaultAsync(hostSpec, trackChanges: false, cancellationToken);
                if (host.Succeeded) vacation.VacationHost = host.Data;
            }

            // Reviews
            var reviewSpec = new LambdaSpec<VacationReview>(r => r.VacationId == vacation.Id);
            reviewSpec.AddInclude(i => i.Include(r => r.Review).ThenInclude(ii => ii.Images).ThenInclude(ii => ii.Image));

            var reviews = await accomodationRepositoryManager.VacationReviews.ListAsync(reviewSpec, trackChanges: false, cancellationToken);
            if (reviews.Succeeded) vacation.Reviews = reviews.Data;

            // Intervals (lodging/destination)
            var intervalSpec = new LambdaSpec<VacationInterval>(iv => iv.VacationId == vacation.Id);
            intervalSpec.AddInclude(i => i.Include(iv => iv.Lodging).ThenInclude(l => l.Images).ThenInclude(ii => ii.Image));
            intervalSpec.AddInclude(i => i.Include(iv => iv.Lodging).ThenInclude(l => l.Country));
            intervalSpec.AddInclude(i => i.Include(iv => iv.Destination).ThenInclude(d => d.Images).ThenInclude(ii => ii.Image));
            var intervals = await accomodationRepositoryManager.VacationIntervals.ListAsync(intervalSpec, trackChanges: false, cancellationToken);
            if (intervals.Succeeded) vacation.Intervals = intervals.Data;

            // Flights
            var flightSpec = new LambdaSpec<Flight>(f => f.VacationId == vacation.Id);
            flightSpec.AddInclude(i => i.Include(f => f.ArrivalAirport).ThenInclude(c => c.City).ThenInclude(c => c.Country));
            flightSpec.AddInclude(i => i.Include(f => f.DepartureAirport).ThenInclude(c => c.City).ThenInclude(c => c.Country));

            var flights = await accomodationRepositoryManager.Flights.ListAsync(flightSpec, trackChanges: false, cancellationToken);
            if (flights.Succeeded) vacation.Flights = flights.Data;

            // Golfer packages
            var golfSpec = new LambdaSpec<GolferPackage>(g => g.VacationId == vacation.Id);
            golfSpec.AddInclude(i => i.Include(g => g.GolfCourse).ThenInclude(c => c.Images).ThenInclude(ii => ii.Image));
            var golf = await accomodationRepositoryManager.GolferPackages.ListAsync(golfSpec, trackChanges: false, cancellationToken);
            if (golf.Succeeded) vacation.GolferPackages = golf.Data;

            // Meals
            var mealSpec = new LambdaSpec<MealAddition>(m => m.VacationId == vacation.Id);
            mealSpec.AddInclude(c => c.Include(g => g.MealAdditionTemplate).ThenInclude(c => c.Restaurant));

            var meals = await accomodationRepositoryManager.MealAdditions.ListAsync(mealSpec, trackChanges: false, cancellationToken);
            if (meals.Succeeded) vacation.MealAdditions = meals.Data;

            // Day tours
            var tourSpec = new LambdaSpec<DayTourActivity>(d => d.VacationId == vacation.Id);
            tourSpec.AddInclude(c => c.Include(g => g.DayTourActivityTemplate));

            var tours = await accomodationRepositoryManager.DayTourActivities.ListAsync(tourSpec, trackChanges: false, cancellationToken);
            if (tours.Succeeded) vacation.DayTourActivities = tours.Data;

            // Inclusion display types
            var incSpec = new LambdaSpec<VacationInclusionDisplayTypeInformation>(i => i.VacationId == vacation.Id);
            var inc = await accomodationRepositoryManager.VacationInclusionDisplayTypeInfos.ListAsync(incSpec, trackChanges: false, cancellationToken);
            if (inc.Succeeded) vacation.VacationInclusionDisplayTypeInfos = inc.Data.ToList();

            // Gifts
            var giftSpec = new LambdaSpec<RoomGift>(g => g.VacationId == vacation.Id);
            giftSpec.AddInclude(c => c.Include(g => g.Gift));
            var gifts = await accomodationRepositoryManager.Gifts.ListAsync(giftSpec, trackChanges: false, cancellationToken);
            if (gifts.Succeeded) vacation.Gifts = gifts.Data.ToList();

            // Payment rules
            var paySpec = new LambdaSpec<PaymentRule>(p => p.VacationId == vacation.Id);
            var pay = await accomodationRepositoryManager.VacationPaymentRules.ListAsync(paySpec, trackChanges: false, cancellationToken);
            if (pay.Succeeded) vacation.PaymentRules = pay.Data;
        }

        #endregion
    }
}
