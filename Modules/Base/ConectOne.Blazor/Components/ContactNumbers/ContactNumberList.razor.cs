using ConectOne.Application.ViewModels;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.DataTransferObjects;
using ConectOne.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace ConectOne.Blazor.Components.ContactNumbers
{
    /// <summary>
    /// A Blazor component for displaying and managing a list of contact numbers (e.g., phone numbers).
    /// It supports creating, updating, and deleting contact numbers through a server API, and
    /// integrates with <see cref="MudBlazor"/> dialogs for user confirmations and data entry.
    /// 
    /// <para>
    /// Typical usage involves binding this component in a parent view or page:
    /// <code>
    /// &lt;ContactNumberList 
    ///     ParentId="@someParentId" 
    ///     ControllerUrlPart="api/users"
    ///     OnUpdated="@HandleContactUpdate"
    ///     OnCreate="@HandleContactCreation"
    ///     OnDelete="@HandleContactDeletion" /&gt;
    /// </code>
    /// </para>
    /// </summary>
    public partial class ContactNumberList<TContactNumberOwner>
    {
        #region Parameters

        /// <summary>
        /// The parent entity's unique identifier (e.g., user ID or customer ID)
        /// to which these contact numbers are associated. Typically needed to form API request paths.
        /// </summary>
        [Parameter] public string ParentId { get; set; } = null!;

        /// <summary>
        /// The API route part for the controller handling contact-number operations,
        /// e.g., "api/users" or "api/customers". This helps build the correct endpoint URLs
        /// for server operations.
        /// </summary>
        [Parameter] public string? ControllerUrlPart { get; set; }

        /// <summary>
        /// An in-memory cache of <see cref="ContactNumberViewModel"/> objects representing
        /// the contact numbers retrieved from the server. If null, the component will fetch them
        /// on initialization via <see cref="OnInitializedAsync"/>.
        /// </summary>
        [Parameter] public List<ContactNumberViewModel>? ContactNumbers { get; set; }

        /// <summary>
        /// Event callback triggered when a contact number is updated. Sends the updated
        /// <see cref="ContactNumberViewModel"/> to the caller for further processing (e.g., logging or UI refresh).
        /// </summary>
        [Parameter] public EventCallback<ContactNumberViewModel> OnUpdated { get; set; }

        /// <summary>
        /// Event callback triggered when a new contact number is created. 
        /// Passes the newly created <see cref="ContactNumberViewModel"/> for external handling.
        /// </summary>
        [Parameter] public EventCallback<ContactNumberViewModel> OnCreate { get; set; }

        /// <summary>
        /// Event callback triggered when a contact number is deleted. 
        /// Passes the <c>Id</c> of the removed contact number for additional operations in the parent.
        /// </summary>
        [Parameter] public EventCallback<string> OnDelete { get; set; }

        #endregion

        #region Injected Services

        /// <summary>
        /// A service (HTTP provider) for performing REST API requests such as GET, POST, PUT, and DELETE
        /// against the server, allowing the component to interact with contact-number endpoints.
        /// </summary>
        [Inject] public IContactInfoService<TContactNumberOwner> Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>This property is typically populated by dependency injection and provides access to
        /// configuration values such as connection strings, application settings, and environment variables.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications in the user interface.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Use this
        /// service to show brief messages or alerts to users, such as confirmations or error notifications.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; }

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications in the user interface.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Use this
        /// service to show brief messages or alerts to users, such as confirmations or error notifications.</remarks>
        [Inject] public IDialogService DialogService { get; set; }

        #endregion

        #region Delete Operations

        /// <summary>
        /// Displays a confirmation dialog to delete a specific contact number. 
        /// If confirmed, performs a DELETE request to the server, then removes the entry from <see cref="ContactNumbers"/>.
        /// </summary>
        /// <param name="contactNumberId">The unique identifier of the contact number to delete.</param>
        private async Task CallDeleteConfirmationModalAsync(string contactNumberId)
        {
            // Prepare dialog parameters for user confirmation.
            var parameters = new DialogParameters<ConformtaionModal>();
            parameters.Add(x => x.ContentText, "Are you sure you want to remove this contact number from your account?");
            parameters.Add(x => x.ButtonText, "Yes");
            parameters.Add(x => x.Color, Color.Success);

            // Show the dialog and await the user's response.
            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            // If the user confirmed (not canceled), proceed with deletion.
            if (!result!.Canceled)
            {
                // If the route part is valid, send DELETE request to the server.
                if (!string.IsNullOrEmpty(ControllerUrlPart))
                {
                    var deleteResult = await Provider.DeleteContactNumberAsync(contactNumberId);
                    deleteResult.ProcessResponseForDisplay(SnackBar, () =>
                    {
                        SnackBar.AddSuccess("Contact Number removed successfully");
                    });
                }

                // Remove it from the in-memory collection.
                ContactNumbers!.Remove(ContactNumbers.FirstOrDefault(c => c.ContactNrId == contactNumberId)!);

                // Notify external components that a contact number was deleted.
                await OnDelete.InvokeAsync(contactNumberId);
            }
        }

        #endregion

        #region Create Operations

        /// <summary>
        /// Opens a dialog for creating a new contact number. 
        /// If the user confirms, sends a PUT request to the server to add the contact number, 
        /// then updates the local list and triggers the <see cref="OnCreate"/> event.
        /// </summary>
        private async Task CallCreateContactNrModalAsync()
        {
            // Prepare parameters for the creation dialog.
            var parameters = new DialogParameters<ContactNumberModal>
            {
                { x => x.ContactNumber, new ContactNumberViewModel() { ParentId = ParentId } },
                { x => x.Title, "Create New Contact Number" },
                { x => x.SuccessText, "Contact Number created succcesfully" },
                { x => x.ButtonText, "Create" }
            };

            // Display the creation dialog.
            var dialog = await DialogService.ShowAsync<ContactNumberModal>("Create Contact Number", parameters);
            var result = await dialog.Result;

            // If user didn't cancel, proceed with creation logic.
            if (!result!.Canceled)
            {
                if (result.Data is ContactNumberViewModel newContactNr && ContactNumbers != null)
                {
                    // If no existing numbers, mark this new one as default.
                    if (!ContactNumbers.Any())
                    {
                        newContactNr.Default = true;
                    }
                    else
                    {
                        // If user sets it as default, unset previous default in the local list.
                        if (newContactNr.Default)
                        {
                            foreach (var item in ContactNumbers.ToList())
                            {
                                if (ContactNumbers.FirstOrDefault(c => c.ContactNrId == item.ContactNrId)!.Default)
                                {
                                    ContactNumbers.FirstOrDefault(c => c.ContactNrId == item.ContactNrId)!.Default = false;

                                    if (string.IsNullOrEmpty(ControllerUrlPart)) continue;
                                    // POST update to server to unset default for the old one.
                                    var updateResult = await Provider.UpdateContactNumberAsync(newContactNr.ToDto());
                                    if (!updateResult.Succeeded)
                                    {
                                        SnackBar.AddErrors(updateResult.Messages);
                                        return;
                                    }
                                }
                            }
                        }
                    }

                    // Add the new contact number to the local in-memory list.
                    ContactNumbers.Add(newContactNr);

                    // Persist it to the server if the URL is valid.
                    if (!string.IsNullOrEmpty(ControllerUrlPart))
                    {
                        var createResult = await Provider.CreateContactNumber(newContactNr.ToDto());
                        createResult.ProcessResponseForDisplay(SnackBar, () =>
                        {
                            SnackBar.AddSuccess("Contact Number created successfully, and added to parent");
                        });
                    }

                    // Notify external components that a contact number was created.
                    await OnCreate.InvokeAsync(newContactNr);
                }
            }
        }

        #endregion

        #region Update Operations

        /// <summary>
        /// Opens a dialog to update an existing contact number. 
        /// If the user confirms, a POST request updates the contact number on the server,
        /// and the local list is refreshed accordingly.
        /// </summary>
        /// <param name="contactNumberId">The unique identifier of the contact number to update.</param>
        private async Task CallUpdateContactNrModalAsync(string contactNumberId)
        {
            // Fetch the existing contact number from the server.
            var contactNr = await Provider.ContactNumberAsync(contactNumberId);

            // Prepare dialog parameters with the fetched data mapped to a view model.
            var parameters = new DialogParameters<ContactNumberModal>
            {
                { x => x.ContactNumber, new ContactNumberViewModel(contactNr.Data) { ParentId = ParentId} },
                { x => x.Title, "Update Contact Number" },
                { x => x.SuccessText, "Contact Number updated successfully" },
                { x => x.ButtonText, "Update" }
            };

            // Show the update dialog.
            var dialog = await DialogService.ShowAsync<ContactNumberModal>("Update Contact Number", parameters);
            var result = await dialog.Result;

            // If user confirms, handle the updated data.
            if (!result!.Canceled)
            {
                if (result.Data is ContactNumberViewModel newContactNr && ContactNumbers != null)
                {
                    // Update the in-memory collection to reflect new data.
                    var index = ContactNumbers.IndexOf(ContactNumbers.FirstOrDefault(c => c.ContactNrId == newContactNr.ContactNrId)!);
                    if (index > -1)
                    {
                        // Map changes to the local item.
                        ContactNumbers[index].ParentId = newContactNr.InternationalCode;
                        ContactNumbers[index].AreaCode = newContactNr.AreaCode;
                        ContactNumbers[index].Number = newContactNr.Number;
                        ContactNumbers[index].Default = newContactNr.Default;
                    }

                    // If there's only one contact number, ensure it remains default.
                    if (ContactNumbers.Count <= 1)
                    {
                        if (!newContactNr.Default)
                        {
                            newContactNr.Default = true;
                            ContactNumbers[index].Default = newContactNr.Default;
                            SnackBar.Add("Single contact nr is always marked as default, if you want to change the default contact nr you have to add another one", Severity.Info);
                        }
                    }
                    else
                    {
                        // If user wants this number to be default, unset any existing default.
                        if (newContactNr.Default)
                        {
                            foreach (var item in ContactNumbers.ToList())
                            {
                                if (ContactNumbers.FirstOrDefault(c => c.ContactNrId == item.ContactNrId)!.Default)
                                {
                                    ContactNumbers.FirstOrDefault(c => c.ContactNrId == item.ContactNrId)!.Default = false;

                                    if (!string.IsNullOrEmpty(ControllerUrlPart))
                                    {
                                        // POST to unset default on the old one in the server.
                                        var updateOtherResult = await Provider.UpdateContactNumberAsync(item.ToDto());
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

                    // Send a POST request to save changes on the server, if URL is valid.
                    if (!string.IsNullOrEmpty(ControllerUrlPart))
                    {
                        var updateResult = await Provider.UpdateContactNumberAsync(newContactNr.ToDto());
                        updateResult.ProcessResponseForDisplay(SnackBar, () =>
                        {
                            SnackBar.AddSuccess("Contact Number updated successfully");
                        });
                    }

                    // Trigger the OnUpdated callback to inform external components of changes.
                    await OnUpdated.InvokeAsync(newContactNr);
                }
            }
        }

        #endregion

        #region Lifecycle

        /// <summary>
        /// If <see cref="ContactNumbers"/> is null, the component fetches them from the server 
        /// (e.g., "api/users/{ParentId}/contactNumbers") on initialization. Otherwise, uses the 
        /// provided list.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // If no contact numbers were injected, retrieve them from the server.
            if (ContactNumbers == null)
            {
                var contactNrResult = await Provider.AllContactNumbers(ParentId);

                contactNrResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    // Map fetched DTOs into the local ViewModel list.
                    ContactNumbers = contactNrResult.Data!
                        .Select(c => new ContactNumberViewModel(c))
                        .ToList();
                });
            }

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
