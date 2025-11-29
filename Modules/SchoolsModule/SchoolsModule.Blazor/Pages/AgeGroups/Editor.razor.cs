using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Interfaces;

namespace SchoolsModule.Blazor.Pages.AgeGroups
{
    /// <summary>
    /// Represents an editor component for managing and updating age group information.
    /// </summary>
    /// <remarks>This component provides functionality for retrieving, updating, and navigating between age
    /// group records. It relies on injected services for HTTP requests, dialog display, notifications, and
    /// navigation.</remarks>
    public partial class Editor
    {
        private string _imageSource = "_content/SchoolsModule.Blazor/images/profileImage128x128.png";
        private AgeGroupViewModel _ageGroup = new AgeGroupViewModel();
        private readonly List<BreadcrumbItem> _items = new List<BreadcrumbItem>
        {
            new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new("Age Groups", href: "/agegroups", icon: Icons.Material.Filled.People),
            new("Update Age Group", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
        };

        /// <summary>
        /// Gets or sets the HTTP provider used for making HTTP requests.
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
        /// Gets or sets the identifier for the age group.
        /// </summary>
        [Parameter] public string AgeGroupId { get; set; } = null!;

        /// <summary>
        /// Handles changes to the cover image by updating the image source.
        /// </summary>
        /// <param name="coverImage">The updated cover image, represented as a <see cref="MudCropperResponse"/> containing the image data in
        /// Base64 format.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
        }

        /// <summary>
        /// Updates the current age group by sending the data to the server asynchronously.
        /// </summary>
        /// <remarks>This method sends the age group data to the server using a POST request.  If the
        /// operation is successful, a success message is displayed in the snack bar.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateAsync()
        {
            var result = await AgeGroupService.UpdateAsync(_ageGroup.ToDto());
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                SnackBar.AddSuccess($"{_ageGroup.Name} was updated successfully");
            });
        }

        /// <summary>
        /// Cancels the current operation and navigates to the Age Groups page.
        /// </summary>
        /// <remarks>This method redirects the user to the "/agegroups" route. Ensure that the navigation
        /// context is properly set up and that the target route exists to avoid navigation errors.</remarks>
        public void Cancel()
        {
            NavigationManager.NavigateTo("/agegroups");
        }

        /// <summary>
        /// Asynchronously initializes the component and retrieves the age group data.
        /// </summary>
        /// <remarks>This method fetches the age group details based on the provided <see
        /// cref="AgeGroupId"/>  and initializes the corresponding view model. If the data retrieval fails, error
        /// messages  are displayed using the configured snackbar.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var result = await AgeGroupService.AgeGroupAsync(AgeGroupId);
            if (result.Succeeded)
            {
                _ageGroup = new AgeGroupViewModel(result.Data);
            }
            SnackBar.AddErrors(result.Messages);
            await base.OnInitializedAsync();
        }
    }
}
