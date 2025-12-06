using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.TermsAndConditionsTemplates;

/// <summary>
/// Represents a component for managing and displaying a list of Terms and Conditions templates.
/// </summary>
/// <remarks>This component provides functionality to create, update, and delete Terms and Conditions templates.
/// It interacts with various services, such as <see cref="MetadataTransferService"/>, <see cref="IDialogService"/>, 
/// and <see cref="ISnackbar"/>, to handle data operations, user interactions, and notifications.  The component also
/// manages the state of the templates list and integrates with a <see cref="MudTable{T}"/>  for displaying the
/// templates in a tabular format. It ensures that the data is loaded and updated dynamically  based on user
/// actions.</remarks>
public partial class List
{
    private List<TermsAndConditionsTemplateDto> _templates = [];
    private MudTable<TermsAndConditionsTemplateDto> _table = null!;
    private bool _loaded;

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
    /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation within the application.
    /// </summary>
    /// <remarks>This property is automatically injected by the dependency injection framework in Blazor
    /// applications. Ensure that the property is properly initialized before use.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to manage terms and conditions templates.
    /// </summary>
    [Inject] public ITermsAndConditionsTemplateService TermsAndConditionsTemplateService { get; set; } = null!;

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
            var deleteResult = await TermsAndConditionsTemplateService.DeleteAsync(templateId);
            if (deleteResult.Succeeded)
            {
                _templates.Remove(_templates.FirstOrDefault(c => c.Id == templateId)!);
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Displays a modal dialog for adding a new terms and conditions template and processes the result.
    /// </summary>
    /// <remarks>This method opens a modal dialog that allows the user to create a new terms and conditions
    /// template.  If the user confirms the operation, the template is added to the service and the local collection is
    /// updated.  The table data is then reloaded to reflect the changes.</remarks>
    /// <returns></returns>
    private async Task AddTemplate()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters<TermsAndConditionsTemplateModal>
        {
            { x => x.Template, new TermsAndConditionsTemplateViewModel() }
        };

        var dialog = await DialogService.ShowAsync<TermsAndConditionsTemplateModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var model = ((TermsAndConditionsTemplateViewModel)result.Data!).ToDto();
            var creationResult = await TermsAndConditionsTemplateService.AddAsync(model);
            if (creationResult.Succeeded)
            {
                _templates.Add(model);
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Displays a dialog to update the specified terms and conditions template and applies the changes if confirmed.
    /// </summary>
    /// <remarks>The method opens a modal dialog allowing the user to edit the provided template. If the user
    /// confirms the changes, the updated template is sent to the service for persistence. Upon a successful update, the
    /// local collection of templates is updated, and the associated data table is reloaded.</remarks>
    /// <param name="template">The template to be updated. This parameter cannot be <see langword="null"/>.</param>
    /// <returns></returns>
    private async Task UpdateTemplate(TermsAndConditionsTemplateDto template)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters<TermsAndConditionsTemplateModal>
        {
            { x => x.Template, new TermsAndConditionsTemplateViewModel(template) }
        };

        var dialog = await DialogService.ShowAsync<TermsAndConditionsTemplateModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var index = _templates.IndexOf(template);
            var model = ((TermsAndConditionsTemplateViewModel)result.Data!).ToDto();
            var updateResult = await TermsAndConditionsTemplateService.EditAsync(model);
            if (updateResult.Succeeded)
            {
                _templates[index] = model;
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Asynchronously initializes the component and sets up the page details, breadcrumbs, and terms and conditions
    /// templates.
    /// </summary>
    /// <remarks>This method configures the page metadata, including the title and breadcrumbs, and retrieves
    /// the list of terms and conditions templates  from the associated service. If the retrieval operation fails, error
    /// messages are displayed using the snack bar.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var request = await TermsAndConditionsTemplateService.GetAllAsync();
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
