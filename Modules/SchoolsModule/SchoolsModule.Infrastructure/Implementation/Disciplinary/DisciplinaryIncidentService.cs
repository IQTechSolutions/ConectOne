using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using IdentityModule.Domain.DataTransferObjects;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using NeuralTech.Base.Enums;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.Discipline;
using SchoolsModule.Domain.Specifications;

namespace SchoolsModule.Infrastructure.Implementation.Disciplinary;

/// <summary>
/// Default implementation of <see cref="IDisciplinaryIncidentService"/>.
/// </summary>
public class DisciplinaryIncidentService(ISchoolsModuleRepoManager repo, IPushNotificationService pushNotificationService) : IDisciplinaryIncidentService
{
    /// <summary>
    /// Creates a new disciplinary incident and persists it to the repository.
    /// </summary>
    /// <remarks>This method also sends notifications to the parents of the learner associated with the
    /// incident, if applicable.</remarks>
    /// <param name="incident">The details of the disciplinary incident to create. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> with
    /// the created <see cref="DisciplinaryIncidentDto"/> if the operation succeeds, or error messages if it fails.</returns>
    public async Task<IBaseResult<DisciplinaryIncidentDto>> CreateAsync(DisciplinaryIncidentDto incident, CancellationToken cancellationToken)
    {
        if (incident is null)
            return await Result<DisciplinaryIncidentDto>.FailAsync("Incident details must be provided");

        var entity = incident.CreateDisciplinaryIncident();
        var create = await repo.DisciplinaryIncidents.CreateAsync(entity, cancellationToken);
        if (!create.Succeeded) return await Result<DisciplinaryIncidentDto>.FailAsync(create.Messages);
        var save = await repo.DisciplinaryIncidents.SaveAsync(cancellationToken);
        if (!save.Succeeded) return await Result<DisciplinaryIncidentDto>.FailAsync(save.Messages);

        // Notify parents of the learner about the incident
        var learnerResult = await repo.Learners.FirstOrDefaultAsync(
            new SingleLearnerWithParentDetailsSpecification(entity.LearnerId),
            false, cancellationToken);
        if (learnerResult.Succeeded && learnerResult.Data is not null)
        {
            var learner = learnerResult.Data;
            var recipients = learner.Parents
                .Where(lp => lp.Parent != null)
                .Select(lp => new RecipientDto(
                    lp.ParentId!,
                    lp.Parent!.FirstName,
                    lp.Parent.LastName,
                    lp.Parent.EmailAddresses.Select(e => e.Email).ToList(),
                    lp.Parent.ReceiveNotifications,
                    lp.Parent.RecieveEmails))
                .ToList();

            if (recipients.Count > 0)
            {
                var notification = new NotificationDto
                {
                    EntityId = learner.Id,
                    Title = $"Disciplinary incident for {learner.FirstName} {learner.LastName}",
                    ShortDescription = $"Incident recorded on {entity.Date.ToShortDateString()}",
                    Message = string.IsNullOrWhiteSpace(entity.Description)
                        ? $"A disciplinary incident was recorded for {learner.FirstName} {learner.LastName}."
                        : entity.Description,
                    MessageType = MessageType.Parent,
                    Created = DateTime.Now,
                    NotificationUrl = $"/learners/{learner.Id}"
                };

                await pushNotificationService.EnqueueNotificationsAsync(recipients, notification);
            }
        }

        incident = incident with { DisciplinaryActionId = entity.Id };
        return await Result<DisciplinaryIncidentDto>.SuccessAsync(incident);
    }

    /// <summary>
    /// Updates an existing disciplinary incident with the provided details.
    /// </summary>
    /// <remarks>The method attempts to locate the disciplinary incident by its identifier. If the incident is
    /// not found, the operation fails with an appropriate error message. If the incident is found, its details are
    /// updated and saved to the repository. The operation may fail if the update or save process encounters an
    /// issue.</remarks>
    /// <param name="incident">The data transfer object containing the updated details of the disciplinary incident.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the update operation. If successful, the result includes a success message;
    /// otherwise, it includes error messages.</returns>
    public async Task<IBaseResult> UpdateAsync(DisciplinaryIncidentDto incident, CancellationToken cancellationToken)
    {
        var spec = new LambdaSpec<DisciplinaryIncident>(i => i.Id == incident.DisciplinaryIncidentId);
        var lookup = await repo.DisciplinaryIncidents.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!lookup.Succeeded || lookup.Data is null)
            return await Result.FailAsync("Incident not found");

        var entity = lookup.Data;
        entity.Date = incident.Date;
        entity.Description = incident.Description;
        entity.LearnerId = incident.LearnerId;
        entity.DisciplinaryActionId = incident.DisciplinaryActionId;
        entity.SeverityScore = incident.SeverityScore;

        var update = repo.DisciplinaryIncidents.Update(entity);
        if (!update.Succeeded) return await Result.FailAsync(update.Messages);
        var save = await repo.DisciplinaryIncidents.SaveAsync(cancellationToken);
        if (!save.Succeeded) return await Result.FailAsync(save.Messages);
        return await Result.SuccessAsync("Incident updated");
    }

    /// <summary>
    /// Deletes a disciplinary incident by its identifier and saves the changes asynchronously.
    /// </summary>
    /// <remarks>This method first attempts to delete the specified disciplinary incident. If the deletion
    /// succeeds, it saves the changes to the repository. If either operation fails, the method returns a failure result
    /// with the corresponding error messages.</remarks>
    /// <param name="incidentId">The unique identifier of the disciplinary incident to delete. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation. If successful, the result includes a success message;
    /// otherwise, it includes error messages.</returns>
    public async Task<IBaseResult> DeleteAsync(string incidentId, CancellationToken cancellationToken)
    {
        var delete = await repo.DisciplinaryIncidents.DeleteAsync(incidentId, cancellationToken);
        if (!delete.Succeeded) return await Result.FailAsync(delete.Messages);
        var save = await repo.DisciplinaryIncidents.SaveAsync(cancellationToken);
        if (!save.Succeeded) return await Result.FailAsync(save.Messages);
        return await Result.SuccessAsync("Incident removed");
    }

    /// <summary>
    /// Retrieves a collection of disciplinary incidents associated with a specific learner.
    /// </summary>
    /// <remarks>The method filters disciplinary incidents by the specified learner ID and returns them as
    /// DTOs. The result indicates success or failure, along with any relevant messages.</remarks>
    /// <param name="learnerId">The unique identifier of the learner whose disciplinary incidents are to be retrieved.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// a collection of <see cref="DisciplinaryIncidentDto"/> objects representing the learner's disciplinary incidents.
    /// If the operation fails, the result contains error messages.</returns>
    public async Task<IBaseResult<IEnumerable<DisciplinaryIncidentDto>>> IncidentsByLearnerAsync(string learnerId, CancellationToken cancellationToken)
    {
        var spec = new LambdaSpec<DisciplinaryIncident>(i => i.LearnerId == learnerId);
        var result = await repo.DisciplinaryIncidents.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded) return await Result<IEnumerable<DisciplinaryIncidentDto>>.FailAsync(result.Messages);
        var dtos = result.Data.Select(i => new DisciplinaryIncidentDto(i));
        return await Result<IEnumerable<DisciplinaryIncidentDto>>.SuccessAsync(dtos.ToList());
    }
}
