using MessagingModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;

namespace MessagingModule.Blazor.Components
{
    /// <summary>
    /// Represents a list of chat groups and provides functionality for handling group member selection events.
    /// </summary>
    /// <remarks>This component is designed to display and manage a collection of chat groups. It allows the
    /// caller to handle events when a group member is selected.</remarks>
    public partial class ChatGroupList
    {
        /// <summary>
        /// Gets or sets the collection of chat groups.
        /// </summary>
        [Parameter, EditorRequired] public List<ChatGroupDto> ChatGroups { get; set; } = [];

        /// <summary>
        /// Gets or sets the callback that is invoked when a group member is selected.
        /// </summary>
        [Parameter] public EventCallback<ChatGroupDto> OnGroupMemberSelected { get; set; }

        /// <summary>
        /// Handles the selection of a group member and triggers the associated event.
        /// </summary>
        /// <remarks>This method invokes the <see cref="OnGroupMemberSelected"/> event asynchronously,
        /// passing the selected group as an argument. Ensure that <see cref="OnGroupMemberSelected"/> is properly
        /// initialized before calling this method.</remarks>
        /// <param name="group">The selected chat group represented as a <see cref="ChatGroupDto"/>.</param>
        private async Task GroupMemberSelected(ChatGroupDto group)
        {
            await OnGroupMemberSelected.InvokeAsync(group);
        }
    }
}
