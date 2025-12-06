using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.VacationTitleTemplates;

/// <summary>
/// Represents a component for managing and displaying a list of vacation title templates.
/// </summary>
/// <remarks>This component provides functionality for adding, updating, and deleting vacation title templates. It
/// interacts with various services, such as HTTP providers, dialog services, and navigation managers, to perform these
/// operations. The component also supports server-side data reloading and displays notifications for user
/// feedback.</remarks>
public partial class List
{
    private List<VacationTitleTemplateDto> _templates = [];
    private MudTable<VacationTitleTemplateDto> _table = null!;
    private bool _loaded;

    /// <summary>
    /// Gets or sets the service used to display dialogs in the application.
    /// </summary>
    [Inject] public IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the injected service for displaying snack bar notifications.
    /// </summary>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation and URI management in the
    /// application.
    /// </summary>
    /// <remarks>This property is typically injected by the framework and should not be manually set in most
    /// scenarios.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to generate vacation title templates.
    /// </summary>
    [Inject] public IVacationTitleTemplateService VacationTitleTemplateService { get; set; } = null!;

    /// <summary>
    /// Deletes a template after user confirmation.
    /// </summary>
    /// <remarks>This method displays a confirmation dialog to the user before proceeding with the deletion. 
    /// If the user confirms, the template is removed from the data source and the UI is updated to reflect the
    /// change.</remarks>
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
            var deleteResult = await VacationTitleTemplateService.DeleteAsync(templateId);
            if (deleteResult.Succeeded)
            {
                _templates.Remove(_templates.FirstOrDefault(c => c.Id == templateId)!);
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Displays a dialog for creating a new vacation title template and adds the template to the collection if
    /// confirmed.
    /// </summary>
    /// <remarks>This method opens a modal dialog where the user can input details for a new vacation title
    /// template.  If the user confirms the dialog, the template is sent to the server for creation.  Upon successful
    /// creation, the template is added to the local collection and the data table is reloaded.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task AddTemplate()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters<VacationTitleTemplateModal>
        {
            { x => x.Template, new VacationTitleTemplateViewModel() }
        };

        var dialog = await DialogService.ShowAsync<VacationTitleTemplateModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var model = ((VacationTitleTemplateViewModel)result.Data!).ToDto();
            var creationResult = await VacationTitleTemplateService.AddAsync(model);
            if (creationResult.Succeeded)
            {
                _templates.Add(model);
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Updates the specified vacation title template by displaying a modal dialog for editing and saving changes.
    /// </summary>
    /// <remarks>This method opens a modal dialog to allow the user to edit the provided template. If the user
    /// confirms the changes, the updated template is sent to the server for saving. Upon a successful update, the local
    /// template collection is updated, and the associated data table is reloaded.</remarks>
    /// <param name="template">The vacation title template to be updated. Cannot be null.</param>
    /// <returns></returns>
    private async Task UpdateTemplate(VacationTitleTemplateDto template)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters<VacationTitleTemplateModal>
        {
            { x => x.Template, new VacationTitleTemplateViewModel(template) }
        };

        var dialog = await DialogService.ShowAsync<VacationTitleTemplateModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var index = _templates.IndexOf(template);
            var model = ((VacationTitleTemplateViewModel)result.Data!).ToDto();
            var updateResult = await VacationTitleTemplateService.EditAsync(model);
            if (updateResult.Succeeded)
            {
                _templates[index] = model;
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Asynchronously initializes the component and loads the vacation title templates.
    /// </summary>
    /// <remarks>This method sets up the page metadata, including breadcrumbs, and retrieves a list of
    /// vacation title templates  from the data provider. If the data retrieval is successful, the templates are loaded
    /// into the component;  otherwise, error messages are displayed using the snackbar.</remarks>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var request = await VacationTitleTemplateService.GetAllAsync();
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
