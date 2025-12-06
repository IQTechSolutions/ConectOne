using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;

namespace Accomodation.Blazor.Components.Vouchers
{
    /// <summary>
    /// Component for displaying a slider of voucher items on the index page.
    /// </summary>
    public partial class VoucherIndexSlider
    {
        private int _selectedIndex;
        private ICollection<VoucherViewModel> _vouchers = new List<VoucherViewModel>();

        /// <summary>
        /// Gets or sets the service used to manage vouchers.
        /// </summary>
        [Inject] public IVoucherService VoucherService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to navigate to different pages or to access details about the current navigation state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        #region Methods

        /// <summary>
        /// Moves the slider to the previous item.
        /// </summary>
        private void MoveToPrevious()
        {
            _selectedIndex = _selectedIndex == 0 ? 1 : 0;
        }

        /// <summary>
        /// Navigates to the page displaying all vouchers.
        /// </summary>
        private void ViewAll()
        {
            NavigationManager.NavigateTo($"/vouchers");
        }

        /// <summary>
        /// Initializes the component and loads the voucher data.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Executes logic after the component has rendered, with additional behavior for the first render.
        /// </summary>
        /// <remarks>When <paramref name="firstRender"/> is <see langword="true"/>, this method performs
        /// the following actions: <list type="bullet"> <item>Checks the application configuration to determine if a
        /// security token should be added.</item> <item>Fetches a paginated list of vouchers from the provider and
        /// updates the component's state.</item> <item>Triggers a state update to reflect the changes.</item> </list>
        /// Subsequent renders do not execute the initialization logic.</remarks>
        /// <param name="firstRender">A value indicating whether this is the first time the component has rendered. If <see langword="true"/>,
        /// additional initialization logic is performed.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var result = await VoucherService.PagedVouchersAsync(new VoucherPageParameters() { PageSize = 100 });
                if (result.Succeeded)
                {
                    _vouchers = result.Data.Select(c => new VoucherViewModel(c)).ToList();
                }
                StateHasChanged();
            }
        }

        #endregion
    }
}
