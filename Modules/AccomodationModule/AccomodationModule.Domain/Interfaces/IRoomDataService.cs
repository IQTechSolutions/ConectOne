using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Provides methods for managing room data, including CRUD operations for rooms, child policy rules, bed types,
    /// meal plans, images, and integrations with external services.
    /// </summary>
    /// <remarks>This interface defines a comprehensive set of operations for interacting with room-related
    /// data in a system. It includes methods for retrieving, creating, updating, and deleting rooms, as well as
    /// managing associated entities such as child policy rules, bed types, meal plans, and images. Additionally, it
    /// provides functionality for integrating with external services like NightsBridge and Cimso.</remarks>
	public interface IRoomDataService
	{
        /// <summary>
        /// Retrieves a paginated list of rooms associated with the specified package.
        /// </summary>
        /// <remarks>Use this method to retrieve rooms in a paginated format, allowing for efficient
        /// handling of large datasets. The <paramref name="args"/> parameter can be used to specify pagination details
        /// such as page size and page number.</remarks>
        /// <param name="packageId">The unique identifier of the package for which rooms are being retrieved. Must not be <see langword="null"/>
        /// or empty.</param>
        /// <param name="args">The parameters that define pagination and filtering options for the request.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of rooms. If no rooms are found, the
        /// result will contain an empty collection.</returns>
        Task<PaginatedResult<RoomDto>> PagedRoomsAsync(RequestParameters args, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves detailed information about a specific room.
        /// </summary>
        /// <remarks>Use this method to fetch information about a room, such as its name, capacity, and
        /// other attributes. Ensure that <paramref name="roomId"/> is valid and corresponds to an existing
        /// room.</remarks>
        /// <param name="roomId">The unique identifier of the room to retrieve. Must be a positive integer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional; defaults to <see langword="default"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping a <see cref="RoomDto"/> with the room's details.</returns>
		Task<IBaseResult<RoomDto>> RoomAsync(int roomId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new room asynchronously based on the provided room model.
        /// </summary>
        /// <remarks>This method performs validation on the provided <paramref name="roomModel"/> before
        /// creating the room. If the operation fails, the returned <see cref="IBaseResult{T}"/> will contain error
        /// details.</remarks>
        /// <param name="roomModel">The data transfer object containing the details of the room to be created. This parameter must not be <see
        /// langword="null"/> and must contain valid room information.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// object with the created room details if the operation is successful.</returns>
        Task<IBaseResult<RoomDto>> CreateRoomAsync(RoomDto roomModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the details of an existing room asynchronously.
        /// </summary>
        /// <remarks>Use this method to update the properties of an existing room. Ensure that the
        /// <paramref name="model"/> contains valid data and matches the expected format for the room being updated. The
        /// operation may fail if the room does not exist or if the provided data is invalid.</remarks>
        /// <param name="model">The <see cref="RoomDto"/> containing the updated room details. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{RoomDto}"/> indicating the outcome of the update operation, including the updated room
        /// details if successful.</returns>
        Task<IBaseResult<RoomDto>> UpdateRoomAsync(RoomDto model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a room with the specified ID asynchronously.
        /// </summary>
        /// <remarks>This method deletes the room identified by <paramref name="roomId"/> from the system.
        /// If the room does not exist, the operation will fail and return an appropriate result.</remarks>
        /// <param name="roomId">The unique identifier of the room to be removed. Must be a positive integer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveRoomAsync(int roomId, CancellationToken cancellationToken = default);

        #region ChildPolicyRules

        /// <summary>
        /// Retrieves the child policy rules associated with a specific room.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch child policy rules for a
        /// given room. Ensure that <paramref name="roomId"/> is valid and greater than zero before calling this
        /// method.</remarks>
        /// <param name="roomId">The unique identifier of the room for which child policy rules are requested.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/>
        /// of <see cref="ChildPolicyRuleDto"/> objects representing the child policy rules for the specified room.</returns>
        Task<IBaseResult<IEnumerable<ChildPolicyRuleDto>>> ChildPolicies(int roomId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the child policy rule associated with a specific room.
        /// </summary>
        /// <remarks>Use this method to retrieve detailed information about a specific child policy rule
        /// for a given room. Ensure that both <paramref name="roomId"/> and <paramref name="childPolicyRuleId"/> are
        /// valid and correspond to existing entities in the system.</remarks>
        /// <param name="roomId">The unique identifier of the room for which the child policy rule is being requested.</param>
        /// <param name="childPolicyRuleId">The unique identifier of the child policy rule to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="ChildPolicyRuleDto"/> for the specified child policy rule.</returns>
        Task<IBaseResult<ChildPolicyRuleDto>> ChildPolicy(int roomId, string childPolicyRuleId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new child policy rule based on the provided data.
        /// </summary>
        /// <remarks>Use this method to create a child policy rule within the system. Ensure that the
        /// <paramref name="childPolicy"/> parameter contains valid data before calling this method. The operation may
        /// be canceled by passing a <see cref="CancellationToken"/>.</remarks>
        /// <param name="childPolicy">The data transfer object containing the details of the child policy rule to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> CreateChildPolicyRule(ChildPolicyRuleDto childPolicy, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing child policy rule with the specified details.
        /// </summary>
        /// <remarks>Use this method to update the details of an existing child policy rule. Ensure that
        /// the provided  <paramref name="childPolicy"/> contains valid data before calling this method.</remarks>
        /// <param name="childPolicy">The data transfer object containing the updated details of the child policy rule. This parameter must not be
        /// <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. If not provided, the default token is used.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the update operation.</returns>
        Task<IBaseResult> UpdateChildPolicyRule(ChildPolicyRuleDto childPolicy, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a child policy rule identified by the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the child policy rule to remove. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveChildPolicyRule(string id, CancellationToken cancellationToken = default);

        #endregion

        #region BedTypes

        /// <summary>
        /// Retrieves a list of bed types associated with the specified room.
        /// </summary>
        /// <remarks>This method is asynchronous and should be awaited. If the operation is canceled via
        /// the  <paramref name="cancellationToken"/>, the returned task will be in a canceled state.</remarks>
        /// <param name="roomId">The unique identifier of the room for which bed types are to be retrieved. Must be a valid, non-negative
        /// integer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// with an enumerable collection of <see cref="BedTypeDto"/> objects representing the bed types.</returns>
        Task<IBaseResult<IEnumerable<BedTypeDto>>> BedTypeList(int roomId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves information about a specific bed type.
        /// </summary>
        /// <remarks>Use this method to fetch detailed information about a specific bed type by its
        /// identifier. The operation may be canceled by passing a cancellation token.</remarks>
        /// <param name="bedTypeId">The unique identifier of the bed type to retrieve. This parameter cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the requested <see cref="BedTypeDto"/> data.</returns>
        Task<IBaseResult<BedTypeDto>> BedType(string bedTypeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new bed type in the system.
        /// </summary>
        /// <remarks>Use this method to add a new bed type to the system. Ensure that the <paramref
        /// name="bedType"/> object contains valid data before calling this method. The operation can be canceled using
        /// the provided <paramref name="cancellationToken"/>.</remarks>
        /// <param name="bedType">The data transfer object containing the details of the bed type to be created. Cannot be null.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> CreateBedType(BedTypeDto bedType, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the details of an existing bed type.
        /// </summary>
        /// <remarks>Use this method to modify the properties of an existing bed type. Ensure that the 
        /// <paramref name="bedType"/> object contains valid data before calling this method. The operation may be
        /// canceled by passing a cancellation token.</remarks>
        /// <param name="bedType">The data transfer object containing the updated information for the bed type. This parameter cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. If omitted, the default token is used.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult"/>
        /// indicating the success or failure of the update operation.</returns>
        Task<IBaseResult> UpdateBedType(BedTypeDto bedType, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a bed type from the system based on the specified identifier.
        /// </summary>
        /// <remarks>Use this method to remove a bed type that is no longer needed in the system. Ensure
        /// that the <paramref name="bedTypeId"/> corresponds to an existing bed type; otherwise, the operation may
        /// fail.</remarks>
        /// <param name="bedTypeId">The unique identifier of the bed type to be removed. This parameter cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveBedType(string bedTypeId, CancellationToken cancellationToken = default);

        #endregion

        #region MealPlans

        /// <summary>
        /// Creates a new meal plan based on the provided details.
        /// </summary>
        /// <remarks>Ensure that the <paramref name="mealPlan"/> object contains valid data before calling
        /// this method. The operation may take time depending on the complexity of the meal plan and system
        /// load.</remarks>
        /// <param name="mealPlan">The details of the meal plan to be created. This must include all required fields for a valid meal plan.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see langword="default"/>, which
        /// means the operation cannot be canceled.</param>
        /// <returns>A task that represents the asynchronous operation. The result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> CreateMealPlan(MealPlanDto mealPlan, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a meal plan identified by the specified ID.
        /// </summary>
        /// <remarks>Use this method to remove an existing meal plan from the system. Ensure that the
        /// provided <paramref name="mealPlanId"/> corresponds to a valid meal plan.</remarks>
        /// <param name="mealPlanId">The unique identifier of the meal plan to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveMealPlan(string mealPlanId, CancellationToken cancellationToken = default);

        #endregion

        #region Images

        /// <summary>
        /// Retrieves a collection of featured images associated with the specified room.
        /// </summary>
        /// <remarks>This method is asynchronous and should be awaited. If no featured images are found
        /// for the specified room, the returned collection will be empty.</remarks>
        /// <param name="roomId">The unique identifier of the room for which featured images are requested. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a collection of <see cref="FeaturedImageDto"/> objects representing the featured images for the
        /// specified room.</returns>
        Task<IBaseResult<ICollection<FeaturedImageDto>>> FeaturedImagesAsync(string roomId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new featured image to the system.
        /// </summary>
        /// <remarks>The method validates the provided <paramref name="dto"/> before adding the featured
        /// image. Ensure that all required fields in the DTO are populated to avoid validation errors.</remarks>
        /// <param name="dto">The data transfer object containing the details of the featured image to add.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the added <see cref="FeaturedImageDto"/> if the operation is successful.</returns>
        Task<IBaseResult<FeaturedImageDto>> AddFeaturedImage(FeaturedImageDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a featured room image based on the specified image ID.
        /// </summary>
        /// <remarks>Use this method to remove a featured room image from the system. Ensure that the
        /// <paramref name="featuredImageId"/>  corresponds to an existing image. If the operation fails, the returned
        /// <see cref="IBaseResult"/> will contain  details about the failure.</remarks>
        /// <param name="featuredImageId">The unique identifier of the featured room image to be removed.  This parameter cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveFeaturedRoomImageAsync(string featuredImageId, CancellationToken cancellationToken = default);

        #endregion
	}
}
