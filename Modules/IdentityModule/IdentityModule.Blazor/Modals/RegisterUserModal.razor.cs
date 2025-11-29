using IdentityModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace IdentityModule.Blazor.Modals
{
    /// <summary>
    /// Represents a modal dialog for registering a new user.
    /// </summary>
    /// <remarks>This component provides a user interface for registering a new user, including fields for
    /// user details and optional pre-filled company information. The modal supports customization of the title, button
    /// text, and API endpoint for registration. It also allows cancellation or submission of the registration
    /// form.</remarks>
    public partial class RegisterUserModal
    {        
        private EmailRegistrationViewModel _registerUserModel = new();
        private bool _isCompanyInputDisabled = false;

        /// <summary>
        /// Gets or sets the instance of the dialog used for managing dialog operations within the component.
        /// </summary>
        /// <remarks>This property is typically set automatically by the cascading parameter mechanism in
        /// Blazor and should not be set manually in most cases.</remarks>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }

        /// <summary>
        /// Gets or sets the API path used to specify the endpoint for the request.
        /// </summary>
        /// <remarks>Ensure that the value provided is properly formatted and corresponds to a valid API
        /// endpoint.</remarks>
        [Parameter] public string ApiPath { get; set; }

        /// <summary>
        /// Gets or sets the title displayed for the registration form.
        /// </summary>
        [Parameter] public string Title { get; set; } = "Register New User";

        /// <summary>
        /// Gets or sets the text displayed on the button.
        /// </summary>
        [Parameter] public string ButtonText { get; set; } = "Register";

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        [Parameter] public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the team.
        /// </summary>
        [Parameter] public string TeamId { get; set; }

        /// <summary>
        /// Cancels the current dialog and closes it.
        /// </summary>
        /// <remarks>This method invokes the cancel operation on the dialog, signaling that the dialog
        /// should be closed without confirming any changes.</remarks>
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        /// <summary>
        /// Submits the registration request asynchronously.
        /// </summary>
        /// <remarks>This method attempts to register a user by sending the provided registration data to
        /// the  registration service. If the registration is successful, a success message is displayed,  and the
        /// dialog is closed with the registered user model. If the registration fails, error  messages are displayed,
        /// and the dialog is canceled.</remarks>
        /// <returns></returns>
        private async Task SubmitAsync()
        {
            try
            {
                //var response = await AccountsProvider.RegisterUserAsync(new RegistrationRequest(_registerUserModel));
                //if (response.IsSuccessfulRegistration)
                //{

                //    Snackbar.Add($"{_registerUserModel.FirstName} {_registerUserModel.LastName} Successfully Registered with Meishi", Severity.Success);
                //    MudDialog.Close(DialogResult.Ok(_registerUserModel));
                //}
                //else
                //{
                //    foreach (var message in response.Errors)
                //    {
                //        Snackbar.Add(message, Severity.Error);
                //    }
                //    MudDialog.Cancel();
                //}
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        /// <summary>
        /// Asynchronously initializes the component and sets up the initial state based on the provided company
        /// information.
        /// </summary>
        /// <remarks>If the <see cref="CompanyName"/> property is not null or empty, the company name and
        /// team ID are pre-populated  in the registration model, and the company input field is disabled.</remarks>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if(!string.IsNullOrEmpty(CompanyName))
            {
                _registerUserModel.CompanyName = CompanyName;
                _registerUserModel.TeamId = TeamId;
                _isCompanyInputDisabled = true;
            }    
        }
    }
}
