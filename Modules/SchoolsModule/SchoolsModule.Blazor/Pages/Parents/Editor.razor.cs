using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Blazor.Components.Learners;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.Interfaces.Parents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.Parents
{
    /// <summary>
    /// A Blazor component for editing an existing parent record.
    /// It displays a form that allows updating basic parent details, 
    /// including the cover image, and can optionally display related 
    /// user account info if the parent has an associated user identity.
    /// </summary>
    public partial class Editor
    {
        #region Injections and Parameters

        /// <summary>
        /// Gets or sets the user service used to manage user-related operations.
        /// </summary>
        [Inject] public IUserService UserService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query parent entities within the application.
        /// </summary>
        [Inject] public IParentQueryService ParentQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage parent command operations.
        /// </summary>
        [Inject] public IParentCommandService ParentCommandService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query learner information.
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
        /// Gets or sets the application configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; }

        /// <summary>
        /// The unique identifier of the parent to be edited. This ID is used 
        /// to fetch the current parent data from the server.
        /// </summary>
        [Parameter] public string ParentId { get; set; } = null!;

        #endregion

        #region Private Members

        /// <summary>
        /// Breadcrumb navigation items, illustrating the path: 
        /// Dashboard -> Parents -> Update Parent.
        /// </summary>
        private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
        {
            new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new("Parents", href: "/parents", icon: Icons.Material.Filled.People),
            new("Update Parent", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
        };

        /// <summary>
        /// Holds the cover image path or URL for the parent. Defaults to a placeholder 
        /// until a custom image is selected.
        /// </summary>
        private string _imageSource = "_content/SchoolsModule.Blazor/images/profileImage128x128.png";

        /// <summary>
        /// Tracks whether multi-selection mode is enabled for linking learners to the parent.
        /// When true, a server-side table can be displayed for selecting multiple learners.
        /// </summary>
        private bool _multiSelection;

        /// <summary>
        /// A set of learner DTOs representing currently selected learners in multi-selection mode.
        /// Used to associate them with the parent if needed.
        /// </summary>
        private HashSet<LearnerDto> _selectedPagedLearners = new();

        /// <summary>
        /// The view model containing editable data about the parent. 
        /// Updated via form fields and posted to the server on submit.
        /// </summary>
        private ParentViewModel _parent = new ParentViewModel();

        private LearnerPageParameters _learnerPageParameters = new LearnerPageParameters();

        /// <summary>
        /// Contains user account details, if the parent has a linked account. 
        /// Populated after fetching user info from <see cref="IAccountsProvider"/>.
        /// </summary>
        private UserInfoDto? _userInfo;

        #endregion

        #region Methods

        /// <summary>
        /// Called when the user selects or updates the parent's cover image. 
        /// Updates both the local <see cref="_imageSource"/> and 
        /// the parent's <see cref="ParentViewModel.CoverImageUrl"/>.
        /// </summary>
        /// <param name="coverImage">The new image path or base64 string.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
        }

        /// <summary>
        /// Submits the updated parent data to the server using a POST request. 
        /// If successful, navigates back to the parent list page.
        /// </summary>
        public async Task UpdateAsync()
        {
            // Convert the selected learners into the parent's Dependants list.
            _parent.Dependants = _selectedPagedLearners.ToList();

            var parentResponse = await ParentCommandService.UpdateAsync(_parent.ToDto());
            parentResponse.ProcessResponseForDisplay(SnackBar, () =>
            {
                SnackBar.AddSuccess("Parent was updated successfully");
                NavigationManager.NavigateTo("parents");
            });
        }

        /// <summary>
        /// Handles the search event for learners.
        /// </summary>
        /// <param name="text">The search text.</param>
        private async void OnLearnerSearh(string text)
        {
            _learnerPageParameters.SearchText = text;
        }

        /// <summary>
        /// If multi-selection is disabled, returns the selected learners as the entire data set,
        /// meaning the user manually manages them. 
        /// If enabled, fetches learners from the server with paging/sorting 
        /// to display in a table for selection.
        /// </summary>
        /// <param name="state">Represents the table state (page, sort, etc.).</param>
        /// <param name="token">A cancellation token for async tasks.</param>
        /// <returns>A <see cref="TableData{LearnerDto}"/> object for the table display.</returns>
        private async Task<TableData<LearnerDto>> ServerReload(TableState state, CancellationToken token)
        {
            if (!_multiSelection)
            {
                // No server call, simply display the _selectedPagedLearners as the data set.
                return new TableData<LearnerDto>
                {
                    TotalItems = _selectedPagedLearners.Count,
                    Items = _selectedPagedLearners
                };
            }

            // If multi-selection is on, fetch learners from the server, applying the table's paging/sorting.
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                var sortDirection = state.SortDirection switch
                {
                    SortDirection.Ascending => "asc",
                    SortDirection.Descending => "desc",
                    _ => string.Empty
                };

                _learnerPageParameters.OrderBy = state.SortDirection != SortDirection.None
                    ? $"{state.SortLabel} {sortDirection}"
                    : string.Empty;
            }

            _learnerPageParameters.PageSize = state.PageSize;
            _learnerPageParameters.PageNr = state.Page + 1;

            var request = await LearnerQueryService.PagedLearnersAsync(_learnerPageParameters);

            if (!request.Succeeded)
            {
                // Display any error messages if the request fails
                SnackBar.AddErrors(request.Messages);
                return new TableData<LearnerDto> { TotalItems = 0, Items = new List<LearnerDto>() };
            }

            if (_selectedPagedLearners.Any())
            {
                var tempSelectedParents = _selectedPagedLearners.ToList();
                foreach (var item in request.Data.Where(c => tempSelectedParents.Any(g => g.LearnerId == c.LearnerId)))
                {
                    _selectedPagedLearners.Remove(_selectedPagedLearners.FirstOrDefault(c => c.LearnerId == item.LearnerId)!);
                    _selectedPagedLearners.Add(item);
                }
            }

            return new TableData<LearnerDto>
            {
                TotalItems = request.TotalCount,
                Items = request.Data
            };
        }

        /// <summary>
        /// Callback for when a new learner is created (via a modal or another component).
        /// Adds the newly created learner to the <see cref="_selectedPagedLearners"/> collection 
        /// if multi-selection linking is being used.
        /// </summary>
        /// <param name="learner">The newly created learner's <see cref="LearnerViewModel"/>.</param>
        private async Task OnLearnerCreated(LearnerViewModel learner)
        {
            _selectedPagedLearners.Add(learner.ToDto());

            var parentLearnerCreationResult = await ParentCommandService.CreateParentLearnerAsync(ParentId, learner.LearnerId);
            if (!parentLearnerCreationResult.Succeeded) SnackBar.AddErrors(parentLearnerCreationResult.Messages);
        }

        /// <summary>
        /// Callback for removing a learner from the selected set if needed.
        /// E.g., if the user wants to unlink a learner from this parent.
        /// </summary>
        /// <param name="learnerId">The unique ID of the learner to remove.</param>
        private async Task OnLearnerRemoved(string learnerId)
        {
            var removalResult = await ParentCommandService.RemoveParentLearnerAsync(_parent.ParentId, learnerId);
            removalResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                SnackBar.AddSuccess("Learner removed successfully.");
                _selectedPagedLearners.Remove(_selectedPagedLearners.FirstOrDefault(c => c.LearnerId == learnerId)!);
            });
        }

        /// <summary>
        /// Configures multi-selection mode for learner linking. 
        /// Toggled by UI elements or parent components.
        /// </summary>
        /// <param name="value">True to enable multi-selection mode, false otherwise.</param>
        private void OnMultiSelectionChanged(bool value)
        {
            _multiSelection = value;
        }

        private void OnLearnerSelectionChanged(LearnerSelectionChangedEventArgs value)
        {
            
        }

        /// <summary>
        /// Cancels the edit, returning to the parent listing page without saving any changes.
        /// </summary>
        public void Cancel()
        {
            NavigationManager.NavigateTo("parents");
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Lifecycle method that runs during component initialization. 
        /// Fetches existing parent details (via <see cref="ParentId"/>) and, if successful, 
        /// populates <see cref="_parent"/>. Also attempts to fetch user account info 
        /// if the parent has an associated user account.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var result = await ParentQueryService.ParentAsync(ParentId, false);
            if (result.Succeeded)
            {
                _parent = new ParentViewModel(result.Data);

                _imageSource = $"{Configuration["ApiConfiguration:BaseApiAddress"]}\\{_parent.CoverImageUrl?.TrimStart('/')}";

                var userInfoResult = await UserService.GetUserInfoAsync(ParentId);
                if (userInfoResult is { Succeeded: true, Data: not null })
                {
                    _userInfo = userInfoResult.Data;
                }

                var learnerParents = await ParentQueryService.ParentLearnersAsync(ParentId);
                if (learnerParents.Succeeded)
                    _selectedPagedLearners = learnerParents.Data!.ToHashSet();
            }

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
