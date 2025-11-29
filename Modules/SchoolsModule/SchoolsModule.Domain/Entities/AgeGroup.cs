using ConectOne.Domain.Entities;

namespace SchoolsModule.Domain.Entities
{
    /// <summary>
    /// Represents a classification of individuals based on their age range.
    /// </summary>
    /// <remarks>An <see cref="AgeGroup"/> defines a specific range of ages using the <see cref="MinAge"/> and
    /// <see cref="MaxAge"/> properties,  along with a descriptive name. This can be used to categorize individuals into
    /// distinct age groups for various purposes,  such as demographic analysis or eligibility criteria.</remarks>
    public class AgeGroup : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the minimum age required for eligibility.
        /// </summary>
        public int MinAge { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowable age, in years.
        /// </summary>
        public int MaxAge { get; set; }
    }
}
