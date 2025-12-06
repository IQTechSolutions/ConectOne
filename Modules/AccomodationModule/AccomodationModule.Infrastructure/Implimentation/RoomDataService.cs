using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using FilingModule.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Provides functionality for managing room data, including CRUD operations, image handling, child policy rules,
    /// meal plans, bed types, pricing, and integration with external services.
    /// </summary>
    /// <remarks>This service interacts with various repositories and external APIs to manage room-related
    /// data. It supports operations such as retrieving paginated room lists, creating and updating rooms, handling
    /// images, managing child policy rules, and integrating with external services like NightsBridge and
    /// Cimso.</remarks>
    /// <param name="accomodationRepositoryManager"></param>
    /// <param name="nightBridgeServiceV5"></param>
    /// <param name="nightBridgeServiceV4"></param>
    /// <param name="cimsoService"></param>
	public class RoomDataService(IAccomodationRepositoryManager accomodationRepositoryManager) : IRoomDataService
	{
        /// <summary>
        /// Retrieves a paginated list of rooms associated with the specified package.
        /// </summary>
        /// <remarks>The method queries the repository for rooms associated with the specified package ID.
        /// The returned rooms include related data such as amenities, meal plans, images, featured images,  child
        /// policy rules, and package details.</remarks>
        /// <param name="packageId">The unique identifier of the package for which rooms are to be retrieved. Must be a valid string
        /// representation of an integer.</param>
        /// <param name="args">The pagination parameters, including page number and page size.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of rooms. If the operation succeeds,
        /// the result includes the room data and pagination details. If the operation fails, the result contains error
        /// messages.</returns>
		public async Task<PaginatedResult<RoomDto>> PagedRoomsAsync(RequestParameters args, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Room>(c => true);
            spec.AddInclude(g => g.Include(c => c.Amneties));
            spec.AddInclude(g => g.Include(c => c.MealPlans));
            spec.AddInclude(g => g.Include(c => c.Images).ThenInclude(c => c.Image));
            spec.AddInclude(g => g.Include(c => c.FeaturedImages));
            spec.AddInclude(g => g.Include(c => c.ChildPolicyRules));
            spec.AddInclude(g => g.Include(c => c.Amneties));
            spec.AddInclude(g => g.Include(c => c.Package).ThenInclude(c => c.Lodging));

            var result = await accomodationRepositoryManager.Rooms.ListAsync(spec, false, cancellationToken);
			if (!result.Succeeded) return PaginatedResult<RoomDto>.Failure(result.Messages);
            {
				return PaginatedResult<RoomDto>.Success(result.Data.Select(c => new RoomDto(c)).ToList(), result.Data.Count(), args.PageNr, args.PageSize);
			}
			
		}

        /// <summary>
        /// Retrieves detailed information about a room, including its amenities, bed types, meal plans, images, child
        /// policy rules, and associated package.
        /// </summary>
        /// <remarks>This method queries the database for a room matching the specified <paramref
        /// name="roomId"/> and includes related entities such as amenities, bed types, meal plans, images, and child
        /// policy rules. If no matching room is found, the result will indicate failure with an appropriate error
        /// message.</remarks>
        /// <param name="roomId">The unique identifier of the room to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult{RoomDto}"/> containing the room details if found, or an error result if the room
        /// does not exist or the operation fails.</returns>
		public async Task<IBaseResult<RoomDto>> RoomAsync(int roomId, CancellationToken cancellationToken = default)
		{
            var spec = new LambdaSpec<Room>(c => c.Id == roomId);
            spec.AddInclude(g => g.Include(c => c.Amneties));
            spec.AddInclude(g => g.Include(c => c.BedTypes));
            spec.AddInclude(g => g.Include(c => c.MealPlans));
            spec.AddInclude(g => g.Include(c => c.Images).ThenInclude(c => c.Image));
            spec.AddInclude(g => g.Include(c => c.FeaturedImages));
            spec.AddInclude(g => g.Include(c => c.ChildPolicyRules.OrderBy(c => c.MinAge)));
            spec.AddInclude(g => g.Include(c => c.Package).ThenInclude(c => c.Lodging));


            var result = await accomodationRepositoryManager.Rooms.FirstOrDefaultAsync(spec, false, cancellationToken);
			if (!result.Succeeded) return await Result<RoomDto>.FailAsync(result.Messages);

            if (result.Data is null)
                return await Result<RoomDto>.FailAsync($"No room macthing id '{roomId}' was found in the database");
            return await Result<RoomDto>.SuccessAsync(new RoomDto(result.Data));
        }

		#region CRUD Functions

        /// <summary>
        /// Creates a new room asynchronously.
        /// </summary>
        /// <remarks>This method attempts to create a new room using the provided details. If the creation
        /// is successful, the result will include the newly created room's details. If the operation fails, the result
        /// will include error messages describing the failure.</remarks>
        /// <param name="detail">The details of the room to be created, represented as a <see cref="RoomDto"/> object.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/> if not
        /// provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with the created room details as a <see cref="RoomDto"/> if the operation succeeds, or error messages if it
        /// fails.</returns>
		public async Task<IBaseResult<RoomDto>> CreateRoomAsync(RoomDto detail, CancellationToken cancellationToken = default)
        {
            var room = detail.ToRoom();
            var result = await accomodationRepositoryManager.Rooms.CreateAsync(room, cancellationToken);
            if (!result.Succeeded) return await Result<RoomDto>.FailAsync(result.Messages);

            var saveResult = await accomodationRepositoryManager.Rooms.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<RoomDto>.FailAsync(saveResult.Messages);

            return await Result<RoomDto>.SuccessAsync(new RoomDto(result.Data));
        }

        /// <summary>
        /// Updates the details of an existing room asynchronously.
        /// </summary>
        /// <remarks>This method updates the room's details, including its amenities, cover image, and
        /// gallery images.  It performs validation to ensure the room exists and handles the addition or removal of
        /// amenities and images as necessary. If the room does not exist or an error occurs during the update process,
        /// the method returns a failure result.</remarks>
        /// <param name="model">The <see cref="RoomDto"/> containing the updated room information, including its properties, amenities, and
        /// images.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/> if not
        /// provided.</param>
        /// <returns>A result object containing the updated <see cref="RoomDto"/> if the operation succeeds; otherwise, an error
        /// message indicating the failure.</returns>
        public async Task<IBaseResult<RoomDto>> UpdateRoomAsync(RoomDto model, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Room>(c => c.Id == Convert.ToInt32(model.RoomTypeId));
            spec.AddInclude(g => g.Include(c => c.Amneties));

            var result = await accomodationRepositoryManager.Rooms.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!result.Succeeded) return await Result<RoomDto>.FailAsync(result.Messages);

            var room = result.Data;
            if (room is null)
                return await Result<RoomDto>.FailAsync($"No Room matching id '{model.RoomTypeId}' was found in the database");

            room.PartnerRoomTypeId = model.UniqueServicePartnerRoomTypeId;
            room.Name = model.Name;
            room.Description = model.Description;
            room.AdditionalInfo = model.AdditionalInfo;
            room.DefaultMealPlanId = model.DefaultMealPlanId;
            room.DefaultBedTypeId = model.DefaultBedTypeId;
            room.RoomCount = model.RoomCount;
            room.MaxOccupancy = model.MaxOccupancy;
            room.MaxAdults = model.MaxAdults;
            room.FirstChildStaysFree = model.FirstChildStaysFree;
            room.BookingTerms = model.BookingTerms;
            room.CancellationPolicy = model.CancellationPolicy;
            room.RateScheme = model.RateScheme;
            room.VoucherRate = model.VoucherRate;

            foreach (var item in model.Amenities)
            {
                if (!room.Amneties.Any(c => c.Name.Trim().Equals(item.Name.Trim(), StringComparison.CurrentCultureIgnoreCase)))
                    await accomodationRepositoryManager.RoomAmenities.CreateAsync(item.ToServiceAmenity(), cancellationToken);
            }
            
            foreach (var r in room.Amneties.ToList())
            {
                if (!model.Amenities.Any(c => c.Name.Trim().Equals(r.Name.Trim(), StringComparison.CurrentCultureIgnoreCase)))
                    accomodationRepositoryManager.RoomAmenities.Delete(r);
            }

            var gg = accomodationRepositoryManager.Rooms.Update(room);

            var saveResult = await accomodationRepositoryManager.Rooms.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
                return await Result<RoomDto>.SuccessAsync(new RoomDto(room));
            return await Result<RoomDto>.FailAsync(saveResult.Messages);
        }

        /// <summary>
        /// Removes a room from the database based on the specified room ID.
        /// </summary>
        /// <remarks>This method attempts to locate the room by its ID and remove it from the database. If
        /// the room does not exist, the operation fails with an appropriate error message. The method also ensures that
        /// changes are saved to the database before returning a success result.</remarks>
        /// <param name="roomId">The unique identifier of the room to be removed.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the outcome of the operation. If successful, the result contains a
        /// success message. If the operation fails, the result contains error messages.</returns>
        public async Task<IBaseResult> RemoveRoomAsync(int roomId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Room>(c => c.Id == roomId);

            var result = await accomodationRepositoryManager.Rooms.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);

            if (result.Data == null)
                return await Result.FailAsync($"No room with id matching'{result.Data?.PartnerRoomTypeId}' found in the database");

            accomodationRepositoryManager.Rooms.Delete(result.Data);
            var saveResult = await accomodationRepositoryManager.Rooms.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
            {
                return await Result.SuccessAsync($"{result.Data.Name} was successfully removed");
            }
            return await Result.FailAsync(saveResult.Messages);
        }

        #endregion

        #region Images

        /// <summary>
        /// Retrieves the featured images associated with a specific room.
        /// </summary>
        /// <remarks>This method queries the repository for featured images associated with the specified
        /// room. The returned collection will be empty if no featured images are found.</remarks>
        /// <param name="roomId">The unique identifier of the room for which featured images are requested. Must be a valid, non-null string.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An asynchronous operation that returns a result containing a collection of <see cref="FeaturedImageDto"/>
        /// objects. If the operation succeeds, the result contains the featured images for the specified room. If the
        /// operation fails, the result contains error messages describing the failure.</returns>
        public async Task<IBaseResult<ICollection<FeaturedImageDto>>> FeaturedImagesAsync(string roomId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<FeaturedImage>(c => c.RoomId.ToString() == roomId);
            var result = await accomodationRepositoryManager.FeaturedImages.ListAsync(spec, false, cancellationToken);
            if (result.Succeeded)
                return await Result<ICollection<FeaturedImageDto>>.SuccessAsync(result.Data.Select(c => new FeaturedImageDto(c)).ToList());
            return await Result<ICollection<FeaturedImageDto>>.FailAsync(result.Messages);
        }

        /// <summary>
        /// Adds a featured image to the repository.
        /// </summary>
        /// <remarks>This method creates a new featured image entry in the repository using the provided
        /// data. The operation may fail if the repository encounters an error or if the provided data is
        /// invalid.</remarks>
        /// <param name="dto">The data transfer object containing the details of the featured image to be added. Must include a valid <see
        /// cref="FeaturedImageDto.EntityId"/> and <see cref="FeaturedImageDto.ImageUrl"/>.</param>
        /// <param name="cancellationToken">An optional token to monitor for cancellation requests.</param>
        /// <returns>A result object containing the added featured image data if the operation succeeds, or error messages if it
        /// fails.</returns>
		public async Task<IBaseResult<FeaturedImageDto>> AddFeaturedImage(FeaturedImageDto dto, CancellationToken cancellationToken = default)
		{
			var featuredServiceImage = new FeaturedImage(null, Convert.ToInt32(dto.EntityId), dto.ImageUrl, UploadType.Image);
			var result = await accomodationRepositoryManager.FeaturedImages.CreateAsync(featuredServiceImage, cancellationToken);

            var saveResult = await accomodationRepositoryManager.FeaturedImages.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
			{
				return await Result<FeaturedImageDto>.SuccessAsync(new FeaturedImageDto(result.Data));
			}
			return await Result<FeaturedImageDto>.FailAsync(saveResult.Messages);
		}

        /// <summary>
        /// Removes a featured room image from the database.
        /// </summary>
        /// <remarks>This method attempts to locate the featured image by its identifier and remove it
        /// from the database. If the image is not found, the operation fails with an appropriate error message. The
        /// method also ensures that changes are saved to the database before returning a success result.</remarks>
        /// <param name="featuredImageId">The unique identifier of the featured image to be removed.  This parameter cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the outcome of the operation.  If successful, the result contains a
        /// success message.  If the operation fails, the result contains error messages describing the failure.</returns>
        public async Task<IBaseResult> RemoveFeaturedRoomImageAsync(string featuredImageId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<FeaturedImage>(c => c.Id == featuredImageId);

            var result = await accomodationRepositoryManager.FeaturedImages.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IBaseResult>.FailAsync(result.Messages);

            if (result.Data == null)
                return await Result<IBaseResult>.FailAsync($"No featured image with id matching '{featuredImageId}' was found in the database");

            accomodationRepositoryManager.FeaturedImages.Delete(result.Data);

            var saveResult = await accomodationRepositoryManager.FeaturedImages.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
            {
                return await Result<IBaseResult>.SuccessAsync("Featured Image removed successfully");
            }
            return await Result<IBaseResult>.FailAsync(saveResult.Messages);
        }

        #endregion

        #region Child Policy Rules

        /// <summary>
        /// Retrieves the child policy rules associated with a specific room.
        /// </summary>
        /// <remarks>This method queries the child policy rules for the specified room and maps them to
        /// <see cref="ChildPolicyRuleDto"/> objects. The caller can use the returned result to determine whether the
        /// operation succeeded and access the associated data or error messages.</remarks>
        /// <param name="roomId">The unique identifier of the room for which child policy rules are to be retrieved.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An asynchronous operation that returns a result containing a collection of <see cref="ChildPolicyRuleDto"/>
        /// objects. The collection is ordered by the minimum age defined in the child policy rules. If the operation
        /// fails, the result will include failure messages.</returns>
        public async Task<IBaseResult<IEnumerable<ChildPolicyRuleDto>>> ChildPolicies(int roomId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ChildPolicyRule>(c => c.RoomId == roomId);

            var result = await accomodationRepositoryManager.ChildPolicyRules.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<ChildPolicyRuleDto>>.FailAsync(result.Messages);

            return await Result<IEnumerable<ChildPolicyRuleDto>>.SuccessAsync(result.Data.OrderBy(c => c.MinAge).Select(c => new ChildPolicyRuleDto(c)).ToList());
        }

        /// <summary>
        /// Retrieves the child policy rule associated with the specified room and rule identifier.
        /// </summary>
        /// <remarks>This method performs a lookup for a child policy rule based on the provided room and
        /// rule identifiers. If no matching rule is found, the result will indicate failure with appropriate error
        /// messages.</remarks>
        /// <param name="roomId">The unique identifier of the room for which the child policy rule is being retrieved.</param>
        /// <param name="childPolicyRuleId">The unique identifier of the child policy rule to retrieve.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object, which includes the <see cref="ChildPolicyRuleDto"/> representing the child policy rule if the
        /// operation succeeds, or error messages if the operation fails.</returns>
        public async Task<IBaseResult<ChildPolicyRuleDto>> ChildPolicy(int roomId, string childPolicyRuleId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ChildPolicyRule>(c => c.RoomId == roomId && c.Id == childPolicyRuleId);

            var result = await accomodationRepositoryManager.ChildPolicyRules.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<ChildPolicyRuleDto>.FailAsync(result.Messages);

            return await Result<ChildPolicyRuleDto>.SuccessAsync(new ChildPolicyRuleDto(result.Data));
        }

        /// <summary>
        /// Creates a new child policy rule and saves it to the repository.
        /// </summary>
        /// <remarks>This method creates a child policy rule based on the provided <paramref
        /// name="childPolicy"/> object and attempts to save it to the repository. If the save operation fails, the
        /// result will contain the failure messages.</remarks>
        /// <param name="childPolicy">The data transfer object representing the child policy rule to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the outcome of the operation. Returns a success result if the child
        /// policy rule was added successfully; otherwise, returns a failure result with error messages.</returns>
        public async Task<IBaseResult> CreateChildPolicyRule(ChildPolicyRuleDto childPolicy, CancellationToken cancellationToken = default)
        {
            await accomodationRepositoryManager.ChildPolicyRules.CreateAsync(childPolicy.ToChildPolicyRule(), cancellationToken);
            var saveResult = await accomodationRepositoryManager.ChildPolicyRules.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
                return await Result.SuccessAsync("Child Policy Rule was successfully added");
            return await Result.FailAsync(saveResult.Messages);
        }

        /// <summary>
        /// Updates an existing child policy rule with the specified details.
        /// </summary>
        /// <remarks>This method updates the child policy rule identified by <see
        /// cref="ChildPolicyRuleDto.ChildPolicyRuleId"/> with the provided details, including age limits, allowance
        /// status, special rate usage, formula values, and custom descriptions. If the update is successful, the
        /// changes are persisted to the database.</remarks>
        /// <param name="childPolicy">The <see cref="ChildPolicyRuleDto"/> containing the updated details for the child policy rule.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the update operation. If successful, the result includes a success
        /// message; otherwise, it includes error messages describing the failure.</returns>
        public async Task<IBaseResult> UpdateChildPolicyRule(ChildPolicyRuleDto childPolicy, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ChildPolicyRule>(c => c.Id == childPolicy.ChildPolicyRuleId);

            var childPolicyResult = await accomodationRepositoryManager.ChildPolicyRules.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (childPolicyResult.Succeeded)
            {
                childPolicyResult.Data.MinAge = childPolicy.MinAge;
                childPolicyResult.Data.MaxAge = childPolicy.MaxAge;
                childPolicyResult.Data.Allowed = childPolicy.Allowed;
                childPolicyResult.Data.UseSpecialRate = childPolicy.UseSpecialRate;
                childPolicyResult.Data.ChildPolicyFormualaValue = childPolicy.Ammount;
                childPolicyResult.Data.ChildPolicyFormualaType = childPolicy.Rule;
                childPolicyResult.Data.CustomDescription = childPolicy.CustomDescription;

                accomodationRepositoryManager.ChildPolicyRules.Update(childPolicyResult.Data);

                var saveResult = await accomodationRepositoryManager.BedTypes.SaveAsync(cancellationToken);
                if (saveResult.Succeeded)
                    return await Result.SuccessAsync("Child Policy Successfully Updated");
                return await Result.FailAsync(saveResult.Messages);
            }
            return await Result.FailAsync(childPolicyResult.Messages);
        }

        /// <summary>
        /// Removes a child policy rule identified by the specified ID.
        /// </summary>
        /// <remarks>This method attempts to locate the child policy rule in the database using the
        /// provided ID. If the rule is found, it is deleted, and the changes are saved to the database. If the rule is
        /// not found or the save operation fails, the method returns a failure result with appropriate error
        /// messages.</remarks>
        /// <param name="id">The unique identifier of the child policy rule to be removed. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the outcome of the operation. If successful, the result contains a
        /// success message. If the operation fails, the result contains error messages.</returns>
        public async Task<IBaseResult> RemoveChildPolicyRule(string id, CancellationToken cancellationToken = default)
        {
            await accomodationRepositoryManager.ChildPolicyRules.DeleteAsync(id, cancellationToken);

            var saveResult = await accomodationRepositoryManager.ChildPolicyRules.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
                return await Result.SuccessAsync("Child Policy Rule was successfully removed");
            return await Result.FailAsync(saveResult.Messages);
        }

        #endregion

        #region Meal Plans

        /// <summary>
        /// Creates a new meal plan and saves it to the repository.
        /// </summary>
        /// <remarks>This method creates a meal plan by converting the provided <paramref
        /// name="mealPlan"/> DTO into a domain entity  and saving it to the repository. Ensure that the <paramref
        /// name="mealPlan"/> contains valid data before calling this method.</remarks>
        /// <param name="mealPlan">The data transfer object containing the details of the meal plan to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/> if not
        /// provided.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the outcome of the operation.  Returns a success result if the meal
        /// plan was added successfully; otherwise, returns a failure result with error messages.</returns>
        public async Task<IBaseResult> CreateMealPlan(MealPlanDto mealPlan, CancellationToken cancellationToken = default)
        {
            await accomodationRepositoryManager.MealPlans.CreateAsync(mealPlan.ToMealPlan(), cancellationToken);
            var saveResult = await accomodationRepositoryManager.MealPlans.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
                return await Result.SuccessAsync("Meal Plan was successfully added");
            return await Result.FailAsync(saveResult.Messages);
        }

        /// <summary>
        /// Removes a meal plan from the database based on the specified meal plan ID.
        /// </summary>
        /// <remarks>This method performs a lookup for the specified meal plan ID in the database. If the
        /// meal plan exists,  it is removed, and the changes are saved. If the meal plan does not exist or the save
        /// operation fails,  the method returns a failure result with appropriate messages.</remarks>
        /// <param name="mealPlanId">The unique identifier of the meal plan to be removed. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the outcome of the operation.  If successful, the result contains a
        /// success message. If the meal plan is not found or the operation fails,  the result contains failure
        /// messages.</returns>
        public async Task<IBaseResult> RemoveMealPlan(string mealPlanId, CancellationToken cancellationToken = default)
        {
            await accomodationRepositoryManager.MealPlans.DeleteAsync(mealPlanId, cancellationToken);

            var saveResult = await accomodationRepositoryManager.MealPlans.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
                return await Result.SuccessAsync("Meal Plan was successfully removed");
            return await Result.FailAsync(saveResult.Messages);
        }

        #endregion

        #region Bed Types

        /// <summary>
        /// Retrieves a list of bed types associated with the specified room.
        /// </summary>
        /// <remarks>This method queries the repository for bed types associated with the specified room
        /// and maps the results to <see cref="BedTypeDto"/> objects. The operation may fail if the room does not exist
        /// or if there is an issue retrieving the data.</remarks>
        /// <param name="roomId">The unique identifier of the room for which bed types are to be retrieved.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An asynchronous operation that returns an <see cref="IBaseResult{T}"/> containing a collection of <see
        /// cref="BedTypeDto"/> objects. If the operation succeeds, the result contains the list of bed types. If the
        /// operation fails, the result contains error messages.</returns>
        public async Task<IBaseResult<IEnumerable<BedTypeDto>>> BedTypeList(int roomId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<BedType>(c => c.RoomId == roomId);

            var result = await accomodationRepositoryManager.BedTypes.ListAsync(spec, false, cancellationToken);
            if (result.Succeeded)
            {
                return await Result<IEnumerable<BedTypeDto>>.SuccessAsync(result.Data.Select(c => new BedTypeDto(c)).ToList());
            }
            return await Result<IEnumerable<BedTypeDto>>.FailAsync(result.Messages);
        }

        /// <summary>
        /// Retrieves information about a specific bed type based on its unique identifier.
        /// </summary>
        /// <remarks>This method queries the repository for a bed type matching the specified identifier.
        /// If the bed type exists, its details are returned in a <see cref="BedTypeDto"/> object. If the bed type does
        /// not exist, the result will indicate failure and include error messages describing the issue.</remarks>
        /// <param name="bedTypeId">The unique identifier of the bed type to retrieve. This parameter cannot be null or empty.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult{T}"/> containing a <see cref="BedTypeDto"/> object with details of the bed type if
        /// found. If the bed type is not found, the result will indicate failure and include relevant error messages.</returns>
        public async Task<IBaseResult<BedTypeDto>> BedType(string bedTypeId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<BedType>(c => c.Id == bedTypeId);

            var result = await accomodationRepositoryManager.BedTypes.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (result.Succeeded)
            {
                return await Result<BedTypeDto>.SuccessAsync(new BedTypeDto(result.Data));
            }

            return await Result<BedTypeDto>.FailAsync(result.Messages);
        }

        /// <summary>
        /// Creates a new bed type and adds it to the repository.
        /// </summary>
        /// <remarks>If the <paramref name="bedType"/> specifies that it is a default entry, the method
        /// attempts to update the associated room's default bed type ID.</remarks>
        /// <param name="bedType">The data transfer object containing the details of the bed type to be created.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation, along with any relevant messages.</returns>
        public async Task<IBaseResult> CreateBedType(BedTypeDto bedType, CancellationToken cancellationToken = default)
        {
            var bedTypeToAdd = bedType.ToBedType();
            bedTypeToAdd.Id = Guid.NewGuid().ToString();

            var result = await accomodationRepositoryManager.BedTypes.CreateAsync(bedTypeToAdd, cancellationToken);

            if (!result.Succeeded)
                return await Result.FailAsync(result.Messages);

            var saveResult = await accomodationRepositoryManager.BedTypes.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            if (bedType.DefaultEntry)
            {
                var spec = new LambdaSpec<Room>(c => c.Id == bedType.RoomId);

                var roomResult = await accomodationRepositoryManager.Rooms.FirstOrDefaultAsync(spec, true, cancellationToken);
                if (roomResult.Succeeded)
                {
                    roomResult.Data.DefaultBedTypeId = bedTypeToAdd.Id;

                    accomodationRepositoryManager.Rooms.Update(roomResult.Data);
                    await accomodationRepositoryManager.Rooms.SaveAsync(cancellationToken);
                }
            }


            return await Result.SuccessAsync("Bed Type was successfully added");
        }

        /// <summary>
        /// Updates the details of an existing bed type.
        /// </summary>
        /// <remarks>This method updates the description and bed count of an existing bed type identified
        /// by its ID. If the bed type is not found, the operation fails and returns an appropriate error message.
        /// Ensure that the <paramref name="bedType"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="bedType">The data transfer object containing the updated details of the bed type, including its ID, description, and
        /// bed count.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the update. If successful, the result includes a success message;
        /// otherwise, it includes error messages.</returns>
        public async Task<IBaseResult> UpdateBedType(BedTypeDto bedType, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<BedType>(c => c.Id == bedType.BedTypeId);

            var result = await accomodationRepositoryManager.BedTypes.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (result.Succeeded)
            {
                result.Data.Description = bedType.Description;
                result.Data.BedCount = bedType.BedCount;

                accomodationRepositoryManager.BedTypes.Update(result.Data);
                
                var saveResult = await accomodationRepositoryManager.BedTypes.SaveAsync(cancellationToken);
                if (saveResult.Succeeded)
                    return await Result.SuccessAsync("Bed type successfully updated");
                return await Result.FailAsync(saveResult.Messages);
            }
            return await Result.FailAsync(result.Messages);
        }

        /// <summary>
        /// Removes a bed type from the database based on the specified bed type ID.
        /// </summary>
        /// <remarks>This method attempts to locate the bed type in the database using the provided ID. If
        /// the bed type is found, it is removed, and the changes are saved. If the bed type does not exist or the save
        /// operation fails, the method returns a failure result with appropriate messages.</remarks>
        /// <param name="bedTypeId">The unique identifier of the bed type to be removed. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the outcome of the operation. If successful, the result contains a
        /// success message. If the bed type is not found or the operation fails, the result contains failure messages.</returns>
        public async Task<IBaseResult> RemoveBedType(string bedTypeId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<BedType>(c => c.Id == bedTypeId);

            var result = await accomodationRepositoryManager.BedTypes.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);

            if (result.Data == null)
                return await Result.FailAsync($"There was no bed type matching '{bedTypeId}' found in the database");

            accomodationRepositoryManager.BedTypes.Delete(result.Data);

            var saveResult = await accomodationRepositoryManager.BedTypes.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
                return await Result.SuccessAsync("Bed Type was successfully removed");
            return await Result.FailAsync(saveResult.Messages);
        }

        #endregion

        #region Pricing

        /// <summary>
        /// Updates the pricing details for a specific room, including commission and markup values.
        /// </summary>
        /// <remarks>This method updates the commission and markup values for the specified room and saves
        /// the changes to the database. If the room is not found, no updates are performed. The method returns a
        /// success result regardless of whether the room exists, but the caller can inspect the result message for
        /// confirmation.</remarks>
        /// <param name="roomId">The unique identifier of the room whose pricing is to be updated.</param>
        /// <param name="commission">The commission percentage to be applied to the room's pricing. Must be a non-negative value.</param>
        /// <param name="markup">The markup percentage to be applied to the room's pricing. Must be a non-negative value.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation, along with a message describing the outcome.</returns>
        public async Task<IBaseResult> UpdateRoomPricing(string roomId, double commission, double markup, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Room>(c => c.Id.ToString() == roomId);

            var roomResult = await accomodationRepositoryManager.Rooms.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (roomResult.Succeeded)
            {
                roomResult.Data.Commision = commission;
                roomResult.Data.MarkUp = markup;

                accomodationRepositoryManager.Rooms.Update(roomResult.Data);
            }

            var saveResult = await accomodationRepositoryManager.Rooms.SaveAsync(cancellationToken);
            return await Result.SuccessAsync("Rooms Pricing updated succesfully");
        }

        #endregion

    }
}
