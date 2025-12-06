using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for managing flight details associated with a vacation or extension.
    /// Provides properties for binding flight data in the UI.
    /// </summary>
    public class FlightViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightViewModel"/> class with default values.
        /// </summary>
        public FlightViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightViewModel"/> class using a <see cref="FlightDto"/>.
        /// Copies the properties from the DTO to the view model.
        /// </summary>
        /// <param name="dto">The data transfer object containing flight details.</param>
        public FlightViewModel(FlightDto dto)
        {
            FlightId = dto.FlightId;
            ArrivalDayNr = dto.ArrivalDayNr;
            DepartureDayNr = dto.DepartureDayNr;
            DepartureTime = dto.DepartureTime;
            ArrivalTime = dto.ArrivalTime;
            DepartureAirport = dto.DepartureAirport;
            ArrivalAirport = dto.ArrivalAirport;
            FlightNumber = dto.FlightNumber;
            ConfirmationNumber = dto.ConfirmationNumber;
            VacationId = dto.VacationId;
            DepartureMovementTime = dto.DepartureMovementTime;
            DepartureMovementDestination = dto.DepartureMovementDestination;
            DepartureMovementMode = dto.DepartureMovementMode;
            DepartureMovementNotes = dto.DepartureMovementNotes;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the flight.
        /// </summary>
        public string FlightId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the arrival day number.
        /// </summary>
        public int ArrivalDayNr { get; set; }

        /// <summary>
        /// Gets or sets the departure day number.
        /// </summary>
        public int? DepartureDayNr { get; set; }

        /// <summary>
        /// Gets or sets the departure time of the flight.
        /// </summary>
        public TimeSpan? DepartureTime { get; set; }

        /// <summary>
        /// Gets or sets the arrival time of the flight.
        /// </summary>
        public TimeSpan? ArrivalTime { get; set; }

        /// <summary>
        /// Gets or sets the departure airport for the flight.
        /// </summary>
        public AirportDto? DepartureAirport { get; set; } 

        /// <summary>
        /// Gets or sets the arrival airport for the flight.
        /// </summary>
        public AirportDto? ArrivalAirport { get; set; } 

        /// <summary>
        /// Gets or sets the flight number.
        /// </summary>
        public string? FlightNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the routing information for the flight.
        /// </summary>
        public string Routing => $"{DepartureAirport?.Code} - {ArrivalAirport?.Code}";

        /// <summary>
        /// Gets or sets the confirmation number for the flight.
        /// </summary>
        public string? ConfirmationNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ID of the associated vacation.
        /// </summary>
        public string? VacationId { get; set; }

        #endregion

        #region Departure Movement Properties

        /// <summary>
        /// Gets or sets the time of the departure movement.
        /// </summary>
        public TimeSpan? DepartureMovementTime { get; set; }

        /// <summary>
        /// Gets or sets the destination of the departure movement.
        /// </summary>
        public string? DepartureMovementDestination { get; set; }

        /// <summary>
        /// Gets or sets the mode of the departure movement.
        /// </summary>
        public string? DepartureMovementMode { get; set; }

        /// <summary>
        /// Gets or sets any notes associated with departure movement.
        /// </summary>
        public string? DepartureMovementNotes { get; set; }

        #endregion

        #region Methods

        public FlightDto ToDto()
        {
            return new FlightDto
            {
                FlightId = this.FlightId,
                ArrivalDayNr = this.ArrivalDayNr,
                DepartureDayNr = this.DepartureDayNr,
                DepartureTime = this.DepartureTime,
                ArrivalTime = this.ArrivalTime,
                DepartureAirport = this.DepartureAirport,
                ArrivalAirport = this.ArrivalAirport,
                FlightNumber = this.FlightNumber,
                ConfirmationNumber = this.ConfirmationNumber,
                VacationId = this.VacationId,
                DepartureMovementTime = this.DepartureMovementTime,
                DepartureMovementDestination = this.DepartureMovementDestination,
                DepartureMovementMode = this.DepartureMovementMode,
                DepartureMovementNotes = this.DepartureMovementNotes
            };
        }

        #endregion
    }
}