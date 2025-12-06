using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// Provides endpoints for managing restaurant-related operations, including retrieving, creating, updating, and
    /// deleting restaurant data.
    /// </summary>
    /// <remarks>This controller handles HTTP requests related to restaurant management. It includes methods
    /// for retrieving all restaurants in a specific area,  retrieving details of a specific restaurant, creating new
    /// restaurants, updating existing restaurants, and deleting restaurants. Each method corresponds to a specific HTTP
    /// verb and is mapped to a route for easy access.</remarks>
    /// <param name="restaurantService"></param>
    [Route("api/restaurants"),ApiController]
    public class RestaurantController(IRestaurantService restaurantService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all restaurant packages for the specified area.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch restaurant packages
        /// associated with the given area ID. The result is returned as an HTTP 200 response with the data, or an
        /// appropriate error response if the operation fails.</remarks>
        /// <param name="areaId">The unique identifier of the area for which restaurant packages are requested. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of restaurant packages for the specified area.</returns>
        [HttpGet] public async Task<IActionResult> Packages()
        {
            var newPackage = await restaurantService.AllRestaurantsAsync();
            return Ok(newPackage);
        }

        /// <summary>
        /// Retrieves the package information for a specified restaurant.
        /// </summary>
        /// <remarks>This method uses the <c>restaurantService.RestaurantAsync</c> to fetch the package
        /// data. Ensure that the <paramref name="restaurantId"/> corresponds to a valid restaurant.</remarks>
        /// <param name="restaurantId">The unique identifier of the restaurant whose package information is to be retrieved. This parameter cannot
        /// be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the package information for the specified restaurant. Returns a
        /// 200 OK response with the package data if successful.</returns>
        [HttpGet("{restaurantId}")] public async Task<IActionResult> Package(string restaurantId)
        {
            var newPackage = await restaurantService.RestaurantAsync(restaurantId);
            return Ok(newPackage);
        }

        /// <summary>
        /// Creates a new restaurant package based on the provided restaurant details.
        /// </summary>
        /// <remarks>This method processes the provided restaurant details and creates a new package
        /// asynchronously. The result is returned as an HTTP response with a status code of 200 (OK) if
        /// successful.</remarks>
        /// <param name="restaurant">The restaurant details used to create the package. This parameter must be provided and cannot be null.</param>
        /// <returns>An <see cref="IActionResult"/> containing the newly created restaurant package.</returns>
        [HttpPut] public async Task<IActionResult> CreatePackage([FromBody] RestaurantDto restaurant)
        {
            var newPackage = await restaurantService.CreateRestaurantAsync(restaurant);
            return Ok(newPackage);
        }

        /// <summary>
        /// Updates the details of a restaurant package.
        /// </summary>
        /// <remarks>This method processes an HTTP POST request to update restaurant package details. The
        /// provided <paramref name="restaurant"/> object must contain all necessary fields for the update.</remarks>
        /// <param name="restaurant">The restaurant data to be updated. This must include valid information about the restaurant.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the update operation. Typically, this will include
        /// the updated restaurant data or a status indicating the success or failure of the operation.</returns>
        [HttpPost] public async Task<IActionResult> EditPackage([FromBody] RestaurantDto restaurant)
        {
            var result = await restaurantService.UpdateRestaurantAsync(restaurant);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a restaurant package identified by the specified restaurant ID.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to delete the restaurant package.
        /// Ensure that the provided <paramref name="restaurantId"/> corresponds to a valid restaurant.</remarks>
        /// <param name="restaurantId">The unique identifier of the restaurant whose package is to be deleted. This parameter cannot be null or
        /// empty.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically, this will be an HTTP 200
        /// response containing the result of the deletion.</returns>
        [HttpDelete] public async Task<IActionResult> DeletePackage(string restaurantId)
        {
            var result = await restaurantService.RemoveRestaurantAsync(restaurantId);
            return Ok(result);
        }
    }
}
