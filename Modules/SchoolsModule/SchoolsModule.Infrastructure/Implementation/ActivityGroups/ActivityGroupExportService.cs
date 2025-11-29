using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using FilingModule.Domain.Interfaces;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.ActivityGroups;

namespace SchoolsModule.Infrastructure.Implementation.ActivityGroups
{
    /// <summary>
    /// Provides services for exporting activity group data to Excel files.
    /// </summary>
    /// <param name="schoolsModuleRepoManager">The repository manager for accessing school-related data.</param>
    /// <param name="excelService">The Excel service used to generate export files.</param>
    public class ActivityGroupExportService(ISchoolsModuleRepoManager schoolsModuleRepoManager, IExcelService excelService) : IActivityGroupExportService
    {
        /// <summary>
        /// Exports learner information for a specific activity group to an Excel file.
        /// </summary>
        /// <param name="activityGroupId">The identifier of the activity group to export.</param>
        /// <param name="cancellationToken">An optional token for cancelling the asynchronous operation.</param>
        /// <returns>A base64-encoded string representing the generated Excel file, wrapped in a result object.</returns>
        public async Task<IBaseResult<string>> ExportActivityGroup(string activityGroupId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ActivityGroupTeamMember>(c => c.ActivityGroupId == activityGroupId);
            spec.AddInclude(c => c.Include(c => c.ActivityGroup).ThenInclude(c => c.TeamMembers).ThenInclude(c => c.Learner).ThenInclude(c => c.SchoolGrade));
            spec.AddInclude(c => c.Include(c => c.ActivityGroup).ThenInclude(c => c.Categories).ThenInclude(c => c.Category));

            var result = await schoolsModuleRepoManager.ActivityGroupTeamMembers.ListAsync(spec, true, cancellationToken);
            if (!result.Succeeded) return await Result<string>.FailAsync(result.Messages);

            List<EventConsentDto> exportLearnerList = [];
            foreach (var activityGroup in result.Data)
            {
                if (exportLearnerList.All(c => c.Id != activityGroup.LearnerId))
                    exportLearnerList.Add(new EventConsentDto(
                        activityGroup.Learner,
                        "true",
                        $"{activityGroup.ActivityGroup.Categories.FirstOrDefault()?.Category.Name} {activityGroup.ActivityGroup.Name}"));
            }

            var data = await excelService.ExportAsync(exportLearnerList, new Dictionary<string, Func<EventConsentDto, object>>
        {
            { "Learner", item => item.Learner },
            { "Grade", item => item.Grade },
            { "Team", item => item.Team }
        });

            return await Result<string>.SuccessAsync(data : data.Data);
        }
    }

}
