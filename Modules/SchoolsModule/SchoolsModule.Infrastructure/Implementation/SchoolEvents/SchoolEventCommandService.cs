using CalendarModule.Domain.Entities;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using FilingModule.Domain.Enums;
using GroupingModule.Domain.Entities;
using IdentityModule.Domain.DataTransferObjects;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Entities;
using MessagingModule.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using NeuralTech.Base.Enums;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.Specifications;

namespace SchoolsModule.Infrastructure.Implementation.SchoolEvents
{
    /// <summary>
    /// Handles creation, updating, and deletion of <see cref="SchoolEventDto"/> entities and their related data.
    /// </summary>
    public class SchoolEventCommandService(ISchoolsModuleRepoManager schoolsModuleRepoManager, IRepository<Notification, string> notificationRepository, ISchoolEventCategoryService schoolEventCategoryService,
        IRepository<Message, string> messageRepository, IPushNotificationService pushNotificationService, IRepository<EntityImage<Event<Category<ActivityGroup>>, string>, string> eventImageRepo,
        IRepository<SchoolEventTicketType, string> ticketTypeRepo) : ISchoolEventCommandService
    {
        /// <summary>
        /// Creates a new school event and its associated data such as categories, teams, documents, and members.
        /// </summary>
        /// <param name="schoolEvent">The DTO representing the school event to create.</param>
        /// <param name="cancellationToken">The cancellation token passed</param>
        /// <returns>A result containing the created <see cref="SchoolEventDto"/> or error messages.</returns>
        public async Task<IBaseResult<SchoolEventDto>> Create(SchoolEventDto schoolEvent, CancellationToken cancellationToken = default)
        {
            var eventEntity = new SchoolEvent<Category<ActivityGroup>>
            {
                Id = schoolEvent.EventId,
                Heading = schoolEvent.Name,
                Description = schoolEvent.Details,
                StartDate = schoolEvent.StartDate,
                EndDate = schoolEvent.EndDate,
                Address = schoolEvent.Address,
                GoogleMapLink = schoolEvent.GoogleMapLink,
                EntityId = schoolEvent.EntityId,
                HomeEvent = schoolEvent.HomeEvent,
                AttendanceConsentRequired = schoolEvent.AttendanceConsentRequired,
                TransportConsentRequired = schoolEvent.TransportPermissionRequired,
                DocumentLinks = string.Join(";", schoolEvent.DocumentLinks),
                ParticipatingCategories = schoolEvent.ParticipatingCategories
                    .Select(c => new ParticipatingActivityGroupCategory
                    {
                        ActivityGroupCategoryId = c.CategoryId,
                        ActivityCategoryParentId = c.ParentCategoryId
                    }).ToList(),
                ParticipatingActivityGroups = schoolEvent.ParticipatingTeams
                    .Select(c => new ParticipatingActivityGroup
                    {
                        ActivityGroupId = c.ActivityGroupId,
                        ParticipatingTeamMembers = c.TeamMembers
                            .Select(g => new ParticipatingActitivityGroupTeamMember
                            {
                                TeamMemberId = g.LearnerId!,
                                ParticipatingActitivityGroupId = c.ActivityGroupId!
                            }).ToList()
                    }).ToList(),
                TicketTypes = schoolEvent.TicketTypes.Select(c => new SchoolEventTicketType(c)).ToList()
            };

            var createResult = await schoolsModuleRepoManager.SchoolEvents.CreateAsync(eventEntity, cancellationToken);
            if (!createResult.Succeeded) return await Result<SchoolEventDto>.FailAsync(createResult.Messages);
            
            var saveResult = await schoolsModuleRepoManager.SchoolEvents.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<SchoolEventDto>.FailAsync(saveResult.Messages);

            return await Result<SchoolEventDto>.SuccessAsync(schoolEvent);
        }

        /// <summary>
        /// Updates an existing school event, including its associated categories, teams, and members.
        /// </summary>
        /// <param name="schoolEventDto">The updated event data.</param>
        /// <returns>A result containing metadata about the update or error messages.</returns>
        public async Task<IBaseResult<EventUpdateResponse>> UpdateAsync(SchoolEventDto schoolEventDto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(schoolEventDto.EventId))
                return await Result<EventUpdateResponse>.FailAsync("EventId is required.");

            var spec = new LambdaSpec<SchoolEvent<Category<ActivityGroup>>>(e => e.Id == schoolEventDto.EventId);
            spec.AddInclude(q => q.Include(e => e.ParticipatingActivityGroups)
                .ThenInclude(p => p.ParticipatingTeamMembers).ThenInclude(pt => pt.TeamMember));
            spec.AddInclude(q => q.Include(e => e.ParticipatingCategories));

            var eventResult = await schoolsModuleRepoManager.SchoolEvents.ListAsync(spec, true, cancellationToken);
            var schoolEvent = eventResult.Data.FirstOrDefault();
            if (schoolEvent == null)
                return await Result<EventUpdateResponse>.FailAsync($"No School Event found for ID '{schoolEventDto.EventId}'.");
            
            // Update core fields
            schoolEvent.Heading = schoolEventDto.Name;
            schoolEvent.Description = schoolEventDto.Details;
            schoolEvent.StartDate = schoolEventDto.StartDate;
            schoolEvent.EndDate = schoolEventDto.EndDate;
            schoolEvent.Address = schoolEventDto.Address;
            schoolEvent.GoogleMapLink = schoolEventDto.GoogleMapLink;
            schoolEvent.EntityId = schoolEventDto.EntityId;
            schoolEvent.HomeEvent = schoolEventDto.HomeEvent;
            schoolEvent.Published = schoolEventDto.Published;
            schoolEvent.DocumentLinks = string.Join(";", schoolEventDto.DocumentLinks);
            schoolEvent.TransportConsentRequired = schoolEventDto.TransportPermissionRequired;
            schoolEvent.AttendanceConsentRequired = schoolEventDto.AttendanceConsentRequired;

            schoolsModuleRepoManager.SchoolEvents.Update(schoolEvent);

            if (schoolEvent.Images.Any(c => c.Image.ImageType == UploadType.Cover))
            {
                await eventImageRepo.DeleteAsync(schoolEvent.Images.First(c => c.Image.ImageType == UploadType.Cover).Id, cancellationToken);
            }

            var folderPath = Path.Combine("StaticFiles", "activitygroup", schoolEvent.Heading);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            //var file = ImageFileFactoryExtensions.ResizeImage(folderPath, schoolEventDto.CoverImageUrl);
            //if (file != null)
            //    await eventImageRepo.CreateAsync(file.ToImageFile<Event<Category<ActivityGroup>>, string>(schoolEvent.Id, UploadType.Cover), cancellationToken);

            var learnersForNotificationList = new HashSet<Learner>();
            if (schoolEvent.Published)
            {
                foreach (var item in schoolEvent.ParticipatingActivityGroups)
                {
                    foreach (var teamMember in item.ParticipatingTeamMembers)
                    {
                        learnersForNotificationList.Add(teamMember.TeamMember);
                    }
                }
            }

            // Sync categories
            var currentCatIds = schoolEvent.ParticipatingCategories.Select(c => c.ActivityGroupCategoryId).ToHashSet();
            var newCatIds = schoolEventDto.ParticipatingCategories.Select(c => c.CategoryId).ToHashSet();
            var catsToRemove = schoolEvent.ParticipatingCategories.Where(c => !newCatIds.Contains(c.ActivityGroupCategoryId)).ToList();

            foreach (var cat in catsToRemove)
            {
                var delResult = await schoolsModuleRepoManager.ParticipatingActivityGroupCategories.DeleteAsync(cat.Id);
                if (!delResult.Succeeded) return await Result<EventUpdateResponse>.FailAsync(delResult.Messages);
            }

            foreach (var dto in schoolEventDto.ParticipatingCategories.Where(c => !currentCatIds.Contains(c.CategoryId)))
            {
                var createResult = await schoolsModuleRepoManager.ParticipatingActivityGroupCategories.CreateAsync(new ParticipatingActivityGroupCategory
                {
                    ActivityGroupCategoryId = dto.CategoryId,
                    ActivityCategoryParentId = dto.ParentCategoryId,
                    EventId = schoolEvent.Id
                });
                if (!createResult.Succeeded) return await Result<EventUpdateResponse>.FailAsync(createResult.Messages);
            }

            // Sync teams
            var currentTeamIds = schoolEvent.ParticipatingActivityGroups.Select(t => t.ActivityGroupId).ToHashSet();
            var newTeamIds = schoolEventDto.ParticipatingTeams.Select(t => t.ActivityGroupId).ToHashSet();
            var teamsToRemove = schoolEvent.ParticipatingActivityGroups.Where(t => !newTeamIds.Contains(t.ActivityGroupId)).ToList();

            foreach (var team in teamsToRemove)
            {
                var delResult = await schoolsModuleRepoManager.ParticipatingActivityGroups.DeleteAsync(team.Id, cancellationToken);
                if (!delResult.Succeeded) return await Result<EventUpdateResponse>.FailAsync(delResult.Messages);
            }

            foreach (var dto in schoolEventDto.ParticipatingTeams.Where(t => !currentTeamIds.Contains(t.ActivityGroupId)))
            {
                var newTeam = new ParticipatingActivityGroup
                {
                    ActivityGroupId = dto.ActivityGroupId,
                    EventId = schoolEvent.Id,
                    ParticipatingTeamMembers = dto.TeamMembers.Select(tm => new ParticipatingActitivityGroupTeamMember
                    {
                        ParticipatingActitivityGroupId = dto.ActivityGroupId!,
                        TeamMemberId = tm.LearnerId!
                    }).ToList()
                };

                var delResult = await schoolsModuleRepoManager.ParticipatingActivityGroups.CreateAsync(newTeam, cancellationToken);
                if (!delResult.Succeeded) return await Result<EventUpdateResponse>.FailAsync(delResult.Messages);

                foreach (var teamMember in newTeam.ParticipatingTeamMembers)
                {
                    var learner = await schoolsModuleRepoManager.Learners.FirstOrDefaultAsync(new SingleLearnerWithParentDetailsSpecification(teamMember.TeamMemberId), false, cancellationToken);
                    learnersForNotificationList.Add(learner.Data!);
                }
            }

            
            // Sync team members
            foreach (var team in schoolEvent.ParticipatingActivityGroups)
            {
                var dto = schoolEventDto.ParticipatingTeams.FirstOrDefault(t => t.ActivityGroupId == team.ActivityGroupId);
                if (dto == null) continue;

                var currentMemberIds = team.ParticipatingTeamMembers.Select(m => m.TeamMemberId).ToHashSet();
                var newMemberIds = dto.TeamMembers.Select(m => m.LearnerId).ToHashSet();

                var membersToRemove = team.ParticipatingTeamMembers.Where(m => !newMemberIds.Contains(m.TeamMemberId)).ToList();
                foreach (var member in membersToRemove)
                {
                    var delResult = await schoolsModuleRepoManager.ParticipatingActivityGroupTeamMembers.DeleteAsync(member.Id, cancellationToken);
                    if (!delResult.Succeeded) return await Result<EventUpdateResponse>.FailAsync(delResult.Messages);
                }
                
                var membersToAdd = dto.TeamMembers.Where(m => !currentMemberIds.Contains(m.LearnerId)).ToList();
                foreach (var member in membersToAdd)
                {
                    var learner = await schoolsModuleRepoManager.Learners.FirstOrDefaultAsync(new SingleLearnerWithParentDetailsSpecification(member.LearnerId), false, cancellationToken);
                    learnersForNotificationList.Add(learner.Data!);

                    var createResult = await schoolsModuleRepoManager.ParticipatingActivityGroupTeamMembers.CreateAsync(new ParticipatingActitivityGroupTeamMember
                    {
                        ParticipatingActitivityGroupId = team.Id,
                        TeamMemberId = member.LearnerId
                    }, cancellationToken);
                    if (!createResult.Succeeded) return await Result<EventUpdateResponse>.FailAsync(createResult.Messages);
                }
            }

            var saveResult = await schoolsModuleRepoManager.SchoolEvents.SaveAsync(cancellationToken);
            if(!saveResult.Succeeded) return await Result<EventUpdateResponse>.FailAsync(saveResult.Messages);

            var notification = new NotificationDto
            {
                EntityId = schoolEventDto.EventId,
                Title = $"Attendance consent required for {schoolEventDto.Name}",
                ShortDescription = $"Your child/children is taking part in the {schoolEventDto.Name} on {schoolEventDto.StartDate.ToShortDateString()}, attendance consent is required",
                Message = $"Your child/children is taking part in the {schoolEventDto.Name} on {schoolEventDto.StartDate.ToShortDateString()}, please advise if your child/children will be attending this event.",
                MessageType = MessageType.Parent,
                Created = DateTime.Now,
                NotificationUrl = $"/events/{schoolEventDto.EventId}"
            };

            var recipientList = learnersForNotificationList.Select(c => new RecipientDto(c.Id, c.FirstName, c.LastName, c.EmailAddresses.Select(e => e.Email).ToList(), true, true, c.Images.FirstOrDefault(c => c.Image.ImageType == UploadType.Cover)?.Image.RelativePath ?? "_content/FilingModule.Blazor/images/profileImage128x128.png", MessageType.Learner)).ToList();

            foreach (var parent in from parentLearnerList in learnersForNotificationList from parent in parentLearnerList.Parents where recipientList.All(c => c.Id != parent.ParentId) select parent)
            {
                recipientList.Add(new RecipientDto(parent.ParentId!, parent.Parent!.FirstName, parent.Parent.LastName, parent.Parent.EmailAddresses.Select(c => c.Email).ToList(), parent.Parent.ReceiveNotifications, parent.Parent.RecieveEmails));
            }

            // Send notifications to new team members
            await pushNotificationService.EnqueueNotificationsAsync(recipientList, notification);
            return await Result<EventUpdateResponse>.SuccessAsync(new EventUpdateResponse { SchoolEvent = schoolEventDto });
        }

