using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Enums;
using FilingModule.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Blazor.Components.Learners.Tables;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.ActivityGroups
{
    /// <summary>
    /// The Create component is responsible for creating a new Activity Group.
    /// </summary>
    public partial class Create
    {
        private readonly List<BreadcrumbItem> _items = new();
        private LearnersTableV2 _table = null!;
        private readonly ActivityGroupViewModel _activityGroup = new() { ActivityGroupId = Guid.NewGuid().ToString() };
        private bool _multiSelection = true;
        private string _imageSource = "_content/SchoolsModule.Blazor/images/NoImage.jpg";

        private IEnumerable<TeacherViewModel> _teachers = null!;
        private readonly Func<TeacherViewModel, string> teacherConverter = p => p?.FirstName != null ? $"{p?.FirstName} {p?.LastName}" : "";

        #region Injections and Parameters

        /// <summary>
        /// Injected HTTP provider for making API calls.
        /// </summary>
        [Inject] public IActivityGroupQueryService ActivityGroupQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to execute commands related to activity groups.
        /// </summary>
        [Inject] public IActivityGroupCommandService ActivityGroupCommandService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage teacher-related operations.
        /// </summary>
        [Inject] public ITeacherService TeacherService { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        [Inject] public ILearnerQueryService LearnerQueryService { get; set; } = null!;

        /// <summary>
        /// Injected dialog service for displaying dialogs.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Injected snack bar service for displaying notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Injected navigation manager for navigating between pages.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// The ID of the category to which the Activity Group belongs.
        /// </summary>
        [Parameter] public string? CategoryId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the cover image URL when the cover image is changed.
        /// </summary>
        /// <param name="coverImage">The new cover image URL.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
        }

        /// <summary>
        /// Updates the selected age group.
        /// </summary>
        /// <param name="ageGroup">The selected age group.</param>
        public async Task AgeGroupChanged(AgeGroupViewModel ageGroup)
        {
            _activityGroup.SelectedAgeGroup = ageGroup;
            await _table.ReloadData();
        }

        /// <summary>
        /// Updates the selected gender.
        /// </summary>
        /// <param name="gender">The selected gender.</param>
        public async Task GenderChanged(Gender gender)
        {
            _activityGroup.Gender = gender;
            await _table.ReloadData();
        }

        /// <summary>
        /// Updates the selected teacher.
        /// </summary>
        /// <param name="teacher">The selected teacher.</param>
        public void TeacherChanged(TeacherViewModel teacher)
        {
            _activityGroup.SelectedTeacher = teacher;
        }

        /// <summary>
        /// Handles the event when the selection of learners in the table changes.
        /// </summary>
        /// <param name="args">The event arguments containing the selected learner and its checked state.</param>
        private void LearnerTableSelectedItemsChanged(CustomCheckValueChangedEventArgs<LearnerDto> args)
        {
            if (args.IsChecked)
            {
                _activityGroup.SelectedTeamMembers.Add(args.Item);
            }
            else
            {
                _activityGroup.SelectedTeamMembers.Remove(_activityGroup.SelectedTeamMembers.FirstOrDefault(c => c.LearnerId == args.Item.LearnerId));
            }
        }

        /// <summary>
        /// Handles the event when the multi-selection option is changed.
        /// </summary>
        /// <param name="value">The new value of the multi-selection option.</param>
        private async Task OnMultiSelectionChanged(bool value)
        {
            _multiSelection = value;

            if (_multiSelection)
            {
                var tempSelected = _activityGroup.SelectedTeamMembers.ToList();
                foreach (var item in _activityGroup.SelectedTeamMembers.ToList().Where(c => tempSelected.Any(g => g.LearnerId == c.LearnerId)))
                {
                    _activityGroup.SelectedTeamMembers.Add(item);
                }

                await InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// Creates a new Activity Group by sending the data to the server.
        /// </summary>
        private async Task CreateAsync()
        {
            var activityGroupDto = _activityGroup.ToDto();
            var result = await ActivityGroupCommandService.CreateAsync(activityGroupDto);

            if (!result.Succeeded)
            {
                SnackBar.AddErrors(result.Messages);
            }
            else
            {
                var teamAdditionResult = await ActivityGroupCommandService.CreateActivityGroupCategoryAsync(CategoryId, activityGroupDto.ActivityGroupId);
                if (!teamAdditionResult.Succeeded)
                {
                    SnackBar.AddErrors(teamAdditionResult.Messages);
                }

                NavigationManager.NavigateTo($"/activities/categories/update/{CategoryId}");
            }
        }

        /// <summary>
        /// Cancels the creation process and navigates back to the previous page.
        /// </summary>
        private void Cancel()
        {
            NavigationManager.NavigateTo("/activities");
        }

        #endregion

        #region Table Setup

        /// <summary>
        /// Reloads the data for the table from the server.
        /// </summary>
        /// <param name="state">The state of the table.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the table data.</returns>
        private async Task<TableData<LearnerDto>> ServerReload(TableState state, CancellationToken token)
        {
            if (_multiSelection)
            {
                var args = new LearnerPageParameters
                {
                    Gender = _activityGroup.Gender,
                    MaxAge = _activityGroup.SelectedAgeGroup is null ? 100 : _activityGroup.SelectedAgeGroup.MaxAge,
                    MinAge = _activityGroup.SelectedAgeGroup is null ? 0 : _activityGroup.SelectedAgeGroup.MinAge,
                    PageSize = state.PageSize,
                    PageNr = state.Page + 1,
                    OrderBy = !string.IsNullOrEmpty(state.SortLabel)
                        ? $"{state.SortLabel} {(state.SortDirection == SortDirection.Ascending ? "asc" : "desc")}"
                        : string.Empty
                };

                var request = await LearnerQueryService.PagedLearnersAsync(args);

                if (!request.Succeeded)
                {
                    // Handle error
                    return new TableData<LearnerDto> { TotalItems = 0, Items = new List<LearnerDto>() };
                }

                return new TableData<LearnerDto>
                {
                    TotalItems = request.TotalCount,
                    Items = request.Data
                };
            }
            else
            {
                return new TableData<LearnerDto> { TotalItems = _activityGroup.SelectedTeamMembers.Count, Items = _activityGroup.SelectedTeamMembers };
            }
        }

        #endregion

        /// <summary>
        /// Performs a search for teachers whose first or last name contains the specified value.
        /// </summary>
        /// <remarks>The search is case-insensitive and uses <see
        /// cref="StringComparison.InvariantCultureIgnoreCase"/> for comparisons.</remarks>
        /// <param name="value">The search term to match against teacher names. If null or empty, all teachers are returned.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A collection of <see cref="TeacherViewModel"/> objects representing the teachers whose first or last name
        /// matches the search term. If no matches are found, an empty collection is returned.</returns>
        private async Task<IEnumerable<TeacherViewModel>> AutoCompleteSearch(string value, CancellationToken token)
        {
            await Task.Delay(1, token);

            if (string.IsNullOrEmpty(value))
                return _teachers;

            return _teachers.Where(x => x.FirstName.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.LastName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Asynchronously initializes the component by retrieving and processing a list of teachers.
        /// </summary>
        /// <remarks>This method fetches a collection of teachers from the data provider and transforms it
        /// into a list of <see cref="TeacherViewModel"/> instances. It is called during the component's initialization
        /// phase.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var amenityResult = await TeacherService.AllTeachersAsync(default);
            if (amenityResult.Succeeded)
            {
                _teachers = amenityResult.Data.Select(c => new TeacherViewModel(c));
            }
            await base.OnInitializedAsync();
        }
    }
}
