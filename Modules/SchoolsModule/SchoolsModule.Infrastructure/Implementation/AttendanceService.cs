using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using FilingModule.Domain.Interfaces;
using IdentityModule.Domain.DataTransferObjects;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Enums;
using MessagingModule.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using NeuralTech.Base.Enums;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Enums;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;
using SchoolsModule.Domain.Specifications;

namespace SchoolsModule.Infrastructure.Implementation
{
    /// <summary>
    /// Provides functionality for managing attendance-related operations, including retrieving attendance lists and
    /// creating attendance groups.
    /// </summary>
    /// <remarks>The <see cref="AttendanceService"/> class supports various attendance types, such as class,
    /// activity group, event team, and event transport. It enables retrieval of attendance lists for learners and
    /// creation of attendance records based on provided data. This service interacts with the repository layer to
    /// perform data operations asynchronously.</remarks>
    /// <param name="schoolsModuleRepoManager"></param>
    public class AttendanceService(ISchoolsModuleRepoManager schoolsModuleRepoManager, IExcelService excelService, IPushNotificationService pushNotificationService) : IAttendanceService
    {
        /// <summary>
        /// Retrieves a list of learners requiring attendance completion based on the specified attendance type and
        /// group identifier.
        /// </summary>
        /// <remarks>The method supports multiple attendance types, including class, activity group, event
        /// team, and event transport. The returned attendance list includes default attendance statuses and notes,
        /// which can be modified as needed.</remarks>
        /// <param name="args">The request parameters containing the attendance type and group identifier.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A result containing a list of <see cref="LearnerAttendanceDto"/> objects representing learners and their
        /// attendance details. If no learners are found, the result will indicate success with an empty list.</returns>
        public async Task<IBaseResult<List<LearnerAttendanceDto>>> GetAttendanceListToCompleteAsync(AttendanceListRequest args, CancellationToken cancellationToken = default)
        {
            var attendanceList = new List<LearnerAttendanceDto>();

            if (args.AttendanceType == AttendanceType.Class)
            {
                var spec = new LambdaSpec<Learner>(c => c.SchoolClassId == args.GroupId);
                var result = await schoolsModuleRepoManager.Learners.ListAsync(spec, false, cancellationToken);
                if (!result.Succeeded) return await Result<List<LearnerAttendanceDto>>.FailAsync(result.Messages);

                attendanceList = result.Data.Select(l => new LearnerAttendanceDto
                {
                    LearnerId = l.Id,
                    FullName = l.FirstName + " " + l.LastName,
                    SelectedStatus = AttendanceStatus.Present, // Default status can be set as needed
                    Notes = string.Empty // Default notes can be empty
                }).ToList();
                return await Result<List<LearnerAttendanceDto>>.SuccessAsync(attendanceList);
            }
            if (args.AttendanceType == AttendanceType.ActivityGroup)
            {
                var spec = new LambdaSpec<ActivityGroupTeamMember>(c => c.ActivityGroupId == args.GroupId);
                spec.AddInclude(c => c.Include(c => c.Learner));

                var result = await schoolsModuleRepoManager.ActivityGroupTeamMembers.ListAsync(spec, false, cancellationToken);
                if (!result.Succeeded) return await Result<List<LearnerAttendanceDto>>.FailAsync(result.Messages);

                attendanceList = result.Data.Select(l => new LearnerAttendanceDto
                {
                    LearnerId = l.Id,
                    FullName = l.Learner.FirstName + " " + l.Learner.LastName,
                    SelectedStatus = AttendanceStatus.Present, // Default status can be set as needed
                    Notes = string.Empty // Default notes can be empty
                }).ToList();
                return await Result<List<LearnerAttendanceDto>>.SuccessAsync(attendanceList);
            }
            if (args.AttendanceType == AttendanceType.EventTeam)
            {
                var spec = new LambdaSpec<ParticipatingActitivityGroupTeamMember>(c => c.ParticipatingActitivityGroupId == args.GroupId);
                spec.AddInclude(c => c.Include(c => c.TeamMember));

                var result = await schoolsModuleRepoManager.ParticipatingActivityGroupTeamMembers.ListAsync(spec, false, cancellationToken);
                if (!result.Succeeded) return await Result<List<LearnerAttendanceDto>>.FailAsync(result.Messages);

                attendanceList = result.Data.Select(l => new LearnerAttendanceDto
                {
                    LearnerId = l.Id,
                    FullName = l.TeamMember.FirstName + " " + l.TeamMember.LastName,
                    SelectedStatus = AttendanceStatus.Present, // Default status can be set as needed
                    Notes = string.Empty // Default notes can be empty
                }).ToList();
                return await Result<List<LearnerAttendanceDto>>.SuccessAsync(attendanceList);
            }
            if (args.AttendanceType == AttendanceType.EventTransportTo)
            {
                var spec = new LambdaSpec<ParentPermission>(c => c.ParticipatingActivityGroupId == args.GroupId && c.ConsentType == ConsentTypes.Transport && (c.ConsentDirection == ConsentDirection.To || c.ConsentDirection == ConsentDirection.ToAndFrom));
                spec.AddInclude(c => c.Include(c => c.Learner));

                var result = await schoolsModuleRepoManager.ParentPermissions.ListAsync(spec, false, cancellationToken);
                if (!result.Succeeded) return await Result<List<LearnerAttendanceDto>>.FailAsync(result.Messages);

                foreach (var link in result.Data)
                {
                    var attendanceDto = new LearnerAttendanceDto
                    {
                        LearnerId = link.LearnerId,
                        FullName = link.Learner.FirstName + " " + link.Learner.LastName,
                        SelectedStatus = AttendanceStatus.Present, // Default status can be set as needed
                        Notes = string.Empty // Default notes can be empty
                    };
                    attendanceList.Add(attendanceDto);
                }
                return await Result<List<LearnerAttendanceDto>>.SuccessAsync(attendanceList);
            }
            if (args.AttendanceType == AttendanceType.EventTransportFrom)
            {
                var spec = new LambdaSpec<ParentPermission>(c => c.ParticipatingActivityGroupId == args.GroupId && c.ConsentType == ConsentTypes.Transport && (c.ConsentDirection == ConsentDirection.From || c.ConsentDirection == ConsentDirection.ToAndFrom));
                spec.AddInclude(c => c.Include(c => c.Learner));

                var result = await schoolsModuleRepoManager.ParentPermissions.ListAsync(spec, false, cancellationToken);
                if (!result.Succeeded) return await Result<List<LearnerAttendanceDto>>.FailAsync(result.Messages);

                foreach (var link in result.Data)
                {
                    var attendanceDto = new LearnerAttendanceDto
                    {
                        LearnerId = link.LearnerId,
                        FullName = link.Learner.FirstName + " " + link.Learner.LastName,
                        SelectedStatus = AttendanceStatus.Present, // Default status can be set as needed
                        Notes = string.Empty // Default notes can be empty
                    };
                    attendanceList.Add(attendanceDto);
                }
                return await Result<List<LearnerAttendanceDto>>.SuccessAsync(attendanceList);
            }
            return await Result<List<LearnerAttendanceDto>>.SuccessAsync("No entries found that require attendance check");
        }

