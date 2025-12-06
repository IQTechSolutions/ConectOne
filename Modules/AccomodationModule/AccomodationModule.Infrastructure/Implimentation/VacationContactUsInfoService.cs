using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Mailing;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.Extensions.Configuration;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Provides services for managing "Vacation Contact Us" information, including retrieving, creating, updating, and
    /// deleting records.
    /// </summary>
    /// <remarks>This service interacts with the underlying repository to perform operations on "Vacation
    /// Contact Us" data. It provides methods to retrieve a list of records, fetch a specific record by its identifier,
    /// create new records, update existing ones, and delete records.</remarks>
    /// <param name="accomodationRepositoryManager"></param>
    public class VacationContactUsInfoService(IAccomodationRepositoryManager accomodationRepositoryManager, DefaultEmailSender emailSender, IConfiguration configuration) : IVacationContactUsInfoService
    {
        /// <summary>
        /// Retrieves a paginated list of vacation contact information based on the specified paging parameters.
        /// </summary>
        /// <remarks>This method retrieves vacation contact information from the repository and applies
        /// the specified paging parameters. If the operation fails, the returned <see cref="PaginatedResult{T}"/> will
        /// indicate failure and include error messages.</remarks>
        /// <param name="pageParameters">The paging parameters that specify the page number and page size for the result set.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="VacationContactUsInfoDto"/> objects
        /// representing the vacation contact information, along with pagination metadata.</returns>
        public async Task<PaginatedResult<VacationContactUsInfoDto>> PagedVacationContactUsInfoListAsync(RequestParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationContactUsInfo>(_ => true);

            var result = await accomodationRepositoryManager.VacationContactUsInfos.ListAsync(spec, trackChanges: false, cancellationToken);
            if (!result.Succeeded)
            {
                return PaginatedResult<VacationContactUsInfoDto>.Failure(result.Messages);
            }

            var page = result.Data.OrderByDescending(c => c.CreatedOn).Select(v => new VacationContactUsInfoDto(v)).ToList();
            return PaginatedResult<VacationContactUsInfoDto>.Success(page, result.Data.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves a list of vacation contact information records.
        /// </summary>
        /// <remarks>This method retrieves vacation contact information records and maps them to <see
        /// cref="VacationContactUsInfoDto"/> objects. If the operation is unsuccessful, the result will indicate
        /// failure and include relevant error messages.</remarks>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="VacationContactUsInfoDto"/> objects. If the
        /// operation fails, the result includes error messages.</returns>
        public async Task<IBaseResult<IEnumerable<VacationContactUsInfoDto>>> VacationContactUsInfoListAsync(CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationContactUsInfo>(r => true);

            var result = await accomodationRepositoryManager.VacationContactUsInfos.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded)
                return await Result<IEnumerable<VacationContactUsInfoDto>>.FailAsync(result.Messages);

            var dtos = result.Data.Select(vr => new VacationContactUsInfoDto(vr));
            return await Result<IEnumerable<VacationContactUsInfoDto>>.SuccessAsync(dtos);
        }

        /// <summary>
        /// Retrieves vacation contact information based on the specified identifier.
        /// </summary>
        /// <remarks>This method queries the repository for vacation contact information matching the
        /// specified identifier. If no matching record is found, the result will indicate failure with appropriate
        /// error messages.</remarks>
        /// <param name="vacationContactUsInfoId">The unique identifier of the vacation contact information to retrieve. Cannot be <see langword="null"/> or
        /// empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the requested <see cref="VacationContactUsInfoDto"/> if the operation succeeds, or
        /// error messages if it fails.</returns>
        public async Task<IBaseResult<VacationContactUsInfoDto>> VacationContactUsInfoAsync(string vacationContactUsInfoId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationContactUsInfo>(r => r.Id == vacationContactUsInfoId);

            var result = await accomodationRepositoryManager.VacationContactUsInfos.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded || result.Data == null)
                return await Result<VacationContactUsInfoDto>.FailAsync(result.Messages);

            return await Result<VacationContactUsInfoDto>.SuccessAsync(new VacationContactUsInfoDto(result.Data));
        }

        /// <summary>
        /// Creates a new "Vacation Contact Us" information entry asynchronously.
        /// </summary>
        /// <remarks>This method validates and persists the provided "Vacation Contact Us" information. If
        /// the creation or save operation fails, the returned result will contain the corresponding error
        /// messages.</remarks>
        /// <param name="dto">The data transfer object containing the details of the "Vacation Contact Us" information to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation, along with any associated messages.</returns>
        public async Task<IBaseResult> CreateVacationContactUsInfoAsync(VacationContactUsInfoDto dto, CancellationToken cancellationToken = default)
        {
            var review = dto.ToVacationContactUsInfo();
            var createResult = await accomodationRepositoryManager.VacationContactUsInfos.CreateAsync(review, cancellationToken);
            if (!createResult.Succeeded)
                return await Result.FailAsync(createResult.Messages);

            var emailResult = await emailSender.SendSupportTicketEmailAsync($"{dto.Name} {dto.Surname}", dto.Email, dto.Name + " " + dto.Surname,
                configuration["EmailConfiguration:From"], dto.Cell, dto.Subject, "Thank you for submitting your request. \r\n One of our consultants will contact you shortly to discuss details with regards to your enquiry.", $"Enquiry Received for {dto.Subject}");
            if (!emailResult.Succeeded)
                return emailResult;

            var adminEmailResult = await emailSender.SendAdminsSupportTicketEmailAsync($"Progolf web admin", configuration["EmailConfiguration:To"], dto.Name, dto.Surname, dto.Email,
                dto.Cell, dto.Subject, dto.Message, "New Enquiry Received");
            if (!adminEmailResult.Succeeded)
                return adminEmailResult;

            var saveReview = await accomodationRepositoryManager.VacationContactUsInfos.SaveAsync(cancellationToken);
            if (!saveReview.Succeeded)
                return await Result.FailAsync(saveReview.Messages);

            return await Result.SuccessAsync("Vacation Contact Us Info was created successfully");
        }

        /// <summary>
        /// Updates the vacation contact information based on the provided data transfer object (DTO).
        /// </summary>
        /// <remarks>This method performs the following steps: <list type="number"> <item>Validates that
        /// the specified record exists in the repository.</item> <item>Updates the record with the provided
        /// information.</item> <item>Saves the changes to the repository.</item> </list> If any step fails, the
        /// operation is aborted, and an appropriate failure result is returned.</remarks>
        /// <param name="dto">The <see cref="VacationContactUsInfoDto"/> containing the updated vacation contact information. The <see
        /// cref="VacationContactUsInfoDto.Id"/> property must match an existing record.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. This parameter is optional and
        /// defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is an <see cref="IBaseResult"/>: <list
        /// type="bullet"> <item> <description>Returns a success result if the vacation contact information is updated
        /// successfully.</description> </item> <item> <description>Returns a failure result if the update fails,
        /// including any relevant error messages.</description> </item> </list></returns>
        public async Task<IBaseResult> UpdateVacationContactUsInfoAsync(VacationContactUsInfoDto dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationContactUsInfo>(r => r.Id == dto.Id);

            var result = await accomodationRepositoryManager.VacationContactUsInfos.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!result.Succeeded || result.Data == null)
                return await Result.FailAsync(result.Messages);


            var updateResult = accomodationRepositoryManager.VacationContactUsInfos.Update(VacationContactUsInfoDto.UpdateVacationContactUsInfo(dto));
            if (!updateResult.Succeeded)
                return await Result.FailAsync(updateResult.Messages);

            accomodationRepositoryManager.VacationContactUsInfos.Update(result.Data);

            var saveReview = await accomodationRepositoryManager.VacationContactUsInfos.SaveAsync(cancellationToken);
            if (!saveReview.Succeeded)
                return await Result.FailAsync(saveReview.Messages);

            return await Result.SuccessAsync("Vacation Contact Us Info updated successfully");
        }

        /// <summary>
        /// Removes the "Vacation Contact Us" information associated with the specified review ID.
        /// </summary>
        /// <remarks>This method attempts to delete the "Vacation Contact Us" information for the
        /// specified review ID. If the deletion or subsequent save operation fails, the result will indicate the
        /// failure and include relevant error messages.</remarks>
        /// <param name="reviewId">The unique identifier of the review whose "Vacation Contact Us" information is to be removed.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation, along with any associated messages.</returns>
        public async Task<IBaseResult> RemoveVacationContactUsInfoAsync(string reviewId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationReview>(r => r.ReviewId == reviewId);

            var deleteResult = await accomodationRepositoryManager.VacationContactUsInfos.DeleteAsync(reviewId, cancellationToken);
            if (!deleteResult.Succeeded)
                return await Result.FailAsync(deleteResult.Messages);

            var saveReview = await accomodationRepositoryManager.VacationContactUsInfos.SaveAsync(cancellationToken);
            if (!saveReview.Succeeded)
                return await Result.FailAsync(saveReview.Messages);

            return await Result.SuccessAsync($"Vacation Contact Us Infowith id '{reviewId}' was successfully removed");
        }
    }
}
