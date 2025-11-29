using FilingModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Entities;

namespace ProductsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a service tier, encapsulating details such as  its identifier, name,
    /// description, price, order, and associated services.
    /// </summary>
    /// <remarks>This DTO is designed to facilitate the transfer of service tier data between different layers
    /// of the application, such as the domain and presentation layers. It includes properties for  identifying the
    /// service tier, describing its characteristics, and associating it with a  collection of services.</remarks>
    public record ServiceTierDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceTierDto"/> class.
        /// </summary>
        public ServiceTierDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceTierDto"/> class using the specified <see
        /// cref="ServiceTier"/> entity.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="ServiceTier"/> entity
        /// to the corresponding properties of the DTO. If the <c>TierServices</c> collection in the entity is not
        /// <c>null</c>, it is transformed into a list of <see cref="ServiceDto"/> objects.</remarks>
        /// <param name="entity">The <see cref="ServiceTier"/> entity from which to populate the DTO properties. Cannot be <c>null</c>.</param>
        public ServiceTierDto(ServiceTier entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Description = entity.Description;
            Price = entity.Price;
            Order = entity.Order;
            Default = entity.Default;
            Active = entity.Active;
            RoleId = entity.RoleId;
            Services = entity.TierServices is not null ? entity.TierServices.Select(c => new ServiceDto(c.OfferedService)).ToList() : [];
            Images = entity.Images.Select(ImageDto.ToDto).ToList();
        }

        #endregion
        
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets or sets the description of the object.
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        public double Price { get; init; }

        /// <summary>
        /// Gets or sets the order in which this item is processed or displayed.
        /// </summary>
        public int Order { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether the default configuration is enabled.
        /// </summary>
        public bool Default { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; init; }

        /// <summary>
        /// Gets the unique identifier for the role.
        /// </summary>
        public string RoleId { get; init; }

        /// <summary>
        /// Gets the collection of services associated with the current context.
        /// </summary>
        public ICollection<ServiceDto> Services { get; init; } = [];

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public ICollection<ImageDto> Images { get; set; } = [];

        /// <summary>
        /// Converts the current object to a <see cref="ServiceTier"/> instance.
        /// </summary>
        /// <returns>A <see cref="ServiceTier"/> object populated with the values from the current instance.</returns>
        public ServiceTier ToServiceTier()
        {
            return new ServiceTier
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Price = this.Price,
                Order = this.Order,
                Default = this.Default,
                Active = this.Active,
                RoleId = this.RoleId
            };
        }

        /// <summary>
        /// Updates the specified <see cref="ServiceTier"/> instance with the current object's properties.
        /// </summary>
        /// <param name="serviceTier">The <see cref="ServiceTier"/> instance to update. This object will have its properties overwritten with the
        /// values from the current instance.</param>
        /// <returns>The updated <see cref="ServiceTier"/> instance with properties copied from the current object.</returns>
        public ServiceTier UpdateServiceTier(ServiceTier serviceTier)
        {
            serviceTier.Id = this.Id;
            serviceTier.Name = this.Name;
            serviceTier.Description = this.Description;
            serviceTier.Price = this.Price;
            serviceTier.Order = this.Order;
            serviceTier.Default = this.Default;
            serviceTier.Active = this.Active;
            serviceTier.RoleId = this.RoleId;

            return serviceTier;
        }
    }
}
