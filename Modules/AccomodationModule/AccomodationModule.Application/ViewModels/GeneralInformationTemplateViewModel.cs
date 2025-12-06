using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Application.ViewModels;

/// <summary>
/// Represents a view model for general information templates, providing properties for the template's identifier, name,
/// and additional information, as well as methods for mapping to and from a DTO.
/// </summary>
/// <remarks>This class is designed to facilitate the transfer of general information template data between
/// different layers of the application. It includes functionality to initialize from a DTO and to convert back to a
/// DTO.</remarks>
public class GeneralInformationTemplateViewModel : EntityBase<string>
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralInformationTemplateViewModel"/> class.
    /// </summary>
    public GeneralInformationTemplateViewModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralInformationTemplateViewModel"/> class from a DTO.
    /// </summary>
    /// <param name="dto">DTO to map from.</param>
    public GeneralInformationTemplateViewModel(GeneralInformationTemplateDto dto)
    {
        Id = dto.Id;
        Name = dto.Name;
        Information = dto.Information;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the template identifier.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the template name.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the additional information for the template.
    /// </summary>
    public string? Information { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Converts the current instance of the object to a <see cref="GeneralInformationTemplateDto"/>.
    /// </summary>
    /// <returns>A <see cref="GeneralInformationTemplateDto"/> containing the data from the current instance.</returns>
    public GeneralInformationTemplateDto ToDto()
    {
        return new GeneralInformationTemplateDto()
        {
            Id = Id,
            Name = Name,
            Information = Information
        };
    }

    #endregion
}