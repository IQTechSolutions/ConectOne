using ConectOne.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using ConectOne.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConectOne.Infrastructure.Implementation
{
    /// <summary>
    /// Provides CRUD and related operations for managing contact information such as phone numbers and email addresses
    /// belonging to a parent entity type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type to which contact information is associated (e.g., Parent, Learner, Teacher).
    /// Must implement <see cref="IAuditableEntity"/>.
    /// </typeparam>
    public class ContactIntoService<TEntity> : IContactInfoService<TEntity> where TEntity : IAuditableEntity<string>
    {
        private readonly IRepository<ContactNumber<TEntity>, string> _contactNumberRepo;
        private readonly IRepository<EmailAddress<TEntity>, string> _emailAddressRepo;

        /// <summary>
        /// Initializes a new instance of <see cref="ContactIntoService{TEntity}"/> with the specified repositories for 
        /// <see cref="ContactNumber{TEntity}"/> and <see cref="EmailAddress{TEntity}"/>, and a logger.
        /// </summary>
        /// <param name="contactNumberRepo">
        /// The repository instance for persisting and retrieving <see cref="ContactNumber{TEntity}"/> records.
        /// </param>
        /// <param name="emailAddressRepo">
        /// The repository instance for persisting and retrieving <see cref="EmailAddress{TEntity}"/> records.
        /// </param>
        /// <param name="logger">
        /// A logging utility conforming to <see cref="ILoggerManager"/> for logging events, warnings, and errors.
        /// </param>
        public ContactIntoService(IRepository<ContactNumber<TEntity>, string> contactNumberRepo, IRepository<EmailAddress<TEntity>, string> emailAddressRepo)
        {
            _contactNumberRepo = contactNumberRepo;
            _emailAddressRepo = emailAddressRepo;
        }

        #region Contact Numbers

        /// <summary>
        /// Retrieves all contact numbers associated with the given parent entity ID.
        /// </summary>
        /// <param name="parentId">
        /// The unique identifier (primary key) for the parent entity (<c>TEntity</c>).
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> wrapping an <see cref="parentId"/> 
        /// of all phone numbers linked to the specified <paramref name="parentId"/>.
        /// If no contact numbers are found or an error occurs, 
        /// returns a failed result with error messages.
        /// </returns>
        public async Task<IBaseResult<IEnumerable<ContactNumberDto>>> AllContactNumbers(string parentId)
        {
            var result = _contactNumberRepo.FindByCondition(c => c.EntityId.Equals(parentId), false);
            if (result.Succeeded)
            {
                return await Result<IEnumerable<ContactNumberDto>>.SuccessAsync(result.Data.Select(c => new ContactNumberDto(c)));
            }
            return await Result<IEnumerable<ContactNumberDto>>.FailAsync(result.Messages);
        }

        /// <summary>
        /// Retrieves a single contact number by its unique identifier.
        /// </summary>
        /// <param name="contactNumberId">The unique ID of the contact number to retrieve.</param>
        /// <returns>
        /// An <see cref="IBaseResult{ContactNumberDto}"/> containing the details of the contact number. 
        /// If the contact number does not exist or an error occurs, returns a failed result with messages.
        /// </returns>
        public async Task<IBaseResult<ContactNumberDto>> ContactNumberAsync(string contactNumberId)
        {
            var result = _contactNumberRepo.FindByCondition(c => c.Id.Equals(contactNumberId), false);
            if (result.Succeeded)
            {
                var response = await result.Data.FirstOrDefaultAsync();
                if (response == null)
                {
                    return Result<ContactNumberDto>.Fail($"No contact number with id matching '{contactNumberId}' was found in the database");
                }
                return Result<ContactNumberDto>.Success(new ContactNumberDto(response));
            }
            return Result<ContactNumberDto>.Fail(result.Messages);
        }

        /// <summary>
        /// Creates a new contact number record for the parent entity, as specified 
        /// in the <paramref name="contactNumber"/> details.
        /// </summary>
        /// <param name="contactNumber">
        /// A <see cref="ContactNumberCreationDto"/> containing necessary info for creation 
        /// (e.g., number, <c>Default</c> flag, and <c>ParentId</c>).
        /// </param>
        /// <returns>
        /// An <see cref="IBaseResult{ContactNumberDto}"/> containing the newly created contact number details if successful,
        /// or a fail result with error messages on failure.
        /// </returns>
        public async Task<IBaseResult<ContactNumberDto>> CreateContactNumber(ContactNumberDto contactNumber)
        {
            var defaultContactNr = contactNumber.Default;
            var result = _contactNumberRepo.FindByCondition(c => c.EntityId.Equals(contactNumber.ParentId), false);

            // If the new number is specified as default, clear default from all existing contact numbers.
            if (defaultContactNr)
            {
                if (result.Data.Any(c => c.Default))
                {
                    foreach (var contact in result.Data.Where(c => c.Default))
                    {
                        contact.Default = false;
                    }
                }
            }
            // If there are no existing contact numbers, ensure the new one is set as default.
            else if (!result.Data.Any())
            {
                defaultContactNr = true;
            }

            var contactNumberToCreate = contactNumber.ToContactNumber<TEntity>(defaultContactNr);
            var creationResult = await _contactNumberRepo.CreateAsync(contactNumberToCreate);

            if (!creationResult.Succeeded) return await Result<ContactNumberDto>.FailAsync(creationResult.Messages);

            var saveResult = await _contactNumberRepo.SaveAsync();
            if (!saveResult.Succeeded) return await Result<ContactNumberDto>.FailAsync(saveResult.Messages);

            return await Result<ContactNumberDto>.SuccessAsync(new ContactNumberDto(contactNumberToCreate));
        }

        /// <summary>
        /// Updates the specified contact number associated with the parent entity.
        /// </summary>
        /// <param name="parentId">The ID of the parent entity (<c>TEntity</c>).</param>
        /// <param name="contactNrId">The ID of the contact number to update.</param>
        /// <param name="contactNr">
        /// A <see cref="ContactNumberEditionDto"/> with updated information 
        /// (e.g., phone number, <c>Default</c> status).
        /// </param>
        /// <returns>
        /// An <see cref="IBaseResult"/> indicating success or failure, along with any relevant messages.
        /// </returns>
        public async Task<IBaseResult> UpdateContactNumberAsync(ContactNumberDto contactNr)
        {
            var result = _contactNumberRepo.FindByCondition(c => c.Id.Equals(contactNr.ContactNumberId), false);
            if (result.Succeeded)
            {
                var response = result.Data.FirstOrDefault();
                if (response == null)
                {
                    return Result.Fail($"No contact number with id matching '{contactNr.ContactNumberId}' was found in the database");
                }

                // If the updated contact number is default, remove default from other contact numbers for the same parent.
                if (contactNr.Default)
                {
                    var allContactNumbers = _contactNumberRepo.FindByCondition(c => c.EntityId.Equals(contactNr.ParentId) && c.Default, false);
                    foreach (var contact in allContactNumbers.Data)
                    {
                        contact.Default = false;
                    }
                }

                response.Number = contactNr.Number;
                response.Default = contactNr.Default;

                _contactNumberRepo.Update(response);
                var saveResult = await _contactNumberRepo.SaveAsync();
                return saveResult.Succeeded
                    ? Result.Success("Contact Nr was successfully updated")
                    : Result.Fail(saveResult.Messages);
            }

            return Result.Fail(result.Messages);
        }

        /// <summary>
        /// Deletes an existing contact number from the parent entity.
        /// </summary>
        /// <param name="parentId">The ID of the parent entity (<c>TEntity</c>).</param>
        /// <param name="contactNrId">The unique ID of the contact number to delete.</param>
        /// <param name="trackChanges">Specifies whether EF Core should track changes for concurrency or not.</param>
        /// <returns>
        /// An <see cref="IBaseResult"/> indicating success or failure, along with any relevant messages.
        /// </returns>
        public async Task<IBaseResult> DeleteContactNumberAsync(string parentId, string contactNrId, bool trackChanges)
        {
            var result = _contactNumberRepo.FindByCondition(c => c.Id.Equals(contactNrId), false);
            if (!result.Succeeded)
            {
                return await Result.FailAsync(result.Messages);
            }

            var response = result.Data.FirstOrDefault();
            if (response == null)
            {
                return await Result.FailAsync($"No contact number with id matching '{contactNrId}' was found in the database");
            }

            // If there are any other non-default contact numbers, mark one as default
            // if we are about to remove the default one.
            var contactNumberResult = _contactNumberRepo.FindByCondition(c => c.EntityId.Equals(parentId) && !c.Default, false);
            if (contactNumberResult.Data.Any(c => c.Id != contactNrId))
            {
                contactNumberResult.Data.FirstOrDefault().Default = true;
                var contactNumberSaveResult = _contactNumberRepo.Update(contactNumberResult.Data.FirstOrDefault());
                await _contactNumberRepo.SaveAsync();

                if (!contactNumberSaveResult.Succeeded)
                {
                    return await Result.FailAsync(
                        $"There was an error setting the default value with detail: {contactNumberResult.Messages.FirstOrDefault()}"
                    );
                }
            }

            _contactNumberRepo.Delete(response);

            var saveResult = await _contactNumberRepo.SaveAsync();
            return saveResult.Succeeded
                ? Result.Success("Contact number was successfully removed")
                : Result.Fail(saveResult.Messages);
        }

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
        public async Task<IBaseResult<IEnumerable<EmailAddressDto>>> AllEmailAddressesAsync(string parentId, bool trackChanges)
        {
            var result = _emailAddressRepo.FindByCondition(c => c.EntityId.Equals(parentId), trackChanges);
            if (result.Succeeded)
            {
                return await Result<IEnumerable<EmailAddressDto>>.SuccessAsync(result.Data.Select(c => new EmailAddressDto(c)));
            }

            return await Result<IEnumerable<EmailAddressDto>>.FailAsync(result.Messages);
        }

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
        public async Task<IBaseResult<EmailAddressDto>> EmailAddressAsync(string emailAddressId, bool trackChanges)
        {
            var result = _emailAddressRepo.FindByCondition(c => c.Id.Equals(emailAddressId), trackChanges);
            if (result.Succeeded)
            {
                var response = await result.Data.FirstOrDefaultAsync();
                if (response == null)
                {
                    return Result<EmailAddressDto>.Fail($"No email address found in the database matching {emailAddressId}");
                }

                return Result<EmailAddressDto>.Success(new EmailAddressDto(response));
            }
            return Result<EmailAddressDto>.Fail($"No email address found in the database matching {emailAddressId}");
        }

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
        public async Task<IBaseResult<EmailAddressDto>> EmailAddressByAddressAsync(string emailAddress, bool trackChanges)
        {
            var result = _emailAddressRepo.FindByCondition(c => c.Email.Equals(emailAddress), trackChanges);
            if (result.Succeeded)
            {
                var response = await result.Data.FirstOrDefaultAsync();
                if (response == null)
                {
                    return Result<EmailAddressDto>.Fail($"No email address found in the database matching {emailAddress}");
                }

                return Result<EmailAddressDto>.Success(EmailAddressDto.CreateDto(response));
            }
            return Result<EmailAddressDto>.Fail($"No email address found in the database matching {emailAddress}");
        }

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
        public async Task<IBaseResult<EmailAddressDto>> CreateEmailAddressAsync(EmailAddressDto emailAddress)
        {
            var defaultContactNr = emailAddress.Default;
            var result = _emailAddressRepo.FindByCondition(c => c.EntityId.Equals(emailAddress.ParentId), false);

            // Ensure only one email address is marked as default,
            // or if this is the first email, force it to be default.
            if (defaultContactNr)
            {
                foreach (var contact in result.Data.Where(c => c.Default))
                {
                    contact.Default = false;
                }
            }
            else if (!result.Data.Any())
            {
                defaultContactNr = true;
            }

            var emailAddressEntity = new EmailAddress<TEntity>()
            {
                Email = emailAddress.EmailAddress,
                Default = defaultContactNr,
                EntityId = emailAddress.ParentId
            };

            var creationResult = await _emailAddressRepo.CreateAsync(emailAddressEntity);
            if (!creationResult.Succeeded) return await Result<EmailAddressDto>.FailAsync(creationResult.Messages);
            
            var saveResult = await _emailAddressRepo.SaveAsync();
            if(!saveResult.Succeeded) return await Result<EmailAddressDto>.FailAsync(saveResult.Messages);

            return await Result<EmailAddressDto>.SuccessAsync(new EmailAddressDto(emailAddressEntity));
        }

        /// <summary>
        /// Updates an existing email address record by its unique ID.
        /// </summary>
        /// <param name="emailAddressNr">
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
        public async Task<IBaseResult> UpdateEmailAddressAsync(EmailAddressDto emailAddress)
        {
            var result = _emailAddressRepo.FindByCondition(c => c.Id.Equals(emailAddress.EmailAddressId), false);
            if (result.Succeeded)
            {
                var response = result.Data.FirstOrDefault();
                if (response == null)
                {
                    return Result.Fail($"No Email Address with id matching '{emailAddress.EmailAddressId}' was found in the database");
                }

                // If this address is set to default, 
                // mark existing default addresses as false.
                if (emailAddress.Default)
                {
                    var allEmailAddresses = _emailAddressRepo.FindByCondition(
                        c => c.EntityId.Equals(emailAddress.ParentId) && c.Default, false
                    );
                    foreach (var email in allEmailAddresses.Data)
                    {
                        email.Default = false;
                    }
                }

                response.Email = emailAddress.EmailAddress;
                response.Default = emailAddress.Default;

                _emailAddressRepo.Update(response);
                var saveResult = await _emailAddressRepo.SaveAsync();
                return saveResult.Succeeded
                    ? Result.Success("Email Address was successfully updated")
                    : Result.Fail(saveResult.Messages);
            }
            return Result.Fail(result.Messages);
        }

        /// <summary>
        /// Removes an existing email address record from the specified parent entity.
        /// </summary>
        /// <param name="parentId">The unique ID of the parent entity.</param>
        /// <param name="emailAddressId">The unique ID of the email address to delete.</param>
        /// <returns>
        /// An <see cref="IBaseResult"/> representing the success/failure of the operation 
        /// and any relevant messages.
        /// </returns>
        public async Task<IBaseResult> DeleteEmailAddressAsync(string parentId, string emailAddressId)
        {
            var result = _emailAddressRepo.FindByCondition(c => c.Id.Equals(emailAddressId), false);
            if (result.Succeeded)
            {
                var response = result.Data.FirstOrDefault();
                if (response == null)
                {
                    return Result.Fail($"No email address with id matching '{emailAddressId}' was found in the database");
                }

                // If removing the default email address, re-assign default to another address
                var emailAddressResult = _emailAddressRepo.FindByCondition(c => c.EntityId.Equals(parentId) && !c.Default, false);
                if (emailAddressResult.Data.Any())
                {
                    emailAddressResult.Data.FirstOrDefault().Default = true;
                    var contactNumberSaveResult = _emailAddressRepo.Update(emailAddressResult.Data.FirstOrDefault());
                    if (!contactNumberSaveResult.Succeeded)
                    {
                        return Result.Fail($"There was an error setting the default value with detail: {emailAddressResult.Messages.FirstOrDefault()}");
                    }
                }

                _emailAddressRepo.Delete(response);

                var saveResult = await _emailAddressRepo.SaveAsync();
                return saveResult.Succeeded
                    ? Result.Success("Contact number was successfully removed")
                    : Result.Fail(saveResult.Messages);
            }
            return Result.Fail(result.Messages);
        }

        #endregion
    }
}
