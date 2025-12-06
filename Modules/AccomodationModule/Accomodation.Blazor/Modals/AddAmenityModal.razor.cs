using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// Modal component for adding or updating an amenity.
    /// </summary>
    public partial class AddAmenityModal
    {
        #region Cascading Parameters

        /// <summary>
        /// Gets or sets the MudBlazor dialog instance.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing amenities.
        /// </summary>
        [Inject] public IAmenityService AmenityService { get; set; }

        /// <summary>
        /// Gets or sets the service used to display transient notification messages to the user.
        /// </summary>
        /// <remarks>The injected ISnackbar service provides methods for showing snack bar notifications,
        /// such as alerts or status messages, within the application's user interface. This property is typically set
        /// by the dependency injection framework.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; }

        #endregion

        #region Parameters

        /// <summary>
        /// Gets or sets the ID of the amenity to be edited. If null, a new amenity will be created.
        /// </summary>
        [Parameter] public int? AmenityId { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the view model for the amenity.
        /// </summary>
        public AmenityModalViewModel Amenity { get; set; } = new AmenityModalViewModel();

        #endregion

        #region Methods

        /// <summary>
        /// Saves the amenity. If AmenityId is null, a new amenity is created; otherwise, the existing amenity is updated.
        /// </summary>
        private async Task SaveAsync()
        {
            IBaseResult<AmenityDto> result;

            if (AmenityId == null)
            {
                result = await AmenityService.CreateAmenity(Amenity.ToDto());
            }
            else
            {
                result = await AmenityService.UpdateAmenity(Amenity.ToDto());
            }

            if (result.Succeeded)
            {
                Snackbar.Add("Document was updated successfully", Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var error in result.Messages)
                {
                    Snackbar.Add(error, Severity.Error);
                }
                MudDialog.Cancel();
            }
        }

        /// <summary>
        /// Cancels the operation and closes the dialog.
        /// </summary>
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        /// <summary>
        /// Initializes the component. If AmenityId is not null, loads the existing amenity data.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            if (AmenityId != null)
            {
                var result = await AmenityService.AmenityAsync(AmenityId.Value);
                if (result.Succeeded)
                {
                    Amenity = new AmenityModalViewModel(result.Data);
                }
                else
                {
                    foreach (var message in result.Messages)
                    {
                        Snackbar.Add(message, Severity.Error);
                    }
                }
            }

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
