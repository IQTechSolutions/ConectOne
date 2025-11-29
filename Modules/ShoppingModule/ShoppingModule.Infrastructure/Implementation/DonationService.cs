using System.Globalization;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PayFast;
using PayFast.Extensions;
using ShoppingModule.Domain.Entities;
using ShoppingModule.Domain.Enums;
using ShoppingModule.Domain.Interfaces;
using ShoppingModule.Domain.RequestFeatures;

namespace ShoppingModule.Infrastructure.Implementation;

/// <summary>
/// Provides functionality for managing donations, including creating and persisting donation records.
/// </summary>
/// <remarks>This service acts as a mediator between the application and the underlying data repository for
/// donation-related operations. It ensures that donation records are created and saved in a consistent and reliable
/// manner.</remarks>
public class DonationService : IDonationService
{
    private readonly IRepository<Donation, string> _repository;
    private readonly ILogger<DonationService> _logger;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="DonationService"/> class, which provides functionality for managing donations.
    /// </summary>
    /// <param name="repository">The repository used to manage <see cref="Donation"/> entities.</param>
    /// <param name="logger">The logger instance used for logging operations within the service.</param>
    /// <param name="configuration">Provides access to application configuration, including payment gateway settings.</param>
    public DonationService(IRepository<Donation, string> repository, ILogger<DonationService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new donation entry asynchronously based on the provided request.
    /// </summary>
    /// <remarks>This method attempts to create a new donation entry and save it to the repository. If the
    /// operation fails at any stage, the result will indicate failure along with the associated error
    /// messages.</remarks>
    /// <param name="request">The request object containing the details of the donation to be created.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// where <c>T</c> is <see cref="DonationSubmissionResult"/>. The result indicates whether the operation succeeded
    /// and includes any relevant messages.</returns>
    public async Task<IBaseResult<DonationSubmissionResult>> CreateDonationAsync(CreateDonationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var donationReference = Guid.NewGuid().ToString("N");

            var payFastRedirect = CreatePayFastRedirect(request, donationReference);
            if (payFastRedirect is null)
            {
                return await Result<DonationSubmissionResult>.FailAsync("Unable to prepare PayFast payment request.");
            }

            var payload = DonationSubmissionResult.Success(donationReference, null, payFastRedirect);
            return await Result<DonationSubmissionResult>.SuccessAsync(payload, "Donation payment successfully initialised.");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to create donation for {Email}.", request.Email);
            return await Result<DonationSubmissionResult>.FailAsync("An unexpected error occurred while creating the donation.");
        }
    }

    /// <summary>
    /// Processes a PayFast payment notification and records the associated donation if the notification is valid and
    /// not a duplicate.
    /// </summary>
    /// <remarks>If the payment status in the notification does not indicate a successful payment, the
    /// notification is ignored. Duplicate notifications for the same donation are detected and skipped. The donation
    /// will not be persisted if the email address is missing. Any unexpected errors during processing will result in a
    /// failure result.</remarks>
    /// <param name="notification">The PayFast payment notification containing donation details to be processed. Must not be null and should
    /// include a valid email address to persist the donation.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A result indicating the outcome of the notification processing. Returns a success result if the donation is
    /// recorded or already exists, or a failure result if the notification is invalid or an error occurs.</returns>
    public async Task<IBaseResult> ProcessPayFastNotificationAsync(DonationPaymentNotification notification, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!IsSuccessfulPaymentStatus(notification.PaymentStatus))
            {
                _logger.LogInformation(
                    "Ignoring PayFast notification for donation {DonationId} with status {Status}.",
                    notification.DonationId,
                    notification.PaymentStatus);

                return await Result.SuccessAsync("Notification processed with no action required.");
            }

            var donationId = string.IsNullOrWhiteSpace(notification.DonationId)
                ? Guid.NewGuid().ToString("N")
                : notification.DonationId;

