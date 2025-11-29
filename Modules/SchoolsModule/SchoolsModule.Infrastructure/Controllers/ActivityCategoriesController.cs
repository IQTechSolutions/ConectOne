using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.RequestFeatures;
using IdentityModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.Interfaces.SchoolEvents;

namespace SchoolsModule.Infrastructure.Controllers
{
    /// <summary>
    /// Controller for managing activity categories.
    /// </summary>
    [Route("api/activities/categories"), ApiController, Authorize(AuthenticationSchemes = "Bearer")]
    public class ActivityCategoriesController(IActivityGroupCategoryService activityGroupCategoryService, ISchoolEventCategoryService schoolEventCategoryService, IActivityGroupNotificationService activityGroupNotificationService) : ControllerBase
    {
        /// <summary>
        /// Retrieves a paginated list of activity categories based on the provided parameters.
        /// </summary>
        /// <param name="categoryPageParameters">Parameters for pagination and filtering.</param>
        /// <returns>A paginated list of activity categories.</returns>
        [HttpGet] public async Task<IActionResult> GetPagedActivityCategories([FromQuery] CategoryPageParameters categoryPageParameters)
        {
            var result = await activityGroupCategoryService.PagedCategoriesAsync(categoryPageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all activity categories.
        /// </summary>
        /// <returns>A list of all activity categories.</returns>
        [HttpGet("all")] public async Task<IActionResult> GetAllActivityCategories()
        {
            var result = await activityGroupCategoryService.CategoriesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all bottom-level activity categories.
        /// </summary>
        /// <returns>A list of all bottom-level activity categories.</returns>
        [HttpGet("bottomlevel")] public async Task<IActionResult> GetAllBottomLevelActivityCategories()
        {
            var result = await activityGroupCategoryService.AllBottomLevelCategoriesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all bottom-level activity categories associated with the specified parent category.
        /// </summary>
        /// <remarks>This method calls the underlying service to fetch the bottom-level categories and
        /// returns the result. Ensure that <paramref name="parentId"/> is a valid identifier for an existing parent
        /// category.</remarks>
        /// <param name="parentId">The unique identifier of the parent category whose bottom-level activity categories are to be retrieved.</param>
        /// <returns>An <see cref="IActionResult"/> containing a collection of bottom-level activity categories. The response is
        /// serialized as JSON and returned with an HTTP 200 status code.</returns>
        [HttpGet("bottomlevel/{parentId}")] public async Task<IActionResult> GetAllBottomLevelActivityCategories(string parentId)
        {
            var result = await activityGroupCategoryService.AllBottomLevelCategoriesAsync(parentId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all parent activity categories.
        /// </summary>
        /// <returns>A list of all parent activity categories.</returns>
        [HttpGet("parents")] public async Task<IActionResult> GetParentActivityCategories()
        {
            var result = await activityGroupCategoryService.CategoriesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Retrieves child activity categories for a given parent category ID.
        /// </summary>
        /// <param name="parentId">The ID of the parent category.</param>
        /// <returns>A list of child activity categories.</returns>
        [HttpGet("childcategories/{parentId}")] public async Task<IActionResult> GetActivityCategories(string parentId)
        {
            var result = await activityGroupCategoryService.CategoriesAsync(parentId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific activity category by its ID.
        /// </summary>
        /// <param name="categoryId">The ID of the activity category.</param>
        /// <returns>The activity category with the specified ID.</returns>
        [HttpGet("{categoryId}")] public async Task<IActionResult> GetActivityCategory(string categoryId)
        {
            var result = await activityGroupCategoryService.CategoryAsync(categoryId);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new activity category.
        /// </summary>
        /// <param name="model">The data for the new activity category.</param>
        /// <returns>The created activity category.</returns>
        [HttpPut] public async Task<IActionResult> CreateActivityCategory([FromBody] CategoryDto model)
        {
            var result = await activityGroupCategoryService.CreateCategoryAsync(model);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing activity category.
        /// </summary>
        /// <param name="model">The data for updating the activity category.</param>
        /// <returns>The updated activity category.</returns>
        [HttpPost] public async Task<IActionResult> UpdateActivityCategory([FromBody] CategoryDto model)
        {
            var result = await activityGroupCategoryService.UpdateCategoryAsync(model);
            return Ok(result);
        }

        /// <summary>
        /// Deletes an activity category by its ID.
        /// </summary>
        /// <param name="categoryId">The ID of the activity category to delete.</param>
        /// <returns>A result indicating the success of the deletion.</returns>
        [HttpDelete("{categoryId}")] public async Task<IActionResult> RemoveActivityCategory(string categoryId)
        {
            var result = await activityGroupCategoryService.DeleteCategoryAsync(categoryId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves categories associated with a specific entity.
        /// </summary>
        /// <param name="entityId">The ID of the entity.</param>
        /// <returns>A list of categories associated with the entity.</returns>
        [HttpGet("entitycategories/{entityId}")] public async Task<IActionResult> GetEntityCategoriesAsync(string entityId)
        {
            var result = await activityGroupCategoryService.EntityCategoriesAsync(entityId);
            return Ok(result);
        }

        /// <summary>
        /// Adds a category to an entity.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <param name="entityId">The ID of the entity.</param>
        /// <returns>A result indicating the success of the addition.</returns>
        [HttpPost("entitycategory/{categoryId}/{entityId}")] public async Task<IActionResult> AddActivityEntityCategoryAsync(string categoryId, string entityId)
        {
            var result = await activityGroupCategoryService.CreateEntityCategoryAsync(categoryId, entityId);
            return Ok(result);
        }

        /// <summary>
        /// Removes a category from an entity.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <param name="entityId">The ID of the entity.</param>
        /// <returns>A result indicating the success of the removal.</returns>
        [HttpDelete("entitycategory/{categoryId}/{entityId}")] public async Task<IActionResult> RemoveActivityEntityCategoryAsync(string categoryId, string entityId)
        {
            var result = await activityGroupCategoryService.RemoveEntityCategoryAsync(categoryId, entityId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific participating activity category in an event.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>The participating activity category in the event.</returns>
        [HttpGet("{eventId}/participating/{categoryId}")] public async Task<IActionResult> GetParticipatingActivityCategory(string eventId, string categoryId)
        {
            var result = await schoolEventCategoryService.ParticipatingActivityGroupCategory(eventId, categoryId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all participating activity categories in an event.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        /// <returns>A list of participating activity categories in the event.</returns>
        [HttpGet("{eventId}/participating")] public async Task<IActionResult> GetParticipatingActivityCategories(string eventId)
        {
            var result = await activityGroupCategoryService.CategoriesAsync(eventId);
            return Ok(result);
        }

        #region Notifications

        /// <summary>
        /// Retrieves a list of notifications for a specified activity category.
        /// </summary>
        /// <remarks>This method calls the underlying service to fetch notifications related to the
        /// specified activity category. The result indicates whether the operation was successful and includes the
        /// relevant data if it was.</remarks>
        /// <param name="activityCategoryId">The unique identifier of the activity category for which to retrieve notifications. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing a successful result with a collection of <see
        /// cref="RecipientDto"/> objects if the operation succeeds; otherwise, a result indicating the failure.</returns>
        [HttpGet("notificationList/{activityCategoryId}")] public async Task<IActionResult> ActivityGroupCategoryNotificationList(string activityCategoryId)
        {
            var result = await activityGroupNotificationService.ActivityGroupCategoryNotificationList(activityCategoryId);
            if (result.Succeeded)
            {
                return Ok(await Result<IEnumerable<RecipientDto>>.SuccessAsync(result.Data));
            }
            return Ok(result);
        }

        #endregion

        #region Images

        /// <summary>
        /// Adds an image to a vacation category.
        /// </summary>
        /// <remarks>This method processes an HTTP POST request to add an image to a specified vacation
        /// category. The operation is asynchronous and respects the cancellation token provided by the HTTP
        /// context.</remarks>
        /// <param name="dto">The data transfer object containing the image details to be added.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("addImage")] public async Task<IActionResult> AddVacationImage([FromBody] AddEntityImageRequest dto)
        {
            var result = await activityGroupCategoryService.AddImage(dto, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a vacation image with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to remove the image associated with the given <paramref
        /// name="imageId"/>. The operation is asynchronous and respects the cancellation token from the HTTP
        /// context.</remarks>
        /// <param name="imageId">The identifier of the image to be deleted. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the delete operation.</returns>
        [HttpDelete("deleteImage/{imageId}/")] public async Task<IActionResult> AddVacationImage(string imageId)
        {
            var result = await activityGroupCategoryService.RemoveImage(imageId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion
    }
}
