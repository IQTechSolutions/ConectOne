using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Infrastructure.Implementation.Learners
{
    /// <summary>
    /// Service for importing learners and updating their associated parent, grade, and class data.
    /// </summary>
    /// <param name="schoolsModuleRepoManager">Repository manager for accessing learner-related entities.</param>
    public class LearnerExportService(ISchoolsModuleRepoManager schoolsModuleRepoManager) : ILearnerExportService
    {
        /// <summary>
        /// Imports new learners and their associated parents into the system.
        /// Skips learners already existing by ID number and links parents by ID number or creates them if missing.
        /// </summary>
        /// <param name="request">Contains a list of learners and their associated parents to be imported.</param>
        /// <param name="cancellationToken">Optional token to cancel the asynchronous operation.</param>
        /// <returns>A result indicating whether the import succeeded and detailing any errors.</returns>
        public async Task<IBaseResult> ImportNewLearnersAndParents(SaveImportedLearnersRequest request, CancellationToken cancellationToken = default)
        {
            var errorList = new List<string>();

            // Load existing learners to skip duplicates
            var existingLearnersResult = await schoolsModuleRepoManager.Learners.ListAsync(trackChanges: false, cancellationToken);
            if (!existingLearnersResult.Succeeded)
                return await Result.FailAsync(existingLearnersResult.Messages);

            var existingLearnerIdNumbers = existingLearnersResult.Data.Select(l => l.IdNumber).ToHashSet();

            // Preload grade and class mappings
            var gradesResult = await schoolsModuleRepoManager.SchoolGrades.ListAsync(trackChanges: false, cancellationToken);
            var classesResult = await schoolsModuleRepoManager.SchoolClasses.ListAsync(trackChanges: false, cancellationToken);

            var gradeMap = gradesResult.Succeeded
                ? gradesResult.Data.ToDictionary(g => g.Id, g => g.Id)
                : new Dictionary<string, string>();

            var classMap = classesResult.Succeeded
                ? classesResult.Data.ToDictionary(c => c.Name, c => c.Id)
                : new Dictionary<string, string>();

            // Import each learner
            foreach (var importL in request.Learners)
            {
                if (existingLearnerIdNumbers.Contains(importL.IdNumber))
                    continue;

                var newLearner = importL.CreateLearner();

                if (!string.IsNullOrEmpty(importL.Grade) && gradeMap.TryGetValue(importL.Grade, out var gradeId))
                    newLearner.SchoolGradeId = gradeId;

                if (!string.IsNullOrEmpty(importL.Class) && classMap.TryGetValue(importL.Class, out var classId))
                    newLearner.SchoolClassId = classId;

                var createResult = await schoolsModuleRepoManager.Learners.CreateAsync(newLearner, cancellationToken);
                if (!createResult.Succeeded)
                {
                    errorList.AddRange(createResult.Messages);
                    continue;
                }

                // Link or create parents
                foreach (var parentDto in importL.Parents)
                {
                    var parentResult = await schoolsModuleRepoManager.Parents
                        .FirstOrDefaultAsync(new LambdaSpec<Parent>(p => p.ParentIdNumber == parentDto.IdNumber), false, cancellationToken);

                    string parentId;
                    if (parentResult.Succeeded && parentResult.Data is not null)
                    {
                        parentId = parentResult.Data.Id;
                    }
                    else
                    {
                        var createParent = await schoolsModuleRepoManager.Parents.CreateAsync(parentDto.CreateParent(), cancellationToken);
                        if (!createParent.Succeeded)
                        {
                            errorList.AddRange(createParent.Messages);
                            continue;
                        }
                        parentId = createParent.Data.Id;
                    }

                    var learnerParentResult = await schoolsModuleRepoManager.LearnerParents.CreateAsync(
                        new LearnerParent(newLearner.Id, parentId), cancellationToken);

                    if (!learnerParentResult.Succeeded)
                        errorList.AddRange(learnerParentResult.Messages);
                }
            }

            if (errorList.Any())
                return await Result.FailAsync(errorList);

            var saveResult = await schoolsModuleRepoManager.Learners.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync("Learners and Parents imported successfully");
        }

        /// <summary>
        /// Updates existing learners' grade and class information using provided mappings.
        /// </summary>
        /// <param name="request">Contains a list of learner ID numbers and their new grade/class assignments.</param>
        /// <param name="cancellationToken">Optional token to cancel the asynchronous operation.</param>
        /// <returns>A result indicating success or failure of the update operation.</returns>
        public async Task<IBaseResult> ImportNewLearnersGradesById(SaveImportedLearnerGradesRequest request, CancellationToken cancellationToken = default)
        {
            var errorList = new List<string>();

            // Load all learners by ID number
            var learnersResult = await schoolsModuleRepoManager.Learners.ListAsync(trackChanges: true, cancellationToken);
            if (!learnersResult.Succeeded)
                return await Result.FailAsync(learnersResult.Messages);

            var allLearners = learnersResult.Data.ToDictionary(l => l.IdNumber, StringComparer.OrdinalIgnoreCase);

            // Load available classes
            var classesResult = await schoolsModuleRepoManager.SchoolClasses.ListAsync(trackChanges: false, cancellationToken);
            var classMap = classesResult.Succeeded
                ? classesResult.Data.ToDictionary(c => c.Name.Trim().ToLower(), c => c.Id)
                : new Dictionary<string, string>();

            // Update learners
            foreach (var learner in request.Learners)
            {
                if (!allLearners.TryGetValue(learner.IDNumber, out var existingLearner))
                {
                    errorList.Add($"Learner with IDNumber '{learner.IDNumber}' not found.");
                    continue;
                }

                var updated = false;

                if (existingLearner.SchoolGradeId != learner.Grade)
                {
                    existingLearner.SchoolGradeId = learner.Grade;
                    updated = true;
                }

                var classKey = learner.Class.Trim().ToLower();
                if (existingLearner.SchoolClassId == null ||
                    !classMap.TryGetValue(classKey, out var classId) ||
                    existingLearner.SchoolClassId != classId)
                {
                    if (classMap.TryGetValue(classKey, out classId))
                    {
                        existingLearner.SchoolClassId = classId;
                        updated = true;
                    }
                    else
                    {
                        errorList.Add($"Class '{learner.Class}' not found for learner {learner.IDNumber}.");
                    }
                }

                if (updated)
                {
                    schoolsModuleRepoManager.Learners.Update(existingLearner);
                }
            }

            var saveResult = await schoolsModuleRepoManager.Learners.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded || errorList.Any())
                return await Result.FailAsync(errorList.Concat(saveResult.Messages).ToList());

            return await Result.SuccessAsync("Learners Grades and Classes imported successfully");
        }
    }
}
