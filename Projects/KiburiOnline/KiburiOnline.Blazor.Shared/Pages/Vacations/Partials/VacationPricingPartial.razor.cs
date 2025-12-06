using System.Globalization;
using Accomodation.Blazor.Modals;
using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Components.SortableItems;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using KiburiOnline.Blazor.Shared.Modals;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations.Partials
{
    public partial class VacationPricingPartial
    {
        private List<SortableItem<VacationPricingItemDto>>? _pricingDropBoxItems = [];
        private VacationPricingItemDto? _pricingItemBeingDragged;
        private NumberFormatInfo nfi;

        private List<BookingTermsDescriptionTemplateDto> _availableBookingTermsDescriptionTemplates = [];
        private readonly Func<BookingTermsDescriptionTemplateDto?, string?> _bookingTermsDescriptionTemplateConverter = p => p?.TemplateName;

        private List<CancellationTermsTemplateDto> _availableCancellationTermsTemplates = [];
        private readonly Func<CancellationTermsTemplateDto?, string?> _cancellationTermsTemplateConverter = p => p?.TemplateName;

        private List<TermsAndConditionsTemplateDto> _availableTermsAndConditionsTemplates = [];
        private readonly Func<TermsAndConditionsTemplateDto?, string?> _termsAndConditionsTemplateConverter = p => p?.TemplateName;

        private List<PaymentExclusionTemplateDto> _availablePaymentExclusionsDescriptionTemplates = [];
        private readonly Func<PaymentExclusionTemplateDto?, string?> _bookingPaymentExclusionTemplateConverter = p => p?.TemplateName;

        private MarkupString? _cancellationTermsText;
        private MarkupString? _termsAndConditionsText;
        private MarkupString? _bookingTermsText;
        private MarkupString? _paymentExclusionText;

        #region Injected Services

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service is typically used to display brief, non-intrusive
        /// notifications to the user. Ensure that the service is properly injected before use.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to calculate vacation pricing.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection. Ensure that a valid
        /// implementation of  <see cref="IVacationPricingService"/> is provided before using this property.</remarks>
        [Inject] public IVacationPricingService VacationPricingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing custom variable tags.
        /// </summary>
        [Inject] public ICustomVariableTagService CustomVariableTagService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing cancellation terms templates.
        /// </summary>
        [Inject] public ICancellationTermsTemplateService CancellationTermsTemplateService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing terms and conditions templates.
        /// </summary>
        [Inject] public ITermsAndConditionsTemplateService TermsAndConditionsTemplateService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing booking terms description templates.
        /// </summary>
        [Inject] public IBookingTermsDescriptionTemplateService BookingTermsDescriptionTemplateService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing payment exclusion templates.
        /// </summary>
        [Inject] public IPaymentExclusionTemplateService PaymentExclusionTemplateService { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// Gets or sets the vacation details for the current context.
        /// </summary>
        [Parameter, EditorRequired] public VacationViewModel Vacation { get; set; } = new VacationViewModel();

        /// <summary>
        /// Gets or sets the callback that is invoked to navigate to the next tab.
        /// </summary>
        /// <remarks>This property is typically used to handle user interactions for advancing to the next
        /// tab in a tabbed interface. Ensure that the callback is properly assigned to handle the navigation
        /// logic.</remarks>
        [Parameter] public EventCallback NextTab { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when the "Previous Tab" action is triggered.
        /// </summary>
        /// <remarks>This callback is typically used to handle navigation to the previous tab in a tabbed
        /// interface. Assign a method or delegate to this property to define the behavior when the action
        /// occurs.</remarks>
        [Parameter] public EventCallback PreviousTab { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when the cancel action is triggered.
        /// </summary>
        /// <remarks>Use this property to specify the action to perform when a cancel event occurs, such
        /// as closing a dialog or reverting changes.</remarks>
        [Parameter] public EventCallback Cancel { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked to handle update events.
        /// </summary>
        /// <remarks>This callback is typically used to notify the component of changes or trigger updates
        /// in response to user actions or other events.</remarks>
        [Parameter] public EventCallback Update { get; set; }

        #endregion

        #region Vacation Pricing Groups

        /// <summary>
        /// Displays a dialog to create a new vacation pricing item, processes the user's input,  and adds the created
        /// item to the vacation's pricing list if the operation succeeds.
        /// </summary>
        /// <remarks>This method opens a modal dialog for the user to input details for a new vacation
        /// pricing item.  If the user confirms the operation, the item is created and sent to the server for
        /// persistence.  Upon successful creation, the item is added to the local vacation pricing list and associated 
        /// dropdown items. If the operation fails, error messages are displayed to the user.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task CreateVacationPriceGroupItem()
        {
            var dialog = await DialogService.ShowAsync<AddVacationPriceGroupItemModal>("Confirm");
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var createdItem = new VacationPriceGroupDto() { Id = Guid.NewGuid().ToString(), Name = (string)result.Data, VacationId = Vacation.VacationId };
                if (NextTab.HasDelegate)
                {
                    Vacation.VacationPriceGroups.Add(createdItem);
                    _pricingDropBoxItems.Add(new SortableItem<VacationPricingItemDto>(createdItem.Name, Vacation.VacationPrices.Where(c => c.Selector == createdItem.Name).OrderBy(c => c.Order).ToList()));
                }
                else
                {
                    var creationResult = await VacationPricingService.CreateVacationPricingGroupAsync(createdItem);
                    if (creationResult.Succeeded)
                    {
                        Vacation.VacationPriceGroups.Add(createdItem);
                        _pricingDropBoxItems.Add(new SortableItem<VacationPricingItemDto>(createdItem.Name, Vacation.VacationPrices.Where(c => c.Selector == createdItem.Name).OrderBy(c => c.Order).ToList()));
                    }
                    else
                    {
                        SnackBar.AddErrors(creationResult.Messages);
                    }
                }
            }
        }

        /// <summary>
        /// Updates a vacation price group item by displaying a confirmation dialog, creating a new item based on user
        /// input,  and adding it to the vacation pricing list if the operation succeeds.
        /// </summary>
        /// <remarks>This method displays a modal dialog to confirm the addition of a new vacation price
        /// group item. If the user confirms,  the method creates a new item, assigns it an order, and sends it to the
        /// server for persistence. Upon successful creation,  the item is added to the local vacation pricing list and
        /// associated dropdown items. If the operation fails, error messages  are displayed to the user.</remarks>
        /// <param name="dto">The data transfer object containing information about the vacation price group to be updated.</param>
        private async Task UpdateVacationPriceGroupItem(VacationPriceGroupDto dto)
        {
            var dialog = await DialogService.ShowAsync<AddVacationPriceGroupItemModal>("Confirm");

            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var returnedModal = (VacationPriceGroupDViewModel)result.Data;
                var createdItem = new VacationPriceGroupDto() { Id = dto.Id, Name = (string)result.Data, VacationId = Vacation.VacationId };

                if (NextTab.HasDelegate)
                {
                    var index = Vacation.VacationPriceGroups.IndexOf(dto);
                    Vacation.VacationPriceGroups[index] = createdItem;
                    foreach (var pricingItemGroup in _pricingDropBoxItems.Where(c => c.Selector == dto.Name))
                    {
                        _pricingDropBoxItems[_pricingDropBoxItems.IndexOf(pricingItemGroup)].Selector = returnedModal.Name;
                    }
                }
                else
                {
                    var creationResult = await VacationPricingService.CreateVacationPricingGroupAsync(createdItem);
                    if (creationResult.Succeeded)
                    {
                        var index = Vacation.VacationPriceGroups.IndexOf(dto);
                        Vacation.VacationPriceGroups[index] = createdItem;
                        foreach (var pricingItemGroup in _pricingDropBoxItems.Where(c => c.Selector == dto.Name))
                        {
                            _pricingDropBoxItems[_pricingDropBoxItems.IndexOf(pricingItemGroup)].Selector = returnedModal.Name;
                        }

                    }
                    else
                    {
                        SnackBar.AddErrors(creationResult.Messages);
                    }
                }
            }
        }

        /// <summary>
        /// Removes a vacation price group item based on the provided data transfer object (DTO).
        /// </summary>
        /// <remarks>This method displays a confirmation dialog before proceeding with the removal. If the
        /// operation is confirmed,  the item is removed from the vacation price group and the associated data
        /// structures are updated accordingly.</remarks>
        /// <param name="dto">The <see cref="VacationPriceGroupDto"/> representing the vacation price group item to be removed.</param>
        private async Task RemoveVacationPriceGroupItem(VacationPriceGroupDto dto)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this pricing group from this vacation?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                if (NextTab.HasDelegate)
                {
                    Vacation.VacationIntervals.Remove(Vacation.VacationIntervals.FirstOrDefault(c => c.VacationIntervalId == dto.VacationId)!);
                    _pricingDropBoxItems.Remove(_pricingDropBoxItems.FirstOrDefault(c => c.Selector == dto.Name));
                }
                else
                {
                    var creationResult = await VacationPricingService.RemoveVacationPricingGroupAsync(dto.Id);
                    if (creationResult.Succeeded)
                    {
                        Vacation.VacationPriceGroups.Remove(dto);
                        _pricingDropBoxItems.Remove(_pricingDropBoxItems.FirstOrDefault(c => c.Selector == dto.Name));
                    }
                    else
                    {
                        SnackBar.AddErrors(creationResult.Messages);
                    }
                }
            }
        }

        #endregion

        #region Pricing Methods

        /// <summary>
        /// Creates a new vacation pricing item.
        /// </summary>
        private async Task CreateVacationPricingItem()
        {
            var parameters = new DialogParameters<AddVacationPricingModal>()
            {
                { x => x.VacationId, Vacation.VacationId }
            };

            if (Vacation.VacationPriceGroups.Any())
                parameters.Add(x => x.PriceCategories, Vacation.VacationPriceGroups.Select(c => c.Name).ToList());

            var dialog = await DialogService.ShowAsync<AddVacationPricingModal>("Confirm", parameters);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var createdItem = ((VacationPricingItemViewModel)result.Data!).ToDto();

                if (NextTab.HasDelegate)
                {
                    Vacation.VacationPrices.Add(createdItem);

                    if (_pricingDropBoxItems.FirstOrDefault(c => c.Selector == createdItem.Selector) == null)
                        _pricingDropBoxItems.Add(new SortableItem<VacationPricingItemDto>(createdItem.Selector, new List<VacationPricingItemDto>()));

                    _pricingDropBoxItems.FirstOrDefault(c => c.Selector == createdItem.Selector).Items.Add(createdItem);
                }
                else
                {
                    createdItem.Order = Vacation.VacationPrices.Count + 1;
                    createdItem = createdItem with { VacationId = Vacation.VacationId };

                    var creationResult = await VacationPricingService.CreateVacationPriceAsync(createdItem);
                    if (creationResult.Succeeded)
                    {
                        Vacation.VacationPrices.Add(createdItem);

                        if (_pricingDropBoxItems.FirstOrDefault(c => c.Selector == createdItem.Selector) == null)
                            _pricingDropBoxItems.Add(new SortableItem<VacationPricingItemDto>(createdItem.Selector, new List<VacationPricingItemDto>()));

                        _pricingDropBoxItems.FirstOrDefault(c => c.Selector == createdItem.Selector).Items.Add(createdItem);
                    }
                    else
                    {
                        SnackBar.AddErrors(creationResult.Messages);
                    }
                }
            }
        }

        /// <summary>
        /// Updates an existing vacation pricing item.
        /// </summary>
        /// <param name="vacationPricingId">The ID of the vacation pricing item to update.</param>
        private async Task UpdateVacationPricingItem(VacationPricingItemDto vacationPricingItem)
        {
            var parameters = new DialogParameters<AddVacationPricingModal>
            {
                { x => x.VacationPricingItem, new VacationPricingItemViewModel(vacationPricingItem) },
                { x => x.VacationId, Vacation.VacationId }
            };

            if (Vacation.VacationPriceGroups.Any())
                parameters.Add(x => x.PriceCategories, Vacation.VacationPriceGroups.Select(c => c.Name).ToList());

            var dialog = await DialogService.ShowAsync<AddVacationPricingModal>("Confirm", parameters);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var modalResult = ((VacationPricingItemViewModel)result.Data!).ToDto();

                if (NextTab.HasDelegate)
                {
                    var index = Vacation.VacationPrices.IndexOf(vacationPricingItem);
                    Vacation.VacationPrices[index] = modalResult;

                    var dropboxItem = _pricingDropBoxItems.FirstOrDefault(c => c.Selector == modalResult.Selector).Items.FirstOrDefault(c => c.VacationPriceItemId == modalResult.VacationPriceItemId);
                    var dropboxItemIndex = _pricingDropBoxItems.FirstOrDefault(c => c.Selector == modalResult.Selector).Items.IndexOf(dropboxItem);
                    _pricingDropBoxItems.FirstOrDefault(c => c.Selector == modalResult.Selector).Items[dropboxItemIndex] = modalResult;
                }
                else
                {
                    var updateResult = await VacationPricingService.UpdateVacationPriceAsync(modalResult);
                    if (updateResult.Succeeded)
                    {
                        var index = Vacation.VacationPrices.IndexOf(vacationPricingItem);
                        Vacation.VacationPrices[index] = modalResult;

                        var dropboxItem = _pricingDropBoxItems.FirstOrDefault(c => c.Selector == modalResult.Selector).Items.FirstOrDefault(c => c.VacationPriceItemId == modalResult.VacationPriceItemId);
                        var dropboxItemIndex = _pricingDropBoxItems.FirstOrDefault(c => c.Selector == modalResult.Selector).Items.IndexOf(dropboxItem);
                        _pricingDropBoxItems.FirstOrDefault(c => c.Selector == modalResult.Selector).Items[dropboxItemIndex] = modalResult;
                    }
                }
            }
        }

        /// <summary>
        /// Removes a vacation pricing item.
        /// </summary>
        /// <param name="vacationPricingItemId">The ID of the pricing item to remove.</param>
        private async Task RemoveVacationPricingItem(string vacationPricingItemId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this pricing item from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                if (NextTab.HasDelegate)
                {
                    var item = Vacation.VacationPrices.FirstOrDefault(c => c.VacationPriceItemId == vacationPricingItemId);

                    Vacation.VacationPrices.Remove(item);
                    _pricingDropBoxItems.FirstOrDefault(c => c.Selector == item.Selector).Items.Remove(
                        _pricingDropBoxItems
                            .FirstOrDefault(c => c.Selector == item.Selector).Items
                            .FirstOrDefault(c => c.VacationPriceItemId == vacationPricingItemId));
                }
                else
                {
                    var item = Vacation.VacationPrices.FirstOrDefault(c => c.VacationPriceItemId == vacationPricingItemId);

                    var removalResult = await VacationPricingService.RemoveVacationPriceAsync(vacationPricingItemId);
                    if (removalResult.Succeeded)
                    {
                        Vacation.VacationPrices.Remove(item);
                        _pricingDropBoxItems.FirstOrDefault(c => c.Selector == item.Selector).Items.Remove(
                            _pricingDropBoxItems
                                .FirstOrDefault(c => c.Selector == item.Selector).Items
                                .FirstOrDefault(c => c.VacationPriceItemId == vacationPricingItemId));
                    }
                    else
                    {
                        SnackBar.AddErrors(removalResult.Messages);
                    }
                }
            }
        }

        /// <summary>
        /// Sorts the vacation pricing list based on the new order.
        /// </summary>
        /// <param name="indices">Tuple containing the old index, new index, and the list of items.</param>
        private void SortPricingList((int oldIndex, int newIndex, List<VacationPricingItemDto> itemList) indices)
        {
            // deconstruct the tuple
            var (oldIndex, newIndex, itemList) = indices;

            var items = itemList;
            var itemToMove = items[oldIndex];
            items.RemoveAt(oldIndex);

            if (newIndex < items.Count)
            {
                items.Insert(newIndex, itemToMove);
            }
            else
            {
                items.Add(itemToMove);
            }

            for (var i = 0; i < items.Count; i++)
            {
                items[i].Order = i;
            }

            StateHasChanged();
        }

        /// <summary>
        /// Handles the removal of a pricing item from the sortable list.
        /// </summary>
        /// <param name="indices">Tuple containing the old index, new index, and the selector ID.</param>
        private void PricingItemRemovedFromSortableList((int oldIndex, int newIndex, string id) indices)
        {
            var (oldIndex, newIndex, id) = indices;

            var items = this._pricingDropBoxItems.FirstOrDefault(c => c.Selector == id).Items;

            items.RemoveAt(oldIndex);
        }

        /// <summary>
        /// Updates the pricing item in the sortable list after a drag-and-drop operation.
        /// </summary>
        /// <param name="indices">Tuple containing the old index, new index, and the selector ID.</param>
        private async Task PricingItemUpdatedOnSortableList((int oldIndex, int newIndex, string id) indices)
        {
            _pricingDropBoxItems.FirstOrDefault(c => c.Selector == indices.id).Items.Insert(indices.newIndex, _pricingItemBeingDragged);

            for (var i = 0; i < _pricingDropBoxItems.FirstOrDefault(c => c.Selector == indices.id).Items.Count; i++)
            {
                _pricingDropBoxItems.FirstOrDefault(c => c.Selector == indices.id).Items[i].Order = i;
                _pricingDropBoxItems.FirstOrDefault(c => c.Selector == indices.id).Items[i].Selector = indices.id;
            }
        }

        /// <summary>
        /// Handles the start of a drag operation for a pricing item.
        /// </summary>
        /// <param name="indices">Tuple containing the old index and the item being dragged.</param>
        private void PricingItemDragStart((int oldIndex, VacationPricingItemDto item) indices)
        {
            _pricingItemBeingDragged = indices.item;
        }

        #endregion

        #region Payment Rules

        /// <summary>
        /// Opens a dialog to create a new payment rule and adds it to the booking form if confirmed.
        /// </summary>
        private async Task CreatePaymentRule()
        {
            var dialog = await DialogService.ShowAsync<PaymentRuleModal>("Confirm");
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                if (NextTab.HasDelegate)
                {
                    Vacation.PaymentRules.Add(((PaymentRuleViewModel)result.Data!).ToDto());
                }
                else
                {
                    var createdItem = ((PaymentRuleViewModel)result.Data!).ToDto();
                    var creationResult = await VacationPricingService.CreatePaymentScheduleEntryAsync(createdItem);
                    if (creationResult.Succeeded)
                    {
                        Vacation.PaymentRules.Add(createdItem);
                    }
                    else
                    {
                        SnackBar.AddErrors(creationResult.Messages);
                    }
                }
            }
        }

        /// <summary>
        /// Updates an existing Payment Schedule Entry associated with the vacation.
        /// Opens a modal dialog pre-filled with the Payment Schedule Entry's current details,
        /// allows the user to modify them, and sends the updated gift DTO
        /// to the backend for persistence. If successful, the gift is updated
        /// in the Payment Schedule Entry ViewModel.
        /// </summary>
        /// <param name="rule">The gift DTO containing the current Payment Schedule Entry details.</param>
        private async Task UpdatePaymentRule(PaymentRuleDto rule)
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            };
            var parameters = new DialogParameters<PaymentRuleModal> { { x => x.PaymentRule, new PaymentRuleViewModel(rule) } };

            var dialog = await DialogService.ShowAsync<PaymentRuleModal>("Confirm", parameters, options);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var modalResult = ((PaymentRuleViewModel)result.Data!).ToDto() with { VacationId = Vacation.VacationId };
                if (NextTab.HasDelegate)
                {
                    var vacationInterval = Vacation.PaymentRules.FirstOrDefault(c => c.PaymentRuleId == rule.PaymentRuleId)!;
                    var index = Vacation.PaymentRules.IndexOf(vacationInterval);
                    Vacation.PaymentRules[index] = modalResult;
                }
                else
                {
                    var updateResult = await VacationPricingService.UpdatePaymentScheduleEntryAsync(modalResult);
                    if (updateResult.Succeeded)
                    {
                        var vacationInterval = Vacation.PaymentRules.FirstOrDefault(c => c.PaymentRuleId == rule.PaymentRuleId)!;
                        var index = Vacation.PaymentRules.IndexOf(vacationInterval);
                        Vacation.PaymentRules[index] = modalResult;
                    }
                }
                
            }
        }

        /// <summary>
        /// Opens a confirmation dialog to remove a payment rule from the booking form.
        /// </summary>
        /// <param name="id">The ID of the payment rule to remove.</param>
        private async Task RemovePaymentRule(string paymentRuleId)
        {
            var parameters = new DialogParameters<ConformtaionModal>();
            parameters.Add(x => x.ContentText, "Are you sure you want to remove this payment rule policy from this application?");
            parameters.Add(x => x.ButtonText, "Yes");
            parameters.Add(x => x.Color, Color.Success);

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                if (NextTab.HasDelegate)
                {
                    var cancellationToRemove = Vacation.PaymentRules.FirstOrDefault(c => c.PaymentRuleId == paymentRuleId);
                    Vacation.PaymentRules.Remove(cancellationToRemove);
                }
                else
                {
                    var removalResult = await VacationPricingService.RemovePaymentScheduleEntryAsync(paymentRuleId);
                    if (removalResult.Succeeded)
                    {
                        var cancellationToRemove = Vacation.PaymentRules.FirstOrDefault(c => c.PaymentRuleId == paymentRuleId);
                        Vacation.PaymentRules.Remove(cancellationToRemove);
                    }
                    else
                    {
                        SnackBar.AddErrors(removalResult.Messages);
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the selection change of a booking terms template.
        /// </summary>
        /// <remarks>Updates the booking terms of the current vacation and processes the description of
        /// the selected template to generate HTML content.</remarks>
        /// <param name="bookingTerms">The selected booking terms template. Cannot be <see langword="null"/>.</param>
        private async Task OnBookingTermsTemplateSelectionChanged(BookingTermsDescriptionTemplateDto bookingTerms)
        {
            if (bookingTerms == null) return;

            Vacation.BookingTerms = bookingTerms;
            var bookingTermsHtml = await ProcessVariableTagsOfText(bookingTerms.Description);
            _bookingTermsText = (MarkupString)bookingTermsHtml;
        }

        /// <summary>
        /// Handles the event when a cancellation terms template is selected.
        /// </summary>
        /// <remarks>Updates the current vacation's cancellation terms and processes the description text
        /// to generate HTML content for display.</remarks>
        /// <param name="cancellationTerms">The selected cancellation terms template. Cannot be null.</param>
        private async Task OnCancellationTermsTemplateSelectionChanged(CancellationTermsTemplateDto cancellationTerms)
        {
            if (cancellationTerms == null) return;

            Vacation.CancellationTerms = cancellationTerms;
            var cancellationTermsHtml = await ProcessVariableTagsOfText(cancellationTerms.Description);
            _cancellationTermsText = (MarkupString)cancellationTermsHtml;
        }

        /// <summary>
        /// Handles the event when a terms and conditions template is selected.
        /// </summary>
        /// <remarks>Updates the current vacation's terms and conditions with the selected template and
        /// processes the template's description to generate HTML content.</remarks>
        /// <param name="termsAndConditions">The selected terms and conditions template. Cannot be null.</param>
        private async Task OnTermsAndConditionsTemplateSelectionChanged(TermsAndConditionsTemplateDto termsAndConditions)
        {
            if (termsAndConditions == null) return;

            Vacation.TermsAndConditions = termsAndConditions;
            var termsAndConditionsHtml = await ProcessVariableTagsOfText(termsAndConditions.Description);
            _termsAndConditionsText = (MarkupString)termsAndConditionsHtml;
        }

        /// <summary>
        /// Handles the selection change of a payment exclusion template.
        /// </summary>
        /// <remarks>Updates the payment exclusion for the current vacation and processes the description 
        /// of the selected template to generate HTML content.</remarks>
        /// <param name="paymentExclusions">The selected <see cref="PaymentExclusionTemplateDto"/> object.  Must not be <see langword="null"/>.</param>
        private async Task OnPaymentExclusionTemplateSelectionChanged(PaymentExclusionTemplateDto paymentExclusions)
        {
            if (paymentExclusions == null) return;

            Vacation.PaymentExclusion = paymentExclusions;
            var paymentExclusionTextHtml = await ProcessVariableTagsOfText(paymentExclusions.Description);
            _paymentExclusionText = (MarkupString)paymentExclusionTextHtml;
        }

        /// <summary>
        /// Handles the event when the selected tab changes.
        /// </summary>
        private async Task OnNextTab()
        {
            await NextTab.InvokeAsync();
        }

        /// <summary>
        /// Invokes the action to navigate to the previous tab asynchronously.
        /// </summary>
        /// <remarks>This method triggers the <see cref="PreviousTab"/> delegate, allowing the caller to
        /// handle the logic for navigating to the previous tab. Ensure that the <see cref="PreviousTab"/> delegate is
        /// not null before invoking this method to avoid runtime exceptions.</remarks>
        /// <returns></returns>
        private async Task OnPreviousTab()
        {
            await PreviousTab.InvokeAsync();
        }

        /// <summary>
        /// Invokes the <see cref="Cancel"/> event asynchronously to signal a cancellation action.
        /// </summary>
        /// <remarks>This method triggers the cancellation logic by invoking the associated event
        /// callback. Ensure that the <see cref="Cancel"/> event is properly configured before calling this
        /// method.</remarks>
        /// <returns></returns>
        private async Task OnCancel()
        {
            await Cancel.InvokeAsync();
        }

        /// <summary>
        /// Invokes the <see cref="Update"/> event asynchronously.
        /// </summary>
        /// <remarks>This method triggers the <see cref="Update"/> event, allowing subscribers to handle
        /// the update operation. Ensure that any event handlers attached to <see cref="Update"/> are thread-safe and
        /// capable of handling asynchronous execution.</remarks>
        /// <returns></returns>
        private async Task OnUpdate()
        {
            await Update.InvokeAsync();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private async Task<string> ProcessVariableTagsOfText(string text)
        {
            var result = text?.Replace("<strong style=\"color: blue\">&lt;---ReferenceNr---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.ReferenceNr}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---HostName---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.Host?.Name}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---ArrivalCity---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.Flights?.FirstOrDefault(c => c.DepartureDayNr == null)?.ArrivalAirport?.City?.Name}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---StartDate---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.StartDate?.ToShortDateString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---EndDate---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.EndDate.Value.ToShortDateString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---NightCount---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.Nights.ToString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---DayCount---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.DayCount.ToString(CultureInfo.InvariantCulture)}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---AccommodationCount---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.RoomCount.ToString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---FlightCount---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.Flights?.Count.ToString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---GolferPackagesCount---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.GolferPackages.Count().ToString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---Consultant---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.Consultant?.Name + Vacation.Consultant?.Surname}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---Coordinator---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.Coordinator?.Name + Vacation.Consultant?.Surname}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---TourDirector---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.Coordinator?.Name + Vacation.Consultant?.Surname}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---CompanyEmail---&gt;</strong>", $"<strong style=\"color: black\">{Configuration["CompanyDetails:EmailAddress"]}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---CompanyPhoneNr---&gt;</strong>", $"<strong style=\"color: black\">{Configuration["CompanyDetails:PhoneNr"]}</strong>");

            var availableCustomVariablePlaceholderResult = await CustomVariableTagService.GetAllAsync();
            if (availableCustomVariablePlaceholderResult.Succeeded)
            {
                foreach (var customTag in availableCustomVariablePlaceholderResult.Data)
                {
                    result = result.Replace($"<strong style=\"color: blue\">&lt;---{customTag.VariablePlaceholder}---&gt;</strong>", $"<strong style=\"color: black\">{customTag.VariablePlaceholder}</strong>");
                }
            }

            return result;
        }

        /// <summary>
        /// Asynchronously initializes the component and prepares data for display.
        /// </summary>
        /// <remarks>This method sets up culture-specific formatting, organizes vacation pricing items
        /// into categories,  and processes various terms and conditions for display. It also invokes the base class's
        /// initialization logic.</remarks>
        protected override async Task OnInitializedAsync()
        {
            nfi = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            nfi.CurrencySymbol = Vacation.CurrencySymbol ?? "$";

            foreach (var item in Vacation.VacationPriceGroups)
            {
                _pricingDropBoxItems.Add(new SortableItem<VacationPricingItemDto>(item.Name, Vacation.VacationPrices.Where(c => c.Selector == item.Name).OrderBy(c => c.Order).ToList()));
            }

            var cancellationTermsTemplateListResult = await CancellationTermsTemplateService.AllAsync();
            if (cancellationTermsTemplateListResult.Succeeded)
            {
                _availableCancellationTermsTemplates = cancellationTermsTemplateListResult.Data.ToList();
            }

            var termsAndConditionsTemplateListResult = await TermsAndConditionsTemplateService.GetAllAsync(); ;
            if (termsAndConditionsTemplateListResult.Succeeded)
            {
                _availableTermsAndConditionsTemplates = termsAndConditionsTemplateListResult.Data.ToList();
            }

            var bookingTermDescriptionTemplateListResult = await BookingTermsDescriptionTemplateService.GetAllAsync(); ;
            if (bookingTermDescriptionTemplateListResult.Succeeded)
            {
                _availableBookingTermsDescriptionTemplates = bookingTermDescriptionTemplateListResult.Data.ToList();
            }

            var paymentExclusionDescriptionTemplateListResult = await PaymentExclusionTemplateService.AllAsync();
            if (paymentExclusionDescriptionTemplateListResult.Succeeded)
            {
                _availablePaymentExclusionsDescriptionTemplates = paymentExclusionDescriptionTemplateListResult.Data.ToList();
            }

            var bookingTermsHtml = Vacation?.BookingTerms == null ? "No Booking Terms available" : await ProcessVariableTagsOfText(Vacation.BookingTerms.Description);
            var paymentExclusionHtml = Vacation?.PaymentExclusion == null ? "No Payment Exclusions available" : await ProcessVariableTagsOfText(Vacation.PaymentExclusion.Description);
            var cancellationTermsHtml = Vacation?.CancellationTerms == null ? "No Cancellation Terms available" : await ProcessVariableTagsOfText(Vacation.CancellationTerms.Description);
            var termsAndConditionsHtml = Vacation?.TermsAndConditions == null ? "No Terms & Conditions available" : await ProcessVariableTagsOfText(Vacation.TermsAndConditions.Description);

            _bookingTermsText = (MarkupString)bookingTermsHtml;
            _paymentExclusionText = (MarkupString)paymentExclusionHtml;
            _cancellationTermsText = (MarkupString)cancellationTermsHtml;
            _termsAndConditionsText = (MarkupString)termsAndConditionsHtml;

            await base.OnInitializedAsync();
        }
    }
}
