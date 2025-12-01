using IdentityModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using MudBlazor;
using PayFast;
using PayFast.Extensions;
using ShoppingModule.Domain.Enums;
using System.Globalization;
using System.Security.Claims;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.Extensions;
using IdentityModule.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using ProductsModule.Domain.Enums;
using ShoppingModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Donations;

/// <summary>
/// Represents a component for creating a new donation, including form validation, submission handling, and error reporting.
/// </summary>
/// <remarks>
/// This component manages the donation creation process by validating the input form, submitting the donation request, and
/// handling success or failure scenarios. It interacts with the <c>DonationService</c> to create donations and provides
/// feedback to the user via a snackbar notification or error messages.
/// </remarks>
public partial class CreateDonation
{
    private CreateDonationRequest _model = new();
    private PayFastSettings? _payFastSettings;
    private string? _payFastProcessUrl;
    private ClaimsPrincipal _user = new ClaimsPrincipal(new ClaimsIdentity());

    private EditForm? _form;
    private bool _isSubmitting;
    private string? _submissionError;
    private readonly Dictionary<string, string> _payFastFormFields = new(StringComparer.OrdinalIgnoreCase);
    private bool _shouldSubmitPayFastForm;
    private ElementReference PayFastForm;

    /// <summary>
    /// Gets the collection of key-value pairs representing the form fields required for a PayFast payment request.
    /// </summary>
    protected IReadOnlyDictionary<string, string> PayFastFormFields => _payFastFormFields;

    /// <summary>
    /// Gets the URL used to initiate a PayFast payment process.
    /// </summary>
    protected string? PayFastProcessUrl => _payFastProcessUrl;

