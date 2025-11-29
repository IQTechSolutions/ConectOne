using CalendarModule.Domain.DataTransferObjects;
using CalendarModule.Domain.Entities;
using CalendarModule.Domain.Enums;
using CalendarModule.Domain.Interfaces;
using CalendarModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;
using IdentityModule.Domain.DataTransferObjects;

namespace CalendarModule.Infrastructure.Implementation
{
    /// <summary>
    /// Provides functionality for managing and interacting with appointments.
    /// </summary>
    /// <remarks>This service is responsible for handling operations related to the <see cref="Appointment"/>
    /// entity, including data access and business logic. It extends the functionality of <see
    /// cref="BaseService{TEntity, TDto, TKey}"/> and implements the <see cref="IAppointmentService"/>
    /// interface.</remarks>
    public class AppointmentService(IRepository<Appointment, string> repository) : IAppointmentService
    {
        /// <summary>
        /// Retrieves all calendar entries asynchronously.
        /// </summary>
        /// <remarks>The method retrieves calendar entries and maps them to <see cref="CalendarEntryDto"/>
        /// objects.  The returned result indicates whether the operation succeeded or failed, along with any relevant 
        /// messages in case of failure.</remarks>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult"/>
        /// containing a list of <see cref="CalendarEntryDto"/> objects  representing the calendar entries. If the
        /// operation fails, the result will include error messages.</returns>
        public async Task<IBaseResult<List<CalendarEntryDto>>> GetAllAsync(CalendarPageParameters requestParameters, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Appointment>(c =>
                c.StartDate.Date >= requestParameters.StartDate && c.EndDate.Date <= requestParameters.EndDate);
            spec.AddInclude(c => c.Include(g => g.UserInvites).ThenInclude(c => c.User));
            spec.AddInclude(c => c.Include(g => g.RoleInvites).ThenInclude(c => c.Role));

            var result = await repository.ListAsync(spec, false, cancellationToken);

            if (!result.Succeeded)
                return await Result<List<CalendarEntryDto>>.FailAsync(result.Messages);

            var dtos = result.Data.Select(ToDto).ToList();
            return await Result<List<CalendarEntryDto>>.SuccessAsync(dtos);
        }

        /// <summary>
        /// Retrieves a calendar entry by its unique identifier.
        /// </summary>
        /// <remarks>This method queries the repository for a calendar entry matching the specified
        /// identifier. If the entry is found, it is mapped to a <see cref="CalendarEntryDto"/> and returned. If the
        /// entry is not found, the result will indicate failure with an appropriate message.</remarks>
        /// <param name="id">The unique identifier of the calendar entry to retrieve. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. The default value is <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <typeparamref name="T"/> is <see cref="CalendarEntryDto"/>. The result indicates whether the operation
        /// succeeded and, if successful, includes the requested calendar entry. If the entry is not found, the result
        /// will indicate failure.</returns>
        public async Task<IBaseResult<CalendarEntryDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Appointment>(c => c.Id == id);
            spec.AddInclude(c => c.Include(g => g.UserInvites).ThenInclude(c => c.User).ThenInclude(c => c.UserInfo));
            spec.AddInclude(c => c.Include(g => g.RoleInvites).ThenInclude(c => c.Role));


            var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);

            if (!result.Succeeded)
                return await Result<CalendarEntryDto>.FailAsync(result.Messages);
            if (result.Data is null)
                return await Result<CalendarEntryDto>.FailAsync("Not Found");

