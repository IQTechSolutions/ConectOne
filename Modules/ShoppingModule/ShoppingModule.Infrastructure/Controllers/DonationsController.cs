using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PayFast;
using ShoppingModule.Domain.Enums;
using ShoppingModule.Domain.Interfaces;
using ShoppingModule.Domain.RequestFeatures;

namespace ShoppingModule.Infrastructure.Controllers;

/// <summary>
/// Provides endpoints for initiating donations and processing PayFast notifications.
/// </summary>
[Route("api/donations"), ApiController]
public class DonationsController : ControllerBase
{
    private readonly IDonationService _service;
    private readonly ILogger<DonationsController> _logger;
    private readonly PayFastSettings? _payFastSettings;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    public DonationsController(IDonationService service, IConfiguration configuration, ILogger<DonationsController> logger)
    {
        _service = service;
        _logger = logger;
        _payFastSettings = configuration.GetSection("PayFast").Get<PayFastSettings>();
    }

    /// <summary>
    /// Initiates the PayFast payment process for a donation request.
    /// </summary>
    [HttpPost] public async Task<IActionResult> CreateDonation([FromBody] CreateDonationRequest model)
    {
        var response = await _service.CreateDonationAsync(model, HttpContext.RequestAborted);
        return Ok(response);
    }

    /// <summary>
    /// Handles PayFast Instant Transaction Notifications for donations.
    /// </summary>
    [HttpPost("payfast/notify"), Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> PayFastNotificationAsync()
    {
        var (notify, errorResult) = await TryBuildNotificationAsync();
        if (errorResult is not null)
        {
            return errorResult;
        }

        var notificationPayload = MapNotification(notify!);
        var result = await _service.ProcessPayFastNotificationAsync(notificationPayload, HttpContext.RequestAborted);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }

        return Ok();
    }

    /// <summary>
    /// Attempts to construct a valid PayFast donation notification from the current HTTP request.
    /// </summary>
    /// <remarks>This method reads and validates the PayFast notification payload from the HTTP request. It
    /// performs signature, merchant, source IP, and optional data validation checks. If any validation fails, an
    /// appropriate error result is returned. This method does not throw exceptions for validation failures; instead, it
    /// returns an error result for the caller to handle.</remarks>
    /// <returns>A tuple containing a <see cref="PayFastNotify"/> object if the notification is valid; otherwise, <see
    /// langword="null"/>. If validation fails, returns an <see cref="IActionResult"/> describing the error; otherwise,
    /// <see langword="null"/>.</returns>
    private async Task<(PayFastNotify? Notify, IActionResult? ErrorResult)> TryBuildNotificationAsync()
    {
        var form = await Request.ReadFormAsync(HttpContext.RequestAborted);
        if (form is null || form.Count < 1)
        {
            _logger.LogWarning("Received an empty PayFast donation notification payload.");
            return (null, BadRequest("Notification payload is required."));
        }

        var properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var field in form)
        {
            if (!properties.ContainsKey(field.Key))
            {
                properties.Add(field.Key, field.Value.ToString());
            }
        }

        var notify = new PayFastNotify();
        notify.FromFormCollection(properties);

        if (_payFastSettings is null)
        {
            _logger.LogError("PayFast settings are not configured; unable to process donation notification {PaymentId}.", notify.pf_payment_id);
            return (null, StatusCode(StatusCodes.Status500InternalServerError, "PayFast is not configured."));
        }

        if (!string.IsNullOrWhiteSpace(_payFastSettings.PassPhrase))
        {
            notify.SetPassPhrase(_payFastSettings.PassPhrase);
        }

        if (!notify.CheckSignature())
        {
            _logger.LogWarning("PayFast signature validation failed for donation notification {PaymentId}.", notify.pf_payment_id);
            return (null, BadRequest("Invalid signature."));
        }

        var remoteAddress = HttpContext.Connection.RemoteIpAddress ?? IPAddress.None;
        var validator = new PayFastValidator(_payFastSettings, notify, remoteAddress);

        if (!validator.ValidateMerchantId())
        {
            _logger.LogWarning("PayFast merchant validation failed for donation notification {PaymentId}.", notify.pf_payment_id);
            return (null, BadRequest("Merchant validation failed."));
        }

        if (remoteAddress != IPAddress.None)
        {
            var sourceValid = await validator.ValidateSourceIp();
            if (!sourceValid)
            {
                _logger.LogWarning("PayFast donation notification received from invalid IP address {IpAddress}.", remoteAddress);
                return (null, BadRequest("Invalid source IP address."));
            }
        }

