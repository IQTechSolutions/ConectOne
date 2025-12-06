using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.MeetAndGreetTemplates;

/// <summary>
/// Represents a component for managing and displaying a list of "Meet & Greet" templates.
/// </summary>
/// <remarks>This component provides functionality to create, update, delete, and display "Meet & Greet"
/// templates. It interacts with various services, such as HTTP providers, dialog services, and navigation managers,  to
/// perform these operations. The component also manages its own state, including the list of templates  and their
/// loading status.</remarks>
public partial class List
{
    private List<MeetAndGreetTemplateDto> _templates = [];
    private MudTable<MeetAndGreetTemplateDto> _table = null!;
    private bool _loaded;

    #region Injections

    /// <summary>
    /// Gets or sets the HTTP provider used for making HTTP requests.
    /// </summary>
    [Inject] public IMeetAndGreetTemplateService MeetAndGreetTemplateService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display dialogs in the application.
    /// </summary>
    [Inject] public IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework. Ensure that the
    /// service is properly configured before use.</remarks>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> used to manage URI navigation and interaction in a Blazor
    /// application.
    /// </summary>
    /// <remarks>This property is typically injected by the Blazor framework and should not be set manually in
    /// most scenarios.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    #endregion

    #region Methods

    /// <summary>
    /// Opens a dialog to create a new "Meet and Greet" template and, if confirmed, saves the template to the server.
    /// </summary>
    /// <remarks>This method displays a modal dialog for the user to input details for a new "Meet and Greet"
    /// template.  If the user confirms the dialog, the template is sent to the server for creation. Upon successful
    /// creation,  the new template is added to the local collection and the data table is refreshed.</remarks>
    /// <returns></returns>
    private async Task AddTemplate()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters<MeetAndGreetTemplateModal>
        {
            { x => x.Template, new MeetAndGreetTemplateViewModel() }
        };

        var dialog = await DialogService.ShowAsync<MeetAndGreetTemplateModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var model = ((MeetAndGreetTemplateViewModel)result.Data!).ToDto();
            var creationResult = await MeetAndGreetTemplateService.AddAsync(model);
            if (creationResult.Succeeded)
            {
                _templates.Add(model);
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Updates the specified meet-and-greet template by displaying a modal dialog for editing and saving changes.
    /// </summary>
    /// <remarks>This method opens a modal dialog to allow the user to edit the provided template. If the user
    /// confirms the changes,  the updated template is sent to the server for saving. Upon a successful update, the
    /// local collection of templates  is updated, and the associated data table is reloaded.</remarks>
    /// <param name="template">The template to be updated. This parameter cannot be null.</param>
    /// <returns></returns>
    private async Task UpdateTemplate(MeetAndGreetTemplateDto template)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters<MeetAndGreetTemplateModal>
        {
            { x => x.Template, new MeetAndGreetTemplateViewModel(template) }
        };

        var dialog = await DialogService.ShowAsync<MeetAndGreetTemplateModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var index = _templates.IndexOf(template);
            var model = ((MeetAndGreetTemplateViewModel)result.Data!).ToDto();
            var updateResult = await MeetAndGreetTemplateService.EditAsync(model);
            if (updateResult.Succeeded)
            {
                _templates[index] = model;
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Deletes a specified template after user confirmation.
    /// </summary>
    /// <remarks>This method displays a confirmation dialog to the user before proceeding with the deletion. 
    /// If the user confirms, the template is removed from the data source and the associated table is
    /// reloaded.</remarks>
    /// <param name="templateId">The unique identifier of the template to be deleted. Cannot be null or empty.</param>
    /// <returns></returns>
    private async Task DeleteTemplate(string templateId)
    {
        var parameters = new DialogParameters<ConformtaionModal>
        {
            { x => x.ContentText, "Are you sure you want to remove this template?" },
            { x => x.ButtonText, "Yes" },
            { x => x.Color, Color.Success }
        };

        var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var deleteResult = await MeetAndGreetTemplateService.DeleteAsync(templateId);
            if (deleteResult.Succeeded)
            {
                _templates.Remove(_templates.FirstOrDefault(c => c.Id == templateId)!);
                await _table.ReloadServerData();
            }
        }
    }

    #endregion

    #region Life Cycle Methods

    /// <summary>
    /// Asynchronously initializes the component and loads the Meet & Greet templates.
    /// </summary>
    /// <remarks>This method sets up the page metadata, including breadcrumbs, and retrieves the list of Meet
    /// & Greet templates  from the data provider. If the data retrieval is successful, the templates are loaded into
    /// the component's state.  Otherwise, error messages are displayed using the snack bar.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var request = await MeetAndGreetTemplateService.AllAsync();
        if (request.Succeeded)
        {
            _templates = request.Data.ToList();
        }
        else
        {
            SnackBar.AddErrors(request.Messages);
            return;
        }

        _loaded = true;

        await base.OnInitializedAsync();
    }

    #endregion
}
