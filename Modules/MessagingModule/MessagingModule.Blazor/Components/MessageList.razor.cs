using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using MudBlazor;
using MessagingModule.Domain.DataTransferObjects;

namespace MessagingModule.Blazor.Components
{
    /// <summary>
    /// Displays a list of chat messages in a styled and aligned layout based on sender identity.
    /// </summary>
    public partial class MessageList
    {
        private ElementReference _container;
        private int _prevCount; 
        private IJSObjectReference? _module;

        #region Parameters

        /// <summary>
        /// Gets or sets the list of chat messages to render.
        /// </summary>
        [Parameter] public List<ChatMessageDto> Messages { get; set; } = [];

        /// <summary>
        /// Gets or sets the current user's ID to distinguish between sent and received messages.
        /// </summary>
        [Parameter] public string CurrentUserId { get; set; }

        /// <summary>
        /// Gets or sets the current user's name.
        /// </summary>
        [Parameter] public string CurrentUserName { get; set; }

        /// <summary>
        /// Gets or sets a dictionary containing read times for each message.
        /// </summary>
        [Parameter] public Dictionary<string, DateTime?> ReadTimes { get; set; } = new();

        #endregion

        #region Injections

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the JavaScript runtime abstraction used to invoke JavaScript functions from .NET code.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework and should
        /// not be set manually in most cases.</remarks>
        [Inject] public IJSRuntime JS { get; set; } = null!;

        #endregion

        #region Utility Methods

        /// <summary>
        /// Returns the CSS class to apply based on whether the message was sent or received.
        /// </summary>
        /// <param name="msg">The chat message being rendered.</param>
        /// <returns>
        /// A CSS class indicating left or right alignment and background color.
        /// </returns>
        private string GetMessageClass(ChatMessageDto msg)
            => msg.SenderId == CurrentUserId
                ? "ml-auto bg-green-100"  // Sent messages
                : "mr-auto bg-white";     // Received messages

        /// <summary>
        /// Returns the alignment for a message container based on the sender.
        /// </summary>
        /// <param name="msg">The chat message to align.</param>
        /// <returns>
        /// <see cref="Justify.FlexEnd"/> if sent by current user; otherwise, <see cref="Justify.FlexStart"/>.
        /// </returns>
        private Justify GetMessageAlignment(ChatMessageDto msg) => msg.SenderId == CurrentUserId ? Justify.FlexEnd : Justify.FlexStart;

        /// <summary>
        /// Extracts and returns the initials from a full name (e.g., "John Doe" => "JD").
        /// </summary>
        /// <param name="name">The full name of the user.</param>
        /// <returns>A string containing the uppercase initials.</returns>
        private string GetInitials(string name)
        {
            var parts = name.Split(' ');
            return string.Concat(parts.Select(p => p.FirstOrDefault())).ToUpperInvariant();
        }

        /// <summary>
        /// Scrolls the message container to the bottom asynchronously.
        /// </summary>
        /// <remarks>This method ensures that the message container is scrolled to the bottom, typically
        /// used to display the most recent messages in a chat or messaging interface. It relies on a JavaScript module
        /// to perform the scrolling operation.</remarks>
        /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
        private async ValueTask ScrollToBottomAsync()
        {
            if (_module is null)
                _module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/MessagingModule.Blazor/js/messageList.js");
            await _module.InvokeVoidAsync("scrollToBottom", _container);

            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Invoked after the component has rendered. Executes additional logic if this is the first render.
        /// </summary>
        /// <remarks>If <paramref name="firstRender"/> is <see langword="true"/>, this method initializes
        /// the previous message count and performs an asynchronous operation to scroll to the bottom of the message
        /// list.</remarks>
        /// <param name="firstRender">A boolean value indicating whether this is the first time the component has been rendered. <see
        /// langword="true"/> if this is the first render; otherwise, <see langword="false"/>.</param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _prevCount = Messages.Count;
                await ScrollToBottomAsync();
            }
        }

        /// <summary>
        /// Invoked when the component's parameters have been set.  Checks for changes in the message count and scrolls
        /// to the bottom if necessary.
        /// </summary>
        /// <remarks>This method ensures that the component reacts to changes in the <see
        /// cref="Messages"/> collection  by scrolling to the bottom of the message list when the count changes. It is
        /// called automatically  by the Blazor framework during the component's lifecycle.</remarks>
        /// <returns></returns>
        protected override async Task OnParametersSetAsync()
        {
            if (Messages.Count != _prevCount)
            {
                _prevCount = Messages.Count;
                await ScrollToBottomAsync();
            }
        }

        #endregion
    }

}
