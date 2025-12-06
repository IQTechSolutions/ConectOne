using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.ShortDescriptionTemplates;

/// <summary>
/// Represents a component that manages a list of short description templates.
/// </summary>
/// <remarks>This component provides functionality to add, update, and delete short description templates. It
/// interacts with various services to perform these operations and updates the UI accordingly.</remarks>
public partial class List
{
    private List<ShortDescriptionTemplateDto> _templates = [];
    private MudTable<ShortDescriptionTemplateDto> _table = null!;
    private bool _loaded;

    /// <summary>
    /// Gets or sets the dialog service used for displaying dialogs within the application.
    /// </summary>
    [Inject] public IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="ISnackbar"/> service used to display snack bar notifications.
    /// </summary>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation operations within the
    /// application.
    /// </summary>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for managing short description templates.
    /// </summary>
    [Inject] public IShortDescriptionTemplateService ShortDescriptionTemplateService { get; set; } = null!;

    /// <summary>
    /// Deletes a template identified by the specified template ID after user confirmation.
    /// </summary>
    /// <remarks>This method prompts the user with a confirmation dialog before proceeding with the deletion.
    /// If the user confirms, the template is removed from the data source and the server data is reloaded.</remarks>
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
            var deleteResult = await ShortDescriptionTemplateService.DeleteAsync(templateId);
            if (deleteResult.Succeeded)
            {
                _templates.Remove(_templates.FirstOrDefault(c => c.Id == templateId));
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Displays a dialog for adding a new short description template and processes the result.
    /// </summary>
    /// <remarks>This method opens a modal dialog allowing the user to input details for a new short
    /// description template. If the dialog is confirmed, the template is sent to the server for creation. Upon
    /// successful creation, the new template is added to the local collection and the data table is reloaded.</remarks>
    /// <returns></returns>
    private async Task AddTemplate()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters<ShortDescriptionTemplateModal>
        {
            { x => x.Template, new ShortDescriptionTemplateViewModel() }
        };

        var dialog = await DialogService.ShowAsync<ShortDescriptionTemplateModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var model = ((ShortDescriptionTemplateViewModel)result.Data!).ToDto();
            var creationResult = await ShortDescriptionTemplateService.AddAsync(model);
            if (creationResult.Succeeded)
            {
                _templates.Add(model);
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Updates the specified short description template by displaying a confirmation dialog and posting the changes to
    /// the server.
    /// </summary>
    /// <remarks>This method opens a dialog for the user to confirm changes to the template. If the user
    /// confirms, the updated template is sent to the server. The method updates the local collection of templates and
    /// reloads the server data if the update is successful.</remarks>
    /// <param name="template">The template to be updated. Must not be null.</param>
    /// <returns></returns>
    private async Task UpdateTemplate(ShortDescriptionTemplateDto template)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters<ShortDescriptionTemplateModal>
        {
            { x => x.Template, new ShortDescriptionTemplateViewModel(template) }
        };

        var dialog = await DialogService.ShowAsync<ShortDescriptionTemplateModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var index = _templates.IndexOf(template);
            var model = ((ShortDescriptionTemplateViewModel)result.Data!).ToDto();
            var updateResult = await ShortDescriptionTemplateService.EditAsync(model);
            if (updateResult.Succeeded)
            {
                _templates[index] = model;
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Asynchronously initializes the component, setting up page details and loading short description templates.
    /// </summary>
    /// <remarks>This method sets the page metadata and breadcrumb navigation for the "Short Description
    /// Templates" page. It retrieves a list of short description templates from the provider and updates the component
    /// state based on the success of the request. If the request fails, error messages are displayed using the SnackBar
    /// service.</remarks>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        var request = await ShortDescriptionTemplateService.GetAllAsync();
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
}