        /// <summary>
        /// Deletes a school event and all related data including teams, categories, notifications, and messages.
        /// </summary>
        /// <param name="schoolEventId">The unique ID of the event to delete.</param>
        /// <returns>A result indicating success or failure of the operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string schoolEventId, CancellationToken cancellationToken = default)
        {
            var spec = new SingleEventSpecification(schoolEventId);
            var eventResult = await schoolsModuleRepoManager.SchoolEvents.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!eventResult.Succeeded || eventResult.Data == null)
                return await Result.FailAsync(eventResult.Messages ?? ["Event not found."]);

            var schoolEvent = eventResult.Data;

            // Delete team members
            foreach (var group in schoolEvent.ParticipatingActivityGroups)
            {
                foreach (var member in group.ParticipatingTeamMembers)
                {
                    var deleteMember = await schoolsModuleRepoManager.ParticipatingActivityGroupTeamMembers.DeleteAsync(member.Id, cancellationToken);
                    if (!deleteMember.Succeeded) return await Result.FailAsync(deleteMember.Messages);
                }

                var deleteGroup = await schoolsModuleRepoManager.ParticipatingActivityGroups.DeleteAsync(group.Id, cancellationToken);
                if (!deleteGroup.Succeeded) return await Result.FailAsync(deleteGroup.Messages);
            }

