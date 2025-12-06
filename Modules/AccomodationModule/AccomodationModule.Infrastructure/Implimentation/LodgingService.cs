using AccomodationModule.Domain.Arguments.Response;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using FilingModule.Domain.Enums;
using FilingModule.Domain.RequestFeatures;
using GroupingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ProductsModule.Domain.DataTransferObjects;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Service implementation for managing lodgings.
    /// </summary>
    public class LodgingService(ILodgingCategoryService categoryService, IAccomodationRepositoryManager accomodationRepo, IRepository<EntityImage<Lodging, string>, string> imageRepository, 
        IRepository<EntityVideo<Lodging, string>, string> lodgingVideoRepository) : ILodgingService
    {
        #region Lodgings

        /// <summary>
        /// Retrieves the total count of lodgings asynchronously.
        /// </summary>
        /// <remarks>This method queries the lodging repository to determine the total number of lodgings.
        /// It returns a result object containing the count of lodgings or an error message if the operation
        /// fails.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// object with a <see cref="LodgingCountResponse"/> indicating the total count of lodgings. If the operation
        /// fails, the result contains error messages.</returns>
        public async Task<IBaseResult<LodgingCountResponse>> LodgingCountAsync(CancellationToken cancellationToken = default)
        {
            var result = await accomodationRepo.Lodgings.ListAsync(false, cancellationToken);
            if (!result.Succeeded) return await Result<LodgingCountResponse>.FailAsync(result.Messages);

            return await Result<LodgingCountResponse>.SuccessAsync(new LodgingCountResponse()
                { LodgingCount = result.Data.Count() });
        }

        /// <summary>
        /// Retrieves a paginated list of lodgings based on the provided page parameters.
        /// </summary>
        /// <param name="pageParameters">The parameters for pagination and filtering.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A paginated result containing lodging DTOs.</returns>
        public async Task<PaginatedResult<LodgingDto>> PagedLodgingsAsync(LodgingParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Lodging>(c => c.Settings.Active == pageParameters.Active);
            spec.AddInclude(c => c.Include(c => c.Images).ThenInclude(c => c.Image));
            spec.AddInclude(c => c.Include(c => c.Settings));
            spec.AddInclude(c => c.Include(c => c.Country));
            spec.AddInclude(c => c.Include(c => c.Categories).ThenInclude(c => c.Category));
            spec.AddInclude(c => c.Include(c => c.Amneties).ThenInclude(c => c.Amenity));

            var result = await accomodationRepo.Lodgings.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return PaginatedResult<LodgingDto>.Failure(result.Messages);

            var response = result.Data;
            if (!string.IsNullOrEmpty(pageParameters.SearchText))
            {
                response = response.Where(c => c.Name.ToUpper().Contains(pageParameters.SearchText)).ToList();
            }

            if (pageParameters.AllowBookings is not null)
                response = response.Where(c => c.Settings.AllowBookings == pageParameters.AllowBookings).ToList();

            if (!string.IsNullOrEmpty(pageParameters.CategoryIds))
            {
                var catgoryIdList = pageParameters.CategoryIds.Split(";").ToList();

                var cc = new List<Lodging>();
                foreach (var r in response)
                {
                    if (r.Categories.Any(v => catgoryIdList.Contains(v.CategoryId)))
                    {
                        cc.Add(r);
                    }
                }

                response = cc;
            }

            if (!string.IsNullOrEmpty(pageParameters.AmenityIds))
            {
                var amenityIdList = pageParameters.AmenityIds.Split(";").ToList();

                var cc = new List<Lodging>();
                foreach (var r in response)
                {
                    if (r.Amneties.Any(v => amenityIdList.Contains(v.AmenityId.ToString())))
                    {
                        cc.Add(r);
                    }
                }

                response = cc;
            }

            var pagedList = response.Select(c => new LodgingDto(c, CalculatePriceAsync(c).Result.Data)).ToList();

            

            var min = pageParameters.MinPrice;
            var max = pageParameters.MaxPrice;

            pagedList = pagedList.ToList();

            return PaginatedResult<LodgingDto>.Success(pagedList, pagedList.Count, pageParameters.PageNr,
                pageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves all lodgings, optionally filtered by category ID.
        /// </summary>
        /// <param name="categoryId">The category ID to filter by (optional).</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result containing a list of lodging DTOs.</returns>
        public async Task<IBaseResult<List<LodgingDto>>> AllLodgings(string? categoryId = null, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Lodging>(c => true);
            spec.AddInclude(c => c.Include(c => c.Images).ThenInclude(c => c.Image));
            spec.AddInclude(c => c.Include(c => c.Categories));
            spec.AddInclude(c => c.Include(c => c.Settings));
            spec.AddInclude(c => c.Include(c => c.Country));

            var result = await accomodationRepo.Lodgings.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<List<LodgingDto>>.FailAsync(result.Messages);

            if (!string.IsNullOrEmpty(categoryId))
            {
                var categorySpec = new LambdaSpec<Lodging>(c => c.Categories.Any(g => g.CategoryId == categoryId));
                categorySpec.AddInclude(c => c.Include(c => c.Images).ThenInclude(c => c.Image));
                categorySpec.AddInclude(c => c.Include(c => c.CancellationRules));

                var categoryLodgingsResult = await accomodationRepo.Lodgings.ListAsync(categorySpec, false, cancellationToken);
                if (categoryLodgingsResult.Succeeded)
                {
                    return await Result<List<LodgingDto>>.SuccessAsync(categoryLodgingsResult.Data.Select(c => new LodgingDto(c, CalculatePriceAsync(c, cancellationToken).Result.Data)).ToList());
                }
                return await Result<List<LodgingDto>>.FailAsync(categoryLodgingsResult.Messages);
            }

            return await Result<List<LodgingDto>>.SuccessAsync(result.Data.Select(c => new LodgingDto(c, CalculatePriceAsync(c, cancellationToken).Result.Data)).ToList());
        }

        /// <summary>
        /// Retrieves a specific lodging by product ID.
        /// </summary>
        /// <param name="productId">The product ID of the lodging to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result containing the lodging DTO.</returns>
        public async Task<IBaseResult<LodgingDto>> LodgingAsync(string productId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Lodging>(c => c.Id == productId);
            spec.AddInclude(c => c.Include(c => c.Destinations).ThenInclude(c => c.Destination));
            spec.AddInclude(c => c.Include(c => c.Videos).ThenInclude(c => c.Video));
            spec.AddInclude(c => c.Include(c => c.Rooms));
            spec.AddInclude(c => c.Include(c => c.LodgingType));
            spec.AddInclude(c => c.Include(c => c.Services));
            spec.AddInclude(c => c.Include(c => c.Images).ThenInclude(c => c.Image));
            spec.AddInclude(c => c.Include(c => c.CancellationRules));
            spec.AddInclude(c => c.Include(c => c.Amneties).ThenInclude(c => c.Amenity));
            spec.AddInclude(c => c.Include(c => c.Categories).ThenInclude(c => c.Category).ThenInclude(c => c.Images));
            spec.AddInclude(c => c.Include(c => c.Settings));
            spec.AddInclude(c => c.Include(c => c.Country));

            var result = await accomodationRepo.Lodgings.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<LodgingDto>.FailAsync(result.Messages);

            if (result.Data == null)
                return await Result<LodgingDto>.FailAsync(
                    $"No lodging matching '{productId}' could be found in the database");

            var packageSpec = new LambdaSpec<LodgingPackage>(c => c.LodgingId == productId);
            packageSpec.AddInclude(c => c.Include(c => c.Rooms).ThenInclude(c => c.MealPlans));
            packageSpec.AddInclude(c => c.Include(c => c.Rooms).ThenInclude(c => c.BedTypes));
            packageSpec.AddInclude(c => c.Include(c => c.Rooms).ThenInclude(c => c.ChildPolicyRules));
            packageSpec.AddInclude(c => c.Include(c => c.Rooms).ThenInclude(c => c.Amneties));

            var accountTypeResult = accomodationRepo.Packages.FindByCondition(c => c.LodgingId.Equals(productId), false);
            if (!accountTypeResult.Succeeded) return await Result<LodgingDto>.FailAsync(accountTypeResult.Messages);

            result.Data.AccountTypes = await accountTypeResult.Data.ToListAsync(cancellationToken: cancellationToken);

            return await Result<LodgingDto>.SuccessAsync(new LodgingDto(result.Data, (await CalculatePriceAsync(result.Data, cancellationToken)).Data));
        }

        /// <summary>
        /// Retrieves a specific lodging by unique service ID.
        /// </summary>
        /// <param name="bbid">The unique service ID of the lodging to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result containing the lodging DTO.</returns>
        public async Task<IBaseResult<LodgingDto>> ProductByUniqueServiceIdAsync(string bbid, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Lodging>(c => !string.IsNullOrEmpty(c.UniquePartnerId) && c.UniquePartnerId == bbid);
            spec.AddInclude(c => c.Include(c => c.Videos).ThenInclude(c => c.Video));
            spec.AddInclude(c => c.Include(c => c.LodgingType));
            spec.AddInclude(c => c.Include(c => c.Services));
            spec.AddInclude(c => c.Include(c => c.Images).ThenInclude(c => c.Image));
            spec.AddInclude(c => c.Include(c => c.CancellationRules));
            spec.AddInclude(c => c.Include(c => c.Amneties));
            spec.AddInclude(c => c.Include(c => c.AccountTypes));
            spec.AddInclude(c => c.Include(c => c.Settings));
            spec.AddInclude(c => c.Include(c => c.Country));

            var result = await accomodationRepo.Lodgings.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (result.Succeeded)
            {
                if (result.Data == null)
                    return await Result<LodgingDto>.FailAsync($"No lodging matching unique partner id with '{bbid}' could be found in the database");
                return await Result<LodgingDto>.SuccessAsync(new LodgingDto(result.Data, (await CalculatePriceAsync(result.Data)).Data));
            }
            return await Result<LodgingDto>.FailAsync(result.Messages);
        }

        /// <summary>
        /// Creates a new lodging.
        /// </summary>
        /// <param name="nbProduct">The lodging DTO containing the details of the new lodging.</param>
        /// <param name="upload">The uploaded file (optional).</param>
        /// <param name="cropSettings">The crop settings for the uploaded file (optional).</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result containing the created lodging DTO.</returns>
        public async Task<IBaseResult<LodgingDto>> CreateLodgingAsync(LodgingDto nbProduct, CancellationToken cancellationToken = default)
        {
            var product = nbProduct.Create();
            var result = await accomodationRepo.Lodgings.CreateAsync(product, cancellationToken);
            if (!result.Succeeded) return await Result<LodgingDto>.FailAsync(result.Messages);

            foreach (var amenity in nbProduct.Amenities)
            {
                await accomodationRepo.LodgingAmenities.CreateAsync(new AmenityItem<Lodging, string>(product.Id, Convert.ToInt32(amenity.AmenityId)), cancellationToken);
            }

            foreach (var category in nbProduct.Categories)
            {
                await categoryService.CreateEntityCategoryAsync(category.CategoryId, product.Id);
            }

            var imageSaveResult = await accomodationRepo.Lodgings.SaveAsync(cancellationToken);
            if (!imageSaveResult.Succeeded)
                return await Result<LodgingDto>.FailAsync("Lodging was created and saved but the following error occured trying to update the cover image : " + imageSaveResult.Messages);
            
            return await Result<LodgingDto>.SuccessAsync(nbProduct);
        }

        /// <summary>
        /// Updates an existing lodging.
        /// </summary>
        /// <param name="lodging">The lodging DTO containing the updated details.</param>
        /// <param name="upload">The uploaded file (optional).</param>
        /// <param name="cropSettings">The crop settings for the uploaded file (optional).</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> UpdateLodgingAsync(LodgingDto lodging, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Lodging>(c => c.Id == lodging.ProductId);
            spec.AddInclude(c => c.Include(c => c.LodgingType));
            spec.AddInclude(c => c.Include(c => c.Images));
            spec.AddInclude(c => c.Include(c => c.Settings));
            spec.AddInclude(c => c.Include(c => c.Rooms));
            spec.AddInclude(c => c.Include(c => c.Country));

            var result = await accomodationRepo.Lodgings.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);

            var response = result.Data;

            if (response is null)
                return await Result.FailAsync($"No product matching id {lodging.ProductId} found in the database");

            response.Update(lodging);

            var packageSpec = new LambdaSpec<LodgingPackage>(c => c.LodgingId == lodging.ProductId);
            packageSpec.AddInclude(c => c.Include(c => c.Rooms));

            var packageResult = await accomodationRepo.Packages.ListAsync(packageSpec, false, cancellationToken);
            if (packageResult.Succeeded)
            {
                foreach (var item in packageResult.Data)
                {
                    var roomSpec = new LambdaSpec<Room>(c => c.PackageId == item.Id);

                    var roomResult = await accomodationRepo.Rooms.ListAsync(roomSpec, false, cancellationToken);

                    foreach (var roomItem in roomResult.Data)
                    {
                        accomodationRepo.Rooms.Update(roomItem);
                    }
                }
            }

            var categoryResult = await UpdateCategoriesAsync(lodging, cancellationToken);
            if (!categoryResult.Succeeded)
                return await Result.FailAsync($"An error occurred while trying to update categories for {lodging.Name} with the following details : {categoryResult.Messages}");

            var amenityResult = await UpdateAmenitiesAsync(lodging, cancellationToken);
            if (!amenityResult.Succeeded)
                return await Result.FailAsync($"An error occurred while trying to update amenities for {lodging.Name} with the following details : {amenityResult.Messages}");

            accomodationRepo.Lodgings.Update(response);

            var updateResult = await accomodationRepo.Lodgings.SaveAsync(cancellationToken);
            if (updateResult.Succeeded) return await Result.SuccessAsync($"{response.Name} was successfully updated");

            return await Result.FailAsync(updateResult.Messages);
        }

        /// <summary>
        /// Removes a specific lodging by ID.
        /// </summary>
        /// <param name="id">The ID of the lodging to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> RemoveLodgingAsync(string id, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Lodging>(c => c.Id == id);
            spec.AddInclude(c => c.Include(c => c.AccountTypes).ThenInclude(c => c.Rooms));

            var lodgingResult = await accomodationRepo.Lodgings.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!lodgingResult.Succeeded) return await Result.FailAsync(lodgingResult.Messages);

            foreach (var package in lodgingResult.Data!.AccountTypes)
            {
                foreach (var room in package.Rooms)
                {
                    await accomodationRepo.Rooms.DeleteAsync(room.Id, cancellationToken);
                }
                await accomodationRepo.Packages.DeleteAsync(package.Id, cancellationToken);
            }

            await accomodationRepo.Lodgings.DeleteAsync(id, cancellationToken);
            
            var result = await accomodationRepo.Lodgings.SaveAsync(cancellationToken);
            if (!result.Succeeded) return result;

            return await Result.SuccessAsync($"Room with id '{id}' was successfully removed");
        }

        #endregion

        #region Videos

        /// <summary>
        /// Adds a video to the lodging entity.
        /// </summary>
        /// <remarks>This method attempts to add a video to the specified lodging entity. It first creates
        /// the video entity and then saves it to the repository. If either the creation or saving operation fails, the
        /// method returns a failure result with the associated error messages.</remarks>
        /// <param name="request">The request containing the video details to be added, including the video ID and entity ID.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> AddVideo(AddEntityVideoRequest request, CancellationToken cancellationToken = default)
        {
            var video = new EntityVideo<Lodging, string>() { VideoId = request.VideoId, EntityId = request.EntityId};

            var addResult = await lodgingVideoRepository.CreateAsync(video, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Removes a video identified by the specified video ID.
        /// </summary>
        /// <remarks>This method attempts to delete the video from the repository and save the changes. If
        /// either operation fails, the method returns a failure result with the associated error messages.</remarks>
        /// <param name="videoId">The unique identifier of the video to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default)
        {
            var addResult = await lodgingVideoRepository.DeleteAsync(videoId, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }


        #endregion

        #region Images

        /// <summary>
        /// Adds an image to the specified entity.
        /// </summary>
        /// <remarks>This method creates an image entity and attempts to save it to the repository. If the
        /// operation fails at any step, it returns a failure result with the associated error messages.</remarks>
        /// <param name="request">The request containing the image and entity details. Must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var image = new EntityImage<Lodging, string>(request.ImageId, request.EntityId) { Selector = request.Selector, Order = request.Order };

            var addResult = await imageRepository.CreateAsync(image, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Removes an image identified by the specified image ID from the repository.
        /// </summary>
        /// <remarks>This method attempts to delete the image from the repository and then save the
        /// changes. If either operation fails, the method returns a failure result with the associated error
        /// messages.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var addResult = await imageRepository.DeleteAsync(imageId, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }


        #endregion

        #region Lodging Listing Requests

        /// <summary>
        /// Retrieves a paginated list of lodging listing requests based on the provided page parameters.
        /// </summary>
        /// <param name="pageParameters">The parameters for pagination and filtering.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A paginated result containing lodging listing request DTOs.</returns>
        public async Task<PaginatedResult<LodgingListingRequestDto>> PagedLodgingListReqeusts(LodgingListingRequestParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await accomodationRepo.LodgingListingRequests.ListAsync(false, cancellationToken);
            if (!result.Succeeded) return PaginatedResult<LodgingListingRequestDto>.Failure(result.Messages);

            var response = result.Data.ToList();

            if (!string.IsNullOrEmpty(pageParameters.SearchText))
            {
                response = response.Where(c => c.Name.Contains(pageParameters.SearchText, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            return PaginatedResult<LodgingListingRequestDto>.Success(response.Select(c => new LodgingListingRequestDto(c)).ToList(), response.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Creates a new lodging listing request.
        /// </summary>
        /// <param name="model">The lodging listing request DTO containing the details of the new request.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result containing the created lodging listing request DTO.</returns>
        public async Task<IBaseResult<LodgingListingRequestDto>> CreateLodgingListReqeust(LodgingListingRequestDto model, CancellationToken cancellationToken = default)
        {
            var result = await accomodationRepo.LodgingListingRequests.CreateAsync(model.ToLodgingListingRequest(), cancellationToken);
            if (result.Succeeded)
            {
                return await Result<LodgingListingRequestDto>.SuccessAsync(new LodgingListingRequestDto(result.Data));
            }
            return await Result<LodgingListingRequestDto>.FailAsync(result.Messages);
        }

        #endregion

        #region Cancellation Rules

        /// <summary>
        /// Creates a new cancellation rule.
        /// </summary>
        /// <param name="cancellation">The cancellation rule DTO containing the details of the new rule.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> CreateCancellationRule(CancellationRuleDto cancellation, CancellationToken cancellationToken = default)
        {
            await accomodationRepo.CancellationRules.CreateAsync(cancellation.ToCancellationRule(), cancellationToken);
            var saveResult = await accomodationRepo.CancellationRules.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
                return await Result.SuccessAsync("Cancellation was successfully removed");
            return await Result.FailAsync(saveResult.Messages);
        }

        /// <summary>
        /// Removes a specific cancellation rule by ID.
        /// </summary>
        /// <param name="id">The ID of the cancellation rule to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> RemoveCancellationRule(int id, CancellationToken cancellationToken = default)
        {
            await accomodationRepo.CancellationRules.DeleteAsync(id, cancellationToken);

            var saveResult = await accomodationRepo.CancellationRules.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
                return await Result.SuccessAsync("Cancellation was successfully removed");
            return await Result.FailAsync(saveResult.Messages);
        }

        #endregion

        #region Images

        /// <summary>
        /// Retrieves a collection of featured images for a specific lodging.
        /// </summary>
        /// <param name="lodgingId">The ID of the lodging to retrieve featured images for.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result containing a collection of featured image DTOs.</returns>
        public async Task<IBaseResult<ICollection<FeaturedImageDto>>> FeaturedImages(string lodgingId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<FeaturedImage>(c => !string.IsNullOrEmpty(c.LodgingId) && c.LodgingId == lodgingId);

            var result = await accomodationRepo.FeaturedImages.ListAsync(spec, false, cancellationToken);
            if (result.Succeeded)
                return await Result<ICollection<FeaturedImageDto>>.SuccessAsync(result.Data.Select(c => new FeaturedImageDto(c)).ToList());
            return await Result<ICollection<FeaturedImageDto>>.FailAsync(result.Messages);
        }

        /// <summary>
        /// Sets a specific image as the featured cover image for a lodging.
        /// </summary>
        /// <param name="lodgingId">The ID of the lodging to set the cover image for.</param>
        /// <param name="featuredImageId">The ID of the image to set as the cover image.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> SetFeaturedCoverImage(string lodgingId, string featuredImageId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<FeaturedImage>(c => c.LodgingId == lodgingId && c.ImageType == UploadType.Cover);

            var result = await accomodationRepo.FeaturedImages.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<ICollection<FeaturedImageDto>>.FailAsync(result.Messages);

            result.Data.ImageType = UploadType.Image;

            var oldCoverImageUpdateResult = accomodationRepo.FeaturedImages.Update(result.Data);
            if (!oldCoverImageUpdateResult.Succeeded) return await Result.FailAsync(oldCoverImageUpdateResult.Messages);

            var newImageSpec = new LambdaSpec<FeaturedImage>(c => c.Id == featuredImageId);

            var newCoverImageResult = await accomodationRepo.FeaturedImages.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!newCoverImageResult.Succeeded)
                return await Result<ICollection<FeaturedImageDto>>.FailAsync(newCoverImageResult.Messages);

            newCoverImageResult.Data.ImageType = UploadType.Cover;

            var newCoverImageUpdateResult = accomodationRepo.FeaturedImages.Update(newCoverImageResult.Data);
            if (!newCoverImageUpdateResult.Succeeded) return await Result.FailAsync(newCoverImageUpdateResult.Messages);

            var saveResult = await accomodationRepo.FeaturedImages.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return await Result<ICollection<FeaturedImageDto>>.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Removes the featured cover image for a lodging.
        /// </summary>
        /// <param name="featuredImageId">The ID of the featured image to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> RemoveFeaturedCoverImage(string featuredImageId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<FeaturedImage>(c => c.Id == featuredImageId);

            var result = await accomodationRepo.FeaturedImages.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<ICollection<FeaturedImageDto>>.FailAsync(result.Messages);

            result.Data.ImageType = UploadType.Image;

            var updateResult = accomodationRepo.FeaturedImages.Update(result.Data);
            if (!updateResult.Succeeded)
                return await Result<ICollection<FeaturedImageDto>>.FailAsync(updateResult.Messages);

            var saveResult = await accomodationRepo.FeaturedImages.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return await Result<ICollection<FeaturedImageDto>>.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        ///// <summary>
        ///// Retrieves a collection of gallery images for a specific lodging.
        ///// </summary>
        ///// <param name="lodgingId">The ID of the lodging to retrieve gallery images for.</param>
        ///// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        ///// <returns>A result containing a collection of file base DTOs representing the gallery images.</returns>
        //public async Task<IBaseResult<ICollection<FileBaseDto>>> GalleryImages(string lodgingId, CancellationToken cancellationToken = default)
        //{
        //    var spec = new LambdaSpec<ImageFile<Lodging, string>>(c => !string.IsNullOrEmpty(c.EntityId) && c.EntityId == lodgingId);

        //    var result = await accomodationRepo.LodgingImages.ListAsync(spec, false, cancellationToken);
        //    if (result.Succeeded)
        //        return await Result<ICollection<FileBaseDto>>.SuccessAsync(result.Data.Select(c => FileBaseDto.ToFileBaseDto<Lodging>(c)).ToList());
        //    return await Result<ICollection<FileBaseDto>>.FailAsync(result.Messages);
        //}

        /// <summary>
        /// Adds a new featured image for a lodging.
        /// </summary>
        /// <param name="dto">The featured image DTO containing the details of the new image.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result containing the created featured image DTO.</returns>
        public async Task<IBaseResult<FeaturedImageDto>> AddFeaturedImage(FeaturedImageDto dto, CancellationToken cancellationToken = default)
        {
            var featuredServiceImage = new FeaturedImage(dto.EntityId, null, dto.ImageUrl, UploadType.Image);
            await accomodationRepo.FeaturedImages.CreateAsync(featuredServiceImage, cancellationToken);

            var result = await accomodationRepo.FeaturedImages.SaveAsync(cancellationToken);
            if (result.Succeeded)
            {
                return await Result<FeaturedImageDto>.SuccessAsync(dto);
            }

            return await Result<FeaturedImageDto>.FailAsync(result.Messages);
        }

        /// <summary>
        /// Removes a specific featured lodging image by ID.
        /// </summary>
        /// <param name="featuredImageId">The ID of the featured image to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> RemoveFeaturedLodgingImageAsync(string featuredImageId, CancellationToken cancellationToken = default)
        {
            await accomodationRepo.FeaturedImages.DeleteAsync(featuredImageId);

            var saveResult = await accomodationRepo.FeaturedImages.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
            {
                return await Result<IBaseResult>.SuccessAsync("Featured Image removed successfully");
            }

            return await Result<IBaseResult>.FailAsync(saveResult.Messages);
        }
        
        #endregion

        /// <summary>
        /// Updates the categories associated with a specific lodging.
        /// </summary>
        /// <param name="product">The lodging DTO containing the updated category details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        private async Task<IBaseResult> UpdateCategoriesAsync(LodgingDto product, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(product.ProductId))
                return await Result.FailAsync("Product Id cannot be empty or null");

            var categories = await categoryService.EntityCategoriesAsync(product.ProductId);

            foreach (var category in categories.Data)
            {
                if (!product.Categories.Any(c => c.CategoryId == category.CategoryId))
                {
                    await categoryService.RemoveEntityCategoryAsync(category.CategoryId, product.ProductId);
                }
            }

            foreach (var category in product.Categories)
            {
                if (!categories.Data.Any(c => c.CategoryId == category.CategoryId))
                {
                    await categoryService.CreateEntityCategoryAsync(category.CategoryId, product.ProductId);
                }
            }

            return await Result.SuccessAsync("Categories successfully updated");
        }

        /// <summary>
        /// Updates the amenities associated with a specific lodging.
        /// </summary>
        /// <param name="product">The lodging DTO containing the updated amenity details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        private async Task<IBaseResult> UpdateAmenitiesAsync(LodgingDto product, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Lodging>(c => c.Id == product.ProductId);
            spec.AddInclude(c => c.Include(g => g.Amneties));

            var result = await accomodationRepo.Lodgings.ListAsync(spec, true, cancellationToken);
            var amenities = result.Data;

            foreach (var amenity in amenities)
            {
                if (!product.Amenities.Any(c => c.AmenityId == amenity.Id.ToString()))
                {
                    var amenitySpec = new LambdaSpec<AmenityItem<Lodging, string>>(c => c.AmenityId.ToString() == amenity.Id && c.LodgingId == product.ProductId);
                    var amentityToRemove = await accomodationRepo.LodgingAmenities.FirstOrDefaultAsync(amenitySpec, false, cancellationToken);

                    if (amentityToRemove != null)
                        accomodationRepo.LodgingAmenities.Delete(amentityToRemove.Data);
                }
            }

            foreach (var amenity in product.Amenities)
            {
                if (!amenities.Any(c => c.Id.ToString() == amenity.AmenityId))
                {
                    await accomodationRepo.LodgingAmenities.CreateAsync(new AmenityItem<Lodging, string>
                        { AmenityId = Convert.ToInt32(amenity.AmenityId), LodgingId = product.ProductId }, cancellationToken);
                }
            }

            return await Result.SuccessAsync($"Amenities successfully updated");
        }

        /// <summary>
        /// Calculates the price for a specific lodging.
        /// </summary>
        /// <param name="lodging">The lodging entity to calculate the price for.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result containing the calculated pricing DTO.</returns>
        public async Task<IBaseResult<PricingDto>> CalculatePriceAsync(Lodging lodging, CancellationToken cancellationToken = default)
        {
            var dateTime = DateTime.Now.Date.ToOADate();
            var price = 0d;

            price = lodging.Rate;

            return await Result<PricingDto>.SuccessAsync(new PricingDto()
            {
                SellingPrice = price
            });
        }
    }
}
