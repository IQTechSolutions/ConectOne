using ConectOne.Domain.ResultWrappers;
using ShoppingModule.Domain.RequestFeatures;

namespace ShoppingModule.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for a service that handles donation-related operations.
    /// </summary>
    /// <remarks>This interface provides methods for creating and managing donations. Implementations of this
    /// interface should ensure thread safety and proper handling of asynchronous operations. The service is designed to
    /// work with donation-related data models and return standardized results.</remarks>
    public interface IDonationService
    {
        /// <summary>
        /// Creates a new donation asynchronously based on the provided request.
        /// </summary>
        /// <remarks>This method performs the donation creation operation asynchronously. Ensure that the
        /// <paramref name="request"/> parameter is properly populated with valid data before calling this
        /// method.</remarks>
        /// <param name="request">The request object containing the details of the donation to be created.</param>
        /// <param name="cancellationToken">An optional token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// object with the result of the donation submission, including any relevant status or data.</returns>
        Task<IBaseResult<DonationSubmissionResult>> CreateDonationAsync(CreateDonationRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Records a donation once PayFast confirms that payment has succeeded.
        /// </summary>
        /// <param name="notification">The notification payload received from PayFast.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        /// <returns>An <see cref="IBaseResult"/> describing the outcome of the persistence operation.</returns>
        Task<IBaseResult> ProcessPayFastNotificationAsync(DonationPaymentNotification notification, CancellationToken cancellationToken = default);
    }
}
