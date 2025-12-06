using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.GeneralInformationTemplates;

/// <summary>
/// Represents a component that manages a list of general information templates, providing functionality for adding,
/// updating, and deleting templates, as well as displaying them in a user interface.
/// </summary>
/// <remarks>This class is responsible for handling user interactions related to general information templates,
/// including displaying dialogs for confirmation and data entry, and communicating with a backend service to persist
/// changes. It utilizes various services such as <see cref="MetadataTransferService"/>, <see
/// cref="IBaseHttpProvider"/>, <see cref="IDialogService"/>, <see cref="ISnackbar"/>, and <see
/// cref="NavigationManager"/> to perform its operations.</remarks>
public partial class List
{
    private List<GeneralInformationTemplateDto> _templates = [];
    private MudTable<GeneralInformationTemplateDto> _table = null!;
    private bool _loaded;

    /// <summary>
    /// Gets or sets the dialog service used for displaying dialogs within the application.
    /// </summary>
    [Inject] public IDialogService DialogService { get; set; } = null!;

    [Inject] public IGeneralInformationTemplateService GeneralInformationTemplateService { get; set; } = null!;

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
    /// Deletes a template identified by the specified template ID after user confirmation.
    /// </summary>
    /// <remarks>This method prompts the user with a confirmation dialog before proceeding with the deletion.
    /// If the user confirms, the template is removed from the data source and the UI is updated to reflect the
    /// change.</remarks>
    /// <param name="templateId">The unique identifier of the template to be deleted. Cannot be null or empty.</param>
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
            var deleteResult = await GeneralInformationTemplateService.DeleteAsync(templateId);
            if (deleteResult.Succeeded)
            {
                _templates.Remove(_templates.FirstOrDefault(c => c.Id == templateId)!);
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Displays a dialog for adding a new general information template and processes the result.
    /// </summary>
    /// <remarks>This method opens a modal dialog to collect information for a new general information
    /// template. If the dialog is not canceled, it attempts to save the template using the provider service. Upon
    /// successful creation, the new template is added to the local collection and the data table is reloaded.</remarks>
    private async Task AddTemplate()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters<GeneralInformationTemplateModal>
        {
            { x => x.Template, new GeneralInformationTemplateViewModel() }
        };

        var dialog = await DialogService.ShowAsync<GeneralInformationTemplateModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var model = ((GeneralInformationTemplateViewModel)result.Data!).ToDto();
            var creationResult = await GeneralInformationTemplateService.AddAsync(model);
            if (creationResult.Succeeded)
            {
                _templates.Add(model);
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Updates the specified general information template by displaying a confirmation dialog and posting the updated
    /// data to the server if confirmed.
    /// </summary>
    /// <remarks>This method opens a dialog for the user to confirm changes to the template. If the user
    /// confirms, the updated template is sent to the server. The local collection of templates is updated upon a
    /// successful server response, and the data table is reloaded.</remarks>
    /// <param name="template">The template to be updated. Cannot be null.</param>
    private async Task UpdateTemplate(GeneralInformationTemplateDto template)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters<GeneralInformationTemplateModal>
        {
            { x => x.Template, new GeneralInformationTemplateViewModel(template) }
        };

        var dialog = await DialogService.ShowAsync<GeneralInformationTemplateModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var index = _templates.IndexOf(template);
            var model = ((GeneralInformationTemplateViewModel)result.Data!).ToDto();
            var updateResult = await GeneralInformationTemplateService.EditAsync(model);
            if (updateResult.Succeeded)
            {
                _templates[index] = model;
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Asynchronously initializes the component by setting up page details and loading general information templates.
    /// </summary>
    /// <remarks>This method configures the page metadata and breadcrumb navigation, then retrieves a list of
    /// general information templates from the provider. If the retrieval is successful, the templates are stored for
    /// further use; otherwise, error messages are displayed.</remarks>
    protected override async Task OnInitializedAsync()
    {
        var request = await GeneralInformationTemplateService.GetAllAsync();
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

