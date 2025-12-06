using System.Collections.ObjectModel;
using AccomodationModule.Domain.Constants;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using Radzen;

namespace KiburiOnline.Blazor.Shared.Pages.Contacts
{
    /// <summary>
    /// Represents the list page for managing teachers.
    /// This component displays a table of teachers and provides functionality to search, sort, and delete teachers.
    /// </summary>
    public partial class List
    {
        private bool _loaded;

        private bool _dense;
        private bool _striped = true;
        private bool _bordered;

        private bool _canSearchContacts;
        private bool _canCreateContact;
        private bool _canEditContact;
        private bool _canDeleteContact;

        private ObservableCollection<ContactDto> _contacts = [];
        private IList<ContactDto> selectedContacts;
        private ContactDto draggedItem;

        #region Injections

        /// <summary>
        /// Gets or sets the task that provides the current authentication state.
        /// </summary>
        /// <remarks>This property is typically used in Blazor applications to access the user's
        /// authentication state. The <see cref="AuthenticationState"/> object contains information about the user's
        /// identity and authentication status.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for handling authorization operations.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected instance of <see cref="ISnackbar"/> used to display notifications or messages.
        /// </summary>
        [Inject] private ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the dialog service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage contact-related operations.
        /// </summary>
        [Inject] public IContactService ContactService { get; set; } = null!;

        #endregion

        /// <summary>
        /// Configures the attributes and event handlers for a data row to enable drag-and-drop functionality.
        /// </summary>
        /// <remarks>This method adds attributes to the row to make it draggable, including setting the
        /// row's ID, title, and style. It also defines event handlers for drag-and-drop operations, such as starting a
        /// drag, handling drag-over and drag-leave events, and processing the drop event. The drop event updates the
        /// order of the rows and persists the changes to the data source.  The following attributes are added to the
        /// row: <list type="bullet"> <item><description><c>id</c>: A unique identifier for the row based on the
        /// contact's ID.</description></item> <item><description><c>title</c>: A tooltip indicating that the row can be
        /// dragged to reorder.</description></item> <item><description><c>style</c>: Sets the cursor to "grab" to
        /// indicate draggable behavior.</description></item> <item><description><c>draggable</c>: Enables the row to be
        /// draggable.</description></item> <item><description><c>ondragover</c>: Prevents the default drag-over
        /// behavior and adds a CSS class to the row.</description></item> <item><description><c>ondragleave</c>:
        /// Removes the CSS class added during the drag-over event.</description></item>
        /// <item><description><c>ondragstart</c>: Initializes the drag operation by storing the dragged
        /// item.</description></item> <item><description><c>ondrop</c>: Handles the drop event, reorders the rows,
        /// updates their order in the data source, and removes the temporary CSS class.</description></item> </list> 
        /// Additionally, a JavaScript function is invoked to set a custom drag image for the row.</remarks>
        /// <param name="args">The event arguments containing the row data and attributes to be modified. The <see
        /// cref="RowRenderEventArgs{T}.Data"/> property provides the data object for the row, and the <see
        /// cref="RowRenderEventArgs{T}.Attributes"/> dictionary allows adding custom attributes.</param>
        private void RowRender(RowRenderEventArgs<ContactDto> args)
        {
            args.Attributes.Add("id", $"row-{args.Data.ContactId}");
            args.Attributes.Add("title", "Drag row to reorder");
            args.Attributes.Add("style", "cursor:grab");
            args.Attributes.Add("draggable", "true");
            args.Attributes.Add("ondragover", "event.preventDefault();event.target.closest('.rz-data-row').classList.add('my-class');");
            args.Attributes.Add("ondragleave", "event.target.closest('.rz-data-row').classList.remove('my-class')");
            
            args.Attributes.Add("ondragstart", EventCallback.Factory.Create<DragEventArgs>(this, () =>
            {
                draggedItem = args.Data;

            }));
            args.Attributes.Add("ondrop", EventCallback.Factory.Create<DragEventArgs>(this, async () =>
            {
                var draggedIndex = _contacts.IndexOf(draggedItem);
                var droppedIndex = _contacts.IndexOf(args.Data);
                _contacts.Remove(draggedItem);
                _contacts.Insert(draggedIndex <= droppedIndex ? droppedIndex++ : droppedIndex, draggedItem);

                for (var i = 0; i < _contacts.Count; i++)
                {
                    _contacts[i].Order = i;
                    var updateResult = await ContactService.EditAsync(_contacts[i]);
                    if (!updateResult.Succeeded) SnackBar.AddErrors(updateResult.Messages);
                }
                JSRuntime.InvokeVoidAsync("eval", $"document.querySelector('.my-class').classList.remove('my-class')");
            }));

            JSRuntime.InvokeVoidAsync("setDragImageToRow", $"row-{args.Data.ContactId}");
        }

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Checks the user's permissions to determine if they can create, edit, or delete teachers.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var currentUser = authState.User;

            _canSearchContacts = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Contact.Search)).Succeeded;
            _canCreateContact = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Contact.Create)).Succeeded;
            _canEditContact = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Contact.Edit)).Succeeded;
            _canDeleteContact = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Contact.Delete)).Succeeded;

            var request = await ContactService.GetAllAsync();
            if (request.Succeeded)
            {
                _contacts = new ObservableCollection<ContactDto>(request.Data.OrderBy(c => c.Order));
                selectedContacts = new List<ContactDto>() { _contacts.FirstOrDefault() };
            }
            else
            {
                SnackBar.AddErrors(request.Messages);
            }
                
            _loaded = true;

            await base.OnInitializedAsync();
        }
        
        /// <summary>
        /// Deletes a guides by their ID after user confirmation.
        /// </summary>
        /// <param name="guideId">The ID of the guide to delete.</param>
        private async Task DeleteGuide(string guideId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this guide from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var deleteResult = await ContactService.DeleteAsync(guideId);
                if (deleteResult.Succeeded)
                {
                    var item = _contacts.FirstOrDefault(c => c.ContactId == guideId);
                    _contacts.Remove(item);
                }
                else
                {
                    SnackBar.AddErrors(deleteResult.Messages);
                }
            }
        }
    }
}
