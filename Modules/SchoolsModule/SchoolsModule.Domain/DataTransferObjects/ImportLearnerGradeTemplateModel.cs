using ConectOne.Domain.Entities;
using ConectOne.Domain.Enums;

namespace SchoolsModule.Domain.DataTransferObjects;

/// <summary>
/// Represents a record for importing new learner data from a CSV file. This model is used 
/// as a template when parsing data that represents learners (students) along with their 
/// class, grade, and personal details.
/// </summary>
public record ImportLearnerGradeTemplateModel : CsvableBase
{
    /// <summary>
    /// A number used to track imports. It might be a reference or an index within the 
    /// import file, indicating the sequence of this learner's record.
    /// </summary>
    public string? ImportNr { get; set; } = null!;

    /// <summary>
    /// Possibly a specific accession number or code assigned to the learner in the school database.
    /// </summary>
    public string? AccessionNr { get; set; } = null!;

    /// <summary>
    /// The learner's given name (first name).
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The learner's surname (last name).
    /// </summary>
    public string Surname { get; set; } = null!;

    /// <summary>
    /// The learner's gender, represented by a Gender enum.
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// The class the learner belongs to (e.g., "Class A", "Class 1C"). This indicates 
    /// the specific classroom grouping within the grade.
    /// </summary>
    public string Class { get; set; } = null!;

    /// <summary>
    /// The grade level of the learner (e.g., "Grade 5", "Grade 10"), representing their 
    /// year or level within the school system.
    /// </summary>
    public string Grade { get; set; } = null!;

    /// <summary>
    /// A unique identifier for the learner, possibly a national ID number or a school-issued ID.
    /// </summary>
    public string IDNumber { get; set; } = null!;

    /// <summary>
    /// The learner's birth date, stored as a string. Parsing and validation of this string 
    /// would likely occur downstream. This can be used to confirm age or enrollment eligibility.
    /// </summary>
    public string BirthDate { get; set; } = null!;

    /// <summary>
    /// Populates the properties of the record from a CSV array. The CSV array should be 
    /// indexed in a manner consistent with the defined record structure. 
    /// The method maps each CSV field to the corresponding property:
    ///  - values[0] -> ImportNr
    ///  - values[1] -> AccessionNr
    ///  - values[2] -> Name
    ///  - values[3] -> Surname
    ///  - values[4] -> Gender (parsed via GenderExtensions)
    ///  - values[5] -> Class
    ///  - values[7] -> Grade
    ///  - values[8] -> IDNumber
    ///  - values[10] -> BirthDate
    ///
    /// Note: Some values (like values[6] and values[9]) are seemingly skipped, indicating 
    /// that the CSV has more columns than are currently being used, or the template expects 
    /// data in certain positions.
    /// </summary>
    /// <param name="values">The array of CSV values.</param>
    public override void FromCsv(string[] values)
    {
        if (values.Length < 9) // Adjust based on expected minimum columns
        {
            throw new ArgumentException("The CSV row does not contain enough columns.", nameof(values));
        }

        ImportNr = values[0];
        AccessionNr = values[1];
        Surname = values[2];
        Name = values[3];
        Gender = GenderExtensions.ToGender(values[4]); // Handle gender conversion
        Class = values[5];
        Grade = values[6];
        IDNumber = string.IsNullOrWhiteSpace(values[7]) ? null : values[7]; // Handle missing IDNumber
        BirthDate = values[8];
    }
}