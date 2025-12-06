using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Application.ViewModels;

/// <summary>
/// View model for <see cref="VacationTitleTemplate"/> instances.
/// </summary>
public class VacationTitleTemplateViewModel : EntityBase<string>
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="VacationTitleTemplateViewModel"/> class.
    /// </summary>
    public VacationTitleTemplateViewModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="VacationTitleTemplateViewModel"/> class from a DTO.
    /// </summary>
    /// <param name="dto">DTO to map from.</param>
    public VacationTitleTemplateViewModel(VacationTitleTemplateDto dto)
    {
        Id = dto.Id;
        VacationTitle = dto.VacationTitle;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the template identifier.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the vacation title.
    /// </summary>
    public string VacationTitle { get; set; } = null!;

    #endregion

    #region Methods

    /// <summary>
    /// Converts the current instance to a <see cref="VacationTitleTemplateDto"/>.
    /// </summary>
    /// <returns>A <see cref="VacationTitleTemplateDto"/> that represents the current instance.</returns>
    public VacationTitleTemplateDto ToDto()
    {
        return new VacationTitleTemplateDto()
        {
            Id = Id,
            VacationTitle = VacationTitle
        };
    }

    #endregion

}
