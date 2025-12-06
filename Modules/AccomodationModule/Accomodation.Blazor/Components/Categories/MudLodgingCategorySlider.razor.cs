using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Components.Categories
{
    /// <summary>
    /// Represents a slider component for displaying featured lodgings within a specific category.
    /// Allows users to navigate between lodgings and view all lodgings in the category.
    /// </summary>
    public partial class MudLodgingCategorySlider : ComponentBase
    {
        #region Parameters

        /// <summary>
        /// Gets or sets the service responsible for managing lodging-related operations.
        /// </summary>
        [Inject] public ILodgingService LodgingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage URI navigation and location state within the application.
        /// </summary>
        /// <remarks>This property is typically injected by the Blazor framework. It provides methods for
        /// programmatic navigation and for responding to location changes. Do not set this property manually unless
        /// overriding the default navigation behavior.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display transient messages to the user.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Use the
        /// Snackbar service to show notifications, alerts, or brief messages in the application's user
        /// interface.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the category to display lodgings for.
        /// </summary>
        [Parameter, EditorRequired] public string CategoryId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the title of the slider.
        /// </summary>
        [Parameter, EditorRequired] public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets the auto-cycle time for the slider in seconds.
        /// </summary>
        [Parameter] public int AutoCycleTime { get; set; } = 5;

        #endregion

        #region Fields

        /// <summary>
        /// The index of the currently selected lodging in the slider.
        /// </summary>
        private int _selectedIndex;

        /// <summary>
        /// The collection of lodgings to display in the slider.
        /// </summary>
        public ICollection<LodgingViewModel> Lodgings { get; set; } = new List<LodgingViewModel>();

        #endregion

        #region Methods

        /// <summary>
        /// Moves the slider to the previous lodging.
        /// </summary>
        public void MoveToPrevious()
        {
            if (_selectedIndex == 1)
            {
                _selectedIndex = 0;
            }
            else
            {
                _selectedIndex = 1;
            }
        }

        /// <summary>
        /// Navigates to the page displaying all lodgings in the category.
        /// </summary>
        public void ViewAll()
        {
            NavigationManager.NavigateTo($"/accommodation/{CategoryId}");
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Called after the component has rendered. If this is the first render,
        /// it fetches the lodgings for the specified category and updates the slider.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first render of the component.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                try
                {
                    // Fetch featured lodgings for the specified category
                    var result = await LodgingService.PagedLodgingsAsync(new LodgingParameters
                    {
                        PageSize = 100,
                        CategoryIds = string.Join(";", new List<string> { CategoryId }),
                        Featured = true
                    });

                    if (!result.Succeeded)
                    {
                        Snackbar.Add("Error loading lodgings", Severity.Error);
                    }
                    else
                    {
                        foreach (var item in result.Data)
                        {
                            Lodgings.Add(new LodgingViewModel(item));
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Snackbar.Add("Error loading lodgings", Severity.Error);
                }

                StateHasChanged();
            }
        }

        #endregion
    }
}
