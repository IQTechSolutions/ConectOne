using ConectOne.Domain.DataTransferObjects;
using ConectOne.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConectOne.Infrastructure.Controllers
{
    /// <summary>
    /// A generic base controller that provides RESTful endpoints for managing contact information 
    /// (contact numbers and email addresses) associated with a parent entity of type <typeparamref name="TParent"/>.
    /// This base controller is meant to be inherited and specialized in actual implementations or 
    /// used directly to handle contact info for a broad set of parent entities.
    /// 
    /// Controller routes should typically be decorated in the derived controllers 
    /// or via custom route attributes, like:
    /// <code>
    /// [Route("api/[controller]/{parentId}")]
    /// public class CustomerContactInfoController : BaseContactInfoApiController&lt;Customer&gt; { ... }
    /// </code>
    /// 
    /// Usage Example:
    /// 1. GET, POST, PUT, DELETE for 'contactNumbers' CRUD.
    /// 2. GET, POST, PUT, DELETE for 'emailAddresses' CRUD.
    /// 
    /// TParent is the parent entity type that these contacts are linked to 
    /// (e.g., a Customer, a User, or another domain entity).
    /// </summary>
    /// <typeparam name="TParent">
    /// The domain entity type that holds the contact information 
    /// (e.g., a "Customer" or "User" class).
    /// </typeparam>
    public class BaseContactInfoApiController<TParent>(IContactInfoService<TParent> service) : Controller
    {
        /// <summary>
        /// A service responsible for retrieving and manipulating contact info objects 
        /// (phone numbers, email addresses, etc.) associated with a given parent entity.
        /// </summary>
        private readonly IContactInfoService<TParent> _service = service;

        
        #region Contact Numbers

        /// <summary>
        /// Retrieves all contact numbers associated with a specified parent entity.
        /// </summary>
        /// <param name="parentId">The identifier of the parent entity to retrieve numbers for.</param>
        /// <returns>
        /// A list of <see cref="ContactNumberDto"/> objects if successful; otherwise, an 
        /// error or empty result.
        /// </returns>
        [HttpGet("{parentId}/contactNumbers")]
        public async Task<IActionResult> GetAllContactNumbersAsync(string parentId)
        {
            var contactNumbers = await _service.AllContactNumbers(parentId);
            return Ok(contactNumbers);
        }

        /// <summary>
        /// Retrieves a single contact number by its unique identifier, belonging to a given parent entity.
        /// </summary>
        /// <param name="parentId">The identifier of the parent entity.</param>
        /// <param name="contactNumberId">The unique identifier of the contact number to retrieve.</param>
        /// <returns>
        /// The requested <see cref="ContactNumberDto"/> if found; otherwise, an error or not found result.
        /// </returns>
        [HttpGet("{parentId}/contactNumbers/{contactNumberId}")]
        public async Task<IActionResult> GetContactNumberById(string parentId, string contactNumberId)
        {
            // parentId is not directly used in this method but can be useful 
            // for verification, logging, or future expansions.
            var customer = await _service.ContactNumberAsync(contactNumberId);
            return Ok(customer);
        }

        /// <summary>
        /// Creates (adds) a new contact number for the given parent entity.
        /// </summary>
        /// <param name="parentId">The identifier of the parent entity to associate the new contact number with.</param>
        /// <param name="contactNumber">A <see cref="ContactNumberCreationDto"/> containing the new contact details.</param>
        /// <returns>The newly created contact number.</returns>
        [HttpPut("{parentId}/contactNumbers")]
        public async Task<IActionResult> CreateContactNumber(string parentId, [FromBody] ContactNumberDto contactNumber)
        {
            var createdCustomer = await _service.CreateContactNumber(contactNumber);
            return Ok(createdCustomer);
        }

        /// <summary>
        /// Updates an existing contact number belonging to a specified parent entity.
        /// </summary>
        /// <param name="parentId">The identifier of the parent entity.</param>
        /// <param name="contactNrId">The unique identifier of the contact number to update.</param>
        /// <param name="customer">A <see cref="ContactNumberEditionDto"/> containing the updated contact details.</param>
        /// <returns>An HTTP result indicating success or failure of the update.</returns>
        [HttpPost("{parentId}/contactNumbers/{contactNrId}")]
        public async Task<IActionResult> UpdateContactNumber(string parentId, string contactNrId, [FromBody] ContactNumberDto customer)
        {
            var result = await _service.UpdateContactNumberAsync(customer);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a contact number by its ID from a specified parent entity.
        /// </summary>
        /// <param name="parentId">The identifier of the parent entity owning this contact number.</param>
        /// <param name="contactNumberId">The unique ID of the contact number to remove.</param>
        /// <returns>An HTTP result indicating success or failure of the deletion.</returns>
        [HttpDelete("{parentId}/contactNumbers/{contactNumberId}")]
        public async Task<IActionResult> DeleteContactNumberAsync(string parentId, string contactNumberId)
        {
            var result = await _service.DeleteContactNumberAsync(parentId, contactNumberId, trackChanges: false);
            return Ok(result);
        }

        #endregion

        #region Email Addresses

        /// <summary>
        /// Retrieves all email addresses associated with a specified parent entity.
        /// </summary>
        /// <param name="parentId">The ID of the parent entity.</param>
        /// <returns>
        /// A list of <see cref="EmailAddressDto"/> objects if found, otherwise an empty list or error messages.
        /// </returns>
        [HttpGet("{parentId}/emailAddresses")]
        public async Task<IActionResult> GetAllEmailAddressesAsync(string parentId)
        {
            var emailAddresses = await _service.AllEmailAddressesAsync(parentId, trackChanges: false);
            return Ok(emailAddresses);
        }

        /// <summary>
        /// Retrieves a single email address by its unique identifier, belonging to a specified parent entity.
        /// </summary>
        /// <param name="parentId">The ID of the parent entity. (Not directly used but kept for route clarity.)</param>
        /// <param name="emailAddressId">The unique identifier of the email address to retrieve.</param>
        /// <returns>The requested <see cref="EmailAddressDto"/> if found; otherwise, an error or not found result.</returns>
        [HttpGet("{parentId}/emailAddresses/{emailAddressId}")]
        public async Task<IActionResult> GetEmailAddressById(string parentId, string emailAddressId)
        {
            var emailAddress = await _service.EmailAddressAsync(emailAddressId, trackChanges: false);
            return Ok(emailAddress);
        }

        /// <summary>
        /// Creates (adds) a new email address for the given parent entity.
        /// </summary>
        /// <param name="emailAddress">
        /// A <see cref="EmailAddressCreationDto"/> object containing the new email address details 
        /// (e.g., EmailAddress string, if it's default, and the parent ID).
        /// </param>
        /// <remarks>
        /// The <paramref name="parentId"/> is not included as a separate parameter here; 
        /// this <c>[HttpPut("emailAddresses")]</c> approach ties the route to creation of addresses. 
        /// Depending on your structure, you could include the parent ID in the route if needed.
        /// </remarks>
        /// <returns>The newly created email address resource.</returns>
        [HttpPut("{parentId}/emailAddresses")]
        public async Task<IActionResult> CreateEmailAddress(string parentId, [FromBody] EmailAddressDto emailAddress)
        {
            if (emailAddress is null)
                return BadRequest("EmailAddressCreationDto object is null");

            var createdCustomer = await _service.CreateEmailAddressAsync(emailAddress);
            return Ok(createdCustomer);
        }

        /// <summary>
        /// Updates an existing email address belonging to a specified parent entity.
        /// </summary>
        /// <param name="parentId">The ID of the parent entity. (Included in the route mainly for clarity.)</param>
        /// <param name="emailAddressId">The unique identifier of the email address to update.</param>
        /// <param name="emailAddress">
        /// A <see cref="EmailAddressEditionDto"/> containing updated fields (EmailAddress, Default, etc.).
        /// </param>
        /// <returns>An HTTP result indicating success or failure of the update.</returns>
        [HttpPost("{parentId}/emailAddresses/{emailAddressId}")]
        public async Task<IActionResult> UpdateEmailAddress(string parentId, string emailAddressId, [FromBody] EmailAddressDto emailAddress)
        {
            if (emailAddress is null)
                return BadRequest("EmailAddressEditionDto object is null");

            var result = await _service.UpdateEmailAddressAsync(emailAddress);
            return Ok(result);
        }

        /// <summary>
        /// Deletes an email address by its ID from a specified parent entity.
        /// </summary>
        /// <param name="parentId">The ID of the parent entity owning this email address.</param>
        /// <param name="emailAddressId">The unique identifier of the email address to remove.</param>
        /// <returns>An HTTP result indicating success or failure of the deletion.</returns>
        [HttpDelete("{parentId}/emailAddresses/{emailAddressId}")]
        public async Task<IActionResult> DeleteEmailAddressAsync(string parentId, string emailAddressId)
        {
            var result = await _service.DeleteEmailAddressAsync(parentId, emailAddressId);
            return Ok(result);
        }

        #endregion
    }
}
