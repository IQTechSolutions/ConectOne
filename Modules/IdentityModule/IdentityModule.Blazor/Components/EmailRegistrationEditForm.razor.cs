using IdentityModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace IdentityModule.Blazor.Components
{
    public partial class EmailRegistrationEditForm
    {
        /// <summary>
        /// Gets or sets the content to be rendered inside the component.
        /// </summary>
        /// <remarks>Use this parameter to specify child elements or markup that should appear within the
        /// component's output. Typically set implicitly by placing content between the component's opening and closing
        /// tags in Razor syntax.</remarks>
        [Parameter] public RenderFragment ChildContent { get; set; } = null!;

        /// <summary>
        /// Gets or sets a reference to the registration form used for editing user input.
        /// </summary>
        /// <remarks>This property allows access to the underlying <see cref="EditForm"/> instance,
        /// enabling programmatic interaction with the form, such as triggering validation or submission. The property
        /// is typically set by the Blazor framework when the component is rendered.</remarks>
        [Parameter] public EditForm RegistrationFormReference { get; set; } = null!;

        /// <summary>
        /// Gets or sets the model containing user registration data for email-based registration.
        /// </summary>
        [Parameter, EditorRequired] public EmailRegistrationViewModel RegisterUserModel { get; set; } = null!;

        /// <summary>
        /// Gets or sets the callback that is invoked when a cancel action is requested.
        /// </summary>
        /// <remarks>This event is typically triggered when the user cancels an operation, such as closing
        /// a dialog or dismissing a form. Assign a handler to perform any necessary cleanup or rollback actions in
        /// response to the cancellation.</remarks>
        [Parameter, EditorRequired] public EventCallback OnCancel { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when the form is submitted.
        /// </summary>
        /// <remarks>This callback is triggered when the user submits the form, allowing custom logic to
        /// be executed in response to the submission event. The callback does not receive any event arguments. This
        /// property is required and must be set for the form to function correctly.</remarks>
        [Parameter, EditorRequired] public EventCallback OnSubmit { get; set; }

        /// <summary>
        /// Gets or sets the text to display as the title.
        /// </summary>
        [Parameter, EditorRequired] public string TitleText { get; set; } = null!;

        /// <summary>
        /// Gets or sets the text displayed on the submit button.
        /// </summary>
        [Parameter, EditorRequired] public string SubmitButtonText { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the company input field is disabled.
        /// </summary>
        [Parameter] public bool IsCompanyInputDisabled { get; set; }

        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
        private bool _passwordVisibility;

        /// <summary>
        /// Toggles the visibility state of the password input field between visible and hidden modes.
        /// </summary>
        /// <remarks>This method updates the internal state to switch the password input between plain
        /// text and masked entry. It also updates any associated UI elements, such as the visibility icon, to reflect
        /// the current state. Typically used in response to user actions, such as clicking a 'show/hide password'
        /// button.</remarks>
        private void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }

        /// <summary>
        /// Asynchronously triggers the cancellation logic associated with this instance.
        /// </summary>
        /// <returns>A task that represents the asynchronous cancellation operation.</returns>
        private async Task Cancel()
        {
            await OnCancel.InvokeAsync();
        }

        /// <summary>
        /// Invokes the submit action asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous submit operation.</returns>
        private async Task Submit()
        {
            await OnSubmit.InvokeAsync();
        }
    }
}
