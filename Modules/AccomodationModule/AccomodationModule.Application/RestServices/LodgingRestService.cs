using AccomodationModule.Domain.Arguments.Response;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides a set of methods for managing lodging-related operations, including creating, updating, retrieving, and
    /// deleting lodgings, as well as handling associated resources such as images, videos, and cancellation rules.
    /// </summary>
    /// <remarks>This service acts as a bridge between the application and the underlying HTTP provider,
    /// enabling asynchronous operations for lodging management.  It supports operations such as creating and updating
    /// lodging requests, retrieving paginated results, managing featured images and videos, and interacting with
    /// external systems like NightsBridge.</remarks>
    /// <param name="provider"></param>
    public class LodgingRestService(IBaseHttpProvider provider) : ILodgingService
    {
        /// <summary>
        /// Creates or updates a lodging listing request.
        /// </summary>
        /// <remarks>This method sends the provided lodging listing request data to the underlying
        /// provider for creation or update. Ensure that the <paramref name="model"/> parameter contains valid data
        /// before calling this method.</remarks>
        /// <param name="model">The lodging listing request data to be created or updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// object wrapping the created or updated <see cref="LodgingListingRequestDto"/>.</returns>
        public async Task<IBaseResult<LodgingListingRequestDto>> CreateLodgingListReqeust(LodgingListingRequestDto model, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<LodgingListingRequestDto, LodgingListingRequestDto>("lodgings/lodginglistingreqeust", model);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of lodging listing requests based on the specified parameters.
        /// </summary>
        /// <remarks>This method asynchronously fetches lodging listing requests from the data source
        /// using the provided pagination and filtering parameters.</remarks>
        /// <param name="pageParameters">The parameters used to define the pagination and filtering criteria for the lodging listing requests.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of <see cref="LodgingListingRequestDto"/>
        /// objects.</returns>
        public async Task<PaginatedResult<LodgingListingRequestDto>> PagedLodgingListReqeusts(LodgingListingRequestParameters pageParameters, CancellationToken cancellationToken)
        {
            var result = await provider.GetPagedAsync<LodgingListingRequestDto, LodgingListingRequestParameters>("lodgings/lodginglistingreqeust", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves the total count of lodgings asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to the underlying provider to fetch the count of
        /// lodgings. The operation can be canceled by passing a cancellation token.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping a <see cref="LodgingCountResponse"/> that includes the total count of lodgings.</returns>
        public async Task<IBaseResult<LodgingCountResponse>> LodgingCountAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<LodgingCountResponse>("lodgings/count");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of lodgings based on the specified paging parameters.
        /// </summary>
        /// <remarks>This method queries the data source for lodgings and returns the results in a
        /// paginated format.  Use the <paramref name="pageParameters"/> to specify the page size, page number, and any
        /// filtering criteria.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering options for the lodgings.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="LodgingDto"/> objects that match
        /// the specified paging parameters.</returns>
        public async Task<PaginatedResult<LodgingDto>> PagedLodgingsAsync(LodgingParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<LodgingDto, LodgingParameters>("lodgings", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a list of all lodgings, optionally filtered by category.
        /// </summary>
        /// <remarks>If no lodgings are available, the returned list will be empty. Ensure to handle
        /// cancellation appropriately when passing a <paramref name="cancellationToken"/>.</remarks>
        /// <param name="categoryId">The optional category identifier to filter the lodgings. If <see langword="null"/>, all lodgings are
        /// retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation is canceled if the token is triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a list of <see cref="LodgingDto"/> objects representing the lodgings.</returns>
        public async Task<IBaseResult<List<LodgingDto>>> AllLodgings(string? categoryId = null, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<List<LodgingDto>>("lodgings/all");
            return result;
        }

        /// <summary>
        /// Retrieves lodging details for the specified product.
        /// </summary>
        /// <remarks>This method uses the underlying provider to fetch lodging details. Ensure that the
        /// <paramref name="productId"/> corresponds to a valid product in the system. The operation may be canceled by
        /// passing a cancellation token.</remarks>
        /// <param name="productId">The unique identifier of the product for which lodging details are requested. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with lodging details as a <see cref="LodgingDto"/>. If the product is not found, the result may
        /// indicate an error.</returns>
        public async Task<IBaseResult<LodgingDto>> LodgingAsync(string productId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<LodgingDto>($"lodgings/details/{productId}");
            return result;
        }

        /// <summary>
        /// Retrieves lodging details based on a unique service identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch lodging details using the
        /// provided unique identifier. Ensure that the <paramref name="bbid"/> parameter is valid and not null before
        /// calling this method.</remarks>
        /// <param name="bbid">The unique identifier of the lodging service.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the lodging details as a <see cref="LodgingDto"/>. If no lodging is found, the result may
        /// indicate an empty or null state depending on the implementation.</returns>
        public async Task<IBaseResult<LodgingDto>> ProductByUniqueServiceIdAsync(string bbid, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<LodgingDto>($"lodgings/details/byUniqueId/{bbid}");
            return result;
        }

        /// <summary>
        /// Creates a new lodging.
        /// </summary>
        /// <param name="nbProduct">The lodging DTO containing the details of the new lodging.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result containing the created lodging DTO.</returns>
        public async Task<IBaseResult<LodgingDto>> CreateLodgingAsync(LodgingDto nbProduct, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<LodgingDto, LodgingDto>($"lodgings/createLodging", nbProduct);
            return result;
        }

        /// <summary>
        /// Updates an existing lodging.
        /// </summary>
        /// <param name="lodging">The lodging DTO containing the updated details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> UpdateLodgingAsync(LodgingDto lodging, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<LodgingDto, LodgingDto>($"lodgings/createLodging", lodging);
            return result;
        }

        /// <summary>
        /// Removes a lodging resource identified by the specified ID.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the lodging
        /// resource. Ensure the specified ID corresponds to an existing resource.</remarks>
        /// <param name="id">The unique identifier of the lodging resource to remove. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveLodgingAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"lodgings", id);
            return result;
        }

        /// <summary>
        /// Adds an image to an entity based on the specified request.
        /// </summary>
        /// <remarks>This method sends the image data to the "lodgings/addImage" endpoint. Ensure that the
        /// request object is properly populated before calling this method.</remarks>
        /// <param name="request">The request containing the details of the image to be added, including the entity identifier and image data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("lodgings/addImage", request);
            return result;
        }

        /// <summary>
        /// Removes an image with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to delete the image identified by <paramref
        /// name="imageId"/>.  Ensure that the provided identifier corresponds to an existing image.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("lodgings/deleteImage", imageId);
            return result;
        }

        /// <summary>
        /// Adds a video to the specified entity.
        /// </summary>
        /// <remarks>This method sends a request to add a video to an entity. Ensure that the <paramref
        /// name="request"/>  contains valid data, as required by the API endpoint. The operation is asynchronous and
        /// can be canceled  using the provided <paramref name="cancellationToken"/>.</remarks>
        /// <param name="request">The request containing the details of the video to be added, including the entity ID and video metadata.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddVideo(AddEntityVideoRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("lodgings/addVideo", request);
            return result;
        }

        /// <summary>
        /// Removes a video with the specified identifier.
        /// </summary>
        /// <param name="videoId">The unique identifier of the video to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("lodgings/deleteVideo", videoId);
            return result;
        }

        /// <summary>
        /// Creates a new cancellation rule by sending the specified data to the provider.
        /// </summary>
        /// <remarks>This method sends the cancellation rule data to the provider's endpoint
        /// "lodgings/createCancellationRule". Ensure that the <paramref name="cancellation"/> parameter contains valid
        /// data before calling this method.</remarks>
        /// <param name="cancellation">The cancellation rule data to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateCancellationRule(CancellationRuleDto cancellation, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync("lodgings/createCancellationRule", cancellation);
            return result;
        }

        /// <summary>
        /// Removes a cancellation rule with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to delete the cancellation rule identified by <paramref
        /// name="id"/>.  Ensure the specified rule exists before calling this method.</remarks>
        /// <param name="id">The unique identifier of the cancellation rule to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveCancellationRule(int id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"lodgings/deleteCancellationRule/{id}", "");
            return result;
        }

        /// <summary>
        /// Creates a new lodging entity in the NightsBridge system.
        /// </summary>
        /// <remarks>This method sends a request to the NightsBridge system to create a new lodging
        /// entity. If the operation is successful, the resulting lodging details are returned.</remarks>
        /// <param name="bbid">The unique identifier of the NightsBridge property. This value can be <see langword="null"/> if the property
        /// does not yet have an associated identifier.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of type <see cref="LodgingDto"/> representing the created lodging entity.</returns>
        public async Task<IBaseResult<LodgingDto>> CreateNewLodgingNighsBridgeAsync(string? bbid, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<LodgingDto, string>("lodgings/createNightsBridgeLodging", bbid);
            return result;
        }

        /// <summary>
        /// Unlinks a product from the NightsBridge system.
        /// </summary>
        /// <remarks>This method sends a request to the NightsBridge system to unlink the specified
        /// product. Ensure that the  <paramref name="bbid"/> corresponds to a valid product identifier.</remarks>
        /// <param name="bbid">The unique identifier of the product to be unlinked. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the unlink operation.</returns>
        public async Task<IBaseResult> UnlinkProductFromNighsBridgeAsync(string bbid, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("lodgings/unLinkNightsBridgeLodging", bbid);
            return result;
        }

        /// <summary>
        /// Retrieves the collection of featured images for a specified lodging.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to retrieve the featured images for
        /// the specified lodging. Ensure that the <paramref name="lodgingId"/> is valid and corresponds to an existing
        /// lodging.</remarks>
        /// <param name="lodgingId">The unique identifier of the lodging for which to retrieve featured images.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing a collection of <see cref="FeaturedImageDto"/> objects representing the featured images for the
        /// lodging.</returns>
        public async Task<IBaseResult<ICollection<FeaturedImageDto>>> FeaturedImages(string lodgingId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<ICollection<FeaturedImageDto>>($"lodgings/featuredImages/{lodgingId}");
            return result;
        }

        /// <summary>
        /// Sets the featured cover image for a specified lodging.
        /// </summary>
        /// <remarks>This method sends a request to update the featured cover image for the specified
        /// lodging. Ensure that the  provided identifiers are valid and correspond to existing resources.</remarks>
        /// <param name="lodgingId">The unique identifier of the lodging for which the featured image is being set.</param>
        /// <param name="featuredImageId">The unique identifier of the image to be set as the featured cover image.</param>
        /// <param name="cancellationToken">An optional token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> SetFeaturedCoverImage(string lodgingId, string featuredImageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<string>($"lodgings/unLinkNightsBridgeLodging/{lodgingId}/{featuredImageId}", "");
            return result;
        }

        /// <summary>
        /// Removes the featured cover image associated with the specified image ID.
        /// </summary>
        /// <remarks>This method sends a request to remove the featured cover image identified by
        /// <paramref name="featuredImageId"/>. Ensure that the provided ID corresponds to an existing featured
        /// image.</remarks>
        /// <param name="featuredImageId">The unique identifier of the featured image to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveFeaturedCoverImage(string featuredImageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<string>($"lodgings/removeFeaturedCoverImage/{featuredImageId}", "");
            return result;
        }

        /// <summary>
        /// Adds a featured image for a lodging.
        /// </summary>
        /// <remarks>This method sends a PUT request to the "lodgings/addFeaturedImage" endpoint to add
        /// the specified featured image.</remarks>
        /// <param name="dto">The data transfer object containing the details of the featured image to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the added <see cref="FeaturedImageDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<FeaturedImageDto>> AddFeaturedImage(FeaturedImageDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<FeaturedImageDto, FeaturedImageDto>($"lodgings/addFeaturedImage", dto);
            return result;
        }

        /// <summary>
        /// Removes a featured lodging image identified by the specified ID.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified featured lodging image. Ensure
        /// the <paramref name="featuredImageId"/> corresponds to an existing image before calling this
        /// method.</remarks>
        /// <param name="featuredImageId">The unique identifier of the featured lodging image to remove. This value cannot be <see langword="null"/>
        /// or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveFeaturedLodgingImageAsync(string featuredImageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("lodgings/featuredImages", featuredImageId);
            return result;
        }
    }
}
