using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Gifts;

/// <summary>
/// Represents a component for managing and interacting with a collection of gifts.
/// </summary>
/// <remarks>This class provides functionality for displaying, adding, updating, and deleting gifts. It relies on
/// several injected services, including <see cref="IGiftService"/> for gift-related operations,  <see
/// cref="IDialogService"/> for displaying dialogs, and <see cref="ISnackbar"/> for showing notifications. The component
/// also manages page metadata and breadcrumbs for navigation.</remarks>
public partial class List
{
    private List<GiftDto> _gifts = [];
    private bool _loaded;

    /// <summary>
    /// Gets or sets the service used to display dialogs in the application.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework. Ensure that a
    /// valid implementation of <see cref="IDialogService"/> is provided before using this property.</remarks>
    [Inject] public IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework. Ensure that the
    /// service is properly configured before use.</remarks>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> used for handling navigation and URI management in a Blazor
    /// application.
    /// </summary>
    /// <remarks>This property is typically injected by the Blazor framework and should not be set manually in
    /// most cases.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for managing gift-related operations.
    /// </summary>
    [Inject] public IGiftService GiftService { get; set; } = null!;
    
    /// <summary>
    /// Deletes a gift after user confirmation.
    /// </summary>
    /// <remarks>This method displays a confirmation dialog to the user before proceeding with the deletion. 
    /// If the user confirms, the gift is removed using the <see cref="GiftService.RemoveGiftAsync"/> method.  The gift
    /// is also removed from the local collection if the deletion operation succeeds.</remarks>
    /// <param name="giftId">The unique identifier of the gift to be deleted. Cannot be null or empty.</param>
    /// <returns></returns>
    private async Task DeleteGift(string giftId)
    {
        var parameters = new DialogParameters<ConformtaionModal>
        {
            { x => x.ContentText, "Are you sure you want to remove this gift?" },
            { x => x.ButtonText, "Yes" },
            { x => x.Color, Color.Success }
        };

        var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var deleteResult = await GiftService.RemoveGiftAsync(giftId);
            if (deleteResult.Succeeded)
            {
                _gifts.Remove(_gifts.FirstOrDefault(c => c.GiftId == giftId)!);
            }
        }
    }

    /// <summary>
    /// Displays a dialog for adding a new gift and processes the result.
    /// </summary>
    /// <remarks>This method opens a modal dialog to collect information about a new gift. If the user
    /// confirms the dialog,  the gift data is submitted to the gift service for creation. Upon successful creation, the
    /// new gift is added  to the internal collection of gifts.</remarks>
    /// <returns></returns>
    private async Task AddGift()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters<GiftModal>
        {
            { x => x.Gift, new GiftViewModel() }
        };

        var dialog = await DialogService.ShowAsync<GiftModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var gift = ((GiftViewModel)result.Data!).ToDto();
            var creationResult = await GiftService.CreateGiftAsync(gift);
            if (creationResult.Succeeded)
            {
                _gifts.Add(gift);
            }
        }
    }

    /// <summary>
    /// Updates the specified gift by displaying a modal dialog for editing and saving changes.
    /// </summary>
    /// <remarks>This method opens a modal dialog to allow the user to edit the details of the specified gift.
    /// If the user confirms the changes, the updated gift is sent to the gift service for saving.  The local collection
    /// of gifts is updated only if the update operation succeeds.</remarks>
    /// <param name="gift">The gift to be updated. This parameter cannot be null.</param>
    /// <returns></returns>
    private async Task UpdateGift(GiftDto gift)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters<GiftModal>
        {
            { x => x.Gift, new GiftViewModel(gift) }
        };

        var dialog = await DialogService.ShowAsync<GiftModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var index = _gifts.IndexOf(gift);
            var updatedGift = ((GiftViewModel)result.Data!).ToDto();
            var updateResult = await GiftService.UpdateGiftAsync(updatedGift);
            if (updateResult.Succeeded)
            {
                _gifts[index] = updatedGift;
            }
        }
    }

    /// <summary>
    /// Asynchronously initializes the component and sets up the page details, breadcrumbs, and gift data.
    /// </summary>
    /// <remarks>This method configures the page metadata, including breadcrumbs for navigation, and retrieves
    /// the list of gifts  from the gift service. If the data retrieval fails, error messages are displayed using the
    /// snack bar.</remarks>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        var request = await GiftService.GiftListAsync();
        if (request.Succeeded)
        {
            _gifts = request.Data == null ? [] : request.Data.ToList();
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

