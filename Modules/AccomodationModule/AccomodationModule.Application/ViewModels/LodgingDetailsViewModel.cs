using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AccomodationModule.Domain.DataTransferObjects;
using FilingModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents the details of a lodging, including its name, description, room information, attractions, and
    /// facilities.
    /// </summary>
    /// <remarks>This view model is designed to encapsulate lodging-related data for display purposes in a
    /// user interface. It can be initialized using a <see cref="LodgingDto"/> or <see cref="ContentResponse"/> object, 
    /// and provides properties for accessing and updating lodging details.</remarks>
    public class LodgingDetailsViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingDetailsViewModel"/> class.
        /// </summary>
        /// <remarks>This constructor creates a default instance of the <see
        /// cref="LodgingDetailsViewModel"/> class. Use this constructor when no initial data is required.</remarks>
        public LodgingDetailsViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingDetailsViewModel"/> class using the specified lodging
        /// data.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="LodgingDto"/> object
        /// to the corresponding properties of the <see cref="LodgingDetailsViewModel"/>.</remarks>
        /// <param name="product">A <see cref="LodgingDto"/> object containing the details of the lodging, such as its ID, name, description,
        /// and associated information.</param>
        public LodgingDetailsViewModel(LodgingDto product) 
        {
            LodgingId = product.ProductId;
            Name = product.Name;
            TeaserText = product.TeaserText;
            Description = product.Description;
            OnlineDescription = product.OnlineDescription;
            RoomInformation = product.RoomInformation;
            Attractions = product.Attractions;
            Facilities = product.Facilities;
            Grading = product.Grading;
            Rating = product.Rating;

            Images = product.Images;
            Videos = product.Videos;
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for the lodging entity.
        /// </summary>
        public string? LodgingId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [DisplayName("Product Name"), Required] public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the teaser text displayed as a brief summary or introduction.
        /// </summary>
        [DisplayName("Teaser")] public string? TeaserText { get; set; }

        /// <summary>
        /// Gets or sets the information about the room.
        /// </summary>
        [DisplayName("Room Info")] public string? RoomInformation { get; set; }

        /// <summary>
        /// Gets or sets the description text associated with the object.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the description of the item when it is available online.
        /// </summary>
        public string? OnlineDescription { get; set; }

        /// <summary>
        /// Gets or sets the list of attractions associated with the entity.
        /// </summary>
        public string? Attractions { get; set; }

        /// <summary>
        /// Gets or sets the facilities associated with the entity.
        /// </summary>
        public string? Facilities { get; set; }

        /// <summary>
        /// Gets the grading value associated with the current instance.
        /// </summary>
        public int Grading { get; set; }

        /// <summary>
        /// Gets the rating value associated with the entity.
        /// </summary>
        public int Rating { get; set; }

        #region Videos & Images

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public ICollection<ImageDto> Images { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of video files to be displayed.
        /// </summary>
        public List<VideoDto> Videos { get; set; } = [];

        #endregion
    }
}
