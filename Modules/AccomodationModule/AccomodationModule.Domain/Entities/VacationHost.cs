using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Enums;
using FilingModule.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a host entity that organizes and manages vacations, along with associated extensions and metadata.
    /// </summary>
    /// <remarks>The <see cref="VacationHost"/> class provides properties to store information about the host,
    /// such as its name,  description, and collections of vacations and vacation extensions. It inherits functionality
    /// from  <see cref="FileCollection{T, TKey}"/> to manage associated image files.</remarks>
    public class VacationHost : FileCollection<VacationHost, string>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the address associated with the entity.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the name of the suburb associated with the address.
        /// </summary>
        public string? Suburb { get; set; }

        /// <summary>
        /// Gets or sets the latitude coordinate of a geographic location.
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude component of a geographic coordinate.
        /// </summary>
        public double Lng { get; set; }

        /// <summary>
        /// Gets or sets the zoom level for the view.
        /// </summary>
        public int Zoom { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated area.
        /// </summary>
        [ForeignKey(nameof(Area))] public string? AreaId { get; set; }

        /// <summary>
        /// Gets or sets the geographical area associated with the entity.
        /// </summary>
        public Area? Area { get; set; }

        #endregion

        #region Reprasentative

        /// <summary>
        /// Gets or sets the title of the representative.
        /// </summary>
        public Title RepresentativeTitle { get; set; } = Title.Me;

        /// <summary>
        /// Gets or sets the name of the representative.
        /// </summary>
        public string? RepresentativeName2 { get; set; } 

        /// <summary>
        /// Gets or sets the surname of the representative.
        /// </summary>
        public string? RepresentativeSurname2 { get; set; } 

        /// <summary>
        /// Gets or sets the phone number of the representative.
        /// </summary>
        public string? RepresentativePhone { get; set; }

        /// <summary>
        /// Gets or sets the email address of the representative.
        /// </summary>
        public string? RepresentativeEmail { get; set; }

        /// <summary>
        /// Gets or sets the biography of the representative.
        /// </summary>
        public string? RepresentativeBio { get; set; }

        #endregion


        /// <summary>
        /// Gets or sets the collection of vacations associated with the entity.
        /// </summary>
        public ICollection<Vacation> Vacations { get; set; } = [];

        /// <summary>
        /// Returns a string representation of the current object.
        /// </summary>
        /// <returns>A string that represents the current object, typically used for debugging or logging purposes.</returns>
        public override string ToString()
        {
            return $"Vacation Host";
        }
    }
}
