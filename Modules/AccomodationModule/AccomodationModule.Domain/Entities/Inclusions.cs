using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FilingModule.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents an inclusion in a vacation package, including name, description, and associated images.
    /// Inherits from <see cref="ImageFileCollection{Inclusions, string}"/> to manage associated images.
    /// </summary>
    public class Inclusions : FileCollection<Inclusions, string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Inclusions"/> class.
        /// </summary>
        public Inclusions() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Inclusions"/> class by copying another instance.
        /// </summary>
        /// <param name="inclusion">The inclusion to copy.</param>
        public Inclusions(Inclusions inclusion)
        {
            Name = inclusion.Name;
            Description = inclusion.Description;
            Selector = inclusion.Selector;
            Order = inclusion.Order;

            if (inclusion.Images is not null && inclusion.Images.Any())
            {
                foreach (var price in inclusion.Images)
                {
                    Images.Add(price);
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the inclusion.
        /// </summary>
        [MaxLength(1000)] public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the inclusion.
        /// </summary>
        [MaxLength(5000)] public string Description { get; set; } = null!;

        public string Selector { get; set; }
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the ID of the associated vacation.
        /// </summary>
        [ForeignKey(nameof(Vacation))] public string? VacationId { get; set; }

        /// <summary>
        /// Navigation property to the associated vacation.
        /// </summary>
        public Vacation? Vacation { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a copy of the current inclusion.
        /// </summary>
        /// <returns>A new instance of <see cref="Inclusions"/> that is a copy of the current instance.</returns>
        public Inclusions Clone()
        {
            return new Inclusions(this);
        }

        #endregion
    }
}
