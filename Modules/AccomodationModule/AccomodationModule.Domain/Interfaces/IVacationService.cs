using AccomodationModule.Domain.Arguments.Requests;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Implements the vacation service, providing methods to manage vacation data.
    /// </summary>
    public interface IVacationService
    {
        /// <summary>
        /// Retrieves a paginated list of vacations based on the specified page parameters.
        /// </summary>
        /// <remarks>Use this method to retrieve vacation data in a paginated format, which is useful for
        /// scenarios involving large datasets. Ensure that the <paramref name="pageParameters"/> are correctly
        /// configured to avoid unexpected results.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination settings, such as page number and page size.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the requested page of vacation data. The result
        /// includes metadata such as total item count and current page information.</returns>
        Task<PaginatedResult<VacationDto>> PagedAsync(VacationPageParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all vacations based on the provided parameters.
        /// </summary>
        /// <param name="pageParameters">Parameters for filtering and sorting.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result containing a collection of vacation data.</returns>
        Task<IBaseResult<IEnumerable<VacationDto>>> AllVacationsAsync(VacationPageParameters pageParameters, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Retrieves a specific vacation by its ID.
        /// </summary>
        /// <param name="vacationId">The ID of the vacation to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result containing the vacation data.</returns>
        Task<IBaseResult<VacationDto>> VacationAsync(string vacationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific vacation by its name.
        /// </summary>
        /// <param name="vacationName">The name of the vacation to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result containing the vacation data.</returns>
        Task<IBaseResult<VacationDto>> VacationFromNameAsync(string vacationName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves vacation details based on the provided slug.
        /// </summary>
        /// <remarks>This method is asynchronous and should be awaited. The caller is responsible for
        /// handling any errors indicated in the returned <see cref="IBaseResult{T}"/> object.</remarks>
        /// <param name="slug">The unique identifier or slug representing the vacation. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the vacation details as a <see cref="VacationDto"/>. If the slug is invalid or not found, the
        /// result may indicate an error.</returns>
        Task<IBaseResult<VacationDto>> VacationFromSlugAsync(string slug, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a summary of a specific vacation host by its ID.
        /// </summary>
        /// <param name="vacationHostId">The identity of the host that contains the summaries</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result containing the vacation data.</returns>
        Task<IBaseResult<VacationDto>> VacationSummaryAsync(string vacationHostId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a summary of a specific vacation by its name.
        /// </summary>
        /// <param name="vacationName">The name of the vacation to retrieve the summary for.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result containing the vacation summary data.</returns>
        Task<IBaseResult<VacationDto>> VacationSummaryFromNameAsync(string vacationName, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Duplicates an existing vacation.
        /// </summary>
        /// <param name="vacationId">The ID of the vacation to duplicate.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult> DuplicateAsync(string vacationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new vacation entry asynchronously.
        /// </summary>
        /// <remarks>This method performs the creation operation asynchronously. Ensure that the provided
        /// <paramref name="uploadPath"/>  is a valid and accessible file path. The operation may be canceled by passing
        /// a cancellation token.</remarks>
        /// <param name="dto">The data transfer object containing the details of the vacation to be created.</param>
        /// <param name="uploadPath">The file path where any associated uploads will be stored. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the creation operation.</returns>
        Task<IBaseResult> CreateAsync(VacationDto dto, string uploadPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing vacation record with the provided data and uploads associated files if specified.
        /// </summary>
        /// <remarks>This method performs an update operation on an existing vacation record. If the
        /// <paramref name="uploadPath"/>  is provided, associated files will be uploaded to the specified location.
        /// Ensure that the provided  <paramref name="dto"/> contains valid data and that the <paramref
        /// name="uploadPath"/> is accessible.</remarks>
        /// <param name="dto">The data transfer object containing the updated vacation details.</param>
        /// <param name="uploadPath">The file path where associated files should be uploaded. This path must be valid and accessible.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        Task<IBaseResult> UpdateAsync(VacationDto dto, string uploadPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a vacation.
        /// </summary>
        /// <param name="id">The ID of the vacation to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult> RemoveAsync(string id, CancellationToken cancellationToken = default);

        #region Vacation Inclusion Display Info

        /// <summary>
        /// Creates a new vacation inclusion display section based on the provided information.
        /// </summary>
        /// <remarks>Use this method to create and persist a new vacation inclusion display section.
        /// Ensure that the  <paramref name="dto"/> parameter contains valid and complete information before calling
        /// this method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the vacation inclusion display section to create.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> CreateVacationInclusionDisplaySectionAsync(VacationInclusionDisplayTypeInformationDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the vacation inclusion display section with the specified information.
        /// </summary>
        /// <remarks>This method performs an asynchronous update operation to modify the vacation
        /// inclusion display section based on the provided information. Ensure that the <paramref name="dto"/>
        /// parameter contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the updated vacation inclusion display type information.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        Task<IBaseResult> UpdateVacationInclusionDisplaySectionAsync(VacationInclusionDisplayTypeInformationDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the display order of vacation inclusion display sections based on the provided update request.
        /// </summary>
        /// <param name="dto">The request object containing the updated display order information for the vacation inclusion display
        /// sections.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the update operation.</returns>
        Task<IBaseResult> UpdateVacationInclusionDisplaySectionDisplayOrderAsync(VacationInclusionDisplayTypeInformationGroupUpdateRequest dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the vacation inclusion display section associated with the specified identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to remove the specified vacation
        /// inclusion display section. Ensure that the provided identifier corresponds to an existing section. The
        /// operation may fail if the identifier does not exist or if there are constraints preventing its
        /// removal.</remarks>
        /// <param name="vacationInclusionDisplayTypeInformationId">The unique identifier of the vacation inclusion display section to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> RemoveVacationInclusionDisplaySectionAsync(string vacationInclusionDisplayTypeInformationId, CancellationToken cancellationToken = default);

        #endregion

        #region Images

        /// <summary>
        /// Adds an image to a vacation entity.
        /// </summary>
        /// <remarks>This method allows adding an image to a vacation entity, which can be used to enhance
        /// the entity's details with visual content.</remarks>
        /// <param name="request">The request containing the image and associated metadata to be added to the vacation entity. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> AddVacationImage(AddEntityImageRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a vacation image identified by the specified image ID.
        /// </summary>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveVacationImage(string imageId, CancellationToken cancellationToken = default);

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
        Task<IBaseResult> AddVideo(AddEntityVideoRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a video identified by the specified video ID.
        /// </summary>
        /// <remarks>This method attempts to delete the video from the repository and save the changes. If
        /// either operation fails, the method returns a failure result with the associated error messages.</remarks>
        /// <param name="videoId">The unique identifier of the video to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default);

        #endregion

        #region Extensions

        /// <summary>
        /// Retrieves all vacation extensions asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{VacationDto}"/> representing the collection of vacation extensions.</returns>
        Task<IBaseResult<IEnumerable<VacationDto>>> AllExtensionsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves all vacation extensions associated with a specified vacation.
        /// </summary>
        /// <param name="vacationId">The unique identifier of the vacation for which extensions are to be retrieved. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IEnumerable{VacationDto}"/> of vacation extensions.</returns>
        Task<IBaseResult<IEnumerable<VacationDto>>> VacationExtensionsAsync(string vacationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously creates a vacation extension based on the specified request.
        /// </summary>
        /// <param name="request">The request containing details for the vacation extension to be created. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> CreateExtensionAsync(CreateVacationExtensionForVacationRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously removes a vacation extension identified by the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the vacation extension to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation, along with any relevant messages.</returns>
        Task<IBaseResult> RemoveExtensionAsync(string id, CancellationToken cancellationToken = default);

        #endregion

        #region Reviews

        /// <summary>
        /// Asynchronously creates a new vacation review based on the provided data transfer object.
        /// </summary>
        /// <param name="dto">The data transfer object containing the details of the vacation review to be created. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> CreateVacationReviewAsync(VacationReviewDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the vacation review with the specified details.
        /// </summary>
        /// <param name="dto">The data transfer object containing the updated vacation review details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the update operation.</returns>
        Task<IBaseResult> UpdateVacationReviewAsync(VacationReviewDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a vacation review identified by the specified ID.
        /// </summary>
        /// <remarks>Use this method to delete a vacation review from the system. Ensure the <paramref
        /// name="id"/> corresponds  to an existing review. If the review does not exist, the operation may fail
        /// depending on the implementation.</remarks>
        /// <param name="id">The unique identifier of the vacation review to remove. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> RemoveVacationReviewAsync(string id, CancellationToken cancellationToken = default);

        #endregion
    }
}
