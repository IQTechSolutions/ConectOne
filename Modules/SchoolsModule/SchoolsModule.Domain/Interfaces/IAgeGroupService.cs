using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Interfaces
{
    /// <summary>
    /// Service interface for managing age groups.
    /// </summary>
    public interface IAgeGroupService
    {
        /// <summary>
        /// Retrieves all age groups from the data store.
        /// </summary>
        Task<IBaseResult<IEnumerable<AgeGroupDto>>> AllAgeGroupsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of age groups.
        /// </summary>
        Task<PaginatedResult<AgeGroupDto>> PagedAgeGroupsAsync(AgeGroupPageParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single age group by ID.
        /// </summary>
        Task<IBaseResult<AgeGroupDto>> AgeGroupAsync(string ageGroupId, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new age group.
        /// </summary>
        Task<IBaseResult> CreateAsync(AgeGroupDto ageGroup, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing age group.
        /// </summary>
        Task<IBaseResult> UpdateAsync(AgeGroupDto ageGroup, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an age group and associated notifications and messages.
        /// </summary>
        Task<IBaseResult> DeleteAsync(string ageGroupId, CancellationToken cancellationToken = default);
    }
}