            // Delete categories
            foreach (var category in schoolEvent.ParticipatingCategories)
            {
                var deleteCategory = await schoolsModuleRepoManager.ParticipatingActivityGroupCategories.DeleteAsync(category.Id, cancellationToken);
                if (!deleteCategory.Succeeded) return await Result.FailAsync(deleteCategory.Messages);
            }

            // Delete event
            var deleteEvent = await schoolsModuleRepoManager.SchoolEvents.DeleteAsync(schoolEventId, cancellationToken);
            if (!deleteEvent.Succeeded) return await Result.FailAsync(deleteEvent.Messages);

            // Delete notifications
            var notifications = await notificationRepository.ListAsync(new LambdaSpec<Notification>(c => c.EntityId == schoolEventId), true, cancellationToken);
            foreach (var note in notifications.Data)
            {
                var deleteNote = await notificationRepository.DeleteAsync(note.Id, cancellationToken);
                if (!deleteNote.Succeeded) return await Result.FailAsync(deleteNote.Messages);
            }

            // Delete messages
            var messages = await messageRepository.ListAsync(new LambdaSpec<Message>(c => c.EntityId == schoolEventId), true, cancellationToken);
            foreach (var msg in messages.Data)
            {
                var deleteMsg = await messageRepository.DeleteAsync(msg.Id, cancellationToken);
                if (!deleteMsg.Succeeded) return await Result.FailAsync(deleteMsg.Messages);
            }

