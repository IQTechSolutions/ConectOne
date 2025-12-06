using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;

namespace AccomodationModule.Infrastructure.Implimentation;

/// <summary>
///     Service layer that encapsulates all read/write operations for
///     <see cref="VacationBooking" /> aggregates.
/// </summary>
/// <remarks>
///     <params>
///         • Exposes business-centric methods (page, activate, CRUD) so API/UI layers
///           do not deal directly with repositories or specifications.<br/>
///         • Uses <see cref="PaginatedResult{T}"/> to standardise paging responses.<br/>
///         • All calls are asynchronous to preserve thread resources and improve scalability.
///     </params>
/// </remarks>
public class VacationBookingService(IRepository<VacationBooking, string> repository) : IVacationBookingService
{
    /// <summary>
    /// Returns a paged list of bookings, optionally filtered by <paramref name="parameters.UserId"/>.
    /// </summary>
    /// <remarks>
    /// The method delegates filtering to a <c>LambdaSpec</c> instance which can later be
    /// combined with more complex criteria without touching this service.
    /// </remarks>
    public async Task<PaginatedResult<VacationBookingDto>> PagedAsync(VacationBookingPageParams parameters, CancellationToken cancellationToken = default)
    {
        var spec = string.IsNullOrEmpty(parameters.UserId) 
            ? new LambdaSpec<VacationBooking>(c => true) 
            : new LambdaSpec<VacationBooking>(c => c.UserId == parameters.UserId);

        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return PaginatedResult<VacationBookingDto>.Failure(result.Messages);

        var pagedList = result.Data.Select(c => new VacationBookingDto(c)).ToList();

        return PaginatedResult<VacationBookingDto>.Success(pagedList, pagedList.Count, parameters.PageNr, parameters.PageSize);
    }

    /// <summary>
    /// Marks the specified booking as <see cref="BookingStatus.Active"/>.
    /// </summary>
    /// <param name="vacationBookingId">Primary key of the booking to activate.</param>
    /// <param name="cancellationToken">Token to abort the operation.</param>
    /// <returns>
    ///     <see cref="Result.SuccessAsync"/> on success; otherwise error messages.
    /// </returns>
    public async Task<IBaseResult> MarkAsActiveAsync(string vacationBookingId, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<VacationBooking>(c => c.Id == vacationBookingId);

        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result.FailAsync(result.Messages);

        result.Data.BookingStatus = BookingStatus.Active;
        var updateResult = repository.Update(result.Data);
        if (!updateResult.Succeeded)
            return await Result.FailAsync(updateResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        return saveResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync(saveResult.Messages);
    }

    /// <summary>
    /// Retrieves every booking in the data store.
    /// </summary>
    public async Task<IBaseResult<List<VacationBookingDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<VacationBooking>(c => true);

        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<List<VacationBookingDto>>.FailAsync(result.Messages);

        var dtoList = result.Data.Select(c => new VacationBookingDto(c)).ToList();
        return await Result<List<VacationBookingDto>>.SuccessAsync(dtoList);
    }

    /// <summary>
    /// Retrieves a single booking by its primary key.
    /// </summary>
    public async Task<IBaseResult<VacationBookingDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<VacationBooking>(c => c.Id == id);

        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<VacationBookingDto>.FailAsync(
                result.Succeeded ? $"Entity with ID {id} not found." : string.Join(",", result.Messages));

        return await Result<VacationBookingDto>.SuccessAsync(new VacationBookingDto(result.Data));
    }

    /// <summary>
    /// Persists a new booking.
    /// </summary>
    /// <remarks>
    /// For brevity the sample maps zero fields; extend as needed.
    /// </remarks>
    public async Task<IBaseResult<VacationBookingDto>> AddAsync(VacationBookingDto dto, CancellationToken cancellationToken = default)
    {
        var createResult = await repository.CreateAsync(new VacationBooking(), cancellationToken);
        if (!createResult.Succeeded)
            return await Result<VacationBookingDto>.FailAsync(createResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<VacationBookingDto>.FailAsync(saveResult.Messages);

        return await Result<VacationBookingDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Updates an existing booking with values from the supplied DTO.
    /// </summary>
    public async Task<IBaseResult<VacationBookingDto>> EditAsync(VacationBookingDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<VacationBooking>(c => c.Id == dto.BookingId);
        
        var result = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<VacationBookingDto>.FailAsync(
                result.Succeeded ? $"Entity with ID {dto.BookingId} not found." : string.Join(",", result.Messages));

        result.Data.Id = dto.BookingId;
        result.Data.StartDate = dto.StartDate;
        result.Data.EndDate = dto.EndDate;
        result.Data.ReferenceNr = dto.ReferenceNr;
        result.Data.PaymentMethod = dto.PaymentMethod;
        result.Data.ReservationType = dto.ReservationType;
        result.Data.BookingStatus = dto.BookingStatus;
        result.Data.AmountDueIncl = dto.AmountDueIncl;

        var updateResult = repository.Update(result.Data);
        if (!updateResult.Succeeded)
            return await Result<VacationBookingDto>.FailAsync(updateResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<VacationBookingDto>.FailAsync(saveResult.Messages);

        return await Result<VacationBookingDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Physically deletes a booking.
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
}
