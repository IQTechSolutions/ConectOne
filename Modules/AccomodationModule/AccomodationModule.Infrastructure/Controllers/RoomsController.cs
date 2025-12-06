using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// Controller for handling room-related API requests.
    /// </summary>
    [Route("api/rooms"), ApiController]
    public class RoomsController(IRoomDataService roomDataService) : ControllerBase
    {
        /// <summary>
        /// Retrieves a paged list of rooms for a specific lodging and package.
        /// </summary>
        /// <param name="lodgingId">The ID of the lodging.</param>
        /// <param name="packageId">The ID of the package.</param>
        /// <param name="args">The parameters for filtering, sorting, and pagination.</param>
        /// <returns>An IActionResult containing the paged list of rooms.</returns>
        [HttpGet] public async Task<IActionResult> PagedRooms([FromQuery] RequestParameters args)
        {
            var newPackage = await roomDataService.PagedRoomsAsync(args);
            return Ok(newPackage);
        }

        /// <summary>
        /// Retrieves details of a specific room by its ID.
        /// </summary>
        /// <param name="roomId">The ID of the room to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the room details.</returns>
        [HttpGet("{roomId}")] public async Task<IActionResult> RoomAsync(string roomId)
        {
            var newPackage = await roomDataService.RoomAsync(Convert.ToInt32(roomId), HttpContext.RequestAborted);
            return Ok(newPackage);
        }
        
        /// <summary>
        /// Creates a new room.
        /// </summary>
        /// <param name="model">The DTO containing the data for the new room.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPut] public async Task<IActionResult> CreateRoom([FromBody] RoomDto model)
        {
            var room = await roomDataService.CreateRoomAsync(model);
            return Ok(room);
        }

        /// <summary>
        /// Updates an existing room.
        /// </summary>
        /// <param name="model">The DTO containing the updated data for the room.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPost] public async Task<IActionResult> EditRoom([FromBody] RoomDto model)
        {
            var result = await roomDataService.UpdateRoomAsync(model);

            return Ok(result);
        }

        /// <summary>
        /// Deletes a room by its ID.
        /// </summary>
        /// <param name="roomId">The ID of the room to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("{roomId}")] public async Task<IActionResult> DeleteRoom(int roomId)
        {
            var result = await roomDataService.RemoveRoomAsync(roomId);
            return Ok(result);
        }

        #region ChildPolicyRules

        /// <summary>
        /// Retrieves all child policy rules for a specific room.
        /// </summary>
        /// <param name="roomId">The ID of the room to retrieve child policy rules for.</param>
        /// <returns>An IActionResult containing the child policy rules.</returns>
        [HttpGet("{roomId}/childpolicyrules")]
        public async Task<IActionResult> ChildPolicyRules(string roomId)
        {
            var newPackage = await roomDataService.ChildPolicies(Convert.ToInt32(roomId));
            return Ok(newPackage);
        }

        /// <summary>
        /// Retrieves a specific child policy rule for a specific room.
        /// </summary>
        /// <param name="roomId">The ID of the room to retrieve the child policy rule for.</param>
        /// <param name="childPolicyRuleId">The ID of the child policy rule to retrieve.</param>
        /// <returns>An IActionResult containing the child policy rule.</returns>
        [HttpGet("{roomId}/childpolicyrules/{childPolicyRuleId}")]
        public async Task<IActionResult> ChildPolicyRules(string roomId, string childPolicyRuleId)
        {
            var newPackage = await roomDataService.ChildPolicy(Convert.ToInt32(roomId), childPolicyRuleId);
            return Ok(newPackage);
        }

        /// <summary>
        /// Creates a new child policy rule.
        /// </summary>
        /// <param name="modal">The DTO containing the data for the new child policy rule.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPut("createChildPolicyRule")]
        public async Task<IActionResult> CreateChildPolicyRule([FromBody] ChildPolicyRuleDto modal)
        {
            var result = await roomDataService.CreateChildPolicyRule(modal);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing child policy rule.
        /// </summary>
        /// <param name="modal">The DTO containing the updated data for the child policy rule.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPost("updateChildPolicyRule")]
        public async Task<IActionResult> UpdateChildPolicyRule([FromBody] ChildPolicyRuleDto modal)
        {
            var result = await roomDataService.UpdateChildPolicyRule(modal);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a child policy rule by its ID.
        /// </summary>
        /// <param name="childPolicyRuleId">The ID of the child policy rule to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("deleteChildPolicyRuleRule/{childPolicyRuleId}")]
        public async Task<IActionResult> DeleteChildPolicyRule(string childPolicyRuleId)
        {
            var result = await roomDataService.RemoveChildPolicyRule(childPolicyRuleId);
            return Ok(result);
        }

        #endregion

        #region Bed Types

        /// <summary>
        /// Retrieves a list of bed types for a specific room.
        /// </summary>
        /// <param name="roomId">The ID of the room to retrieve bed types for.</param>
        /// <returns>An IActionResult containing the list of bed types.</returns>
        [HttpGet("bedtypeList/{roomId:int}")]
        public async Task<IActionResult> BedTypeList(int roomId)
        {
            var result = await roomDataService.BedTypeList(roomId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific bed type by its ID.
        /// </summary>
        /// <param name="bedTypeId">The ID of the bed type to retrieve.</param>
        /// <returns>An IActionResult containing the bed type.</returns>
        [HttpGet("bedtypes/{bedTypeId}")]
        public async Task<IActionResult> BedType(string bedTypeId)
        {
            var result = await roomDataService.BedType(bedTypeId);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new bed type.
        /// </summary>
        /// <param name="modal">The DTO containing the data for the new bed type.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPut("createBedType")]
        public async Task<IActionResult> CreateBedType([FromBody] BedTypeDto modal)
        {
            var result = await roomDataService.CreateBedType(modal);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing bed type.
        /// </summary>
        /// <param name="modal">The DTO containing the updated data for the bed type.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPost("updateBedType")]
        public async Task<IActionResult> UpdateBedType([FromBody] BedTypeDto modal)
        {
            var result = await roomDataService.UpdateBedType(modal);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a bed type by its ID.
        /// </summary>
        /// <param name="bedTypeId">The ID of the bed type to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("deleteBedType/{bedTypeId}")]
        public async Task<IActionResult> DeleteBedType(string bedTypeId)
        {
            var result = await roomDataService.RemoveBedType(bedTypeId);
            return Ok(result);
        }

        #endregion

        #region Meal Plans

        /// <summary>
        /// Creates a new meal plan.
        /// </summary>
        /// <param name="mealPlan">The DTO containing the data for the new meal plan.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPut("createMealPlan")]
        public async Task<IActionResult> CreateMealPlan([FromBody] MealPlanDto mealPlan)
        {
            var result = await roomDataService.CreateMealPlan(mealPlan);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a meal plan by its ID.
        /// </summary>
        /// <param name="mealPlanId">The ID of the meal plan to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("deleteMealPlan/{mealPlanId}")]
        public async Task<IActionResult> DeleteMealPlan(string mealPlanId)
        {
            var result = await roomDataService.RemoveMealPlan(mealPlanId);
            return Ok(result);
        }

        #endregion

        #region Images

        /// <summary>
        /// Retrieves featured images for a specific room.
        /// </summary>
        /// <param name="roomId">The ID of the room to retrieve featured images for.</param>
        /// <returns>An IActionResult containing the featured images.</returns>
        [HttpGet("rooms/featuredImages/{roomId}")]
        public async Task<IActionResult> FeaturedRoomImages(string roomId)
        {
            var lodging = await roomDataService.FeaturedImagesAsync(roomId);
            return Ok(lodging);
        }

        /// <summary>
        /// Adds a featured image to a room.
        /// </summary>
        /// <param name="dto">The DTO containing the data for the new featured image.</param>
        /// <returns>An IActionResult indicating the result of the add operation.</returns>
        [HttpPut("addFeaturedImage")]
        public async Task<IActionResult> AddFeaturedRoomImage([FromBody] FeaturedImageDto dto)
        {
            var result = await roomDataService.AddFeaturedImage(dto);
            return Ok(result);
        }

        /// <summary>
        /// Removes a featured image from a room.
        /// </summary>
        /// <param name="featuredImageId">The ID of the featured image to remove.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("featuredImages/{featuredImageId}")]
        public async Task<IActionResult> RemoveFeaturedRoomImageAsync(string featuredImageId)
        {
            var result = await roomDataService.RemoveFeaturedRoomImageAsync(featuredImageId);
            return Ok(result);
        }

        #endregion
    }
}