        /// <summary>
        /// Creates a new attendance group and saves the associated attendance records asynchronously.
        /// </summary>
        /// <remarks>This method processes the attendance results provided in the <paramref name="args"/>
        /// parameter, creates corresponding attendance records, and saves them to the repository. Ensure that the
        /// <paramref name="args"/> parameter contains valid data, including a non-null list of attendance results and a
        /// valid group identifier.</remarks>
        /// <param name="args">The request object containing the attendance results, group identifier, date, and other relevant details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult"/> representing the outcome of the save operation. The result indicates whether
        /// the operation was successful.</returns>
        public async Task<IBaseResult> CreateAttendanceGroupAsync(AttendanceResultListRequest args, CancellationToken cancellationToken = default)
        {

            var attendanceGroup = new AttendanceGroup()
            {
                Id = Guid.NewGuid().ToString(),
                Date = args.Date,
                Name = $"{args.GroupName}-{args.Date.ToShortDateString()}",
                Type = args.AttendanceType,
                ParentGroupId = args.ParentGroupId,
            };

            await schoolsModuleRepoManager.AttendanceGroups.CreateAsync(attendanceGroup, cancellationToken);

            foreach (var item in args.AttendanceResults)
            {
                var record = new AttendanceRecord()
                {
                    LearnerId = item.LearnerId,
                    GroupId = args.ParentGroupId,
                    Date = args.Date,
                    Status = item.SelectedStatus,
                    Notes = item.Notes
                };

                await schoolsModuleRepoManager.AttendanceRecords.CreateAsync(record, cancellationToken);
            }

            var saveResult = await schoolsModuleRepoManager.AttendanceRecords.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return saveResult;

            var absentLearners = args.AttendanceResults
                .Where(r => r.SelectedStatus != AttendanceStatus.Present)
                .Select(r => r.LearnerId)
                .Distinct()
                .ToList();

            foreach (var learnerId in absentLearners)
            {
                var learnerResult = await schoolsModuleRepoManager.Learners.FirstOrDefaultAsync(new SingleLearnerWithParentDetailsSpecification(learnerId), false, cancellationToken);
                if (!learnerResult.Succeeded || learnerResult.Data == null)
                    continue;

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

                if (recipients.Count == 0)
                    continue;

                var notification = new NotificationDto
                {
                    EntityId = learner.Id,
                    Title = $"{learner.FirstName} {learner.LastName} absent",
                    ShortDescription = $"{learner.FirstName} {learner.LastName} was marked absent on {args.Date.ToShortDateString()}",
                    Message = $"{learner.FirstName} {learner.LastName} was not present on {args.Date.ToShortDateString()}.",
                    MessageType = MessageType.Parent,
                    Created = DateTime.Now,
                    NotificationUrl = $"/learners/{learner.Id}"
                };

                await pushNotificationService.EnqueueNotificationsAsync(recipients, notification);
            }

            return saveResult;
        }