            var existingDonation = await _repository.FindByConditionAsync(
                d => d.Id == donationId,
                trackChanges: false,
                cancellationToken);

            if (existingDonation.Succeeded && existingDonation.Data?.Any() == true)
            {
                _logger.LogInformation(
                    "Donation {DonationId} has already been recorded. Skipping duplicate notification.",
                    donationId);

                return await Result.SuccessAsync("Donation already recorded.");
            }

            if (string.IsNullOrWhiteSpace(notification.Email))
            {
                _logger.LogWarning(
                    "PayFast notification for donation {DonationId} is missing an email address and cannot be persisted.",
                    donationId);

                return await Result.FailAsync("Unable to persist donation without an email address.");
            }

            var donation = new Donation
            {
                Id = donationId,
                FirstName = notification.FirstName ?? string.Empty,
                LastName = notification.LastName ?? string.Empty,
                Email = notification.Email!,
                PhoneNumber = notification.PhoneNumber,
                Amount = notification.Amount,
                Designation = notification.Designation,
                Message = notification.Message,
                IsAnonymous = notification.IsAnonymous,
                IsRecurring = notification.IsRecurring,
                Frequency = notification.Frequency,
                AgreeToTerms = true,
                CreatedOn = DateTime.UtcNow
            };

            var createResult = await _repository.CreateAsync(donation, cancellationToken);
            if (!createResult.Succeeded)
            {
                _logger.LogWarning(
                    "Failed to persist donation {DonationId} from PayFast notification. Errors: {Errors}",
                    donationId,
                    string.Join(", ", createResult.Messages ?? new List<string>()));

                return await Result.FailAsync(createResult.Messages ?? new List<string> { "Unable to save donation." });
            }

            var saveResult = await _repository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
            {
                _logger.LogWarning(
                    "Failed to save donation {DonationId} after PayFast notification. Errors: {Errors}",
                    donationId,
                    string.Join(", ", saveResult.Messages ?? new List<string>()));

                return await Result.FailAsync(saveResult.Messages ?? new List<string> { "Unable to save donation." });
            }

            _logger.LogInformation(
                "Donation {DonationId} recorded successfully following PayFast notification.",
                donationId);