    /// <summary>
    /// Gets or sets the task that provides the current authentication state.
    /// </summary>
    /// <remarks>This property is typically used in Blazor components to access the authentication state of
    /// the current user. The task should not be null and is expected to complete with a valid <see
    /// cref="AuthenticationState"/>.</remarks>
    [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

    /// <summary>
    /// Gets or sets the JavaScript runtime instance used to invoke JavaScript functions from .NET.
    /// </summary>
    /// <remarks>This property is typically populated by the Blazor dependency injection system. Ensure that
    /// the JavaScript runtime is properly configured before using it to invoke JavaScript functions.</remarks>
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    /// <summary>
    /// Gets or sets the HTTP provider used for making HTTP requests.
    /// </summary>
    /// <remarks>This property is typically injected via dependency injection and must be set before
    /// use.</remarks>
    [Inject] public IUserService UserService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications to the user.
    /// </summary>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the application configuration settings.
    /// </summary>
    [Inject] public IConfiguration Configuration { get; set; } = null!;

    /// <summary>
    /// Attempts to submit a payment request to PayFast using the current donation model and the specified user
    /// identifier.
    /// </summary>
    /// <remarks>Returns <see langword="false"/> if the PayFast configuration is invalid or if the donation
    /// frequency is unsupported for recurring donations. This method prepares the payment form and triggers a UI update
    /// if submission is successful.</remarks>
    /// <param name="userId">The unique identifier of the user making the donation. This value is included in the payment request for
    /// tracking purposes.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the payment
    /// request was successfully prepared and submitted; otherwise, <see langword="false"/>.</returns>
    private async Task<bool> TrySubmitPayFastPaymentAsync(string userId)
    {
        if (!ValidatePayFastConfiguration())
        {
            return false;
        }

        var paymentId = Guid.NewGuid().ToString("N");
        var firstName = _model.FirstName ?? string.Empty;
        var lastName = _model.LastName ?? string.Empty;
        var email = _model.Email ?? string.Empty;
        var phone = _model.PhoneNumber ?? string.Empty;

        var request = CreatePayFastRequest();

        request.name_first = firstName;
        request.name_last = lastName;
        request.email_address = email;
        request.cell_number = phone;
        request.m_payment_id = paymentId;
        request.amount = Math.Round(_model.Amount, 2, MidpointRounding.AwayFromZero);

        request.item_name = $"Donation {paymentId}";
        request.item_description = $"Donation {paymentId} from {firstName} {lastName} was made for {_model.Designation}";
        request.custom_str1 = paymentId;
        request.custom_str2 = userId;
        request.custom_str3 = _model.Designation;
        request.custom_str4 = _model.Message;

        request.custom_int3 = _model.IsAnonymous ? 1 : 0;
        request.custom_int4 = _model.IsRecurring ? 1 : 0;
        request.custom_int5 = (int)_model.Frequency;

        if (_model.IsRecurring)
        {
            var frequency = MapBillingFrequency(_model.Frequency);
            if (frequency is null)
            {
                SnackBar.Add($"Unsupported donation frequency {_model.Frequency} for recurring donation {paymentId}.", Severity.Error);
                return false;
            }

            request.subscription_type = SubscriptionType.Subscription;
            request.frequency = frequency;
            request.billing_date = DateTime.Today;
            request.recurring_amount = _model.Amount;
            request.cycles = 0;
        }

        PopulatePayFastFormFields(request);
        _shouldSubmitPayFastForm = true;
        await InvokeAsync(StateHasChanged);

        return true;
    }

    /// <summary>
    /// Maps a donation frequency value to the corresponding billing frequency, if a mapping exists.
    /// </summary>
    /// <param name="frequency">The donation frequency to convert to a billing frequency.</param>
    /// <returns>A <see cref="BillingFrequency"/> value that corresponds to the specified donation frequency, or <see
    /// langword="null"/> if the frequency does not have a corresponding billing frequency.</returns>
    private static PayfastBillingFrequency? MapBillingFrequency(DonationFrequency frequency) => frequency switch
    {
        DonationFrequency.Monthly => PayfastBillingFrequency.Monthly,
        DonationFrequency.Quarterly => PayfastBillingFrequency.Quarterly,
        DonationFrequency.Annually => PayfastBillingFrequency.Annual,
        _ => null
    };

    /// <summary>
    /// Validates that the PayFast payment gateway configuration is present and contains all required values.
    /// </summary>
    /// <remarks>If the configuration is invalid, an error message is added to the user interface to indicate
    /// the missing or incomplete settings. This method should be called before initiating any PayFast payment
    /// operations to ensure that the gateway is properly configured.</remarks>
    /// <returns>true if the PayFast configuration is valid and all required fields are set; otherwise, false.</returns>
    private bool ValidatePayFastConfiguration()
    {
        if (_payFastSettings is null)
        {
            SnackBar.Add("PayFast configuration is missing.", Severity.Error);
            return false;
        }

        if (string.IsNullOrWhiteSpace(_payFastSettings.MerchantId) || string.IsNullOrWhiteSpace(_payFastSettings.MerchantKey))
        {
            SnackBar.Add("PayFast merchant details are not configured.", Severity.Error);
            return false;
        }

        if (string.IsNullOrWhiteSpace(_payFastSettings.ProcessUrl) || string.IsNullOrWhiteSpace(_payFastSettings.ReturnUrl) || string.IsNullOrWhiteSpace(_payFastSettings.CancelUrl) || string.IsNullOrWhiteSpace(_payFastSettings.NotifyUrl))
        {
            SnackBar.Add("PayFast URLs are not fully configured.", Severity.Error);
            return false;
        }

        _payFastProcessUrl = _payFastSettings.ProcessUrl;

        return true;
    }

    /// <summary>
    /// Creates and configures a new PayFastRequest instance using the current PayFast settings and application
    /// configuration.
    /// </summary>
    /// <remarks>The returned PayFastRequest includes merchant credentials and callback URLs sourced from both
    /// the PayFast settings and application configuration. This method ensures that all required fields for a standard
    /// PayFast payment flow are set appropriately.</remarks>
    /// <returns>A PayFastRequest object populated with merchant and URL information required for initiating a PayFast payment.</returns>
    private PayFastRequest CreatePayFastRequest()
    {
        var request = string.IsNullOrWhiteSpace(_payFastSettings?.PassPhrase)
            ? new PayFastRequest()
            : new PayFastRequest(_payFastSettings!.PassPhrase);

        request.merchant_id = Configuration["PayFast:MerchantId"];
        request.merchant_key = Configuration["PayFast:MerchantKey"];
        request.return_url = _payFastSettings?.ReturnUrl ?? Configuration["PayFast:DonationsReturnUrl"];
        request.cancel_url = Configuration["PayFast:CancelUrl"];
        request.notify_url =Configuration["PayFast:DonationsNotifyUrl"];
        request.custom_str5 = _payFastSettings?.RedirectUrl ?? _payFastSettings?.ReturnUrl ?? Configuration["PayFast:DonationsReturnUrl"];

        return request;
    }

    /// <summary>
    /// Populates the PayFast form fields with the data provided in the <see cref="PayFastRequest"/> object.
    /// </summary>
    /// <remarks>This method clears any existing form fields before populating them with the values
    /// from the <paramref name="request"/>. Required fields are always added, while optional fields are added only
    /// if they contain non-null and non-whitespace values. The method also ensures that numeric and boolean values
    /// are formatted appropriately for the PayFast API.</remarks>
    /// <param name="request">The <see cref="PayFastRequest"/> containing the data to populate the form fields. This includes required
    /// fields such as merchant details, payment information, and optional fields for additional metadata.</param>
    private void PopulatePayFastFormFields(PayFastRequest request)
    {
        _payFastFormFields.Clear();

        void AddField(string key, string? value)
        {
            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                _payFastFormFields[key] = value;
            }
        }

        void AddOptionalField(string key, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _payFastFormFields[key] = value;
            }
        }

