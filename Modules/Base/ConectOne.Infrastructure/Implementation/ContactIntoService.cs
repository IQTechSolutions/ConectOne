using ConectOne.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using ConectOne.Infrastructure.Interfaces;

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
        /// Initializes a new instance of the ContactIntoService class with the specified repositories for contact
        /// numbers and email addresses.
        /// </summary>
        /// <param name="contactNumberRepo">The repository used to manage contact numbers associated with the entity type.</param>
        /// <param name="emailAddressRepo">The repository used to manage email addresses associated with the entity type.</param>
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
            var spec = new LambdaSpec<ContactNumber<TEntity>>(c => c.EntityId == parentId);
            var result = await _contactNumberRepo.ListAsync(spec);
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
            var spec = new LambdaSpec<ContactNumber<TEntity>>(c => c.Id == contactNumberId);
            var result = await _contactNumberRepo.FirstOrDefaultAsync(spec);
            if (result.Succeeded)
            {
                if (result.Data == null)
                {
                    return await Result<ContactNumberDto>.FailAsync($"No contact number with id matching '{contactNumberId}' was found in the database");
                }
                return await Result<ContactNumberDto>.SuccessAsync(new ContactNumberDto(result.Data));
            }
            return await Result<ContactNumberDto>.FailAsync(result.Messages);
        }

        /// <summary>
        /// Creates a new contact number for the specified parent entity.
        /// </summary>
        /// <remarks>If the specified contact number is marked as default, any existing default contact
        /// number for the same parent entity will be unset. If no contact numbers exist for the parent entity, the new
        /// contact number will be set as default regardless of the value of <see
        /// cref="ContactNumberDto.Default"/>.</remarks>
        /// <param name="contactNumber">The contact number data to create. The <see cref="ContactNumberDto.Default"/> property indicates whether
        /// this number should be set as the default for the parent entity. The <see cref="ContactNumberDto.ParentId"/>
        /// property specifies the parent entity to which the contact number will be associated. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{ContactNumberDto}"/> indicating the outcome of the operation and, if successful, the
        /// created contact number.</returns>
        public async Task<IBaseResult<ContactNumberDto>> CreateContactNumber(ContactNumberDto contactNumber)
        {
            var defaultContactNr = contactNumber.Default;
            var spec = new LambdaSpec<ContactNumber<TEntity>>(c => c.EntityId == contactNumber.ParentId);
            var result = await _contactNumberRepo.ListAsync(spec);

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
        /// Updates the contact number information for an existing contact number record asynchronously.
        /// </summary>
        /// <remarks>If the updated contact number is marked as default, any other contact numbers for the
        /// same parent entity that are currently set as default will have their default status removed. The operation
        /// does not create new contact numbers; it only updates existing ones.</remarks>
        /// <param name="contactNr">The data transfer object containing the updated contact number information. Must not be null. The
        /// ContactNumberId property identifies the contact number to update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IBaseResult indicating
        /// whether the update was successful. If the contact number does not exist, the result will indicate failure
        /// with an appropriate message.</returns>
        public async Task<IBaseResult> UpdateContactNumberAsync(ContactNumberDto contactNr)
        {
            var spec = new LambdaSpec<ContactNumber<TEntity>>(c => c.Id == contactNr.ContactNumberId);
            var result = await _contactNumberRepo.FirstOrDefaultAsync(spec);
            if (result.Succeeded)
            {
                if (result.Data == null)
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

                result.Data.Number = contactNr.Number;
                result.Data.Default = contactNr.Default;

                _contactNumberRepo.Update(result.Data);
                var saveResult = await _contactNumberRepo.SaveAsync();
                return saveResult.Succeeded
                    ? Result.Success("Contact Nr was successfully updated")
                    : Result.Fail(saveResult.Messages);
            }

            return Result.Fail(result.Messages);
        }

        /// <summary>
        /// Deletes the contact number with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>If the deleted contact number was the default for its entity and other contact
        /// numbers exist, another contact number will be set as the new default. The operation fails if the contact
        /// number cannot be found or if an error occurs while updating the default contact number.</remarks>
        /// <param name="contactNrId">The unique identifier of the contact number to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating whether the deletion was successful. If the contact number does not exist, the result will
        /// indicate failure.</returns>
        public async Task<IBaseResult> DeleteContactNumberAsync(string contactNrId)
        {
            var spec = new LambdaSpec<ContactNumber<TEntity>>(c => c.Id == contactNrId);
            var result = await _contactNumberRepo.FirstOrDefaultAsync(spec);

            if (!result.Succeeded)
            {
                return await Result.FailAsync(result.Messages);
            }
            if (result.Data == null)
            {
                return await Result.FailAsync($"No contact number with id matching '{contactNrId}' was found in the database");
            }

            var allSpec = new LambdaSpec<ContactNumber<TEntity>>(c => c.EntityId == result.Data.EntityId && !c.Default);

            var contactNumberResult = await _contactNumberRepo.ListAsync(spec);
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

            await _contactNumberRepo.DeleteAsync(result.Data.Id);

            var saveResult = await _contactNumberRepo.SaveAsync();
            return saveResult.Succeeded
                ? Result.Success("Contact number was successfully removed")
                : Result.Fail(saveResult.Messages);
        }

        #endregion

        #region Email Addresses

        /// <summary>
        /// Asynchronously retrieves all email addresses associated with the specified parent entity.
        /// </summary>
        /// <param name="parentId">The unique identifier of the parent entity for which to retrieve email addresses. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{IEnumerable{EmailAddressDto}}"/> with the collection of email addresses if successful;
        /// otherwise, contains error messages.</returns>
        public async Task<IBaseResult<IEnumerable<EmailAddressDto>>> AllEmailAddressesAsync(string parentId)
        {
            var spec = new LambdaSpec<EmailAddress<TEntity>>(c => c.EntityId == parentId);
            var result = await _emailAddressRepo.ListAsync(spec);
            if (result.Succeeded)
            {
                return await Result<IEnumerable<EmailAddressDto>>.SuccessAsync(result.Data.Select(c => new EmailAddressDto(c)));
            }

            return await Result<IEnumerable<EmailAddressDto>>.FailAsync(result.Messages);
        }

        /// <summary>
        /// Retrieves the email address details for the specified email address identifier asynchronously.
        /// </summary>
        /// <param name="emailAddressId">The unique identifier of the email address to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{EmailAddressDto}"/> with the email address details if found; otherwise, a failure result
        /// with an appropriate error message.</returns>
        public async Task<IBaseResult<EmailAddressDto>> EmailAddressAsync(string emailAddressId)
        {
            var spec = new LambdaSpec<EmailAddress<TEntity>>(c => c.Id == emailAddressId);
            var result = await _emailAddressRepo.FirstOrDefaultAsync(spec);
            if (result.Succeeded)
            {
                if (result.Data == null)
                {
                    return await Result<EmailAddressDto>.FailAsync($"No email address found in the database matching {emailAddressId}");
                }

                return await Result<EmailAddressDto>.SuccessAsync(new EmailAddressDto(result.Data));
            }
            return await Result<EmailAddressDto>.FailAsync($"No email address found in the database matching {emailAddressId}");
        }

        /// <summary>
        /// Asynchronously retrieves an email address record that matches the specified email address.
        /// </summary>
        /// <param name="emailAddress">The email address to search for. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{EmailAddressDto}"/> with the matching email address data if found; otherwise, a failed
        /// result.</returns>
        public async Task<IBaseResult<EmailAddressDto>> EmailAddressByAddressAsync(string emailAddress)
        {
            var spec = new LambdaSpec<EmailAddress<TEntity>>(c => c.Email == emailAddress);
            var result = await _emailAddressRepo.FirstOrDefaultAsync(spec);
            if (!result.Succeeded || result.Data == null)
                return await Result<EmailAddressDto>.FailAsync($"No email address found in the database matching {emailAddress}");

            return await Result<EmailAddressDto>.SuccessAsync(EmailAddressDto.CreateDto(result.Data));
        }

        /// <summary>
        /// Creates a new email address for the specified parent entity asynchronously.
        /// </summary>
        /// <remarks>If the new email address is marked as default, any existing default email address for
        /// the same parent entity will be unset. If this is the first email address for the parent entity, it will be
        /// set as the default regardless of the input value.</remarks>
        /// <param name="emailAddress">An object containing the details of the email address to create, including the parent entity identifier and
        /// whether the address should be set as the default.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IBaseResult with the created
        /// email address data if successful; otherwise, contains error information.</returns>
        public async Task<IBaseResult<EmailAddressDto>> CreateEmailAddressAsync(EmailAddressDto emailAddress)
        {
            var defaultContactNr = emailAddress.Default;
            var spec = new LambdaSpec<EmailAddress<TEntity>>(c => c.EntityId == emailAddress.ParentId);
            var result = await _emailAddressRepo.ListAsync(spec);

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
        /// Asynchronously updates an existing email address record with new values provided in the specified data
        /// transfer object.
        /// </summary>
        /// <remarks>If the updated email address is set as the default, any other email addresses for the
        /// same entity will have their default status cleared. The operation fails if no email address with the
        /// specified ID is found.</remarks>
        /// <param name="emailAddress">An object containing the updated email address information, including the email address ID, new address, and
        /// default status. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating whether the update was successful. If the specified email address ID does not exist, the result
        /// will indicate failure.</returns>
        public async Task<IBaseResult> UpdateEmailAddressAsync(EmailAddressDto emailAddress)
        {
            var spec = new LambdaSpec<EmailAddress<TEntity>>(c => c.Id == emailAddress.EmailAddressId);
            var result = await _emailAddressRepo.FirstOrDefaultAsync(spec);
            if (result.Succeeded)
            {
                if (result.Data == null)
                {
                    return Result.Fail($"No Email Address with id matching '{emailAddress.EmailAddressId}' was found in the database");
                }

                // If this address is set to default, 
                // mark existing default addresses as false.
                if (emailAddress.Default)
                {
                    var allSpec = new LambdaSpec<EmailAddress<TEntity>>(c => c.EntityId == result.Data.EntityId && !c.Default);

                    var allEmailAddresses = await _emailAddressRepo.ListAsync(allSpec);
                    foreach (var email in allEmailAddresses.Data)
                    {
                        email.Default = false;
                    }
                }

                result.Data.Email = emailAddress.EmailAddress;
                result.Data.Default = emailAddress.Default;

                _emailAddressRepo.Update(result.Data);
                var saveResult = await _emailAddressRepo.SaveAsync();
                return saveResult.Succeeded
                    ? await Result.SuccessAsync("Email Address was successfully updated")
                    : await Result.FailAsync(saveResult.Messages);
            }
            return await Result.FailAsync(result.Messages);
        }

        /// <summary>
        /// Asynchronously deletes the email address with the specified identifier.
        /// </summary>
        /// <remarks>If the deleted email address is marked as the default, another email address for the
        /// same entity will be set as the new default if available. The operation fails if the specified email address
        /// does not exist.</remarks>
        /// <param name="emailAddressId">The unique identifier of the email address to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating whether the deletion was successful. If the email address does not exist or an error occurs, the
        /// result contains failure information.</returns>
        public async Task<IBaseResult> DeleteEmailAddressAsync(string emailAddressId)
        {
            var spec = new LambdaSpec<EmailAddress<TEntity>>(c => c.Id == emailAddressId);
            var result = await _emailAddressRepo.FirstOrDefaultAsync(spec);
            if (result.Succeeded)
            {
                if (result.Data == null)
                {
                    return await Result.FailAsync($"No email address with id matching '{emailAddressId}' was found in the database");
                }

                var allSpec = new LambdaSpec<EmailAddress<TEntity>>(c => c.EntityId == result.Data.EntityId && !c.Default);

                // If removing the default email address, re-assign default to another address
                var emailAddressResult = await _emailAddressRepo.ListAsync(spec);
                if (emailAddressResult.Data.Any())
                {
                    emailAddressResult.Data.FirstOrDefault().Default = true;
                    var contactNumberSaveResult = _emailAddressRepo.Update(emailAddressResult.Data.FirstOrDefault());
                    if (!contactNumberSaveResult.Succeeded)
                    {
                        return await Result.FailAsync($"There was an error setting the default value with detail: {emailAddressResult.Messages.FirstOrDefault()}");
                    }
                }

                await _emailAddressRepo.DeleteAsync(result.Data.Id);

                var saveResult = await _emailAddressRepo.SaveAsync();
                return saveResult.Succeeded
                    ? await Result.SuccessAsync("Contact number was successfully removed")
                    : await Result.FailAsync(saveResult.Messages);
            }
            return await Result.FailAsync(result.Messages);
        }

        #endregion
    }
}
