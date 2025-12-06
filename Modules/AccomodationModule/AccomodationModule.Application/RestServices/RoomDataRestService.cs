using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides a REST-based implementation of the <see cref="IRoomDataService"/> interface for managing room data,
    /// including operations for retrieving, creating, updating, and deleting rooms, child policies, bed types, meal
    /// plans, and featured images.
    /// </summary>
    /// <remarks>This service interacts with a REST API through the provided <see cref="IBaseHttpProvider"/>
    /// to perform operations related to room data. It supports paginated retrieval of rooms, CRUD operations for
    /// room-related entities, and additional functionality such as mapping rooms to external services.  The service is
    /// designed to handle various room-related entities, including: - Rooms: Retrieve, create, update, and delete room
    /// data. - Child Policies: Manage child policy rules associated with rooms. - Bed Types: Manage bed type
    /// configurations for rooms. - Meal Plans: Manage meal plans associated with rooms. - Featured Images: Manage
    /// featured images for rooms.  This class is intended to be used in scenarios where room data needs to be managed
    /// via a RESTful API.</remarks>
    /// <param name="provider"></param>
    public class RoomDataRestService(IBaseHttpProvider provider) : IRoomDataService
    {
        /// <summary>
        /// Retrieves a paginated list of rooms associated with the specified package.
        /// </summary>
        /// <remarks>This method retrieves rooms in a paginated format, allowing the caller to specify
        /// filtering and pagination options through the <paramref name="args"/> parameter.</remarks>
        /// <param name="packageId">The unique identifier of the package for which to retrieve the rooms.</param>
        /// <param name="args">The pagination and filtering parameters to apply to the request.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="RoomDto"/> objects and pagination
        /// metadata.</returns>
        public async Task<PaginatedResult<RoomDto>> PagedRoomsAsync(RequestParameters args, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<RoomDto, RequestParameters>($"rooms", args);
            return result;
        }

        /// <summary>
        /// Retrieves the details of a room based on the specified room ID.
        /// </summary>
        /// <remarks>This method uses the underlying provider to fetch room details. Ensure that the
        /// provider is properly configured and accessible before calling this method.</remarks>
        /// <param name="roomId">The unique identifier of the room to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the details of the room as a <see cref="RoomDto"/>. If the room is not found, the result may
        /// indicate an error or an empty response, depending on the implementation of the provider.</returns>
        public async Task<IBaseResult<RoomDto>> RoomAsync(int roomId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<RoomDto>($"rooms/{roomId}");
            return result;
        }

        /// <summary>
        /// Creates a new room asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to create a new room using the provided room details. The
        /// operation is asynchronous and can be canceled using the <paramref name="cancellationToken"/>.</remarks>
        /// <param name="roomModel">The data transfer object containing the details of the room to be created.</param>
        /// <param name="cancellationToken">An optional token to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the created room details.</returns>
        public async Task<IBaseResult<RoomDto>> CreateRoomAsync(RoomDto roomModel, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<RoomDto, RoomDto>($"rooms", roomModel);
            return result;
        }

        /// <summary>
        /// Updates the details of an existing room asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated room details to the server and returns the updated room
        /// information  upon successful completion. Ensure that the provided <paramref name="model"/> contains valid
        /// data  before calling this method.</remarks>
        /// <param name="model">The <see cref="RoomDto"/> object containing the updated room details.  This parameter cannot be <see
        /// langword="null"/>.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// of type <see cref="RoomDto"/> representing the updated room details.</returns>
        public async Task<IBaseResult<RoomDto>> UpdateRoomAsync(RoomDto model, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<RoomDto, RoomDto>($"rooms", model);
            return result;
        }

        /// <summary>
        /// Removes a room with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to delete the room identified by <paramref
        /// name="roomId"/>. Ensure that the specified room ID exists before calling this method. The operation may fail
        /// if the room cannot be found or if there are restrictions preventing its removal.</remarks>
        /// <param name="roomId">The unique identifier of the room to be removed. Must be a valid, existing room ID.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveRoomAsync(int roomId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"rooms/{roomId}", "");
            return result;
        }

        #region ChildPolicies

        /// <summary>
        /// Retrieves the child policy rules associated with a specific room.
        /// </summary>
        /// <remarks>This method fetches child policy rules for the specified room by making an
        /// asynchronous call to the underlying data provider. The result includes details about the rules applicable to
        /// children in the given room.</remarks>
        /// <param name="roomId">The unique identifier of the room for which child policy rules are requested.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{T}"/> containing <see cref="ChildPolicyRuleDto"/> objects that represent the child
        /// policy rules.</returns>
        public async Task<IBaseResult<IEnumerable<ChildPolicyRuleDto>>> ChildPolicies(int roomId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ChildPolicyRuleDto>>($"rooms/{roomId}/childpolicyrules");
            return result;
        }

        /// <summary>
        /// Retrieves the child policy rule for a specific room.
        /// </summary>
        /// <remarks>This method retrieves the child policy rule associated with the specified room and
        /// rule identifiers.  Ensure that the provided identifiers are valid and correspond to existing
        /// resources.</remarks>
        /// <param name="roomId">The unique identifier of the room for which the child policy rule is being retrieved.</param>
        /// <param name="childPolicyRuleId">The unique identifier of the child policy rule to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="ChildPolicyRuleDto"/> representing the child policy rule.</returns>
        public async Task<IBaseResult<ChildPolicyRuleDto>> ChildPolicy(int roomId, string childPolicyRuleId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<ChildPolicyRuleDto>($"rooms/{roomId}/childpolicyrules/{childPolicyRuleId}");
            return result;
        }

        /// <summary>
        /// Creates a new child policy rule asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to create a child policy rule using the provided
        /// <paramref name="childPolicy"/> details. Ensure that the <paramref name="childPolicy"/> object contains valid
        /// data before calling this method.</remarks>
        /// <param name="childPolicy">The data transfer object containing the details of the child policy rule to be created.</param>
        /// <param name="cancellationToken">An optional token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateChildPolicyRule(ChildPolicyRuleDto childPolicy, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"rooms/createChildPolicyRule", childPolicy);
            return result;
        }

        /// <summary>
        /// Updates the child policy rule for a room.
        /// </summary>
        /// <remarks>This method sends the provided child policy rule to the server for updating. Ensure
        /// that the  <paramref name="childPolicy"/> parameter contains valid data before calling this method.</remarks>
        /// <param name="childPolicy">The child policy rule to be updated, represented as a <see cref="ChildPolicyRuleDto"/> object.</param>
        /// <param name="cancellationToken">An optional token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateChildPolicyRule(ChildPolicyRuleDto childPolicy, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"rooms/updateChildPolicyRule", childPolicy);
            return result;
        }

        /// <summary>
        /// Removes a child policy rule identified by the specified ID.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified child policy rule. Ensure the
        /// <paramref name="id"/>  corresponds to an existing rule before calling this method.</remarks>
        /// <param name="id">The unique identifier of the child policy rule to remove. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveChildPolicyRule(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"rooms/deleteChildPolicyRuleRule", id);
            return result;
        }

        #endregion

        #region Bed Types

        /// <summary>
        /// Retrieves a list of bed types associated with the specified room.
        /// </summary>
        /// <remarks>This method communicates with an external provider to fetch the bed type data. Ensure
        /// that the <paramref name="roomId"/> corresponds to a valid room in the system. If no bed types are found for
        /// the specified room, the result may contain an empty collection.</remarks>
        /// <param name="roomId">The unique identifier of the room for which to retrieve bed types.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="BedTypeDto"/> objects representing the bed types for the
        /// specified room.</returns>
        public async Task<IBaseResult<IEnumerable<BedTypeDto>>> BedTypeList(int roomId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<BedTypeDto>>($"rooms/bedtypeList/{roomId}");
            return result;
        }

        /// <summary>
        /// Retrieves the details of a specific bed type by its identifier.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the details of a bed type using the provided
        /// identifier. Ensure that the <paramref name="bedTypeId"/> corresponds to a valid bed type in the
        /// system.</remarks>
        /// <param name="bedTypeId">The unique identifier of the bed type to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// object with the details of the specified bed type.</returns>
        public async Task<IBaseResult<BedTypeDto>> BedType(string bedTypeId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<BedTypeDto>($"rooms/bedtypeList/{bedTypeId}");
            return result;
        }

        /// <summary>
        /// Creates a new bed type in the system.
        /// </summary>
        /// <remarks>This method sends a request to create a new bed type using the provided details.
        /// Ensure that the  <paramref name="bedType"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="bedType">The data transfer object containing the details of the bed type to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateBedType(BedTypeDto bedType, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"rooms/createBedType", bedType);
            return result;
        }

        /// <summary>
        /// Updates the bed type information for a room.
        /// </summary>
        /// <remarks>This method sends the updated bed type details to the provider's API endpoint. Ensure
        /// that the <paramref name="bedType"/> parameter contains valid data before calling this method.</remarks>
        /// <param name="bedType">The data transfer object containing the updated bed type details.</param>
        /// <param name="cancellationToken">An optional token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateBedType(BedTypeDto bedType, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"rooms/updateBedType", bedType);
            return result;
        }

        /// <summary>
        /// Removes a bed type identified by the specified ID.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified bed type. Ensure the provided
        /// <paramref name="bedTypeId"/> corresponds to an existing bed type.</remarks>
        /// <param name="bedTypeId">The unique identifier of the bed type to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveBedType(string bedTypeId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"rooms/deleteBedType", bedTypeId);
            return result;
        }

        #endregion

        #region Meal Plans

        /// <summary>
        /// Creates a new meal plan by sending the specified data to the server.
        /// </summary>
        /// <remarks>This method sends a PUT request to the server to create a meal plan. Ensure that the
        /// <paramref name="mealPlan"/>  object contains all required fields before calling this method.</remarks>
        /// <param name="mealPlan">The data transfer object containing the details of the meal plan to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateMealPlan(MealPlanDto mealPlan, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"rooms/createMealPlan", mealPlan);
            return result;
        }

        /// <summary>
        /// Removes a meal plan with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified meal plan. Ensure the provided
        /// <paramref name="mealPlanId"/>  corresponds to an existing meal plan. The operation may fail if the meal plan
        /// does not exist or if there are  restrictions preventing its removal.</remarks>
        /// <param name="mealPlanId">The unique identifier of the meal plan to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveMealPlan(string mealPlanId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"rooms/deleteMealPlan", mealPlanId);
            return result;
        }

        #endregion

        #region Meal Plans

        /// <summary>
        /// Retrieves the collection of featured images associated with the specified room.
        /// </summary>
        /// <remarks>This method communicates with an external provider to fetch the featured images for
        /// the specified room. Ensure that the <paramref name="roomId"/> corresponds to a valid room in the
        /// system.</remarks>
        /// <param name="roomId">The unique identifier of the room for which to retrieve featured images. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that holds a collection of <see cref="FeaturedImageDto"/> representing the featured images for the
        /// room.</returns>
        public async Task<IBaseResult<ICollection<FeaturedImageDto>>> FeaturedImagesAsync(string roomId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<ICollection<FeaturedImageDto>>($"rooms/featuredImages/{roomId}");
            return result;
        }

        /// <summary>
        /// Adds a featured image for a room.
        /// </summary>
        /// <remarks>This method sends a request to add a featured image for a room. Ensure that the
        /// <paramref name="dto"/> contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the featured image to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the added <see cref="FeaturedImageDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<FeaturedImageDto>> AddFeaturedImage(FeaturedImageDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<FeaturedImageDto, FeaturedImageDto>($"rooms/addFeaturedImage", dto);
            return result;
        }

        /// <summary>
        /// Removes a featured room image by its unique identifier.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified featured room image. Ensure the
        /// <paramref name="featuredImageId"/> corresponds to an existing image before calling this method.</remarks>
        /// <param name="featuredImageId">The unique identifier of the featured room image to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>An <see cref="IBaseResult"/> representing the result of the operation. The result indicates whether the
        /// removal was successful.</returns>
        public async Task<IBaseResult> RemoveFeaturedRoomImageAsync(string featuredImageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"rooms/featuredImages", featuredImageId);
            return result;
        }

        #endregion
    }
}