            var dto = ToDto(result.Data);
            return await Result<CalendarEntryDto>.SuccessAsync(dto);
        }

        /// <summary>
        /// Asynchronously adds a new calendar entry based on the provided data transfer object (DTO).
        /// </summary>
        /// <remarks>This method validates the provided DTO and attempts to create a new calendar entry in
        /// the repository. If the creation or subsequent save operation fails, the method returns a failure result with
        /// the corresponding error messages.</remarks>
        /// <param name="dto">The <see cref="CalendarEntryDto"/> containing the details of the calendar entry to be added. The DTO must
        /// include valid values for required fields such as <see cref="CalendarEntryDto.StartDate"/> and <see
        /// cref="CalendarEntryDto.EndDate"/>.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is <see cref="CalendarEntryDto"/>. If the operation succeeds, the result contains the created
        /// calendar entry. If the operation fails, the result contains error messages describing the failure.</returns>
        public async Task<IBaseResult<CalendarEntryDto>> AddAsync(CalendarEntryDto dto, CancellationToken cancellationToken = default)
        {
            var appointment = new Appointment
            {
                Id = dto.Id,
                Heading = dto.Name,
                StartDate = dto.StartDate!.Value,
                StartTime = dto.StartTime!.Value,
                EndDate = dto.EndDate!.Value,
                Color = dto.Color,
                EndTime = dto.EndTime!.Value,
                FullDayEvent = dto.FullDayEvent,
                AudienceType = dto.AudienceType,
                UserInvites = dto.InvitedUsers.Select(userId => new AppointmentUserInvite
                {
                    AppointmentId = dto.Id,
                    UserId = userId.UserId
                }).ToList(),
                RoleInvites = dto.InvitedRoles.Select(roleId => new AppointmentRoleInvite
                {
                    AppointmentId = dto.Id,
                    RoleId = roleId.Id
                }).ToList()
            };

            var result = await repository.CreateAsync(appointment, cancellationToken);
            if(!result.Succeeded)
                return await Result<CalendarEntryDto>.FailAsync(result.Messages);

            var saveResult = await repository.SaveAsync(cancellationToken);
            if(!saveResult.Succeeded)
                return await Result<CalendarEntryDto>.FailAsync(saveResult.Messages);

            var spec = new LambdaSpec<Appointment>(c => c.Id == appointment.Id);
            spec.AddInclude(c => c.Include(g => g.UserInvites).ThenInclude(c => c.User).ThenInclude(c => c.UserInfo));
            spec.AddInclude(c => c.Include(g => g.RoleInvites).ThenInclude(c => c.Role));
            var returnResult = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);

            var createdDto = ToDto(returnResult.Data);
            return await Result<CalendarEntryDto>.SuccessAsync(createdDto);
        }

        /// <summary>
        /// Updates an existing calendar entry with the provided details.
        /// </summary>
        /// <remarks>This method updates an existing calendar entry by matching the provided <paramref
        /// name="dto"/> with an entry in the repository using its <see cref="CalendarEntryDto.Id"/>. If the entry is
        /// not found, the operation fails with an appropriate error message. The method also validates and applies the
        /// updated details, ensuring that all required fields in the <paramref name="dto"/> are populated. Changes are
        /// persisted to the repository, and the updated entry is returned upon success.</remarks>
        /// <param name="dto">The data transfer object containing the updated details of the calendar entry. The <see
        /// cref="CalendarEntryDto.Id"/> property must correspond to an existing calendar entry.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. This token can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is <see cref="CalendarEntryDto"/>. If the operation succeeds, the result contains the updated
        /// calendar entry. If the operation fails, the result contains error messages.</returns>
        public async Task<IBaseResult<CalendarEntryDto>> EditAsync(CalendarEntryDto dto, CancellationToken cancellationToken = default)
        {
            var fetchResult = await repository.FindByConditionAsync(
                c => c.Id == dto.Id,
                trackChanges: true,
                cancellationToken,
                c => c.UserInvites,
                c => c.RoleInvites);

            if (!fetchResult.Succeeded)
                return await Result<CalendarEntryDto>.FailAsync(fetchResult.Messages);

            var entity = fetchResult.Data.FirstOrDefault();
            if (entity is null)
                return await Result<CalendarEntryDto>.FailAsync("Not Found");

            entity.Heading = dto.Name;
            entity.Description = dto.Description;
            entity.Color = dto.Color;
            entity.StartDate = dto.StartDate!.Value;
            entity.StartTime = dto.StartTime!.Value;
            entity.EndDate = dto.EndDate!.Value;
            entity.EndTime = dto.EndTime!.Value;
            entity.FullDayEvent = dto.FullDayEvent;
            entity.AudienceType = dto.AudienceType;

            entity.UserInvites.Clear();
            foreach (var userId in dto.InvitedUsers)
            {
                entity.UserInvites.Add(new AppointmentUserInvite { AppointmentId = entity.Id, UserId = userId.UserId });
            }

            entity.RoleInvites.Clear();
            foreach (var roleId in dto.InvitedRoles)
            {
                entity.RoleInvites.Add(new AppointmentRoleInvite { AppointmentId = entity.Id, RoleId = roleId.Id });
            }

            var updateResult = repository.Update(entity);
            if(!updateResult.Succeeded)
                return await Result<CalendarEntryDto>.FailAsync(updateResult.Messages);

            var saveResult = await repository.SaveAsync(cancellationToken);
            if(!saveResult.Succeeded)
                return await Result<CalendarEntryDto>.FailAsync(saveResult.Messages);

            var updatedDto = ToDto(updateResult.Data);
            return await Result<CalendarEntryDto>.SuccessAsync(updatedDto);
        }

        private static CalendarEntryDto ToDto(Appointment appointment)
        {
            var dto = new CalendarEntryDto(appointment.Id, appointment.Heading, appointment.StartDate, appointment.StartTime,
                appointment.EndDate, appointment.EndTime, CalendarEntryType.Appointment, "", appointment.FullDayEvent)
            {
                Color = appointment.Color,
                Description = appointment.Description,
                AudienceType = appointment.AudienceType,
                InvitedUsers = appointment.UserInvites.Select(u => new UserInfoDto(u.User)).ToList(),
                InvitedRoles = appointment.RoleInvites.Select(r => new RoleDto(r.Role)).ToList()
            };

            return dto;
        }

        /// <summary>
        /// Deletes an entity with the specified identifier and persists the changes asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation. If the deletion or save operation fails, the result will include
        /// the corresponding error messages.</returns>
        public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await repository.DeleteAsync(id, cancellationToken);
            if(!result.Succeeded)
                return await Result.FailAsync(result.Messages);
            var saveResult = await repository.SaveAsync(cancellationToken);
            if(!saveResult.Succeeded)
                return await Result.FailAsync(saveResult.Messages);
            return await Result.SuccessAsync();
        }
    }
}
