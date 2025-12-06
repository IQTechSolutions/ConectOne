using System.ComponentModel.DataAnnotations;
using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    public class VacationContactUsInfoDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationContactUsInfoDto"/> class.
        /// </summary>
        public VacationContactUsInfoDto(){}

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationContactUsInfoDto"/> class using the specified <see
        /// cref="VacationContactUsInfo"/> entity.
        /// </summary>
        /// <param name="entity">The <see cref="VacationContactUsInfo"/> entity containing the data to populate the DTO. Cannot be <see
        /// langword="null"/>.</param>
        public VacationContactUsInfoDto(VacationContactUsInfo entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Cell = entity.Cell;
            Email = entity.Email;
            Subject = entity.Subject;
            Website = entity.Website;
            Message = entity.Message;
            CreatedOn = entity.CreatedOn;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier for the entity.
        /// </summary>
        public string? Id { get; init; }

        /// <summary>
        /// Gets the name associated with the object.
        /// </summary>
        public string? Name { get; init; } 

        /// <summary>
        /// Gets the name associated with the object.
        /// </summary>
        public string? Surname { get; init; } = null!;

        /// <summary>
        /// Gets the email address associated with the entity.
        /// </summary>
        [Required] public string Cell { get; set; } = null!;

        /// <summary>
        /// Gets the email address associated with the entity.
        /// </summary>
        public string? Email { get; init; } 

        /// <summary>
        /// Gets the subject associated with the current instance.
        /// </summary>
        public string? Subject { get; init; }

        /// <summary>
        /// Gets the website URL associated with the entity.
        /// </summary>
        public string? Website { get; init; } 

        /// <summary>
        /// Gets the message associated with the current instance.
        /// </summary>
        public string? Message { get; init; }

        /// <summary>
        /// Gets the date and time when the entity was created, or <see langword="null"/> if the creation date is not
        /// set.
        /// </summary>
        public DateTime? CreatedOn { get; init; } = DateTime.Now;

        /// <summary>
        /// Converts the current instance to a <see cref="VacationContactUsInfo"/> object.
        /// </summary>
        /// <returns>A <see cref="VacationContactUsInfo"/> object containing the values of the current instance's properties,
        /// including <see cref="Id"/>, <see cref="Name"/>, <see cref="Email"/>, <see cref="Subject"/>, <see
        /// cref="Website"/>, and <see cref="Message"/>.</returns>
        public VacationContactUsInfo ToVacationContactUsInfo()
        {
            return new VacationContactUsInfo()
            {
                Id = Id,
                Name = Name,
                Cell = Cell,
                Email = Email,
                Subject = Subject,
                Website = Website,
                Message = Message,
                CreatedOn = CreatedOn
            };
        }

        /// <summary>
        /// Updates the vacation contact information based on the provided data transfer object (DTO).
        /// </summary>
        /// <param name="dto">A <see cref="VacationContactUsInfoDto"/> containing the updated contact information,  including the name,
        /// email, subject, website, and message.</param>
        /// <returns>A <see cref="VacationContactUsInfo"/> object populated with the updated contact information.</returns>
        public static VacationContactUsInfo UpdateVacationContactUsInfo(VacationContactUsInfoDto dto)
        {
            return new VacationContactUsInfo()
            {
                Name = dto.Name,
                Cell = dto.Cell,
                Email = dto.Email,
                Subject = dto.Subject,
                Website = dto.Website,
                Message = dto.Message
            };
        }

        #endregion
    }
}
