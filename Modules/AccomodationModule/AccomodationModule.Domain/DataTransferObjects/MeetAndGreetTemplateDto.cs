using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects;

/// <summary>
/// Data Transfer Object for <see cref="MeetAndGreetTemplate"/>.
/// </summary>
public record MeetAndGreetTemplateDto
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public MeetAndGreetTemplateDto() { }

    /// <summary>
    /// Initializes a new instance from the specified entity.
    /// </summary>
    /// <param name="entity">Entity to map from.</param>
    public MeetAndGreetTemplateDto(MeetAndGreetTemplate entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        Description = entity.Description;
        Location = entity.Location;
        TimeDescription = entity.TimeDescription;
        Contact = entity.Contact == null ? new ContactDto() : new ContactDto(entity.Contact);
    }

    #endregion

    /// <summary>
    /// Gets the template identifier.
    /// </summary>
    public string? Id { get; init; }
    
    /// <summary>
    /// Gets the name associated with the object.
    /// </summary>
    public string? Name { get; init; }
    
    /// <summary>
    /// Gets or sets the description of the template.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets or sets the location of the meeting or gathering.
    /// </summary>
    public string? Location { get; init; }

    /// <summary>
    /// Gets a description of the time associated with the current instance.
    /// </summary>
    public string? TimeDescription { get; init; }

    /// <summary>
    /// Gets or sets the identifier of the associated contact person.
    /// </summary>
    public ContactDto? Contact { get; init; }
}
