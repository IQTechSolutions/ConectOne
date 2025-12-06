using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.CustomVariableTags;

/// <summary>
/// Represents a component for managing and displaying a list of custom variable tags.
/// </summary>
/// <remarks>This component provides functionality for adding, updating, and deleting custom variable tags. It
/// interacts with various services, such as <see cref="ICustomVariableTagService"/> for data operations, <see
/// cref="IDialogService"/> for user confirmation dialogs, and <see cref="ISnackbar"/> for displaying notifications. The
/// component also initializes metadata for page details and breadcrumbs.</remarks>
public partial class List
{
    private List<CustomVariableTagDto> _tags = [];
    private MudTable<CustomVariableTagDto> _table = null!;
    private bool _loaded;
    
    /// <summary>
    /// Gets or sets the service responsible for managing custom variable tags.
    /// </summary>
    [Inject] public ICustomVariableTagService CustomVariableTagService { get; set; } = null!;

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
    /// Gets or sets the <see cref="NavigationManager"/> used to manage navigation and URI manipulation within the
    /// application.
    /// </summary>
    /// <remarks>This property is typically injected by the framework and should not be set manually in most
    /// scenarios.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Deletes a tag with the specified identifier after user confirmation.
    /// </summary>
    /// <remarks>This method displays a confirmation dialog to the user before proceeding with the deletion.
    /// If the user confirms, the tag is removed from the data source and the UI is updated to reflect the
    /// change.</remarks>
    /// <param name="id">The unique identifier of the tag to be deleted. Cannot be null or empty.</param>
    /// <returns></returns>
    private async Task DeleteTag(string id)
    {
        var parameters = new DialogParameters<ConformtaionModal>
        {
            { x => x.ContentText, "Are you sure you want to remove this tag?" },
            { x => x.ButtonText, "Yes" },
            { x => x.Color, Color.Success }
        };

        var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var deleteResult = await CustomVariableTagService.DeleteAsync(id);
            if (deleteResult.Succeeded)
            {
                _tags.Remove(_tags.FirstOrDefault(c => c.Id == id));
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Displays a dialog for adding a new custom variable tag and processes the result.
    /// </summary>
    /// <remarks>This method opens a modal dialog to collect input for a new custom variable tag.  If the
    /// dialog is confirmed, the tag is added to the data source and the table is reloaded. The dialog can be canceled
    /// by the user, in which case no changes are made.</remarks>
    /// <returns></returns>
    private async Task AddTag()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters<CustomVariableTagModal>
        {
            { x => x.Tag, new CustomVariableTagViewModel() }
        };

        var dialog = await DialogService.ShowAsync<CustomVariableTagModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var model = ((CustomVariableTagViewModel)result.Data!).ToDto();
            var creationResult = await CustomVariableTagService.AddAsync(model);
            if (creationResult.Succeeded)
            {
                _tags.Add(model);
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Updates the specified tag by displaying a dialog for editing its details and applying the changes if confirmed.
    /// </summary>
    /// <remarks>This method opens a dialog to allow the user to edit the details of the specified tag. If the
    /// user confirms the changes, the tag is updated in the data source and the table is reloaded to reflect the
    /// changes. If the user cancels the dialog, no changes are applied.</remarks>
    /// <param name="tag">The tag to be updated. This parameter cannot be <see langword="null"/>.</param>
    /// <returns></returns>
    private async Task UpdateTag(CustomVariableTagDto tag)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters<CustomVariableTagModal>
        {
            { x => x.Tag, new CustomVariableTagViewModel(tag) }
        };

        var dialog = await DialogService.ShowAsync<CustomVariableTagModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var index = _tags.IndexOf(tag);
            var model = ((CustomVariableTagViewModel)result.Data!).ToDto();
            var updateResult = await CustomVariableTagService.EditAsync(model);
            if (updateResult.Succeeded)
            {
                _tags[index] = model;
                await _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Asynchronously initializes the component and sets up the page details, breadcrumbs, and custom variable tags.
    /// </summary>
    /// <remarks>This method configures the page metadata, including the title and breadcrumbs, and retrieves
    /// a list of custom variable tags  from the associated service. If the retrieval operation fails, error messages
    /// are displayed using the snack bar, and the  initialization process is halted. Upon successful initialization,
    /// the component is marked as loaded.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var request = await CustomVariableTagService.GetAllAsync();
        if (request.Succeeded)
        {
            _tags = request.Data.ToList();
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
