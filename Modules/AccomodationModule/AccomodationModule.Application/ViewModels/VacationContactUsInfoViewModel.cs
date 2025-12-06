using System.ComponentModel.DataAnnotations;
using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents the contact information submitted through a "Contact Us" form for vacation-related inquiries.
    /// </summary>
    /// <remarks>This view model is typically used to capture and process user-submitted data, such as name,
    /// email, subject,  and message content, for vacation-related inquiries. All properties are immutable and must be
    /// initialized  when creating an instance.</remarks>
    public class VacationContactUsInfoViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationContactUsInfoViewModel"/> class.
        /// </summary>
        public VacationContactUsInfoViewModel() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationContactUsInfoViewModel"/> class using the specified
        /// data transfer object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see
        /// cref="VacationContactUsInfoDto"/> to the corresponding properties of the <see
        /// cref="VacationContactUsInfoViewModel"/>.</remarks>
        /// <param name="dto">The data transfer object containing the contact information to populate the view model.</param>
        public VacationContactUsInfoViewModel(VacationContactUsInfoDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Surname = dto.Surname;
            Email = dto.Email;
            Subject = dto.Subject;
            Website = dto.Website;
            Message = dto.Message;
            Cell = dto.Cell;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier for the entity.
        /// </summary>
        [Required] public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets the name associated with the object.
        /// </summary>
        [Required] public string Name { get; set; } = null!;

        /// <summary>
        /// Gets the name associated with the object.
        /// </summary>
        [Required] public string Surname { get; set; } = null!;

        /// <summary>
        /// Gets the email address associated with the entity.
        /// </summary>
        [Required] public string Cell { get; set; } = null!;

        /// <summary>
        /// Gets the email address associated with the entity.
        /// </summary>
        [Required] public string Email { get; set; } = null!;

        /// <summary>
        /// Gets the subject associated with the current instance.
        /// </summary>
        [Required] public string Subject { get; set; } = "Pro Golf Contact Us Form Submission";

        /// <summary>
        /// Gets the website URL associated with the entity.
        /// </summary>
        public string Website { get; set; } = null!;

        /// <summary>
        /// Gets the message associated with the current instance.
        /// </summary>
        [Required, DataType(DataType.MultilineText)]public string Message { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of <see cref="VacationContactUsInfo"/> to a  <see
        /// cref="VacationContactUsInfoDto"/>.
        /// </summary>
        /// <returns>A <see cref="VacationContactUsInfoDto"/> containing the data from the current instance.</returns>
        public VacationContactUsInfoDto ToDto()
        {
            return new VacationContactUsInfoDto
            {
                Id = Id,
                Name = Name,
                Surname = Surname,
                Email = Email,
                Subject = Subject,
                Website = Website,
                Message = Message,
                Cell = Cell
            };
        }

        #endregion
    }
}
