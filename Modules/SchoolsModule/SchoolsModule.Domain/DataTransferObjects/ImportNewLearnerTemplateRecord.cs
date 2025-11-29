using ConectOne.Domain.Entities;
using ConectOne.Domain.Enums;

namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a record for importing new learner data from a CSV file.
    /// </summary>
    public record ImportNewLearnerTemplateRecord : CsvableBase
    {
        /// <summary>
        /// Gets or sets the first name of the parent.
        /// </summary>
        public string ParentName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the surname of the parent.
        /// </summary>
        public string ParentSurname { get; set; } = null!;

        /// <summary>
        /// Gets or sets the mobile number of the parent.
        /// </summary>
        public string ParentMobileNo { get; set; } = null!;

        /// <summary>
        /// Gets or sets the email address of the parent.
        /// </summary>
        public string ParentEmail { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identification number of the parent.
        /// </summary>
        public string ParentIdNumber { get; set; } = null!;

        /// <summary>
        /// Gets or sets the primary address line of the parent.
        /// </summary>
        public string ParentAddressLine1 { get; set; } = null!;

        /// <summary>
        /// Gets or sets the first name of the learner.
        /// </summary>
        public string LearnerName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the surname of the learner.
        /// </summary>
        public string LearnerSurname { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identification or passport number of the learner.
        /// </summary>
        public string LearnerIdPassportNumber { get; set; } = null!;

        /// <summary>
        /// Gets or sets the gender of the learner.
        /// </summary>
        public Gender LearnerGender { get; set; }

        /// <summary>
        /// Gets or sets the grade (or year level) in which the learner is enrolled.
        /// </summary>
        public string? Grade { get; set; }

        /// <summary>
        /// Gets or sets the class or homeroom identifier for the learner.
        /// </summary>
        public string? Class { get; set; }

        /// <summary>
        /// Populates the properties of the record from a CSV array.
        /// </summary>
        /// <param name="values">The array of CSV values.</param>
        public override void FromCsv(string[] values)
        {
            ParentName = values[0];
            ParentSurname = values[1];
            ParentMobileNo = values[2];
            ParentEmail = values[3];
            ParentIdNumber = values[4];
            ParentAddressLine1 = values[5];
            LearnerName = values[7];
            LearnerSurname = values[8];
            LearnerIdPassportNumber = values[10];
            Grade = values[12];
            Class = values[11].Replace(",",".");
            LearnerGender = GenderExtensions.ToGender(values[13]);
        }
    }
}
