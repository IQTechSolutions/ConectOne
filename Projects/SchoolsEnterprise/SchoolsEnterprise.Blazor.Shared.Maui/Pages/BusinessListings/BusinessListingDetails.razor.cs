using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Interfaces;
using BusinessModule.Domain.RequestFeatures;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.BusinessListings
{
    /// <summary>
    /// Represents a business directory component that manages navigation and category data retrieval.
    /// </summary>
    /// <remarks>This class is designed to interact with a backend service to retrieve paginated category data
    /// and provides functionality for navigating to specific pages within the application.</remarks>
    public partial class BusinessListingDetails
    {
        private BusinessListingDto _listing;
        private bool _isSubmitting;

        /// <summary>
        /// Gets or sets the service used to query the business directory.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection. Ensure that a valid
        /// implementation is provided before using this property.</remarks>
        [Inject] public IBusinessDirectoryQueryService BusinessDirectoryQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the command service used for business directory operations.
        /// </summary>
        [Inject] public IBusinessDirectoryCommandService BusinessDirectoryCommandService { get; set; } = null!;

        [Inject] public IConfiguration Configuration { get; set; } = null!;

        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the component.
        /// </summary>
        [Parameter] public string Id { get; set; }

        /// <summary>
        /// Gets or sets the full name of the individual.
        /// </summary>
        private string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address associated with the entity.
        /// </summary>
        private string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the message associated with the current instance.
        /// </summary>
        private string Message { get; set; } = string.Empty;

        /// <summary>
        /// Asynchronously initializes the component by retrieving a paged list of categories and handling the response.
        /// </summary>
        /// <remarks>This method fetches a paged list of categories from the specified provider and
        /// updates the local category data.  If the operation fails, error messages are displayed using the configured
        /// snackbar.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var result = await BusinessDirectoryQueryService.ListingAsync(Id);
            if (!result.Succeeded)
            {
                SnackBar.AddErrors(result.Messages);
            }
            _listing = result.Data;

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Sends the enquiry to the listing owner via the backend API.
        /// </summary>
        private async Task SendMessageAsync()
        {
            if (_listing is null)
                return;

            if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Message))
            {
                SnackBar.Add("Please provide your name, email address, and a message before sending.", Severity.Warning);
                return;
            }

            if (_isSubmitting)
                return;

            _isSubmitting = true;

            try
            {
                var request = new ListingContactRequest
                {
                    ListingId = _listing.Id,
                    FullName = FullName.Trim(),
                    Email = Email.Trim(),
                    Message = Message.Trim()
                };

                var result = await BusinessDirectoryCommandService.ContactListingOwnerAsync(request);
                if (!result.Succeeded)
                {
                    SnackBar.AddErrors(result.Messages);
                    return;
                }

                var successMessage = result.Messages.FirstOrDefault() ?? "Your enquiry was sent successfully.";
                SnackBar.AddSuccess(successMessage);

                if (result.Messages.Count > 1)
                {
                    for (var i = 1; i < result.Messages.Count; i++)
                    {
                        SnackBar.Add(result.Messages[i], Severity.Warning);
                    }
                }

                FullName = string.Empty;
                Email = string.Empty;
                Message = string.Empty;
            }
            finally
            {
                _isSubmitting = false;
            }
        }
    }
}
