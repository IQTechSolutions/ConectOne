using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Components.AgeGroups
{
    /// <summary>
    /// Component for selecting an Age Group from a dropdown box.
    /// </summary>
    public partial class AgeGroupSelectionDropdownBox : ComponentBase
    {
        /// <summary>
        /// Gets or sets the Age Group provider, which handles Age Group-related operations.
        /// </summary>
        [Inject] public IAgeGroupService AgeGroupService { get; set; } = null!;

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
        /// Gets or sets the selected Age Group.
        /// </summary>
        [Parameter] public AgeGroupViewModel AgeGroup { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Parent ID.
        /// </summary>
        [Parameter] public string ParentId { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether validation is required.
        /// </summary>
        [Parameter] public bool Validate { get; set; }

        /// <summary>
        /// Event callback for when the selected Age Group changes.
        /// </summary>
        [Parameter] public EventCallback<AgeGroupViewModel> AgeGroupChanged { get; set; }

        /// <summary>
        /// Indicates whether the component has finished loading.
        /// </summary>
        private bool _loaded = false;

        /// <summary>
        /// The list of available Age Groups.
        /// </summary>
        private IEnumerable<AgeGroupViewModel> _ageGroups = Array.Empty<AgeGroupViewModel>();

        /// <summary>
        /// Handles the change event for the selected Age Group.
        /// </summary>
        /// <param name="value">The selected Age Group.</param>
        public async Task SelectedValuesChanged(AgeGroupViewModel value)
        {
            AgeGroup = value;
            await AgeGroupChanged.InvokeAsync(value);
        }

        /// <summary>
        /// Gets or sets the variant of the dropdown box.
        /// </summary>
        [Parameter] public Variant Variant { get; set; } = Variant.Text;

        /// <summary>
        /// Converts an Age Group to its string representation.
        /// </summary>
        private readonly Func<AgeGroupViewModel, string> _ageGroupConverter = p => !string.IsNullOrEmpty(p.Name) ? p.Name : "";

        /// <summary>
        /// Initializes the component asynchronously.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var amenityResult = await AgeGroupService.PagedAgeGroupsAsync(new AgeGroupPageParameters { PageSize = 100 });
            if (amenityResult.Succeeded)
            {
                _ageGroups = amenityResult.Data.Select(c => new AgeGroupViewModel(c));

                await base.OnInitializedAsync();
                _loaded = true;
            }
            else
            {
                foreach (var error in amenityResult.Messages)
                {
                    SnackBar.Add(error, Severity.Error);
                }
            }
        }
    }
}
