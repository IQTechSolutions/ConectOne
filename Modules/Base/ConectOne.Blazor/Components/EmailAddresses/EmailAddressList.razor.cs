using ConectOne.Application.ViewModels;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.Interfaces;
using ConectOne.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace ConectOne.Blazor.Components.EmailAddresses
{
    /// <summary>
    /// A Blazor component that manages a list of email addresses associated 
    /// with a parent entity (identified by <see cref="ParentId"/>). 
    /// Users can create, update, or delete email addresses. 
    /// The component relies on the server's REST API for data operations, 
    /// supported by a <see cref="IBaseHttpProvider"/> for making requests.
    /// </summary>
    public partial class EmailAddressList<TEmailAddressOwner>
    {
        #region Parameters

        /// <summary>
        /// Holds the collection of email addresses retrieved from the server.
        /// Each item is mapped to <see cref="EmailAddressViewModel"/> for display or editing in the UI.
        /// If null, the component will fetch them from the API on initialization.
        /// </summary>
        [Parameter] public List<EmailAddressViewModel>? EmailAddresses { get; set; }

        /// <summary>
        /// The unique identifier of the parent entity (e.g., user or customer) 
        /// owning these email addresses. Used to build API request paths.
        /// </summary>
        [Parameter] public string ParentId { get; set; } = null!;

        /// <summary>
        /// A partial URL for the API controller that handles email address operations. 
        /// For instance, "api/users" or "api/customers".
        /// </summary>
        [Parameter] public string ControllerPartUrl { get; set; } = null!;

        /// <summary>
        /// An event callback fired after a user successfully updates an email address.
        /// The updated <see cref="EmailAddressViewModel"/> is passed as an argument.
        /// </summary>
        [Parameter] public EventCallback<EmailAddressViewModel> OnUpdated { get; set; }

        /// <summary>
        /// An event callback fired after a user successfully creates a new email address.
        /// The new <see cref="EmailAddressViewModel"/> is passed as an argument.
        /// </summary>
        [Parameter] public EventCallback<EmailAddressViewModel> OnCreate { get; set; }

        /// <summary>
        /// An event callback fired after a user successfully deletes an email address.
        /// The <c>Id</c> of the deleted email address is passed as an argument.
        /// </summary>
        [Parameter] public EventCallback<string> OnDelete { get; set; }

        #endregion

        #region Injected Services

        /// <summary>
        /// An HTTP provider for making API requests (GET, PUT, POST, DELETE).
        /// </summary>
        [Inject] public IContactInfoService<TEmailAddressOwner> Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>This property is typically populated by dependency injection and provides access to
        /// configuration values such as connection strings, application settings, and environment variables.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        #endregion

        #region Delete

        /// <summary>
        /// Opens a confirmation dialog to remove a specified email address. 
        /// If confirmed, issues a DELETE request and refreshes the list.
        /// </summary>
        /// <param name="emailAddressId">The unique identifier of the email address to delete.</param>
        private async Task CallDeleteConfirmationModalAsync(string emailAddressId)
        {
            // Prepare the dialog parameters for user confirmation.
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this email address from your account?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            // Display the confirmation dialog.
            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            // If the user confirmed deletion, send the DELETE request, then update the local list.
            if (!result!.Canceled)
            {
                // Only attempt a server call if ControllerPartUrl is valid.
                if (!string.IsNullOrEmpty(ControllerPartUrl))
                {
                    var deleteResult = await Provider.DeleteEmailAddressAsync(emailAddressId);
                    deleteResult.ProcessResponseForDisplay(SnackBar, () =>
                    {
                        SnackBar.AddSuccess("Email Address removed successfully");
                    });
                }

                // Remove from local memory.
                EmailAddresses!.Remove(EmailAddresses.FirstOrDefault(c => c.EmailAddressId == emailAddressId)!);

                // Fire the OnDelete callback.
                await OnDelete.InvokeAsync(emailAddressId);
            }
        }

        #endregion

        #region Create

        /// <summary>
        /// Opens a modal dialog to create a new email address. 
        /// If the user confirms, a PUT request is sent to add the email address, and the list is refreshed.
        /// </summary>
        private async Task CallCreateEmailAddressModalAsync()
        {
            // Prepare the parameters for the creation dialog.
            var parameters = new DialogParameters<EmailAddressModal>
            {
                { x => x.EmailAddress, new EmailAddressViewModel { ParentId = ParentId } },
                { x => x.Title, "Create New Email Address" },
                { x => x.SuccessText, "Email Address created succcesfully" },
                { x => x.ButtonText, "Create" }
            };

            // Show the creation dialog.
            var dialog = await DialogService.ShowAsync<EmailAddressModal>("Create Email Address", parameters);
            var result = await dialog.Result;

            // If the user confirmed creation, send a PUT request to add the new address.
            if (!result!.Canceled)
            {
                // Check that the dialog returned a valid EmailAddressViewModel.
                if (result.Data is EmailAddressViewModel newEmailAddress && EmailAddresses != null)
                {
                    // If no addresses exist, automatically set this as default.
                    if (!EmailAddresses.Any())
                    {
                        newEmailAddress.Default = true;
                    }
                    else
                    {
                        // If user wants to make this new address default, unset defaults on existing addresses.
                        if (newEmailAddress.Default)
                        {
                            foreach (var item in EmailAddresses.ToList())
                            {
                                if (EmailAddresses.FirstOrDefault(c => c.EmailAddressId == item.EmailAddressId)!.Default)
                                {
                                    EmailAddresses.FirstOrDefault(c => c.EmailAddressId == item.EmailAddressId)!.Default = false;

                                    if (!string.IsNullOrEmpty(ControllerPartUrl))
                                    {
                                        var updateResult = await Provider.UpdateEmailAddressAsync(newEmailAddress.ToDto());
                                        if (!updateResult.Succeeded)
                                        {
                                            SnackBar.AddErrors(updateResult.Messages);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Add to local list.
                    EmailAddresses.Add(newEmailAddress);

                    // Persist to server (PUT) if valid URL is provided.
                    if (!string.IsNullOrEmpty(ControllerPartUrl))
                    {
                        var createResult = await Provider.CreateEmailAddressAsync(newEmailAddress.ToDto());
                        createResult.ProcessResponseForDisplay(SnackBar, () =>
                        {
                            SnackBar.AddSuccess("Email Address created successfully, and added to parent");
                        });
                    }

                    // Invoke the OnCreate callback to notify external components of the new address.
                    await OnCreate.InvokeAsync(newEmailAddress);
                }
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Opens a modal dialog to update an existing email address identified by <paramref name="emailAddressId"/>.
        /// After user confirmation, a POST request updates the address, then the list is refreshed.
        /// </summary>
        /// <param name="emailAddressId">The identifier of the email address to update.</param>
        private async void CalUpdateEmailAddressModal(string emailAddressId)
        {
            // Fetch the current email address details from the server.
            var emailAddress = await Provider.EmailAddressAsync(emailAddressId);

            // Prepare dialog parameters for updating the email address.
            var parameters = new DialogParameters<EmailAddressModal>
            {
                { x => x.EmailAddress, new EmailAddressViewModel(emailAddress.Data) },
                { x => x.Title, "Update Email Address" },
                { x => x.SuccessText, "Email Address updated successfully" },
                { x => x.ButtonText, "Update" }
            };

            // Show the update dialog.
            var dialog = await DialogService.ShowAsync<EmailAddressModal>("Update Email Address", parameters);
            var result = await dialog.Result;

            // If the user confirmed, post updates to the server and sync local changes.
            if (!result!.Canceled)
            {
                if (result.Data is EmailAddressViewModel newEmailAddress && EmailAddresses != null)
                {
                    // Update the in-memory list.
                    var index = EmailAddresses.IndexOf(EmailAddresses.FirstOrDefault(c => c.EmailAddressId == newEmailAddress.EmailAddressId)!);
                    if (index > -1)
                    {
                        EmailAddresses[index].ParentId = newEmailAddress.ParentId;
                        EmailAddresses[index].EmailAddress = newEmailAddress.EmailAddress;
                        EmailAddresses[index].Default = newEmailAddress.Default;
                    }

                    // If only one email address exists, ensure it remains default. 
                    if (EmailAddresses.Count <= 1)
                    {
                        if (!newEmailAddress.Default)
                        {
                            newEmailAddress.Default = true;
                            EmailAddresses[index].Default = newEmailAddress.Default;
                            SnackBar.Add(
                                "Single email address is always marked as default. " +
                                "If you want to change the default email address, " +
                                "you must add another one.",
                                Severity.Info
                            );
                        }
                    }
                    else
                    {
                        // If this new email is set as default, unset the default from others.
                        if (newEmailAddress.Default)
                        {
                            foreach (var item in EmailAddresses.ToList())
                            {
                                if (EmailAddresses.FirstOrDefault(c => c.EmailAddressId == item.EmailAddressId)!.Default)
                                {
                                    EmailAddresses.FirstOrDefault(c => c.EmailAddressId == item.EmailAddressId)!.Default = false;

                                    if (!string.IsNullOrEmpty(ControllerPartUrl))
                                    {
                                        var updateOtherResult = await Provider.UpdateEmailAddressAsync(item.ToDto());
                                        if (!updateOtherResult.Succeeded)
                                        {
                                            SnackBar.AddErrors(updateOtherResult.Messages);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Persist changes to the server (POST) if a valid URL is provided.
                    if (!string.IsNullOrEmpty(ControllerPartUrl))
                    {
                        var updateResult = await Provider.UpdateEmailAddressAsync(newEmailAddress.ToDto()); ;
                        updateResult.ProcessResponseForDisplay(SnackBar, () =>
                        {
                            SnackBar.AddSuccess("Email Address updated successfully");
                        });
                    }

                    // Notify external consumers that an update occurred.
                    await OnUpdated.InvokeAsync(newEmailAddress);
                }
            }

            StateHasChanged();
        }

        #endregion

        #region Lifecycle

        /// <summary>
        /// If <see cref="EmailAddresses"/> is null, fetches the email address list from the server. 
        /// Otherwise, reuses the provided list.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            if (EmailAddresses == null)
            {
                var emailAddressListResult = await Provider.AllEmailAddressesAsync(ParentId);
                emailAddressListResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    EmailAddresses = emailAddressListResult.Data!
                        .Select(c => new EmailAddressViewModel(c))
                        .ToList();
                });
            }
            await base.OnInitializedAsync();
        }

        #endregion
    }
}
