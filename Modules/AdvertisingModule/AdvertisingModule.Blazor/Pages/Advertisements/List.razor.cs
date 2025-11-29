using AdvertisingModule.Application.ViewModels;
using AdvertisingModule.Domain.Constants;
using AdvertisingModule.Domain.Interfaces;
using AdvertisingModule.Domain.RequestFeatures;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace AdvertisingModule.Blazor.Pages.Advertisements;

public partial class List
{
    private IEnumerable<AdvertisementViewModel> _advertisements = null!;
    private MudTable<AdvertisementViewModel> _table = null!;

    private readonly List<BreadcrumbItem> _items = new()
    {
        new BreadcrumbItem("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new BreadcrumbItem("Advertisements", href: null, disabled: true, icon: Icons.Material.Filled.List)
    };

    private int _totalItems;
    private bool _dense;
    private bool _striped = true;
    private bool _bordered;
    private bool _loaded;

    private bool _canCreate;
    private bool _canEdit;
    private bool _canDelete;

    /// <summary>
    /// Gets or sets the task that represents the asynchronous operation to retrieve the current authentication state.
    /// </summary>
    [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

    /// <summary>
    /// Injects the authorization service for checking user permissions.
    /// </summary>
    [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to query advertisements.
    /// </summary>
    [Inject] public IAdvertisementQueryService AdvertisementQueryService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for executing advertisement-related commands.
    /// </summary>
    [Inject] public IAdvertisementCommandService AdvertisementCommandService { get; set; } = null!;

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
    /// Gets or sets the application's configuration settings.[Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;
    /// </summary>
    [Inject] public IConfiguration Configuration { get; set; } = null!;

    /// <summary>
    /// Asynchronously initializes the component and sets the initial state.
    /// </summary>
    /// <remarks>This method sets the component's loaded state to <see langword="true"/> and then calls the
    /// base implementation to perform any additional initialization logic. It is typically used to prepare the
    /// component for rendering.</remarks>
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;

        _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.Advertisement.Create)).Succeeded;
        _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.Advertisement.Edit)).Succeeded;
        _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.Advertisement.Delete)).Succeeded;

        _loaded = true;
        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Asynchronously reloads the server data and returns the updated table data.
    /// </summary>
    /// <remarks>This method refreshes the data by reloading it from the server. Ensure that the operation is
    /// not prematurely canceled by the provided <paramref name="token"/>.</remarks>
    /// <param name="state">The current state of the table, which may influence data loading.</param>
    /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated table data, including the
    /// total number of items and the list of advertisements.</returns>
    private async Task<TableData<AdvertisementViewModel>> ServerReload(TableState state, CancellationToken token)
    {
        await LoadData();
        return new TableData<AdvertisementViewModel> { TotalItems = _totalItems, Items = _advertisements };
    }

    /// <summary>
    /// Asynchronously loads advertisement data and updates the internal state with the retrieved information.
    /// </summary>
    /// <remarks>This method fetches a collection of advertisements from a remote source and updates the
    /// internal list of advertisements and the total item count. If the request is unsuccessful or the data is null,
    /// the internal state remains unchanged. Any error messages from the request are added to the snack bar for user
    /// notification.</remarks>
    private async Task LoadData()
    {
        var request = await AdvertisementQueryService.PagedListingsAsync(new AdvertisementListingPageParameters());
        if (request.Succeeded && request.Data is not null)
        {
            var data = request.Data.ToList();
            _totalItems = data.Count;
            _advertisements = data.Select(c => new AdvertisementViewModel(c));
        }
        SnackBar.AddErrors(request.Messages);
    }

    /// <summary>
    /// Asynchronously reloads the server data for the table.
    /// </summary>
    /// <remarks>This method refreshes the data by invoking the server-side reload operation. It should be
    /// called when the table's data needs to be updated to reflect the latest state.</remarks>
    private async Task Reload()
    {
        await _table.ReloadServerData();
    }

    /// <summary>
    /// Deletes an advertisement after user confirmation.
    /// </summary>
    /// <remarks>This method displays a confirmation dialog to the user before proceeding with the deletion. 
    /// If the user confirms, the advertisement is removed from the data source, and the server data is
    /// reloaded.</remarks>
    /// <param name="advertisementId">The unique identifier of the advertisement to be deleted. Cannot be null or empty.</param>
    private async Task DeleteAdvertisement(string advertisementId)
    {
        var parameters = new DialogParameters<ConformtaionModal>
        {
            { x => x.ContentText, "Are you sure you want to remove this advertisement from this application?" },
            { x => x.ButtonText, "Yes" },
            { x => x.Color, Color.Success }
        };

        var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await AdvertisementCommandService.RemoveAsync(advertisementId);
            await _table.ReloadServerData();
        }
    }
}
