using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System.Collections.ObjectModel;
using AdvertisingModule.Domain.Constants;
using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Radzen;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;

namespace AdvertisingModule.Blazor.Pages.Affiliates;

/// <summary>
/// Represents a component that displays and manages a list of affiliates, including functionality for loading,
/// displaying, and interacting with affiliate data.
/// </summary>
/// <remarks>This component provides server-side data loading, table management, and user interactions such as
/// deleting affiliates. It integrates with various services, including HTTP providers, dialog services, and
/// notifications, to facilitate these operations.</remarks>
public partial class List
{
    private ObservableCollection<AffiliateDto> _affiliates = null!;
    private IList<AffiliateDto> _selectedAffiliates = null!;

    private MudTable<AffiliateDto> _table = null!;

    private readonly List<BreadcrumbItem> _items = new()
    {
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Affiliates", href: null, disabled: true, icon: Icons.Material.Filled.List)
    };

    private int _totalItems;
    private bool _dense;
    private bool _striped = true;
    private bool _bordered;
    private bool _loaded;

    private bool _canCreate;
    private bool _canEdit;
    private bool _canDelete;

    private AffiliateDto draggedItem;

    /// <summary>
    /// Configures the attributes and event handlers for a row in the data grid to enable drag-and-drop reordering.
    /// </summary>
    /// <remarks>This method adds attributes to the row to make it draggable and sets up event handlers for
    /// drag-and-drop operations. The row is styled with a "grab" cursor and includes logic to handle drag start, drag
    /// over, drag leave, and drop events. The drop operation updates the display order of the rows and synchronizes the
    /// changes with the server.</remarks>
    /// <param name="args">The event arguments containing the row data and attributes to be modified.</param>
    public void RowRender(RowRenderEventArgs<AffiliateDto> args)
    {
        args.Attributes.Add("title", "Drag row to reorder");
        args.Attributes.Add("style", "cursor:grab");
        args.Attributes.Add("draggable", "true");
        args.Attributes.Add("ondragover", "event.preventDefault();event.target.closest('.rz-data-row').classList.add('my-class')");
        args.Attributes.Add("ondragleave", "event.target.closest('.rz-data-row').classList.remove('my-class')");
        args.Attributes.Add("ondragstart", EventCallback.Factory.Create<DragEventArgs>(this, () => draggedItem = args.Data));
        args.Attributes.Add("ondrop", EventCallback.Factory.Create<DragEventArgs>(this, async () =>
        {
            var draggedIndex = _affiliates.IndexOf(draggedItem);
            var droppedIndex = _affiliates.IndexOf(args.Data);
            _affiliates.Remove(draggedItem);
            _affiliates.Insert(draggedIndex <= droppedIndex ? droppedIndex++ : droppedIndex, draggedItem);

            await JSRuntime.InvokeVoidAsync("eval", $"document.querySelector('.my-class').classList.remove('my-class')");

            foreach (var item in _affiliates)
            {
                item.DisplayOrder = _affiliates.IndexOf(item);
            }

            var result = await AffiliateCommandService.UpdateAffiliateDisplayOrderAsync(new AffiliateOrderUpdateRequest(_affiliates.ToList()));
        }));
    }

    /// <summary>
    /// Gets or sets the task that provides the current authentication state.
    /// </summary>
    /// <remarks>This property is typically used in Blazor components to access the user's authentication
    /// state. The task should not be null and is expected to complete with a valid <see
    /// cref="AuthenticationState"/>.</remarks>
    [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

    /// <summary>
    /// Injects the authorization service for checking user permissions.
    /// </summary>
    [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to query affiliate-related data.
    /// </summary>
    [Inject] public IAffiliateQueryService AffiliateQueryService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the JavaScript runtime instance used to invoke JavaScript functions from .NET.
    /// </summary>
    /// <remarks>This property is typically populated by the dependency injection framework. Ensure it is
    /// properly initialized before using it to perform JavaScript interop.</remarks>
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for executing affiliate-related commands.
    /// </summary>
    [Inject] public IAffiliateCommandService AffiliateCommandService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display dialogs in the application.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework. Ensure that a
    /// valid  implementation of <see cref="IDialogService"/> is provided before using this property.</remarks>
    [Inject] public IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications.
    /// </summary>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> instance used to manage URI navigation and interaction in a
    /// Blazor application.
    /// </summary>
    /// <remarks>This property is typically injected by the Blazor framework and should not be set manually in
    /// most scenarios.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the application configuration settings.
    /// </summary>
    [Inject] public IConfiguration Configuration { get; set; } = null!;
    
    /// <summary>
    /// Asynchronously loads affiliate data and updates the internal state with the retrieved information.
    /// </summary>
    /// <remarks>This method retrieves a collection of affiliate data from the provider and updates the total
    /// item count  and the list of affiliates. If the request fails or no data is returned, the error messages are
    /// added  to the snack bar for user notification.</remarks>
    /// <returns></returns>
    private async Task LoadData()
    {
        var request = await AffiliateQueryService.AllAffiliatesAsync();
        if (request.Succeeded && request.Data is not null)
        {
            var data = request.Data.ToList();
            _selectedAffiliates = new List<AffiliateDto>() { data.FirstOrDefault() };
            _totalItems = data.Count;
            _affiliates = new ObservableCollection<AffiliateDto>(data);
        }
        SnackBar.AddErrors(request.Messages);
    }

    /// <summary>
    /// Reloads the server data for the table asynchronously.
    /// </summary>
    /// <remarks>This method retrieves the latest data from the server and updates the table's state.  It
    /// should be called when the table's data needs to be refreshed.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task Reload()
    {
        await LoadData();
    }

    /// <summary>
    /// Deletes an affiliate with the specified identifier after user confirmation.
    /// </summary>
    /// <remarks>This method displays a confirmation dialog to the user before proceeding with the deletion.
    /// If the user confirms, the affiliate is removed from the application, and the data table is reloaded to reflect
    /// the changes.</remarks>
    /// <param name="affiliateId">The unique identifier of the affiliate to be deleted. Cannot be null or empty.</param>
    /// <returns></returns>
    private async Task DeleteAffiliate(string affiliateId)
    {
        var parameters = new DialogParameters<ConformtaionModal>
        {
            { x => x.ContentText, "Are you sure you want to remove this affiliate from this application?" },
            { x => x.ButtonText, "Yes" },
            { x => x.Color, Color.Success }
        };

        var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            var removalResult =  await AffiliateCommandService.RemoveAsync(affiliateId);
            removalResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                _affiliates.Remove(_affiliates.FirstOrDefault(c => c.Id == affiliateId));
            });
        }
    }

    #region Lifecycle Methods

    /// <summary>
    /// Asynchronously initializes the component and sets its initial state.
    /// </summary>
    /// <remarks>This method is called by the Blazor framework during the component's initialization process.
    /// It sets the component's loaded state to <see langword="true"/> and invokes the base implementation.</remarks>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;

        _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.Affiliates.Create)).Succeeded;
        _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.Affiliates.Edit)).Succeeded;
        _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.Affiliates.Delete)).Succeeded;

        await LoadData();
        _loaded = true;
        await base.OnInitializedAsync();
    }

    #endregion
}
