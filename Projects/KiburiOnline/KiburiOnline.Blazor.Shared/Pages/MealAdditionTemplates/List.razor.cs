using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.MealAdditionTemplates;

/// <summary>
/// Component for listing meal addition templates.
/// </summary>
public partial class List
{
    private List<MealAdditionTemplateDto> _templates = [];
    private MudTable<MealAdditionTemplateDto> _table = null!;
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
    /// Gets or sets the <see cref="NavigationManager"/> used to manage URI navigation and interaction in a Blazor
    /// application.
    /// </summary>
    /// <remarks>This property is typically injected by the Blazor framework and should not be set manually in
    /// most cases.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to manage meal addition templates.
    /// </summary>
    [Inject] public IMealAdditionTemplateService MealAdditionTemplateService { get; set; } = null!;

    /// <summary>
    /// Deletes a meal addition template after user confirmation.
    /// </summary>
    /// <remarks>This method displays a confirmation dialog to the user before proceeding with the deletion. 
    /// If the user confirms, the template is removed from the data source and the table is reloaded.  If the user
    /// cancels, no action is taken.</remarks>
    /// <param name="templateId">The unique identifier of the template to be deleted. Cannot be null or empty.</param>
    /// <returns></returns>
    private async Task DeleteTemplate(string templateId)
    {
        var parameters = new DialogParameters<ConformtaionModal>
        {
            { x => x.ContentText, "Are you sure you want to remove this meal addition template?" },
            { x => x.ButtonText, "Yes" },
            { x => x.Color, Color.Success }
        };

        var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var deleteResult = await MealAdditionTemplateService.DeleteAsync(templateId);
            if (deleteResult.Succeeded)
            {
                _templates.Remove(_templates.FirstOrDefault(c => c.Id == templateId));
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Displays a modal dialog for adding a new meal addition template and processes the result.
    /// </summary>
    /// <remarks>This method opens a modal dialog that allows the user to create a new meal addition template.
    /// If the user confirms the operation, the template is added to the data source and the table is
    /// reloaded.</remarks>
    /// <returns></returns>
    private async Task AddTemplate()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters<MealAdditionTemplateModal>
        {
            { x => x.Template, new MealAdditionTemplateViewModel() }
        };

        var dialog = await DialogService.ShowAsync<MealAdditionTemplateModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var dto = ((MealAdditionTemplateViewModel)result.Data!).ToDto();
            var creationResult = await MealAdditionTemplateService.AddAsync(dto);
            if (creationResult.Succeeded)
            {
                _templates.Add(dto);
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Updates the specified meal addition template by displaying a modal dialog for editing and applying changes if
    /// confirmed.
    /// </summary>
    /// <remarks>This method opens a modal dialog to allow the user to edit the provided template. If the user
    /// confirms the changes, the updated template is sent to the service for persistence. The local collection of
    /// templates is updated, and the associated data table is reloaded to reflect the changes.</remarks>
    /// <param name="template">The meal addition template to be updated. This parameter cannot be null.</param>
    /// <returns></returns>
    private async Task UpdateTemplate(MealAdditionTemplateDto template)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters<MealAdditionTemplateModal>
        {
            { x => x.Template, new MealAdditionTemplateViewModel(template) }
        };

        var dialog = await DialogService.ShowAsync<MealAdditionTemplateModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var index = _templates.IndexOf(template);
            var dto = ((MealAdditionTemplateViewModel)result.Data!).ToDto();
            var updateResult = await MealAdditionTemplateService.EditAsync(dto);
            if (updateResult.Succeeded)
            {
                _templates[index] = dto;
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Asynchronously initializes the component and loads the meal addition templates.
    /// </summary>
    /// <remarks>This method sets up the page metadata, including breadcrumbs, and retrieves the list of meal
    /// addition templates  from the service. If the retrieval is successful, the templates are loaded into the
    /// component's state.  Otherwise, error messages are displayed using the snack bar.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var request = await MealAdditionTemplateService.GetAllAsync();
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
