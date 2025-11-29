using FilingModule.Blazor.Modals;
using IdentityModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;

namespace SchoolsEnterprise.Blazor.Shared.Pages.Authentication
{
    /// <summary>
    /// Represents a user profile component that provides functionality for handling user interactions such as
    /// submitting, canceling, navigating back, and updating the cover image.
    /// </summary>
    /// <remarks>This class is designed to be used in a Blazor application. It includes parameters for
    /// handling user interaction events and managing user profile data. The component exposes methods to trigger these
    /// events and update the user's cover image.</remarks>
    public partial class Profile
    {
        /// <summary>
        /// Gets or sets the user information associated with the current context.
        /// </summary>
        [Parameter] public UserInfoViewModel User { get; set; } = new UserInfoViewModel();

        /// <summary>
        /// Gets or sets the callback to be invoked when a submit action occurs.
        /// </summary>
        /// <remarks>The callback receives a <see cref="string"/> parameter representing the submitted
        /// value.</remarks>
        [Parameter] public EventCallback<string> OnSubmit { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when a cancel action occurs.
        /// </summary>
        /// <remarks>The callback is triggered when a cancel event is raised, allowing the caller to
        /// handle the event and perform any necessary actions.</remarks>
        [Parameter] public EventCallback<string> OnCancel { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when a "Back" action occurs.
        /// </summary>
        /// <remarks>Use this property to handle "Back" navigation or similar actions in a component. The
        /// callback receives a string parameter that can be used to identify the source or context of the
        /// action.</remarks>
        [Parameter] public EventCallback<string> OnBack { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when the cover image is changed.
        /// </summary>
        /// <remarks>This callback is typically used to handle updates to the cover image, such as saving
        /// the new image URL or performing additional actions when the cover image is modified.</remarks>
        [Parameter] public EventCallback<string> OnCoverImageChanged { get; set; }

        /// <summary>
        /// Cancels the current operation by invoking the associated cancellation logic asynchronously.
        /// </summary>
        /// <remarks>This method triggers the <see cref="OnCancel"/> event, allowing subscribers to handle
        /// the cancellation process. Ensure that any necessary cleanup or rollback logic is implemented in the event
        /// handler.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task Cancel()
        {
            await OnCancel.InvokeAsync();
        }

        /// <summary>
        /// Invokes the <see cref="OnSubmit"/> event asynchronously.
        /// </summary>
        /// <remarks>This method triggers the <see cref="OnSubmit"/> event, allowing subscribers to handle
        /// the submission process. Ensure that any event handlers attached to <see cref="OnSubmit"/> are prepared to
        /// execute asynchronously.</remarks>
        /// <returns></returns>
        public async Task Submit()
        {
            await OnSubmit.InvokeAsync();
        }

        /// <summary>
        /// Triggers the asynchronous back navigation event.
        /// </summary>
        /// <remarks>This method invokes the <see cref="OnBack"/> event handler asynchronously.  It is
        /// typically used to signal a back navigation action in the application.</remarks>
        /// <returns></returns>
        public async Task Back()
        {
            await OnBack.InvokeAsync();
        }

        /// <summary>
        /// Updates the user's cover image URL based on the provided image data.
        /// </summary>
        /// <param name="coverImage">The image data containing the new cover image in Base64 format.</param>
        public void CoverImageChanged(MudCropperResponse coverImage)
        {
            User.CoverImageUrl = coverImage.Base64String;
        }
    }
}
