using FilingModule.Domain.DataTransferObjects;
using ProductsModule.Domain.DataTransferObjects;

namespace ProductsModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for a service tier, containing details such as its identifier, name, description, price,
    /// and associated services.
    /// </summary>
    /// <remarks>This model is typically used to represent a service tier in a user interface or API response.
    /// It includes properties for managing display order, activation status, and default configuration.</remarks>
    public record ServiceTierViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceTierViewModel"/> class.
        /// </summary>
        public ServiceTierViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceTierViewModel"/> class using the specified data transfer
        /// object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="ServiceTierDto"/> to
        /// the corresponding properties of the <see cref="ServiceTierViewModel"/>. Ensure that the <paramref
        /// name="dto"/> parameter is properly populated before calling this constructor.</remarks>
        /// <param name="dto">The data transfer object containing the service tier details. Cannot be null.</param>
        public ServiceTierViewModel(ServiceTierDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Description = dto.Description;
            Price = dto.Price;
            Order = dto.Order;
            Default = dto.Default;
            Active = dto.Active;
            RoleId = dto.RoleId;
            Services = dto.Services;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the object.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the order in which this item is processed or displayed.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the default configuration is enabled.
        /// </summary>
        public bool Default { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Gets the unique identifier for the role.
        /// </summary>
        public string RoleId { get; set; } = string.Empty;

        /// <summary>
        /// Gets the collection of services associated with the current context.
        /// </summary>
        public ICollection<ServiceDto> Services { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public ICollection<ImageDto> Images { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of the service tier to a <see cref="ServiceTierDto"/>.
        /// </summary>
        /// <returns>A <see cref="ServiceTierDto"/> object containing the data from the current service tier instance.</returns>
        public ServiceTierDto ToDto()
        {
            return new ServiceTierDto()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Price = Price,
                Order = Order,
                Default = Default,
                Active = Active,
                RoleId = RoleId,
                Services = Services
            };
        }

        #endregion
    }
}
