using GroupingModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Modals
{
    /// <summary>
    /// The AddEventToActivityGroupModal component is responsible for displaying a modal dialog
    /// that allows the user to select an activity group to add an event to.
    /// </summary>
    public partial class AddEventToActivityGroupModal
    {
        /// <summary>
        /// Injected HTTP provider for making API calls.
        /// </summary>
        [Inject] public IActivityGroupQueryService ActivityGroupQueryService { get; set; } = null!;

        /// <summary>
        /// The parent category for which the activity groups are to be fetched.
        /// </summary>
        [Parameter] public CategoryViewModel? ParentCategory { get; set; }

        /// <summary>
        /// The MudDialog instance for controlling the modal dialog.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        private bool _loaded;
        private ActivityGroupViewModel? _selectedActivityGroup;
        private IEnumerable<ActivityGroupViewModel> _activityGroups = new List<ActivityGroupViewModel>();

        /// <summary>
        /// Converter function to display the name of the activity group in the dropdown.
        /// </summary>
        readonly Func<ActivityGroupViewModel, string> _activityGroupConverter = p => string.IsNullOrEmpty(p.Name) ? "" : p.Name;

        /// <summary>
        /// Submits the selected activity group and closes the modal dialog.
        /// </summary>
        public void SubmitAsync()
        {
            MudDialog.Close(_selectedActivityGroup);
        }

        /// <summary>
        /// Cancels the operation and closes the modal dialog.
        /// </summary>
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        #region Overrides

        /// <summary>
        /// Initializes the component and loads the activity groups for the specified parent category.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // Fetch the activity groups for the specified parent category
            var categoriesResult = await ActivityGroupQueryService.PagedActivityGroupsAsync(new ActivityGroupPageParameters { CategoryIds = ParentCategory?.CategoryId });

            if (categoriesResult.Succeeded)
            {
                // Convert the fetched activity groups to view models and set the first one as the selected activity group
                _activityGroups = categoriesResult.Data.Select(c => new ActivityGroupViewModel(c));
                _selectedActivityGroup = _activityGroups.FirstOrDefault();
            }

            _loaded = true;
        }

        #endregion
    }
}
