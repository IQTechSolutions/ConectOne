using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.MeetAndGreetTemplates;

/// <summary>
/// Represents a modal dialog for creating or editing a "Meet and Greet" template.
/// </summary>
/// <remarks>This component provides functionality for managing a "Meet and Greet" template, including saving
/// changes, canceling the operation, and loading a list of available contacts. It interacts with external services for
/// data retrieval and displays notifications for errors.</remarks>
public partial class MeetAndGreetTemplateModal
{
    /// <summary>
    /// Gets the current instance of the dialog, allowing interaction with the dialog's lifecycle.
    /// </summary>
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    /// <summary>
    /// Gets or sets the template used for configuring the meet-and-greet view.
    /// </summary>
    [Parameter] public MeetAndGreetTemplateViewModel Template { get; set; } = new MeetAndGreetTemplateViewModel();

    /// <summary>
    /// Gets or sets the service used to manage and retrieve contact information.
    /// </summary>
    [Inject] public IContactService ContactService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the injected service for displaying snack bar notifications.
    /// </summary>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for managing meet-and-greet templates.
    /// </summary>
    [Inject] public IMeetAndGreetTemplateService MeetAndGreetTemplateService { get; set; } = null!;

    /// <summary>
    /// Converts a <see cref="ContactDto"/> object to its string representation, using the contact's name if available.
    /// </summary>
    /// <remarks>If the provided <see cref="ContactDto"/> is null, the converter returns an empty
    /// string.</remarks>
    private readonly Func<ContactDto?, string> _contactsConverter = p => p?.Name ?? "";

    /// <summary>
    /// Gets or sets the collection of contacts associated with the current entity.
    /// </summary>
    public ICollection<ContactDto> Contacts { get; set; } = [];

    /// <summary>
    /// Saves the current template and closes the dialog.
    /// </summary>
    /// <remarks>This method finalizes the current operation by passing the template to the dialog's close
    /// mechanism. Ensure that the template is in a valid state before invoking this method.</remarks>
    private void SaveAsync()
    {
        MudDialog.Close(Template);
    }

    /// <summary>
    /// Cancels the current dialog operation.
    /// </summary>
    /// <remarks>This method terminates the dialog and triggers any cancellation logic associated with it. Use
    /// this method to programmatically close a dialog when a cancellation action is required.</remarks>
    public void Cancel()
    {
        MudDialog.Cancel();
    }

    /// <summary>
    /// Asynchronously initializes the component and loads the list of contacts.
    /// </summary>
    /// <remarks>This method retrieves a collection of contacts from the data provider and populates the  <see
    /// cref="Contacts"/> property. If the operation fails, an error message is displayed  using the <see
    /// cref="SnackBar"/> component.</remarks>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var result = await ContactService.GetAllAsync();
        if (result.Succeeded)
        {
            Contacts = result.Data.ToList();
        }
        else
        {
            SnackBar.Add("Failed to load contacts.", Severity.Error);
        }
        await base.OnInitializedAsync();
    }
}
