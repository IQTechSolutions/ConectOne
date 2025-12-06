using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents the parameters associated with child age configurations, including name, type, value, and service
    /// details.
    /// </summary>
    /// <remarks>This class is used to define and manage child age-related parameters, which may include
    /// optional values such as type,  numerical value, and a unique identifier. It also supports linking to a related
    /// service via a foreign key.</remarks>
    public class ChildAgeParams : EntityBase<int>
    {
        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type identifier associated with the current object.
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// Gets or sets the numeric value associated with this instance.
        /// </summary>
        public double? Value { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier associated with the entity.
        /// </summary>
        public string UniqueNr { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated service.
        /// </summary>
        [ForeignKey(nameof(Service))] public int? ServiceId { get; set; }

        /// <summary>
        /// Gets or sets the service instance used to perform operations or provide functionality.
        /// </summary>
        public LodgingService Service { get; set; }

        /// <summary>
        /// Returns a string representation of the current object.
        /// </summary>
        /// <returns>A string that represents the current object, specifically "Child Age Params".</returns>
        public override string ToString()
        {
            return $"Child Age Params";
        }
    }
}