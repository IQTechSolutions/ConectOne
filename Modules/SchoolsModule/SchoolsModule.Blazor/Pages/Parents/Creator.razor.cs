using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.Parents
{
    /// <summary>
    /// A Blazor component for creating a new parent record in the Schools Module.
    /// Provides a form for basic parent details (cover image, name, etc.), as well as
    /// an optional mechanism for linking learners to this parent via a multi-selection table.
    /// 
    /// <para>
    /// Once the user completes and submits the form, a PUT request is sent to the server 
    /// to create the parent record. If successful, the component navigates back to 
    /// a listing of all parents.
    /// </para>
    /// </summary>
    public partial class Creator
    {
        #region Injected Services

        /// <summary>
        /// Gets or sets the service used to query learner information.
        /// </summary>
        [Inject] public ILearnerQueryService LearnerQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the parent command service used to coordinate command execution across related components.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Assigning
        /// a value manually is not recommended unless overriding the default service behavior.</remarks>
        [Inject] public IParentService ParentService { get; set; } = null!;

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

        #endregion

        #region Parameters

        /// <summary>
        /// An optional parameter representing a parent identifier if extra context is required 
        /// (e.g., editing or referencing a specific parent). Not strictly necessary for creation logic, 
        /// but available if needed.
        /// </summary>
        [Parameter] public string ParentId { get; set; } = null!;

        #endregion

        #region Breadcrumb

        /// <summary>
        /// A list of breadcrumb items showing the navigation path to this page:
        /// "Dashboard" -> "Parents" -> "Update Parent".
        /// For UI display in a MudBlazor Breadcrumb component.
        /// </summary>
        private readonly List<BreadcrumbItem> _items =
        [
            new ("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new ("Parents", href: "/parents", icon: Icons.Material.Filled.People),
            new ("Update Parent", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
        ];

        #endregion

        #region Fields

        /// <summary>
        /// Holds the path or data URL for the parent's cover image.
        /// Defaults to a placeholder until the user selects or uploads a custom image.
        /// </summary>
        private string _imageSource = "_content/SchoolsModule.Blazor/images/profileImage128x128.png";

        /// <summary>
        /// A view model capturing parent details (e.g., name, cover image, etc.).
        /// This data is serialized and sent to the server upon creation.
        /// </summary>
        private readonly ParentViewModel _parent = new();

        /// <summary>
        /// Tracks whether multi-selection mode is enabled for linking learners to the parent.
        /// When true, a server-side table can be displayed for selecting multiple learners.
        /// </summary>
        private bool _multiSelection;

        /// <summary>
        /// A set of learner DTOs representing currently selected learners in multi-selection mode.
        /// Used to associate them with the parent if needed.
        /// </summary>
        private readonly HashSet<LearnerDto> _selectedPagedLearners = [];

        /// <summary>
        /// Represents the parameters used to configure a learner page.
        /// </summary>
        /// <remarks>This field holds an instance of <see cref="LearnerPageParameters"/> that defines the
        /// settings or options for the learner page. It is intended for internal use and should be properly initialized
        /// before accessing.</remarks>
        private LearnerPageParameters args = new LearnerPageParameters();

        #endregion

        #region Image Handling

        /// <summary>
        /// Called when a new cover image is chosen or uploaded for the parent.
        /// Updates both <see cref="_imageSource"/> and <see cref="ParentViewModel.CoverImageUrl"/> 
        /// to reflect the new image.
        /// </summary>
        /// <param name="coverImage">The new cover image path or base64 data.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
        }

        #endregion

        #region Learner Linking Logic

        /// <summary>
        /// Callback for when a new learner is created (via a modal or another component).
        /// Adds the newly created learner to the <see cref="_selectedPagedLearners"/> collection 
        /// if multi-selection linking is being used.
        /// </summary>
        /// <param name="learner">The newly created learner's <see cref="LearnerViewModel"/>.</param>
        private void OnLearnerCreated(LearnerViewModel learner)
        {
            _selectedPagedLearners.Add(learner.ToDto());
        }

        /// <summary>
        /// Callback for removing a learner from the selected set if needed.
        /// E.g., if the user wants to unlink a learner from this parent.
        /// </summary>
        /// <param name="learnerId">The unique ID of the learner to remove.</param>
        private void OnLearnerRemoved(string learnerId)
        {
            _selectedPagedLearners.Remove(_selectedPagedLearners.FirstOrDefault(c => c.LearnerId == learnerId)!);
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
            // If multi-selection is disabled, just show the locally selected set.
            if (!_multiSelection)
            {
                return new TableData<LearnerDto>
                {
                    TotalItems = _selectedPagedLearners.Count,
                    Items = _selectedPagedLearners
                };
            }

            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                var sortDirection = state.SortDirection switch
                {
                    SortDirection.Ascending => "asc",
                    SortDirection.Descending => "desc",
                    _ => string.Empty
                };

                args.OrderBy = state.SortDirection != SortDirection.None
                    ? $"{state.SortLabel} {sortDirection}"
                    : string.Empty;
            }

            args.PageSize = state.PageSize;
            args.PageNr = state.Page + 1;

            // Perform the server call to retrieve paged learners.
            var request = await LearnerQueryService.PagedLearnersAsync(args);

            // Display error messages if the request fails.
            if (!request.Succeeded)
            {
                SnackBar.AddErrors(request.Messages);
                return new TableData<LearnerDto>
                {
                    TotalItems = 0,
                    Items = new List<LearnerDto>()
                };
            }

            // Re-sync local selections with the newly fetched data items.
            foreach (var item in request.Data)
            {
                if (_selectedPagedLearners.Any(c => c.LearnerId == item.LearnerId))
                {
                    _selectedPagedLearners.RemoveWhere(c => c.LearnerId == item.LearnerId);
                    _selectedPagedLearners.Add(item);
                }
            }

            // Return the table data (paged learners).
            return new TableData<LearnerDto>
            {
                TotalItems = request.TotalCount,
                Items = request.Data
            };
        }

        /// <summary>
        /// Handles the search event for learners.
        /// </summary>
        /// <param name="text">The search text.</param>
        private async void OnLearnerSearh(string text)
        {
            args.SearchText = text;
        }

        #endregion

        #region Form Actions

        /// <summary>
        /// Sends a PUT request to create a new parent record on the server.
        /// If successful, navigates to the "parents" listing page.
        /// Displays any error or success messages using the shared SnackBar.
        /// </summary>
        private async Task CreateAsync()
        {
            // Convert the selected learners into the parent's Dependants list.
            _parent.Dependants = _selectedPagedLearners.ToList();

            // Map the parent view model to a ParentDto and send a PUT request to create.
            var parentResponse = await ParentService.CreateAsync(_parent.ToDto());

            // Process the server's response. If successful, navigate to the parent listing.
            parentResponse.ProcessResponseForDisplay(SnackBar, () =>
            {
                NavigationManager.NavigateTo("parents");
            });
        }

        /// <summary>
        /// Cancels the creation process and navigates back to the parents listing,
        /// discarding any changes.
        /// </summary>
        private void Cancel()
        {
            NavigationManager.NavigateTo("/parents");
        }

        #endregion
    }
}
