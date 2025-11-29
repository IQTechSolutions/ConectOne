using ConectOne.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;
using ConectOne.Domain.Enums;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using IdentityModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object for the Teacher entity.
    /// </summary>
    public record TeacherDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherDto"/> class.
        /// </summary>
        public TeacherDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherDto"/> class with the specified Teacher entity.
        /// </summary>
        /// <param name="teacher">The Teacher entity.</param>
        public TeacherDto(Teacher teacher)
        {
            TeacherId = teacher.Id;
            CoverImageUrl = teacher.Images.FirstOrDefault(c => c.Image.ImageType == UploadType.Cover)?.Image.RelativePath;
            Title = teacher.Title;
            FirstName = teacher.Name;
            LastName = teacher.Surname;
            RecieveEmails = teacher.RecieveEmails;
            ReceiveMessages = teacher.ReceiveMessages;
            ReceiveNotifications = teacher.ReceiveNotifications;

            if (teacher.Grade is not null)
            {
                Grade = new SchoolGradeDto(teacher.Grade);
            }
            if (teacher.SchoolClass is not null)
            {
                SchoolClass = new SchoolClassDto(teacher.SchoolClass);
            }
            if (teacher.Address is not null)
            {
                Address = new AddressDto(teacher.Address);
            }
            if (teacher.ContactNumbers is not null)
            {
                ContactNumber = new ContactNumberDto(teacher.ContactNumbers.FirstOrDefault(c => c.Default) == null ? teacher.ContactNumbers.FirstOrDefault() : teacher.ContactNumbers.FirstOrDefault(c => c.Default));
            }
            if (teacher.EmailAddresses is not null)
            {
                EmailAddress = new EmailAddressDto(teacher.EmailAddresses.FirstOrDefault(c => c.Default) == null ? teacher.EmailAddresses.FirstOrDefault() : teacher.EmailAddresses.FirstOrDefault(c => c.Default));
            }

            Images = teacher?.Images == null ? [] : teacher.Images.Select(c => ImageDto.ToDto(c)).ToList();
        }

        #endregion

        /// <summary>
        /// Gets the ID of the teacher.
        /// </summary>
        public string? TeacherId { get; init; }

        /// <summary>
        /// Gets the URL of the cover image.
        /// </summary>
        public string? CoverImageUrl { get; init; }

        /// <summary>
        /// Gets the title of the teacher.
        /// </summary>
        public Title Title { get; init; }

        /// <summary>
        /// Gets the first name of the teacher.
        /// </summary>
        public string? FirstName { get; init; } = null!;

        /// <summary>
        /// Gets the last name of the teacher.
        /// </summary>
        public string? LastName { get; init; } = null!;

        /// <summary>
        /// Gets the display name of the teacher.
        /// </summary>
        public string? DisplayName => $"{FirstName} {LastName}";

        /// <summary>
        /// Indicates whether the parent should receive notifications (push, SMS, or other types).
        /// </summary>
        public bool ReceiveNotifications { get; init; } = true;

        /// <summary>
        /// Indicates whether the parent should receive messages related to school activities, announcements, etc.
        /// </summary>
        public bool ReceiveMessages { get; init; } = true;

        /// <summary>
        /// Indicates whether the parent should receive emails for communication from the school.
        /// </summary>
        public bool RecieveEmails { get; init; } = true;

        /// <summary>
        /// Gets the grade of the teacher.
        /// </summary>
        public SchoolGradeDto? Grade { get; init; } = default!;

        /// <summary>
        /// Gets the school class of the teacher.
        /// </summary>
        public SchoolClassDto? SchoolClass { get; init; } = default!;

        /// <summary>
        /// Gets the address of the teacher.
        /// </summary>
        public AddressDto? Address { get; init; } = default!;

        /// <summary>
        /// Gets the contact number of the teacher.
        /// </summary>
        public ContactNumberDto? ContactNumber { get; init; } = default!;

        /// <summary>
        /// Gets the email address of the teacher.
        /// </summary>
        public EmailAddressDto? EmailAddress { get; init; } = default!;

        /// <summary>
        /// Gets or sets the collection of images to be displayed.
        /// </summary>
        public ICollection<ImageDto> Images { get; set; } = [];

        /// <summary>
        /// Converts the TeacherDto to a Teacher entity.
        /// </summary>
        /// <returns>The Teacher entity.</returns>
        public Teacher ToTeacher()
        {
            return new Teacher
            {
                Id = string.IsNullOrEmpty(TeacherId) ? Guid.NewGuid().ToString() : TeacherId,
                Title = Title,
                Name = FirstName,
                Surname = LastName,
                GradeId = Grade?.SchoolGradeId,
                SchoolClassId = SchoolClass?.SchoolClassId,
                ReceiveNotifications = ReceiveNotifications,
                RecieveEmails = RecieveEmails,
                ReceiveMessages = ReceiveMessages,
                ContactNumbers = new List<ContactNumber<Teacher>>() { new() { InternationalCode = ContactNumber?.InternationalCode, AreaCode = ContactNumber?.AreaCode, Number = ContactNumber?.Number } },
                EmailAddresses = new List<EmailAddress<Teacher>>() { new() { Email = EmailAddress?.EmailAddress } },
                Address = new Address<Teacher>() { Country = "South Africa" }
            };
        }

        /// <summary>
        /// Creates and returns a <see cref="UserInfoDto"/> object populated with the current user's information.
        /// </summary>
        /// <returns>A <see cref="UserInfoDto"/> containing the user's ID, contact details, and notification preferences.</returns>
        public UserInfoDto GetUserInfoDto()
        {
            return new UserInfoDto()
            {
                UserId = TeacherId,
                CoverImageUrl = CoverImageUrl,
                FirstName = FirstName,
                MiddleName = "",
                DisplayName = DisplayName,
                LastName = LastName,
                Description = "",
                ReceiveNotifications = ReceiveNotifications,
                ReceiveMessages = ReceiveMessages,
                ReceiveEmails = RecieveEmails,
                RequireConsent = false,
                EmailAddress = EmailAddress.EmailAddress,
                PhoneNr = ContactNumber.Number
            };
        }
    }
}
