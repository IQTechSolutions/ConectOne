using AccomodationModule.Domain.Arguments.Requests;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// API controller for managing vacation entities.
    /// </summary>
    [Route("api/vacations"), ApiController]
    public class VacationController(IVacationService vacationService) : ControllerBase
    {
        /// <summary>
        /// Retrieves a paged list of vacations based on the provided parameters.
        /// </summary>
        /// <param name="pageParameters">The parameters for pagination.</param>
        /// <returns>An IActionResult containing the paged list of vacations.</returns>
        [HttpGet] public async Task<IActionResult> PagedAsync([FromQuery] VacationPageParameters pageParameters)
        {
            var newPackage = await vacationService.PagedAsync(pageParameters, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Retrieves all vacations.
        /// </summary>
        /// <param name="pageParameters">The parameters for pagination.</param>
        /// <returns>An IActionResult containing all vacations.</returns>
        [HttpGet("all")] public async Task<IActionResult> AllVacationsAsync([FromQuery] VacationPageParameters pageParameters)
        {
            var newPackage = await vacationService.AllVacationsAsync(pageParameters, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Retrieves a vacation by its unique identifier.
        /// </summary>
        /// <param name="vacationId">The ID of the vacation to retrieve.</param>
        /// <returns>An IActionResult containing the vacation with the specified ID.</returns>
        [HttpGet("{vacationId}")] public async Task<IActionResult> VacationAsync(string vacationId)
        {
            var newPackage = await vacationService.VacationAsync(vacationId, HttpContext.RequestAborted);
            return Ok(newPackage);
        }
        
        /// <summary>
        /// Retrieves vacation package details based on the provided slug.
        /// </summary>
        /// <remarks>This method uses the provided slug to look up the corresponding vacation package.  If
        /// the slug is invalid or does not match any package, the response will indicate the failure.</remarks>
        /// <param name="slug">The unique identifier for the vacation package, typically a URL-friendly string.</param>
        /// <returns>An <see cref="IActionResult"/> containing the vacation package details if found, or an appropriate HTTP
        /// response if not.</returns>
        [HttpGet("fromSlug/{slug}")]
        public async Task<IActionResult> VacationFromSlugAsync(string slug)
        {
            var newPackage = await vacationService.VacationFromSlugAsync(slug, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Retrieves a vacation by its name.
        /// </summary>
        /// <param name="vacationName">The name of the vacation to retrieve.</param>
        /// <returns>An IActionResult containing the vacation with the specified name.</returns>
        [HttpGet("fromName/{vacationName}")] public async Task<IActionResult> VacationFromNameAsync(string vacationName)
        {
            var newPackage = await vacationService.VacationFromNameAsync(vacationName, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Retrieves a vacation by its unique identifier.
        /// </summary>
        /// <param name="vacationId">The ID of the vacation to retrieve.</param>
        /// <returns>An IActionResult containing the vacation with the specified ID.</returns>
        [HttpGet("summaries/{vacationId}")] public async Task<IActionResult> VacationSummaryAsync(string vacationId)
        {
            var newPackage = await vacationService.VacationSummaryAsync(vacationId, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Retrieves a summary of a vacation by its name.
        /// </summary>
        /// <param name="vacationName">The name of the vacation to retrieve the summary for.</param>
        /// <returns>An IActionResult containing the vacation summary.</returns>
        [HttpGet("summaries/fromName/{vacationName}")] public async Task<IActionResult> VacationSummaryFromNameAsync(string vacationName)
        {
            var newPackage = await vacationService.VacationSummaryFromNameAsync(vacationName, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Creates a new vacation.
        /// </summary>
        /// <param name="dto">The DTO containing the vacation data.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPut] public async Task<IActionResult> CreateAsync([FromBody] VacationDto dto)
        {
            var path = Path.Combine("StaticFiles", "vacations", "images");
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);
            Directory.CreateDirectory(rootPath);

            var newPackage = await vacationService.CreateAsync(dto, rootPath, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Duplicates an existing vacation.
        /// </summary>
        /// <param name="vacationId">The ID of the vacation to duplicate.</param>
        /// <returns>An IActionResult indicating the result of the duplicate operation.</returns>
        [HttpPost("duplicate/{vacationId}")] public async Task<IActionResult> DuplicateAsync(string vacationId)
        {
            var newPackage = await vacationService.DuplicateAsync(vacationId, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Updates an existing vacation.
        /// </summary>
        /// <param name="dto">The DTO containing the updated vacation data.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPost] public async Task<IActionResult> UpdateAsync([FromBody] VacationDto dto)
        {
            var result = await vacationService.UpdateAsync(dto, "", HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a vacation by its unique identifier.
        /// </summary>
        /// <param name="vacationId">The ID of the vacation to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("{vacationId}")] public async Task<IActionResult> RemoveAsync(string vacationId)
        {
            var result = await vacationService.RemoveAsync(vacationId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #region Images

        /// <summary>
        /// Adds an image to a vacation entity.
        /// </summary>
        /// <remarks>This method processes an HTTP POST request to add an image to a specified vacation
        /// entity. The operation is asynchronous and respects the cancellation token provided by the HTTP
        /// context.</remarks>
        /// <param name="dto">The data transfer object containing the image and associated vacation entity details.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("addImage")] public async Task<IActionResult> AddVacationImage([FromBody] AddEntityImageRequest dto)
        {
            var result = await vacationService.AddVacationImage(dto, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a vacation image identified by the specified image ID.
        /// </summary>
        /// <remarks>This method removes the specified vacation image and returns an HTTP 200 OK response
        /// with the result of the operation.</remarks>
        /// <param name="imageId">The unique identifier of the image to be deleted. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the delete operation.</returns>
        [HttpDelete("deleteImage/{imageId}/")] public async Task<IActionResult> AddVacationImage(string imageId)
        {
            var result = await vacationService.RemoveVacationImage(imageId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion

        #region Vacation Inclusion Display Info

        /// <summary>
        /// Creates a new Vacation Inclusion Display Info Section.
        /// </summary>
        /// <param name="dto">The DTO containing the Vacation Inclusion Display Info Section data.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPut("vacationInclusionDisplayInfoSection")] public async Task<IActionResult> CreateVacationInclusionDisplayInfoSectionAsync([FromBody] VacationInclusionDisplayTypeInformationDto dto)
        {
            var newPackage = await vacationService.CreateVacationInclusionDisplaySectionAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Updates a Vacation Inclusion Display Info Section.
        /// </summary>
        /// <param name="dto">The DTO containing the Vacation Inclusion Display Info Section data.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPost("vacationInclusionDisplayInfoSection")] public async Task<IActionResult> UpdateVacationInclusionDisplayInfoSectionAsync([FromBody] VacationInclusionDisplayTypeInformationDto dto)
        {
            var newPackage = await vacationService.UpdateVacationInclusionDisplaySectionAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Updates a Vacation Inclusion Display Info Section.
        /// </summary>
        /// <param name="dto">The DTO containing the Vacation Inclusion Display Info Section data.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPost("vacationInclusionDisplayInfoSection/updateDisplayOrder")] public async Task<IActionResult> UpdateVacationInclusionDisplaySectionDisplayOrderAsync([FromBody] VacationInclusionDisplayTypeInformationGroupUpdateRequest dto)
        {
            var newPackage = await vacationService.UpdateVacationInclusionDisplaySectionDisplayOrderAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Deletes a vacation highlight by its unique identifier.
        /// </summary>
        /// <param name="vacationInclusionDisplayTypeInformationId">The ID of the vacation highlight to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("vacationInclusionDisplayInfoSection/{vacationInclusionDisplayTypeInformationId}")] public async Task<IActionResult> RemoveVacationInclusionDisplayInfoSectionAsync(string vacationInclusionDisplayTypeInformationId)
        {
            var result = await vacationService.RemoveVacationInclusionDisplaySectionAsync(vacationInclusionDisplayTypeInformationId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion

        #region Vacation Extensions

        /// <summary>
        /// Retrieves all extensions associated with the specified vacation.
        /// </summary>
        /// <remarks>The operation is asynchronous and respects the cancellation token provided by the
        /// HTTP context.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation for which to retrieve extensions.</param>
        /// <returns>An <see cref="IActionResult"/> containing a collection of extensions related to the specified vacation.</returns>
        [HttpGet("extensions")] public async Task<IActionResult> AllExtensionsAsync()
        {
            var result = await vacationService.AllExtensionsAsync(HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all vacation extensions for a specific vacation.
        /// </summary>
        /// <param name="vacationId">The ID of the vacation to retrieve extensions for.</param>
        /// <returns>An IActionResult containing the vacation extensions.</returns>
        [HttpGet("vacationextensions/{vacationId}")] public async Task<IActionResult> VacationExtensionsAsync(string vacationId)
        {
            var result = await vacationService.VacationExtensionsAsync(vacationId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new vacation extension.
        /// </summary>
        /// <param name="dto">The DTO containing the vacation extension data.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPut("vacationextensions")] public async Task<IActionResult> CreateVacationExtensionsAsync([FromBody] CreateVacationExtensionForVacationRequest dto)
        {
            var newPackage = await vacationService.CreateExtensionAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Deletes a vacation extension by its unique identifier.
        /// </summary>
        /// <param name="vacationExtensionId">The ID of the vacation extension to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("vacationextensions/{vacationExtensionId}")] public async Task<IActionResult> RemoveVacationExtensionsAsync(string vacationExtensionId)
        {
            var result = await vacationService.RemoveExtensionAsync(vacationExtensionId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion

        #region Reviews

        /// <summary>
        /// Adds a new vacation review.
        /// </summary>
        /// <remarks>This method processes the review asynchronously and respects the cancellation token
        /// provided by the HTTP context.</remarks>
        /// <param name="dto">The data transfer object containing the details of the vacation review to be added.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation, typically an HTTP 200 OK response
        /// with the created review.</returns>
        [HttpPut("review")] public async Task<IActionResult> AddReview([FromBody] VacationReviewDto dto)
        {
            var result = await vacationService.CreateVacationReviewAsync(dto, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing vacation review with the provided details.
        /// </summary>
        /// <remarks>This method processes a POST request to update a vacation review. The update
        /// operation is performed asynchronously, and the result is returned as an HTTP response.</remarks>
        /// <param name="dto">The data transfer object containing the updated review details.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.</returns>
        [HttpPost("review")] public async Task<IActionResult> UpdateReview([FromBody] VacationReviewDto dto)
        {
            var result = await vacationService.UpdateVacationReviewAsync(dto, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Removes a review identified by the specified ID.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to remove a vacation review. The
        /// operation respects the cancellation token provided by the HTTP context.</remarks>
        /// <param name="id">The unique identifier of the review to be removed. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("review/{id}")] public async Task<IActionResult> RemoveReview(string id)
        {
            var result = await vacationService.RemoveVacationReviewAsync(id, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion
    }
}
