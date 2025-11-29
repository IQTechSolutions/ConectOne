using ConectOne.Domain.Entities;

namespace SchoolsModule.Domain.Entities
{
    /// <summary>
    /// Represents a grade level in a school, such as "Grade 1" or "Grade 12".
    /// </summary>
    /// <remarks>A school grade typically contains a collection of classes associated with it.  This class
    /// provides properties to manage the grade's name and its related classes.</remarks>
    public class SchoolGrade : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of school classes associated with the entity.
        /// </summary>
        public ICollection<SchoolClass> Classes { get; set; } = [];
    }
}
