using ConectOne.Application.ViewModels;
using ConectOne.Domain.Enums;
using FilingModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for a teacher, encapsulating personal and professional details.
    /// </summary>
    /// <remarks>This class provides a structured representation of a teacher's information, including
    /// identifiers, contact details, and associated school data. It can be initialized with a <see cref="TeacherDto"/>
    /// to populate its properties.</remarks>
    public class TeacherViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherViewModel"/> class.
        /// </summary>
        public TeacherViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherViewModel"/> class using the specified <see
        /// cref="TeacherDto"/>.
        /// </summary>
        /// <remarks>This constructor maps the properties from the <see cref="TeacherDto"/> to the
        /// corresponding properties of the <see cref="TeacherViewModel"/>. If certain properties in the <paramref
        /// name="dto"/> are null, the corresponding view model properties will also be null.</remarks>
        /// <param name="dto">The data transfer object containing teacher information. Must not be null.</param>
        public TeacherViewModel(TeacherDto dto)
        {
            try
            {
                TeacherId = dto.TeacherId;

                CoverImageUrl = dto.CoverImageUrl;

                Title = dto.Title;
                FirstName = dto.FirstName;
                LastName = dto.LastName;

                if (dto.Grade != null)
                {
                    Grade = new SchoolGradeViewModel(dto.Grade);
                }

                if (dto.SchoolClass != null)
                {
                    Class = new SchoolClassViewModel(dto.SchoolClass);
                }

                if (dto.ContactNumber != null)
                {
                    ContactNumber = new ContactNumberViewModel(dto.ContactNumber);
                }

                if (dto.EmailAddress != null)
                {
                    EmailAddress = new EmailAddressViewModel(dto.EmailAddress);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for the teacher.
        /// </summary>
        public string? TeacherId { get; set; }

        /// <summary>
        /// Gets or sets the URL of the cover image.
        /// </summary>
        public string? CoverImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the title associated with the entity.
        /// </summary>
        public Title Title { get; set; }

        /// <summary>
        /// Gets or sets the first name of the individual.
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the last name of the individual.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the grade information for the school.
        /// </summary>
        public SchoolGradeViewModel? Grade { get; set; }

        /// <summary>
        /// Gets or sets the view model representing a school class.
        /// </summary>
        public SchoolClassViewModel? Class { get; set; }

        /// <summary>
        /// Gets or sets the contact number associated with the view model.
        /// </summary>
        public ContactNumberViewModel ContactNumber { get; set; } 

        /// <summary>
        /// Gets or sets the email address associated with the view model.
        /// </summary>
        public EmailAddressViewModel EmailAddress { get; set; } 

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public ICollection<ImageDto> Images { get; set; } = [];

        #region Methods

        /// <summary>
        /// Converts the current <see cref="Teacher"/> instance to a <see cref="TeacherDto"/> representation.
        /// </summary>
        /// <remarks>This method maps the properties of the <see cref="Teacher"/> object to a new <see
        /// cref="TeacherDto"/> object. Nested objects, such as <see cref="Grade"/> and <see cref="SchoolClass"/>, are
        /// also converted to their respective DTOs if they are not null.</remarks>
        /// <returns>A <see cref="TeacherDto"/> object containing the data from the current <see cref="Teacher"/> instance.</returns>
        public TeacherDto ToDto()
        {
            return new TeacherDto
            {
                TeacherId = TeacherId,
                CoverImageUrl = CoverImageUrl,
                Title = Title,
                FirstName = FirstName,
                LastName = LastName,
                Grade = Grade is not null ? Grade.ToDto() : null,
                SchoolClass = Class is not null ? Class.ToDto() : null,
                ContactNumber = ContactNumber.ToDto(),
                EmailAddress = EmailAddress?.ToDto(),
                Images = Images
            };
        }

        #endregion
    }
}