        AddField("merchant_id", request.merchant_id);
        AddField("merchant_key", request.merchant_key);
        AddField("return_url", request.return_url);
        AddField("cancel_url", request.cancel_url);
        AddField("notify_url", request.notify_url);
        AddField("m_payment_id", request.m_payment_id);
        AddField("amount", request.amount.ToString("F2", CultureInfo.InvariantCulture));
        AddField("item_name", request.item_name);
        AddOptionalField("item_description", request.item_description);
        AddOptionalField("name_first", request.name_first);
        AddOptionalField("name_last", request.name_last);
        AddOptionalField("email_address", request.email_address);
        AddOptionalField("cell_number", request.cell_number);
        AddOptionalField("custom_str1", request.custom_str1);
        AddOptionalField("custom_str2", request.custom_str2);
        AddOptionalField("custom_str3", request.custom_str3);
        AddOptionalField("custom_str4", request.custom_str4);
        AddOptionalField("custom_str5", request.custom_str5);
        AddOptionalField("custom_int1", request.custom_int1?.ToString(CultureInfo.InvariantCulture));
        AddOptionalField("custom_int2", request.custom_int2?.ToString(CultureInfo.InvariantCulture));
        AddOptionalField("custom_int3", request.custom_int3?.ToString(CultureInfo.InvariantCulture));
        AddOptionalField("custom_int4", request.custom_int4?.ToString(CultureInfo.InvariantCulture));
        AddOptionalField("custom_int5", request.custom_int5?.ToString(CultureInfo.InvariantCulture));

        if (request.email_confirmation.HasValue)
        {
            _payFastFormFields["email_confirmation"] = request.email_confirmation.Value ? "1" : "0";
        }

        if (_model.IsRecurring && request.frequency.HasValue)
        {
            AddField("subscription_type", ((int)request.subscription_type.Value).ToString(CultureInfo.InvariantCulture));
            AddField("frequency", ((int)request.frequency.Value).ToString(CultureInfo.InvariantCulture));
            AddField("billing_date", request.billing_date?.ToPayFastDate() ?? DateTime.Today.ToPayFastDate());
            AddField("recurring_amount", (request.recurring_amount ?? request.amount).ToString("F2", CultureInfo.InvariantCulture));
            AddField("cycles", (request.cycles ?? 0).ToString(CultureInfo.InvariantCulture));
        }

