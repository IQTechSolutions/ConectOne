using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// Controller for managing amenities.
    /// </summary>
    [Route($"api/amenities"), ApiController]
    public class AmenitiesController : ControllerBase
    {
        private readonly IAmenityService _amenityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmenitiesController"/> class.
        /// </summary>
        /// <param name="amenityService">The amenity service instance.</param>
        public AmenitiesController(IAmenityService amenityService)
        {
            _amenityService = amenityService;
        }

        /// <summary>
        /// Gets a paginated list of amenities.
        /// </summary>
        /// <param name="pageParameters">The pagination parameters.</param>
        /// <returns>A paginated list of amenities.</returns>
        [HttpGet]
        public async Task<IActionResult> PagedAmenitiesAsync([FromQuery] RequestParameters pageParameters)
        {
            var result = await _amenityService.PagedAmenitiesAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Gets a paginated list of amenities.
        /// </summary>
        /// <param name="pageParameters">The pagination parameters.</param>
        /// <returns>A paginated list of amenities.</returns>
        [HttpGet("all")]
        public async Task<IActionResult> AllAmenitiesAsync()
        {
            var result = await _amenityService.AllAmenitiesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Gets a specific amenity by its ID.
        /// </summary>
        /// <param name="amenityId">The ID of the amenity.</param>
        /// <returns>The amenity details.</returns>
        [HttpGet, Route("amenities/{amenityId}")]
        public async Task<IActionResult> AmenityAsync(string amenityId)
        {
            var result = await _amenityService.AmenityAsync(Convert.ToInt32(amenityId));
            return Ok(result);
        }

        /// <summary>
        /// Creates a new amenity.
        /// </summary>
        /// <param name="model">The amenity data transfer object.</param>
        /// <returns>The created amenity.</returns>
        [HttpPut]
        public async Task<IActionResult> Create([FromBody] AmenityDto model)
        {
            var newPackage = await _amenityService.CreateAmenity(model);
            return Ok(newPackage);
        }

        /// <summary>
        /// Edits an existing amenity.
        /// </summary>
        /// <param name="model">The amenity data transfer object.</param>
        /// <returns>The updated amenity.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] AmenityDto model)
        {
            var result = await _amenityService.UpdateAmenity(model);
            return Ok(result);
        }

        /// <summary>
        /// Deletes an amenity by its ID.
        /// </summary>
        /// <param name="amenityId">The ID of the amenity to delete.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete, Route("amenities/{amenityId}")]
        public async Task<IActionResult> Delete(int amenityId)
        {
            var result = await _amenityService.RemoveAmentity(amenityId);
            return Ok(result);
        }

        /// <summary>
        /// Gets the amenities for a specific entity by its parent ID.
        /// </summary>
        /// <param name="parentId">The parent ID of the entity.</param>
        /// <returns>The list of amenities for the entity.</returns>
        [HttpGet, Route("children/{parentId}")] public async Task<IActionResult> EntityAmenitiesAsync(string parentId)
        {
            var result = await _amenityService.LodgingAmenitiesAsync(parentId);
            return Ok(result);
        }

        /// <summary>
        /// Adds an amenity to a specific entity.
        /// </summary>
        /// <param name="model">The model containing the amenity and entity details.</param>
        /// <returns>The result of the add operation.</returns>
        [HttpPost, Route("addEntityAmenity")] public async Task<IActionResult> AddEntityAmenity([FromBody] AddOwnerAmenityModalViewModel model)
        {
            var result = await _amenityService.AddLodgingAmenityItem(model.AmenityId, model.LodgingId!);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a lodging amenity associated with the specified amenity ID and entity ID.
        /// </summary>
        /// <remarks>This method removes the association between a lodging amenity and an entity.  Ensure
        /// that both <paramref name="amentityId"/> and <paramref name="entityId"/> are valid and exist in the
        /// system.</remarks>
        /// <param name="amentityId">The unique identifier of the amenity to be deleted.</param>
        /// <param name="entityId">The unique identifier of the entity associated with the amenity.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Returns an HTTP 200 OK response with
        /// the result of the deletion if successful.</returns>
        [HttpDelete, Route("lodgings/children/{amentityId}/{entityId}")] public async Task<IActionResult> DeleteLodgingAmenity(int amentityId, string entityId)
        {
            var result = await _amenityService.RemoveLodgingAmenityItem(amentityId, entityId);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a room amenity associated with the specified entity.
        /// </summary>
        /// <remarks>This method removes the association between a room amenity and an entity.  Ensure
        /// that both <paramref name="amentityId"/> and <paramref name="entityId"/> are valid and exist in the
        /// system.</remarks>
        /// <param name="amentityId">The unique identifier of the amenity to be deleted.</param>
        /// <param name="entityId">The unique identifier of the entity associated with the amenity.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically returns an HTTP 200 OK
        /// response with the result of the deletion.</returns>
        [HttpDelete, Route("rooms/children/{amentityId}")] public async Task<IActionResult> DeleteRoomAmenity(int amentityId, int entityId)
        {
            var result = await _amenityService.RemoveRoomAmenityItem(amentityId,entityId);
            return Ok(result);
        }
    }
}
