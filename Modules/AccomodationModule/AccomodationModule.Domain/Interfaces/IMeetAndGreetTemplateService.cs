using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces;

/// <summary>
/// Defines the contract for managing Meet and Greet templates, including operations to retrieve, create, update, and
/// delete templates.
/// </summary>
/// <remarks>This service provides methods to interact with Meet and Greet templates, which are represented as
/// DTOs for external use. The service ensures that templates are retrieved, modified, or deleted in a consistent and
/// reliable manner, handling infrastructure errors transparently.</remarks>
public interface IMeetAndGreetTemplateService
{
    /// <summary>
    /// Retrieves every template in the system (eager-loading its associated
    /// <see cref="Contact"/>), maps them to DTOs, and returns them in a success
    /// envelope or returns infrastructure errors unchanged.
    /// </summary>
    Task<IBaseResult<IEnumerable<MeetAndGreetTemplateDto>>> AllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a single template by primary key.
    /// </summary>
    /// <param name="id">Unique identifier of the template.</param>
    /// <returns>
    /// <list type="bullet">
    ///   <item><description>
    ///     <b>Success</b>: <see cref="MeetAndGreetTemplateDto"/> wrapped in
    ///     <c>SuccessAsync</c>.
    ///   </description></item>
    ///   <item><description>
    ///     <b>Failure</b>: Error messages if the entity isn’t found or repository
    ///     fails.
    ///   </description></item>
    /// </list>
    /// </returns>
    Task<IBaseResult<MeetAndGreetTemplateDto>> ByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new <see cref="MeetAndGreetTemplate"/> from its DTO
    /// representation and persists it.
    /// </summary>
    /// <remarks>
    /// The DTO itself is returned on success because callers often need
    /// the user-supplied data, not the internal entity.
    /// </remarks>
    Task<IBaseResult<MeetAndGreetTemplateDto>> AddAsync(MeetAndGreetTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Modifies an existing template in-place. Only user-mutable fields
    /// are patched; auditing fields remain untouched.
    /// </summary>
    /// <param name="id">Identifier of the entity to update.</param>
    /// <param name="dto">New values coming from the client/UI layer.</param>
    Task<IBaseResult<MeetAndGreetTemplateDto>> EditAsync(MeetAndGreetTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a template permanently from the data store.
    /// </summary>
    /// <param name="id">Primary key of the template to delete.</param>
    Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
}