        /// <summary>
        /// Exports attendance group data to an Excel file.
        /// </summary>
        /// <remarks>The exported Excel file includes attendance records with the following columns: "Id",
        /// "FirstName", "LastName", "Status", and "Notes". The sheet name is formatted as "<c>{GroupName} --
        /// {Date}</c>", where <c>{GroupName}</c> and <c>{Date}</c> are derived from the attendance group
        /// data.</remarks>
        /// <param name="request">The request containing the parameters for the attendance group export, including filters and group details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with the file path of the exported Excel file if the operation succeeds, or error messages if it fails.</returns>
        public async Task<IBaseResult<string>> ExportAttendanceGroup(ExportAttendanceRequest request, CancellationToken cancellationToken = default)
        {
            var spec = new ExportAttendanceGroupSpec(request);
            var result = await schoolsModuleRepoManager.AttendanceGroups.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded)
                return await Result<string>.FailAsync(result.Messages);

            var export = await excelService.ExportAsync(result.Data.AttendanceRecords, new Dictionary<string, Func<AttendanceRecord, object>>
                {
                    { "Id", p => p.Id },
                    { "FirstName", p => p.Learner.FirstName },
                    { "LastName", p => p.Learner.LastName },
                    { "Status", p => p.Status.ToString() },
                    { "Notes", p => p.Notes }
                }, sheetName: $"{result.Data.Name} -- {result.Data.Date}");

            return await Result<string>.SuccessAsync(data: export.Data);
        }
    }
}
