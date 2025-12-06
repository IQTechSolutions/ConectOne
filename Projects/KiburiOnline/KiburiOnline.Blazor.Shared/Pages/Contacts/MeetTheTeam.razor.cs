using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Contacts
{
    /// <summary>
    /// Represents a collection of team members and their contact information.
    /// </summary>
    /// <remarks>This class is used to manage and access the contact details of team members.</remarks>
    public partial class MeetTheTeam
    {
        /// <summary>
        /// Represents a collection of contact data transfer objects.
        /// </summary>
        /// <remarks>This collection is used to store and manage contact information within the
        /// application.</remarks>
        private ICollection<ContactDto> _contacts = [];

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display snack bar notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        [Inject] public IContactService ContactService { get; set; } = null!;

        /// <summary>
        /// Asynchronously initializes the component by retrieving and processing contact data.
        /// </summary>
        /// <remarks>This method fetches a collection of contacts from the provider and processes the
        /// response for display. It updates the internal contacts list upon successful retrieval.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var contactsResult = await ContactService.GetAllAsync();
            contactsResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                _contacts = contactsResult.Data;
            });

            await base.OnInitializedAsync();
        }
    }
}