        AddField("signature", request.signature);
    }

    /// <summary>
    /// Handles the submission of a valid donation form asynchronously.
    /// </summary>
    /// <remarks>
    /// This method validates the form, submits the donation request, and processes the result. If the form is invalid, the
    /// submission is aborted. On successful submission, the component prepares the relevant payment redirect and resets the form.
    /// If an error occurs during submission, an appropriate error message is set.
    /// </remarks>
    private async Task HandleValidSubmitAsync()
    {
        
        _isSubmitting = true;
        _submissionError = null;

        try
        {
            var submitted = await TrySubmitPayFastPaymentAsync(_user.GetUserId());
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Unhandled error while creating donation");
            _submissionError = _localizer["GenericError"];
        }
        finally
        {
            _isSubmitting = false;
        }
    }

    /// <summary>
    /// Asynchronously initializes the component and loads user-specific and PayFast configuration data.
    /// </summary>
    /// <remarks>This method is called by the Blazor framework during component initialization. It retrieves
    /// the current user's authentication state and user information, and loads PayFast settings from the application
    /// configuration. Override this method to perform additional asynchronous setup when the component is
    /// initialized.</remarks>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        _payFastSettings = Configuration.GetSection("PayFast").Get<PayFastSettings>();
        _payFastProcessUrl = _payFastSettings?.ProcessUrl;

        var authState = await AuthenticationStateTask;
        _user = authState.User;

        var userInfoResult = await UserService.GetUserInfoAsync(_user.GetUserId());
        userInfoResult.ProcessResponseForDisplay(SnackBar, () =>
        {
            var userInfo = userInfoResult.Data;
            _model.FirstName = $"{userInfo.FirstName}";
            _model.LastName = $"{userInfo.LastName}";
            _model.PhoneNumber = $"{userInfo.PhoneNr}";
            _model.Email = userInfo.EmailAddress;
        });
    }

    /// <summary>
    /// Invoked after the component has rendered. Performs additional actions such as submitting the PayFast form if
    /// required.
    /// </summary>
    /// <remarks>This method is called by the Blazor framework after each render. If a PayFast form submission
    /// is pending, it attempts to submit the form using JavaScript interop and logs any errors encountered during the
    /// process.</remarks>
    /// <param name="firstRender">true if this is the first time the component has rendered; otherwise, false.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (_shouldSubmitPayFastForm)
        {
            _shouldSubmitPayFastForm = false;

            try
            {
                await JSRuntime.InvokeVoidAsync("submitPayFastForm", PayFastForm);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Failed to submit PayFast form for donation payment.");
            }
        }
    }

    /// <summary>
    /// Initiates the payment redirect process using the specified donation submission result.
    /// </summary>
    /// <remarks>This method prepares and triggers a redirect to the payment provider if the submission result
    /// includes valid redirect information. It updates internal state to facilitate the redirect process.</remarks>
    /// <param name="submission">The result of the donation submission containing payment redirect information. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task TriggerPaymentRedirectAsync(DonationSubmissionResult submission)
    {
        if (submission.PayFastRedirect is { ProcessUrl.Length: > 0 } payFastRedirect)
        {
            PopulateFormFields(_payFastFormFields, payFastRedirect.Fields);
            _payFastProcessUrl = payFastRedirect.ProcessUrl;
            _shouldSubmitPayFastForm = _payFastFormFields.Count > 0;

            if (_shouldSubmitPayFastForm)
            {
                await InvokeAsync(StateHasChanged);
            }
        }
    }

    /// <summary>
    /// Populates the target dictionary with non-empty key-value pairs from the specified fields collection, replacing
    /// any existing entries.
    /// </summary>
    /// <param name="target">The dictionary to populate with form field data. All existing entries are cleared before new values are added.</param>
    /// <param name="fields">A collection of key-value pairs representing form fields to copy to the target dictionary. Only entries with
    /// non-empty keys and values are included. If null, the target dictionary is cleared and left empty.</param>
    private void PopulateFormFields(IDictionary<string, string> target, IDictionary<string, string>? fields)
    {
        target.Clear();

        if (fields is null)
        {
            return;
        }

        foreach (var field in fields)
        {
            if (!string.IsNullOrWhiteSpace(field.Key) && !string.IsNullOrWhiteSpace(field.Value))
            {
                target[field.Key] = field.Value;
            }
        }
    }

    /// <summary>
    /// Resolves an appropriate error message based on the provided donation submission result and submission details.
    /// </summary>
    /// <param name="result">The result object containing messages related to the donation submission process. Cannot be null.</param>
    /// <param name="submission">The donation submission result, which may include a specific error message. Can be null.</param>
    /// <returns>A string containing the most relevant error message. Returns the error message from the submission if available;
    /// otherwise, returns a concatenated list of messages from the result, or a generic error message if none are
    /// present.</returns>
    private string ResolveErrorMessage(IBaseResult<DonationSubmissionResult> result, DonationSubmissionResult? submission)
    {
        if (submission is { ErrorMessage.Length: > 0 } errorMessage)
        {
            return errorMessage.ErrorMessage;
        }

        if (result.Messages?.Any() == true)
        {
            return string.Join(";", result.Messages);
        }

        return _localizer["GenericError"];
    }
}