            return await Result.SuccessAsync("Donation recorded successfully.");
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Failed to process PayFast donation notification for donation {DonationId}.",
                notification.DonationId);

            return await Result.FailAsync("An unexpected error occurred while processing the donation notification.");
        }
    }

    /// <summary>
    /// Creates the details required to redirect a donor to the PayFast payment gateway for processing a donation.
    /// </summary>
    /// <remarks>Returns null if required PayFast configuration values are missing or if the donation
    /// frequency is unsupported for recurring donations. The returned details should be used to construct an HTTP POST
    /// redirect to PayFast. This method does not perform any network operations itself.</remarks>
    /// <param name="request">The donation request containing donor information, donation amount, and payment preferences. Cannot be null.</param>
    /// <param name="donationReference">A unique reference string identifying the donation transaction. Cannot be null or empty.</param>
    /// <returns>A PaymentRedirectDetails object containing the PayFast process URL and form fields for the redirect, or null if
    /// PayFast settings are missing or invalid.</returns>
    private PaymentRedirectDetails? CreatePayFastRedirect(CreateDonationRequest request, string donationReference)
    {
        try
        {

        
        var payFastSettings = _configuration.GetSection("PayFast").Get<PayFastSettings>();
        if (payFastSettings is null)
        {
            _logger.LogError("PayFast settings are not configured; unable to initiate payment for donation {DonationId}.", donationReference);
            return null;
        }

        var merchantId = payFastSettings.MerchantId ?? _configuration["PayFast:MerchantId"];
        var merchantKey = payFastSettings.MerchantKey ?? _configuration["PayFast:MerchantKey"];
        var returnUrl = payFastSettings.ReturnUrl ?? _configuration["PayFast:ReturnUrl"];
        var cancelUrl = payFastSettings.CancelUrl ?? _configuration["PayFast:CancelUrl"];
        var notifyUrl = payFastSettings.NotifyUrl ?? _configuration["PayFast:NotifyUrl"];
        var processUrl = payFastSettings.ProcessUrl ?? _configuration["PayFast:ProcessUrl"];

        if (string.IsNullOrWhiteSpace(merchantId) || string.IsNullOrWhiteSpace(merchantKey) ||
            string.IsNullOrWhiteSpace(returnUrl) || string.IsNullOrWhiteSpace(cancelUrl) ||
            string.IsNullOrWhiteSpace(notifyUrl) || string.IsNullOrWhiteSpace(processUrl))
        {
            _logger.LogError("PayFast settings are incomplete; required values are missing for donation {DonationId}.", donationReference);
            return null;
        }

        var requestBuilder = string.IsNullOrWhiteSpace(payFastSettings.PassPhrase)
            ? new PayFastRequest()
            : new PayFastRequest(payFastSettings.PassPhrase);

        requestBuilder.merchant_id = merchantId;
        requestBuilder.merchant_key = merchantKey;
        requestBuilder.return_url = returnUrl;
        requestBuilder.cancel_url = cancelUrl;
        requestBuilder.notify_url = notifyUrl;
        requestBuilder.custom_str5 = payFastSettings.RedirectUrl ?? returnUrl;
        requestBuilder.name_first = request.FirstName;
        requestBuilder.name_last = request.LastName;
        requestBuilder.email_address = request.Email;
        requestBuilder.cell_number = request.PhoneNumber ?? string.Empty;
        requestBuilder.m_payment_id = donationReference;
        requestBuilder.amount = Math.Round(Convert.ToDouble(request.Amount), 2, MidpointRounding.AwayFromZero);
        requestBuilder.item_name = string.IsNullOrWhiteSpace(request.Designation) ? "Donation" : request.Designation!.TruncateLongString(100);
        requestBuilder.item_description = string.IsNullOrWhiteSpace(request.Message) ? "Donation" : request.Message!.TruncateLongString(255);
        requestBuilder.custom_str1 = donationReference;
        requestBuilder.custom_str2 = request.Designation?.TruncateLongString(255) ?? string.Empty;
        requestBuilder.custom_str3 = request.Message?.TruncateLongString(255) ?? string.Empty;
        requestBuilder.custom_str4 = request.FirstName + " " + request.LastName;
        requestBuilder.custom_int1 = request.IsAnonymous ? 1 : 0;
        requestBuilder.custom_int2 = request.IsRecurring ? 1 : 0;
        requestBuilder.custom_int3 = (int)request.Frequency;

        if (request.IsRecurring)
        {
            var frequency = MapBillingFrequency(request.Frequency);
            if (frequency is null)
            {
                _logger.LogWarning(
                    "Unsupported donation frequency {Frequency} for recurring donation {DonationId}.",
                    request.Frequency,
                    donationReference);

                return null;
            }

            requestBuilder.subscription_type = SubscriptionType.Subscription;
            requestBuilder.frequency = frequency;
            requestBuilder.billing_date = DateTime.Today;
            requestBuilder.recurring_amount = requestBuilder.amount;
            requestBuilder.cycles = 0;
        }

        var fields = BuildPayFastFields(requestBuilder, request.IsRecurring);

        return new PaymentRedirectDetails
        {
            ProcessUrl = processUrl,
            Fields = fields
        };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Builds a dictionary of PayFast payment fields based on the specified request and payment type.
    /// </summary>
    /// <remarks>The returned dictionary is suitable for use with the PayFast payment gateway. Optional fields
    /// are only included if present in the request. When isRecurring is true, subscription-related fields are added if
    /// the request contains the necessary subscription information.</remarks>
    /// <param name="request">The PayFastRequest object containing the payment details to be included in the field dictionary. Cannot be null.</param>
    /// <param name="isRecurring">true to include recurring payment fields for a subscription; otherwise, false for a once-off payment.</param>
    /// <returns>A dictionary containing key-value pairs representing the PayFast payment fields to be submitted. The dictionary
    /// includes all required fields and any optional fields provided in the request.</returns>
    private static Dictionary<string, string> BuildPayFastFields(PayFastRequest request, bool isRecurring)
    {
        var fields = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        void AddField(string key, string? value)
        {
            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                fields[key] = value;
            }
        }

        void AddOptionalField(string key, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                fields[key] = value;
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
            fields["email_confirmation"] = request.email_confirmation.Value ? "1" : "0";
        }

        AddOptionalField("confirmation_address", request.confirmation_address);

        if (isRecurring && request.subscription_type.HasValue && request.frequency.HasValue)
        {
            AddField("subscription_type", ((int)request.subscription_type.Value).ToString(CultureInfo.InvariantCulture));
            AddField("frequency", ((int)request.frequency.Value).ToString(CultureInfo.InvariantCulture));
            AddField("billing_date", request.billing_date?.ToPayFastDate() ?? DateTime.Today.ToPayFastDate());
            AddField("recurring_amount", (request.recurring_amount ?? request.amount).ToString("F2", CultureInfo.InvariantCulture));
            AddField("cycles", (request.cycles ?? 0).ToString(CultureInfo.InvariantCulture));
        }

        AddField("signature", request.signature);

        return fields;
    }

    /// <summary>
    /// Splits a full name string into first and last name components.
    /// </summary>
    /// <remarks>The method splits the input at the first space character. Any additional spaces or names
    /// after the first space are included in the last name component.</remarks>
    /// <param name="fullName">The full name to split. Can be null, empty, or contain leading and trailing whitespace.</param>
    /// <returns>A tuple containing the first name and last name. If the input is null, empty, or whitespace, both values are
    /// empty strings. If only one name is present, the last name is an empty string.</returns>
    private static (string FirstName, string LastName) SplitFullName(string? fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            return (string.Empty, string.Empty);
        }

        var segments = fullName.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        return segments.Length switch
        {
            0 => (string.Empty, string.Empty),
            1 => (segments[0], string.Empty),
            _ => (segments[0], segments[1])
        };
    }

    /// <summary>
    /// Maps a specified donation frequency to its corresponding billing frequency, if one exists.
    /// </summary>
    /// <param name="frequency">The donation frequency to convert to a billing frequency.</param>
    /// <returns>A <see cref="ProductsModule.Domain.Enums.BillingFrequency"/> value that corresponds to the specified donation frequency, or <see
    /// langword="null"/> if the donation frequency does not have a corresponding billing frequency.</returns>
    private static PayfastBillingFrequency? MapBillingFrequency(DonationFrequency frequency) => frequency switch
    {
        DonationFrequency.Monthly => PayfastBillingFrequency.Monthly,
        DonationFrequency.Quarterly => PayfastBillingFrequency.Quarterly,
        DonationFrequency.Annually => PayfastBillingFrequency.Annual,
        _ => null
    };

    /// <summary>
    /// Determines whether the specified payment status string represents a successful payment.
    /// </summary>
    /// <remarks>A status is considered successful if it equals "COMPLETE", "COMPLETE PAYMENT", or "ACTIVE",
    /// ignoring case and leading or trailing whitespace.</remarks>
    /// <param name="status">The payment status to evaluate. The comparison is case-insensitive. May be null or whitespace.</param>
    /// <returns>true if the status indicates a successful payment; otherwise, false.</returns>
    private static bool IsSuccessfulPaymentStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return false;
        }

        return status.Equals("COMPLETE", StringComparison.OrdinalIgnoreCase) ||
               status.Equals("COMPLETE PAYMENT", StringComparison.OrdinalIgnoreCase) ||
               status.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase);
    }
}
