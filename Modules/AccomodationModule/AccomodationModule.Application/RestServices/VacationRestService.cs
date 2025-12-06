using AccomodationModule.Domain.Arguments.Requests;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides a set of methods for managing vacation-related data and operations, including retrieving, creating,
    /// updating, and deleting vacations, as well as managing associated media, extensions, and reviews.
    /// </summary>
    /// <remarks>This service acts as a client for interacting with vacation-related endpoints, enabling
    /// operations such as fetching paginated vacation data, managing vacation media (images and videos), handling
    /// vacation extensions, and working with vacation reviews.  It is designed to be used in applications that require
    /// integration with a vacation management system.</remarks>
    /// <param name="provider"></param>
    public class VacationRestService(IBaseHttpProvider provider) : IVacationService
    {
        /// <summary>
        /// Retrieves a paginated list of vacation records based on the specified page parameters.
        /// </summary>
        /// <remarks>This method fetches vacation data from the underlying data provider using the
        /// specified pagination settings. The result includes the requested page of data along with metadata such as
        /// total item count and page information.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination settings, such as page number and page size.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of vacation records.</returns>
        public async Task<PaginatedResult<VacationDto>> PagedAsync(VacationPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<VacationDto, VacationPageParameters>("vacations", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of all vacations.
        /// </summary>
        /// <remarks>The method fetches vacation data from the underlying provider and applies the
        /// specified pagination parameters. Ensure that <paramref name="pageParameters"/> is properly configured to
        /// avoid unexpected results.</remarks>
        /// <param name="pageParameters">The pagination parameters, including page number and page size, used to control the subset of results
        /// returned.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="VacationDto"/> objects.</returns>
        public async Task<IBaseResult<IEnumerable<VacationDto>>> AllVacationsAsync(VacationPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<VacationDto>>($"vacations/all/{pageParameters.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Retrieves vacation details for the specified vacation ID.
        /// </summary>
        /// <param name="vacationId">The unique identifier of the vacation to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the vacation details as a <see cref="VacationDto"/>. If the vacation ID is invalid or not found,
        /// the result may indicate an error.</returns>
        public async Task<IBaseResult<VacationDto>> VacationAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<VacationDto>($"vacations/{vacationId}");
            return result;
        }

        /// <summary>
        /// Retrieves vacation details based on the specified vacation name.
        /// </summary>
        /// <remarks>This method uses the underlying provider to fetch vacation details. Ensure that the
        /// specified vacation name matches an existing vacation in the system. The operation may fail if the name is
        /// invalid or the provider is unavailable.</remarks>
        /// <param name="vacationName">The name of the vacation to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{VacationDto}"/> object with the details of the vacation. If the vacation is not found, the
        /// result may indicate an error or an empty value.</returns>
        public async Task<IBaseResult<VacationDto>> VacationFromNameAsync(string vacationName, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<VacationDto>($"vacations/fromName/{vacationName}");
            return result;
        }

        /// <summary>
        /// Retrieves vacation details based on the provided slug.
        /// </summary>
        /// <param name="slug">The unique identifier for the vacation, typically a URL-friendly string.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the vacation details as a <see cref="VacationDto"/>. If the slug is invalid or not found, the
        /// result may indicate an error.</returns>
        public async Task<IBaseResult<VacationDto>> VacationFromSlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<VacationDto>($"vacations/fromSlug/{slug}");
            return result;
        }

        /// <summary>
        /// Retrieves a summary of vacation details for the specified vacation ID.
        /// </summary>
        /// <remarks>This method sends a request to retrieve vacation summary details from the underlying
        /// data provider. Ensure that the <paramref name="vacationId"/> corresponds to a valid vacation
        /// entry.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation for which the summary is requested. Cannot be <see langword="null"/>
        /// or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="VacationDto"/> containing the vacation summary details.</returns>
        public async Task<IBaseResult<VacationDto>> VacationSummaryAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<VacationDto>($"vacations/summaries/{vacationId}");
            return result;
        }

        /// <summary>
        /// Retrieves a vacation summary based on the specified vacation name.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch the vacation summary from the
        /// provider. Ensure that the <paramref name="vacationName"/> is valid and corresponds to an existing
        /// vacation.</remarks>
        /// <param name="vacationName">The name of the vacation for which the summary is to be retrieved. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that holds the vacation summary as a <see cref="VacationDto"/>. If the vacation name does not exist,
        /// the result may indicate an error or an empty response.</returns>
        public async Task<IBaseResult<VacationDto>> VacationSummaryFromNameAsync(string vacationName, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<VacationDto>($"vacations/summaries/fromName/{vacationName}");
            return result;
        }

        /// <summary>
        /// Retrieves a vacation summary based on the specified URL.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the vacation summary from the specified URL.
        /// Ensure that the URL provided is valid and accessible. The operation may be canceled by passing a
        /// cancellation token.</remarks>
        /// <param name="vacationUrl">The URL of the vacation to retrieve the summary for. This must be a valid, non-empty URL string.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping a <see cref="VacationDto"/> that represents the vacation summary.</returns>
        public async Task<IBaseResult<VacationDto>> VacationSummaryFromUrlAsync(string vacationUrl, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<VacationDto>($"vacations/summaries/fromUrl/{vacationUrl}");
            return result;
        }

        /// <summary>
        /// Duplicates a vacation record asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to duplicate the specified vacation record. Ensure that
        /// the provided <paramref name="vacationId"/> corresponds to an existing vacation.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation to duplicate. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// representing the duplicated vacation record.</returns>
        public async Task<IBaseResult> DuplicateAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<VacationDto>($"vacations/duplicate/{vacationId}");
            return result;
        }

        /// <summary>
        /// Creates a new vacation entry asynchronously.
        /// </summary>
        /// <remarks>The method sends the vacation data to the underlying provider for processing. Ensure
        /// that the <paramref name="dto"/>  contains valid data before calling this method. The <paramref
        /// name="uploadPath"/> parameter is not currently used  but may be required in future
        /// implementations.</remarks>
        /// <param name="dto">The data transfer object containing the details of the vacation to be created.</param>
        /// <param name="uploadPath">The file path where any associated uploads are stored. This parameter is currently unused but reserved for
        /// future functionality.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the creation operation.</returns>
        public async Task<IBaseResult> CreateAsync(VacationDto dto, string uploadPath, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"vacations", dto);
            return result;
        }

        /// <summary>
        /// Updates vacation information asynchronously.
        /// </summary>
        /// <remarks>This method sends the vacation data to the server for updating. Ensure that the
        /// <paramref name="dto"/> parameter contains valid data before calling this method. If the operation is
        /// canceled via the <paramref name="cancellationToken"/>, the returned task will be in a canceled
        /// state.</remarks>
        /// <param name="dto">The data transfer object containing the vacation details to be updated.</param>
        /// <param name="uploadPath">The file path for any associated uploads. This parameter is optional and may be null or empty if no uploads
        /// are required.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(VacationDto dto, string uploadPath, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"vacations", dto);
            return result;
        }

        /// <summary>
        /// Removes a resource with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the resource.
        /// Ensure the specified <paramref name="id"/> corresponds to an existing resource.</remarks>
        /// <param name="id">The unique identifier of the resource to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"vacations", id);
            return result;
        }

        /// <summary>
        /// Adds an image to a vacation entity.
        /// </summary>
        /// <remarks>This method sends the image data and associated vacation entity details to the server
        /// for processing.  Ensure that the <paramref name="request"/> parameter contains valid data before calling
        /// this method.</remarks>
        /// <param name="request">The request containing the image data and associated vacation entity details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddVacationImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"vacations/addImage", request);
            return result;
        }

        /// <summary>
        /// Removes a vacation image identified by the specified image ID.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified image from the vacation image
        /// repository.  Ensure that the <paramref name="imageId"/> corresponds to an existing image.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveVacationImage(string imageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"vacations/deleteImage", imageId);
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
            var result = await provider.PostAsync("vacations/addVideo", request);
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
            var result = await provider.DeleteAsync("vacations/deleteVideo", videoId);
            return result;
        }

        /// <summary>
        /// Retrieves all vacation extensions asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to the underlying provider to retrieve vacation extension
        /// data. The caller can use the <paramref name="cancellationToken"/> to cancel the operation if
        /// needed.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{T}"/> containing <see cref="VacationDto"/> objects representing the vacation
        /// extensions.</returns>
        public async Task<IBaseResult<IEnumerable<VacationDto>>> AllExtensionsAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<VacationDto>>($"vacations/extensions");
            return result;
        }

        /// <summary>
        /// Retrieves a collection of vacation extensions associated with the specified vacation ID.
        /// </summary>
        /// <remarks>This method sends a request to retrieve vacation extensions for the specified
        /// vacation ID.  The returned collection may be empty if no extensions are found.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation for which extensions are to be retrieved.  This parameter cannot be
        /// null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// object that wraps an enumerable collection of <see cref="VacationDto"/> objects representing the vacation
        /// extensions.</returns>
        public async Task<IBaseResult<IEnumerable<VacationDto>>> VacationExtensionsAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<VacationDto>>($"vacations/vacationextensions/{vacationId}");
            return result;
        }

        /// <summary>
        /// Creates a vacation extension asynchronously based on the provided request.
        /// </summary>
        /// <param name="request">The request object containing the details of the vacation extension to be created.</param>
        /// <param name="cancellationToken">An optional token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// representing the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateExtensionAsync(CreateVacationExtensionForVacationRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"vacations/vacationextensions", request);
            return result;
        }

        /// <summary>
        /// Removes a vacation extension asynchronously.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// vacation extension. Ensure the <paramref name="id"/> corresponds to a valid extension before calling this
        /// method.</remarks>
        /// <param name="id">The unique identifier of the vacation extension to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveExtensionAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"vacations/vacationextensions", id);
            return result;
        }

        /// <summary>
        /// Updates the vacation inclusion display section with the specified information.
        /// </summary>
        /// <remarks>This method sends an HTTP PUT request to update the vacation inclusion display
        /// section. Ensure that the <paramref name="dto"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="dto">An object containing the vacation inclusion display type information to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> CreateVacationInclusionDisplaySectionAsync(VacationInclusionDisplayTypeInformationDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync("vacations/vacationInclusionDisplayInfoSection", dto);
            return result;
        }

        /// <summary>
        /// Updates the vacation inclusion display section with the specified information.
        /// </summary>
        /// <remarks>This method sends the provided vacation inclusion display type information to the
        /// server for updating the corresponding display section. Ensure that the <paramref name="dto"/> contains valid
        /// data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the vacation inclusion display type information to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateVacationInclusionDisplaySectionAsync(VacationInclusionDisplayTypeInformationDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("vacations/vacationInclusionDisplayInfoSection", dto);
            return result;
        }

        /// <summary>
        /// Updates the display order of vacation inclusion display sections asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated display order information to the server for processing.
        /// Ensure that the <paramref name="dto"/> parameter contains valid data before calling this method.</remarks>
        /// <param name="dto">The request object containing the updated display order information for the vacation inclusion display
        /// sections.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateVacationInclusionDisplaySectionDisplayOrderAsync(VacationInclusionDisplayTypeInformationGroupUpdateRequest dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("vacations/vacationInclusionDisplayInfoSection/updateDisplayOrder", dto);
            return result;
        }

        /// <summary>
        /// Removes a vacation inclusion display section based on the specified identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to delete a vacation inclusion display
        /// section. Ensure that the provided identifier corresponds to an existing section. The operation may fail if
        /// the section does not exist.</remarks>
        /// <param name="vacationInclusionDisplayTypeInformationId">The unique identifier of the vacation inclusion display section to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveVacationInclusionDisplaySectionAsync(string vacationInclusionDisplayTypeInformationId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("vacations/vacationInclusionDisplayInfoSection", vacationInclusionDisplayTypeInformationId);
            return result;
        }

        /// <summary>
        /// Creates or updates a vacation review asynchronously.
        /// </summary>
        /// <remarks>This method sends the vacation review data to the "vacations/review" endpoint. Ensure
        /// that the <paramref name="dto"/>  parameter contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the vacation review to be created or updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateVacationReviewAsync(VacationReviewDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync("vacations/review", dto);
            return result;
        }

        /// <summary>
        /// Updates the vacation review by sending the provided data to the server.
        /// </summary>
        /// <remarks>This method sends the vacation review data to the server endpoint "vacations/review"
        /// using a POST request. Ensure that the <paramref name="dto"/> parameter contains valid data before calling
        /// this method.</remarks>
        /// <param name="dto">The data transfer object containing the vacation review details to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateVacationReviewAsync(VacationReviewDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("vacations/review", dto);
            return result;
        }

        /// <summary>
        /// Removes a vacation review with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to delete the vacation review identified by <paramref
        /// name="id"/>.  Ensure the identifier is valid and corresponds to an existing review.</remarks>
        /// <param name="id">The unique identifier of the vacation review to remove. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveVacationReviewAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("vacations/review", id);
            return result;
        }

        
    }
}