            var save = await schoolsModuleRepoManager.SchoolEvents.SaveAsync(cancellationToken);
            return save.Succeeded
                ? await Result.SuccessAsync("Event successfully removed")
                : await Result.FailAsync(save.Messages);
        }

        /// <summary>
        /// Creates a new ticket type for a school event asynchronously.
        /// </summary>
        /// <remarks>This method creates a new ticket type based on the provided <paramref name="dto"/>
        /// and saves it  to the repository. The operation will fail if the creation or save process encounters an
        /// error.</remarks>
        /// <param name="dto">The data transfer object containing the details of the ticket type to be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult indicating the
        /// outcome of the operation. If successful, the result  includes a success message; otherwise, it contains
        /// error messages describing the failure.</returns>
        public async Task<IBaseResult> CreateTicketTypeAsync(SchoolEventTicketTypeDto dto, CancellationToken cancellationToken = default)
        {
            var result = await ticketTypeRepo.CreateAsync(new SchoolEventTicketType(dto));
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);
            var saveResult = await ticketTypeRepo.SaveAsync();
            return saveResult.Succeeded
                ? await Result.SuccessAsync("Ticket type successfully created")
                : await Result.FailAsync(saveResult.Messages);
        }

        /// <summary>
        /// Updates an existing ticket type with the provided data.
        /// </summary>
        /// <remarks>This method updates the ticket type in the repository and saves the changes.  Ensure
        /// that the provided <paramref name="dto"/> contains valid data before calling  this method. The operation will
        /// fail if the repository update or save operation  does not succeed.</remarks>
        /// <param name="dto">The data transfer object containing the updated details of the ticket type.  This parameter cannot be <see
        /// langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult"/>
        /// indicating the outcome of the operation. If the update  is successful, the result will include a success
        /// message; otherwise, it will  contain error messages describing the failure.</returns>
        public async Task<IBaseResult> UpdateTicketTypeAsync(SchoolEventTicketTypeDto dto, CancellationToken cancellationToken = default)
        {
            var result = ticketTypeRepo.Update(new SchoolEventTicketType(dto));
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);
            var saveResult = await ticketTypeRepo.SaveAsync();
            return saveResult.Succeeded
                ? await Result.SuccessAsync("Ticket type successfully updated")
                : await Result.FailAsync(saveResult.Messages);
        }

        /// <summary>
        /// Deletes a ticket type with the specified identifier.
        /// </summary>
        /// <remarks>This method attempts to delete the specified ticket type and persist the changes to
        /// the repository. If the deletion or save operation fails, the result will indicate the failure along with the
        /// associated error messages.</remarks>
        /// <param name="ticketTypeId">The unique identifier of the ticket type to delete. Cannot be <see langword="null"/> or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If successful, the result includes a success message;
        /// otherwise, it contains error messages describing the failure.</returns>
        public async Task<IBaseResult> DeleteTicketTypeAsync(string ticketTypeId, CancellationToken cancellationToken = default)
        {
            var result = await ticketTypeRepo.DeleteAsync(ticketTypeId);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);
            var saveResult = await ticketTypeRepo.SaveAsync();
            return saveResult.Succeeded
                ? await Result.SuccessAsync("Ticket type successfully deleted")
                : await Result.FailAsync(saveResult.Messages);
        }
    }
}