        if (!string.IsNullOrWhiteSpace(_payFastSettings.ValidateUrl))
        {
            var dataValid = await validator.ValidateData();
            if (!dataValid)
            {
                _logger.LogWarning("PayFast data validation failed for donation notification {PaymentId}.", notify.pf_payment_id);
                return (null, BadRequest("PayFast data validation failed."));
            }
        }

        return (notify, null);
    }

    /// <summary>
    /// Maps a PayFast payment notification to a DonationPaymentNotification instance.
    /// </summary>
    /// <param name="notify">The PayFast notification object containing payment details to be mapped. Cannot be null.</param>
    /// <returns>A DonationPaymentNotification object populated with data from the specified PayFast notification.</returns>
    private static DonationPaymentNotification MapNotification(PayFastNotify notify)
    {
        var donationId = !string.IsNullOrWhiteSpace(notify.custom_str1)
            ? notify.custom_str1
            : !string.IsNullOrWhiteSpace(notify.m_payment_id) ? notify.m_payment_id : Guid.NewGuid().ToString("N");

        return new DonationPaymentNotification
        {
            DonationId = donationId,
            PaymentStatus = notify.payment_status,
            PaymentReference = !string.IsNullOrWhiteSpace(notify.pf_payment_id) ? notify.pf_payment_id : notify.m_payment_id,
            FirstName = notify.name_first,
            LastName = notify.name_last,
            Email = notify.email_address,
            PhoneNumber = notify.cell_number,
            Amount = ParseAmount(notify.amount_gross),
            Designation = string.IsNullOrWhiteSpace(notify.custom_str2) ? notify.item_name : notify.custom_str2,
            Message = string.IsNullOrWhiteSpace(notify.custom_str3) ? notify.item_description : notify.custom_str3,
            IsAnonymous = ParseBoolean(notify.custom_int1),
            IsRecurring = ParseBoolean(notify.custom_int2),
            Frequency = ParseFrequency(notify.custom_int3),
            SubscriptionToken = notify.token
        };
    }

    /// <summary>
    /// Parses the specified string representation of a numeric amount using the invariant culture.
    /// </summary>
    /// <param name="amount">A string containing the amount to parse. The string may include decimal separators and optional whitespace, and
    /// is interpreted using invariant culture formatting. May be null.</param>
    /// <returns>The parsed numeric value if the string is a valid number; otherwise, 0.</returns>
    private static double ParseAmount(string? amount)
    {
        if (double.TryParse(amount, NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
        {
            return value;
        }

        return 0;
    }

    /// <summary>
    /// Parses the specified string and determines whether it represents the boolean value true, using the integer value
    /// 1 as true.
    /// </summary>
    /// <remarks>This method treats only the string "1" (invariant culture) as true. All other values,
    /// including null, empty strings, or non-numeric strings, are interpreted as false.</remarks>
    /// <param name="value">The string to parse. The string is interpreted as an integer in invariant culture; only the value "1" is
    /// considered true.</param>
    /// <returns>true if the input string represents the integer value 1; otherwise, false.</returns>
    private static bool ParseBoolean(string? value) => int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue) && intValue == 1;

    /// <summary>
    /// Parses the specified string value into a corresponding DonationFrequency enumeration value.
    /// </summary>
    /// <remarks>If the input string is null, not a valid integer, or does not correspond to a defined
    /// DonationFrequency value, the method returns DonationFrequency.OnceOff.</remarks>
    /// <param name="value">A string representation of the donation frequency, which may be an integer value corresponding to a
    /// DonationFrequency enumeration member. Can be null.</param>
    /// <returns>A DonationFrequency value that corresponds to the parsed integer if valid; otherwise, DonationFrequency.OnceOff.</returns>
    private static DonationFrequency ParseFrequency(string? value)
    {
        if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue) && Enum.IsDefined(typeof(DonationFrequency), intValue))
        {
            return (DonationFrequency)intValue;
        }

        return DonationFrequency.OnceOff;
    }

    /// <summary>
    /// Builds the full name of a PayFast notification recipient using available first and last name information.
    /// </summary>
    /// <remarks>If either the first or last name is missing or consists only of whitespace, only the
    /// available name part is used. If both are missing, the method falls back to the custom_str4 field as the full
    /// name.</remarks>
    /// <param name="notify">The PayFastNotify instance containing recipient name details. Cannot be null.</param>
    /// <returns>A string containing the recipient's full name, composed of the first and last name if available; otherwise,
    /// returns the value of notify.custom_str4.</returns>
    private static string BuildFullName(PayFastNotify notify)
    {
        var parts = new[] { notify.name_first, notify.name_last }
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Select(p => p.Trim());

        var fullName = string.Join(" ", parts);
        return string.IsNullOrWhiteSpace(fullName) ? notify.custom_str4 : fullName;
    }
}
