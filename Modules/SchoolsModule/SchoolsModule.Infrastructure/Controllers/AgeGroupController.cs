using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Infrastructure.Controllers
{
    /// <summary>
    /// The AgeGroupController provides endpoints for managing and retrieving age groups.
    /// This includes creating, updating, deleting, and fetching paginated lists
    /// or specific details of an age group (e.g., for managing learners by age range).
    /// </summary>
    [Route("api/agegroups"), ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AgeGroupController(IAgeGroupService ageGroupService) : ControllerBase
    {
        /// <summary>
        /// Retrieves a paginated list of age groups based on the provided 
        /// <see cref="AgeGroupPageParameters"/> (e.g., page size, filtering).
        /// </summary>
        /// <param name="pageParameters">Contains query params for paging and filtering age groups.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> holding the filtered age groups.</returns>
        [HttpGet("pagedagegroups")] public async Task<IActionResult> PagedAgeGroupsAsync([FromQuery] AgeGroupPageParameters pageParameters)
        {
            var result = await ageGroupService.PagedAgeGroupsAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all age groups.
        /// </summary>
        /// <remarks>This method invokes the age group service to fetch the data and returns the result in
        /// an HTTP response. The response format is typically JSON.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing a collection of all age groups. The response is returned with an
        /// HTTP 200 status code if successful.</returns>
        [HttpGet("all")] public async Task<IActionResult> AllAgeGroupsAsync()
        {
            var result = await ageGroupService.AllAgeGroupsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a single age group by its unique ID.
        /// </summary>
        /// <param name="ageGroupId">The ID of the age group to retrieve.</param>
        /// <returns>An <see cref="IBaseResult"/> containing the <see cref="AgeGroupDto"/>, if found.</returns>
        [HttpGet("{ageGroupId}")] public async Task<IActionResult> ParentAsync(string ageGroupId)
        {
            var result = await ageGroupService.AgeGroupAsync(ageGroupId);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new age group record with the data provided in <paramref name="ageGroup"/>.
        /// </summary>
        /// <param name="ageGroup">A DTO describing the properties of the new age group.</param>
        /// <returns>
        /// A success or failure result from the service, wrapped in an <see cref="IBaseResult"/>.
        /// </returns>
        [HttpPut] public async Task<IActionResult> CreateAsync([FromBody] AgeGroupDto ageGroup)
        {
            var result = await ageGroupService.CreateAsync(ageGroup);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing age group record with the new values provided.
        /// </summary>
        /// <param name="ageGroup">A DTO that holds updated fields for an existing age group.</param>
        /// <returns>
        /// A success or failure result, indicating whether the update was applied successfully.
        /// </returns>
        [HttpPost] public async Task<IActionResult> UpdateAsync([FromBody] AgeGroupDto ageGroup)
        {
            var result = await ageGroupService.UpdateAsync(ageGroup);
            return Ok(result);
        }

        /// <summary>
        /// Deletes an existing age group identified by <paramref name="ageGroupId"/>.
        /// </summary>
        /// <param name="ageGroupId">The unique ID of the age group to delete.</param>
        /// <returns>
        /// A success or failure result, indicating whether the age group was removed.
        /// </returns>
        [HttpDelete("{ageGroupId}")] public async Task<IActionResult> DeleteAsync(string ageGroupId)
        {
            var result = await ageGroupService.DeleteAsync(ageGroupId);
            return Ok(result);
        }
    }
}
