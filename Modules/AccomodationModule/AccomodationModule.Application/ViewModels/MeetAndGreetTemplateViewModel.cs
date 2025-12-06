using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Application.ViewModels;

/// <summary>
/// View model for <see cref="MeetAndGreetTemplate"/> instances.
/// </summary>
public class MeetAndGreetTemplateViewModel : EntityBase<string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MeetAndGreetTemplateViewModel"/> class.
    /// </summary>
    public MeetAndGreetTemplateViewModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MeetAndGreetTemplateViewModel"/> class from a DTO.
    /// </summary>
    /// <param name="dto">DTO to map from.</param>
    public MeetAndGreetTemplateViewModel(MeetAndGreetTemplateDto dto)
    {
        Id = dto.Id;
        Name = dto.Name;
        Description = dto.Description;
        Location = dto.Location;
        TimeDescription = dto.TimeDescription;
        Contact = dto.Contact;
    }

    /// <summary>
    /// Gets or sets the template identifier.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name associated with the object.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets additional description for the template.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the meeting or gathering location.
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Gets or sets a description of the time, typically used to provide a human-readable representation of a time value.
    /// </summary>
    public string TimeDescription { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated contact.
    /// </summary>
    public ContactDto? Contact { get; set; }

    #region Methods

    /// <summary>
    /// Converts the current instance of the <see cref="MeetAndGreetTemplate"/> class to a  <see
    /// cref="MeetAndGreetTemplateDto"/> object.
    /// </summary>
    /// <returns>A <see cref="MeetAndGreetTemplateDto"/> object containing the data from the current instance.</returns>
    public MeetAndGreetTemplateDto ToDto()
    {
        return new MeetAndGreetTemplateDto()
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Location = Location,
            TimeDescription = TimeDescription,
            Contact = Contact
        };
    }

    #endregion
}
