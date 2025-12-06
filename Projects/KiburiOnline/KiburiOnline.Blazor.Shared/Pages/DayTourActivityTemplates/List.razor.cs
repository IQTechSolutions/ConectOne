using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.DayTourActivityTemplates;

/// <summary>
/// Represents a component that manages a list of day tour activity templates, allowing for operations such as adding,
/// updating, and deleting templates.
/// </summary>
/// <remarks>This component interacts with various services to perform CRUD operations on day tour activity
/// templates. It uses dependency injection to obtain necessary services such as <see cref="MetadataTransferService"/>,
/// <see cref="IBaseHttpProvider"/>, <see cref="IDialogService"/>, <see cref="ISnackbar"/>, and <see
/// cref="NavigationManager"/>.</remarks>
public partial class List
{
    private List<DayTourActivityTemplateDto> _templates = [];
    private MudTable<DayTourActivityTemplateDto> _table = null!;
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
    /// Gets or sets the service responsible for managing day tour activity templates.
    /// </summary>
    [Inject] public IDayTourActivityTemplateService DayTourActivityTemplateService { get; set; } = null!;

    /// <summary>
    /// Deletes a template identified by the specified template ID after user confirmation.
    /// </summary>
    /// <remarks>This method prompts the user with a confirmation dialog before proceeding with the deletion.
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
            var deleteResult = await DayTourActivityTemplateService.DeleteAsync(templateId);
            if (deleteResult.Succeeded)
            {
                _templates.Remove(_templates.FirstOrDefault(c => c.Id == templateId));
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Displays a dialog for adding a new day tour activity template and processes the result.
    /// </summary>
    /// <remarks>This method opens a modal dialog to collect information for a new day tour activity template.
    /// If the dialog is confirmed, it creates a new template and sends it to the server for storage. Upon successful
    /// creation, the new template is added to the local collection and the data table is reloaded.</remarks>
    /// <returns></returns>
    private async Task AddTemplate()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters<DayTourActivityTemplateModal>
        {
            { x => x.Template, new DayTourActivityTemplateViewModel() }
        };

        var dialog = await DialogService.ShowAsync<DayTourActivityTemplateModal>("Confirm", options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var model = new DayTourActivityTemplateDto
            {
                Id = ((DayTourActivityTemplateViewModel)result.Data).Id,
                Name = ((DayTourActivityTemplateViewModel)result.Data).Name,
                Summary = ((DayTourActivityTemplateViewModel)result.Data).Summary,
                Description = ((DayTourActivityTemplateViewModel)result.Data).Description,
                GuestType = ((DayTourActivityTemplateViewModel)result.Data).GuestType,
                DisplayInOverview = ((DayTourActivityTemplateViewModel)result.Data).DisplayInOverview
            };

            var creationResult = await DayTourActivityTemplateService.AddAsync(model);
            if (creationResult.Succeeded)
            {
                _templates.Add(model);
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Updates the specified day tour activity template by displaying a dialog for user confirmation and editing.
    /// </summary>
    /// <remarks>This method opens a dialog allowing the user to modify the details of the provided template.
    /// If the user confirms the changes, the template is updated on the server and the local collection is refreshed.
    /// The dialog is configured to close on the escape key and is displayed with medium width.</remarks>
    /// <param name="template">The <see cref="DayTourActivityTemplateDto"/> representing the template to be updated.</param>
    /// <returns></returns>
    private async Task UpdateTemplate(DayTourActivityTemplateDto template)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters<DayTourActivityTemplateModal>
        {
            { x => x.Template, new DayTourActivityTemplateViewModel(template) }
        };

        var dialog = await DialogService.ShowAsync<DayTourActivityTemplateModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var index = _templates.IndexOf(template);
            var model = new DayTourActivityTemplateDto
            {
                Id = ((DayTourActivityTemplateViewModel)result.Data).Id,
                Name = ((DayTourActivityTemplateViewModel)result.Data).Name,
                Summary = ((DayTourActivityTemplateViewModel)result.Data).Summary,
                Description = ((DayTourActivityTemplateViewModel)result.Data).Description,
                GuestType = ((DayTourActivityTemplateViewModel)result.Data).GuestType,
                DisplayInOverview = ((DayTourActivityTemplateViewModel)result.Data).DisplayInOverview
            };

            var updateResult = await DayTourActivityTemplateService.EditAsync(model);
            if (updateResult.Succeeded)
            {
                _templates[index] = model;
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Asynchronously initializes the component's state and retrieves activity templates.
    /// </summary>
    /// <remarks>This method sets up the page details for navigation and breadcrumb display, retrieves a list
    /// of day tour activity templates from the provider, and updates the component's state based on the success of the
    /// data retrieval. If the data retrieval fails, error messages are displayed using a snackbar
    /// notification.</remarks>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        var request = await DayTourActivityTemplateService.GetAllAsync();
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
