using ConectOne.Domain.Enums;

namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// A Data Transfer Object (DTO) used for representing a learner imported from external data,
    /// such as a CSV file. This DTO includes essential learner-related fields that can be mapped 
    /// from or to external sources and passed through application layers.
    /// </summary>
    public class ImportLearnerGradeDto
    {
        /// <summary>
        /// A reference number used during the import process to track the learner record.
        /// This could represent a line number or a unique identifier from the import file.
        /// </summary>
        public string? ImportNr { get; init; } = null!;

        /// <summary>
        /// Possibly a unique school-specific accession number or a code used to store
        /// learners in the institution's system.
        /// </summary>
        public string? AccessionNr { get; init; } = null!;

        /// <summary>
        /// The learner's given (first) name.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// The learner's surname (last name).
        /// </summary>
        public string Surname { get; set; } = null!;

        /// <summary>
        /// The learner's gender, represented by the Gender enum.
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// The class the learner is part of. 
        /// For instance, this might represent a specific homeroom or section, such as "Class A".
        /// </summary>
        public string Class { get; set; } = null!;

        /// <summary>
        /// The learner's grade level, such as "Grade 1" or "Grade 10".
        /// This helps identify the learner's position in the academic progression.
        /// </summary>
        public string Grade { get; set; } = null!;

        /// <summary>
        /// A unique ID number assigned to the learner. This could be a national identification number 
        /// or a school-issued student ID.
        /// </summary>
        public string IDNumber { get; set; } = null!;

        /// <summary>
        /// The learner's birth date, provided as a string. 
        /// This may need parsing or validation when actually used (e.g., converting to a DateTime).
        /// </summary>
        public string BirthDate { get; set; } = null!;

        /// <summary>
        /// Gets or sets the color associated with the object.
        /// </summary>
        public string Color { get; set; }
    }
}
