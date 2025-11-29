using ConectOne.Domain.Extensions;
using ConectOne.Domain.ResultWrappers;
using GroupingModule.Domain.DataTransferObjects;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.StateManagers
{
    /// <summary>
    /// Manages the state of school events, including creating, updating, and managing categories and consents.
    /// </summary>
    public class SchoolEventStateManager
    {
        private readonly ISchoolEventQueryService _schoolEventQueryService;
        private readonly ISchoolEventCommandService _schoolEventCommandService;
        private readonly IActivityGroupQueryService _activityGroupQueryService;
        public bool IsNewEvent { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolEventStateManager"/> class.
        /// </summary>
        /// <param name="provider">The HTTP provider for making API calls.</param>
        public SchoolEventStateManager(ISchoolEventQueryService schoolEventQueryService, ISchoolEventCommandService schoolEventCommandService, IActivityGroupQueryService activityGroupQueryService)
        {
            _schoolEventQueryService = schoolEventQueryService;
            _schoolEventCommandService = schoolEventCommandService;
            _activityGroupQueryService = activityGroupQueryService;
        }

        /// <summary>
        /// Gets or sets the current school event.
        /// </summary>
        public SchoolEventViewModel? SchoolEvent { get; set; }

        /// <summary>
        /// Determines whether there is a current school event.
        /// </summary>
        /// <returns><c>true</c> if there is a current school event; otherwise, <c>false</c>.</returns>
        public bool HasEvent()
        {
            return SchoolEvent != null;
        }

        /// <summary>
        /// Sets the current school event asynchronously.
        /// </summary>
        /// <param name="schoolEventId">The ID of the school event to set. If null, a new event is created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the result of the operation.</returns>
        public async Task<IBaseResult> SetSchoolEventAsync(string? schoolEventId = null)
        {
            if (string.IsNullOrEmpty(schoolEventId))
            {
                SchoolEvent = new SchoolEventViewModel() { EventId = Guid.NewGuid().ToString() };
                IsNewEvent = true;
                return await Result.SuccessAsync("SchoolEvent successfully set");
            }

            var schoolEventResult = await _schoolEventQueryService.SchoolEventAsync(schoolEventId);
            if (!schoolEventResult.Succeeded) return await Result.FailAsync(schoolEventResult.Messages);

            SchoolEvent = new SchoolEventViewModel(schoolEventResult.Data);
            return await Result.SuccessAsync("SchoolEvent successfully set");
        }

        /// <summary>
        /// Sets the category of the current school event.
        /// </summary>
        /// <param name="categoryId">The ID of the category to set. If null, the category is cleared.</param>
        public void SetSchoolEventCategory(string? categoryId = null)
        {
            if (!HasEvent()) return;

            SchoolEvent!.EntityId = categoryId;
            NotifyStateChanged();
        }

        /// <summary>
        /// Creates or updates the current school event asynchronously.
        /// </summary>
        /// <param name="schoolEventId">The ID of the school event to create or update. If null, a new event is created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the result of the operation.</returns>
        public async Task<IBaseResult> CreateSchoolEvent(string? activityGroupCategoryId = null)
        {
            if (!HasEvent()) return await Result.FailAsync("No School Event specified");

            var result = await  _schoolEventCommandService.Create(SchoolEvent.ToDto() with { EntityId = activityGroupCategoryId}, default);
            

            return result;
        }

        /// <summary>
        /// Handles changes to the selected categories asynchronously.
        /// </summary>
        /// <param name="categories">The new set of selected categories.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the result of the operation.</returns>
        public async Task<IBaseResult> SelectedCategoriesChanged(HashSet<CategoryDto> categories)
        {
            if (SchoolEvent is null)
                return await Result.FailAsync("No School Event specified");

            //var tempSelectedActivityGroups = new HashSet<ActivityGroupDto>(SchoolEvent.SelectedActivityGroups);
            //var selectedCategoryIds = categories.Select(c => c.CategoryId).ToHashSet();
            //var currentCategoryIds = SchoolEvent.SelectedActivityCategories.Select(c => c.CategoryId).ToHashSet();

            //// Add new activity groups for newly selected categories
            //foreach (var newCategoryId in selectedCategoryIds.Except(currentCategoryIds))
            //{
            //    var result = await FetchActivityGroupsByCategoryId(newCategoryId);
            //    if (!result.Succeeded)
            //    {
            //        SchoolEvent.SelectedActivityGroups = tempSelectedActivityGroups.ToList();
            //        return await Result.FailAsync(result.Messages);
            //    }

            //    foreach (var group in result.Data)
            //    {
            //        SchoolEvent.SelectedActivityGroups.Add(group);
            //    }
            //}

            //// Remove activity groups for deselected categories
            //foreach (var removedCategoryId in currentCategoryIds.Except(selectedCategoryIds))
            //{
            //    var result = await FetchActivityGroupsByCategoryId(removedCategoryId);
            //    if (!result.Succeeded)
            //    {
            //        SchoolEvent.SelectedActivityGroups = tempSelectedActivityGroups.ToList();
            //        return await Result.FailAsync(result.Messages);
            //    }

            //    foreach (var group in result.Data)
            //    {
            //        var match = SchoolEvent.SelectedActivityGroups.FirstOrDefault(g => g?.ActivityGroupId == group.ActivityGroupId);
            //        if (match != null)
            //        {
            //            SchoolEvent.SelectedActivityGroups.Remove(match);
            //        }
            //    }
            //}

            //SchoolEvent.SelectedActivityCategories = new HashSet<CategoryDto>(categories);
            NotifyStateChanged();

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Retrieves a collection of activity groups associated with the specified category ID.
        /// </summary>
        /// <remarks>This method sends a request to the underlying data provider to retrieve activity
        /// groups filtered by the specified category ID. If the operation fails, the result will include an error
        /// message indicating the failure reason.</remarks>
        /// <param name="categoryId">The unique identifier of the category for which to fetch activity groups. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a collection of <see cref="ActivityGroupDto"/> objects if the operation succeeds, or an error message
        /// if it fails.</returns>
        private async Task<IBaseResult<IEnumerable<ActivityGroupDto>>> FetchActivityGroupsByCategoryId(string categoryId)
        {
            var parameters = new ActivityGroupPageParameters { CategoryIds = categoryId };
            var queryString = parameters.GetQueryString();
            var result = await _activityGroupQueryService.AllActivityGroupsAsync(parameters);

            return result.Succeeded
                ? result
                : await Result<IEnumerable<ActivityGroupDto>>.FailAsync("Failed to fetch activity groups for category " + categoryId);
        }
        
        /// <summary>
        /// Sets the attendance consent requirement for the current school event.
        /// </summary>
        /// <param name="value">If set to <c>true</c>, attendance consent is required; otherwise, it is not required.</param>
        public void SetAttendanceConsentForEvent(bool value)
        {
            if (SchoolEvent is null) return;
            SchoolEvent.AttendanceConsentRequired = value;
            NotifyStateChanged();
        }

        /// <summary>
        /// Occurs when the state of the school event changes.
        /// </summary>
        public event Action? OnNotificationsChanged;

        /// <summary>
        /// Notifies subscribers that the state has changed.
        /// </summary>
        public void NotifyStateChanged() => OnNotificationsChanged?.Invoke();
    }
}
