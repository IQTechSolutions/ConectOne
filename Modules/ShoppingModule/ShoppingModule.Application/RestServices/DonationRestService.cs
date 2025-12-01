using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using Microsoft.Extensions.Logging;
using ShoppingModule.Domain.Interfaces;
using ShoppingModule.Domain.RequestFeatures;

namespace ShoppingModule.Application.RestServices;

/// <summary>
/// Provides RESTful services for managing donation-related operations, including creating new donations.
/// </summary>
/// <remarks>This service acts as a bridge between the application and the underlying HTTP provider, enabling
/// donation-related operations to be performed asynchronously. It relies on dependency injection to provide the
/// necessary HTTP provider and logging functionality.</remarks>
public class DonationRestService : IDonationService
{
    private readonly IBaseHttpProvider _provider;
    private readonly ILogger<DonationRestService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DonationRestService"/> class.
    /// </summary>
    /// <param name="provider">The HTTP provider used to make REST API calls.</param>
    /// <param name="logger">The logger instance used for logging messages and errors.</param>
    public DonationRestService(IBaseHttpProvider provider, ILogger<DonationRestService> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    /// <summary>
    /// Submits a donation request asynchronously and returns the result of the operation.
    /// </summary>
    /// <param name="request">The donation request containing the details of the donation to be created. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// object with the result of the donation submission.</returns>
    public async Task<IBaseResult<DonationSubmissionResult>> CreateDonationAsync(CreateDonationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _provider.PostAsync<DonationSubmissionResult, CreateDonationRequest>("donations", request);
        return result;
    }

    /// <inheritdoc />
    public Task<IBaseResult> ProcessPayFastNotificationAsync(DonationPaymentNotification notification, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("Donation notifications are handled by the API and cannot be triggered from the client application.");
        return Result.FailAsync("Donation notifications must be received directly from PayFast.");
    }
}
