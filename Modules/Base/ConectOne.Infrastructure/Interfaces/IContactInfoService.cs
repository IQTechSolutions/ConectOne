using ConectOne.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace ConectOne.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines a contract for managing contact information 
    /// (e.g., phone numbers and email addresses) for a given parent entity type.
    /// 
    /// <para>
    /// By using <c>TEntity</c> as a generic type parameter, implementations of 
    /// <see cref="IContactInfoService{TEntity}"/> can be reused for different parent
    /// entity classes while maintaining identical operations for contact details.
    /// For instance, <c>TEntity</c> can be <c>UserInfo</c>, <c>Customer</c>, etc.
    /// </para>
    /// 
    /// <example>
    /// Example usage:
    /// <code>
    /// public class UserInfoContactInfoService : IContactInfoService&lt;UserInfo&gt; 
    /// {
    ///     // ... method implementations ...
    /// }
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of the parent entity to which contact information is linked.
    /// Must provide a unique ID or key used to correlate phone numbers and email addresses.
    /// </typeparam>
    public interface IContactInfoService<TEntity>
    {
        #region Contact Numbers

        /// <summary>
        /// Retrieves all contact numbers associated with the given parent entity ID.
        /// </summary>
        /// <param name="parentId">
        /// The unique identifier (primary key) for the parent entity (<c>TEntity</c>).
        /// </param>
        /// <returns>
        /// An <see cref="IBaseResult{T}"/> wrapping an <see cref="IEnumerable{ContactNumberDto}"/> 
        /// of all phone numbers linked to the specified <paramref name="parentId"/>.
        /// If no contact numbers are found or an error occurs, 
        /// returns a failed result with error messages.
        /// </returns>
        Task<IBaseResult<IEnumerable<ContactNumberDto>>> AllContactNumbers(string parentId);

        /// <summary>
        /// Retrieves a single contact number by its unique identifier.
        /// </summary>
        /// <param name="contactNumberId">The unique ID of the contact number to retrieve.</param>
        /// <returns>
        /// An <see cref="IBaseResult{ContactNumberDto}"/> containing the details of the contact number. 
        /// If the contact number does not exist or an error occurs, returns a failed result with messages.
        /// </returns>
        Task<IBaseResult<ContactNumberDto>> ContactNumberAsync(string contactNumberId);

        /// <summary>
        /// Creates a new contact number using the specified contact number details.
        /// </summary>
        /// <param name="contactNumber">The contact number information to create. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{ContactNumberDto}"/> with the created contact number details if successful; otherwise,
        /// contains error information.</returns>
        Task<IBaseResult<ContactNumberDto>> CreateContactNumber(ContactNumberDto contactNumber);

        /// <summary>
        /// Asynchronously updates the contact number information for an existing contact.
        /// </summary>
        /// <param name="contactNr">An object containing the updated contact number details. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating whether the update was successful and any associated messages.</returns>
        Task<IBaseResult> UpdateContactNumberAsync(ContactNumberDto contactNr);

        /// <summary>
        /// Deletes an existing contact number from the parent entity.
        /// </summary>
        /// <param name="parentId">The ID of the parent entity (<c>TEntity</c>).</param>
        /// <param name="contactNrId">The unique ID of the contact number to delete.</param>
        /// <param name="trackChanges">Specifies whether EF Core should track changes for concurrency or not.</param>
        /// <returns>
        /// An <see cref="IBaseResult"/> indicating success or failure, along with any relevant messages.
        /// </returns>
        Task<IBaseResult> DeleteContactNumberAsync(string contactNrId);

        #endregion

        #region Email Addresses

        /// <summary>
        /// Retrieves all email addresses associated with the specified parent entity.
        /// </summary>
        /// <param name="parentId">The unique identifier for the parent entity.</param>
        /// <param name="trackChanges">
        /// If <c>true</c>, the data is tracked by EF Core for concurrency/updates; 
        /// <c>false</c> for read-only operations.
        /// </param>
        /// <returns>
        /// An <see cref="IBaseResult{T}"/> containing an <see cref="IEnumerable{EmailAddressDto}"/> 
        /// of all email addresses linked to <paramref name="parentId"/>. 
        /// Returns a fail result on error.
        /// </returns>
        Task<IBaseResult<IEnumerable<EmailAddressDto>>> AllEmailAddressesAsync(string parentId);

        /// <summary>
        /// Retrieves a specific email address by its unique identifier.
        /// </summary>
        /// <param name="emailAddressId">The ID of the email address to fetch.</param>
        /// <param name="trackChanges">
        /// If <c>true</c>, EF Core tracks the returned entity for changes; 
        /// if <c>false</c>, the data is read-only.
        /// </param>
        /// <returns>
        /// An <see cref="IBaseResult{EmailAddressDto}"/> describing the requested email record, 
        /// or a fail result with messages if not found or an error occurs.
        /// </returns>
        Task<IBaseResult<EmailAddressDto>> EmailAddressAsync(string emailAddressId);

        /// <summary>
        /// Finds an email address record by its email string (e.g., "john.doe@domain.com").
        /// </summary>
        /// <param name="emailAddress">The actual email string to locate (case-insensitive).</param>
        /// <param name="trackChanges">
        /// If <c>true</c>, EF Core tracks the returned entity for concurrency/updates; 
        /// otherwise read-only.
        /// </param>
        /// <returns>
        /// An <see cref="IBaseResult{EmailAddressDto}"/> with the details of the email address 
        /// if found; otherwise, a fail result with error messages.
        /// </returns>
        Task<IBaseResult<EmailAddressDto>> EmailAddressByAddressAsync(string emailAddress);

        /// <summary>
        /// Creates a new email address linked to a parent entity, specified in 
        /// the <paramref name="emailAddress"/> creation DTO.
        /// </summary>
        /// <param name="emailAddress">
        /// A <see cref="EmailAddressCreationDto"/> containing fields like 
        /// <c>EmailAddress</c>, <c>ParentId</c>, and whether it's <c>Default</c>.
        /// </param>
        /// <returns>
        /// An <see cref="IBaseResult{EmailAddressDto}"/> containing the newly created email 
        /// address record on success; otherwise, a fail result with messages.
        /// </returns>
        Task<IBaseResult<EmailAddressDto>> CreateEmailAddressAsync(EmailAddressDto emailAddress);

        /// <summary>
        /// Updates an existing email address record by its unique ID.
        /// </summary>
        /// <param name="emailAddressId">
        /// The ID of the existing email address to update.
        /// </param>
        /// <param name="emailAddress">
        /// A <see cref="EmailAddressEditionDto"/> containing updated fields (e.g., 
        /// changed email string or <c>Default</c> status).
        /// </param>
        /// <returns>
        /// An <see cref="IBaseResult"/> indicating success or failure, 
        /// with corresponding messages for error details.
        /// </returns>
        Task<IBaseResult> UpdateEmailAddressAsync(EmailAddressDto emailAddress);

        /// <summary>
        /// Removes an existing email address record from the specified parent entity.
        /// </summary>
        /// <param name="parentId">The unique ID of the parent entity.</param>
        /// <param name="emailAddressId">The unique ID of the email address to delete.</param>
        /// <returns>
        /// An <see cref="IBaseResult"/> representing the success/failure of the operation 
        /// and any relevant messages.
        /// </returns>
        Task<IBaseResult> DeleteEmailAddressAsync(string emailAddressId);

        #endregion
    }
}
