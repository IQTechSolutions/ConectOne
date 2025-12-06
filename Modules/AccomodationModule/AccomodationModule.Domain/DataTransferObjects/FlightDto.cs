using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for a flight associated with a vacation or extension.
    /// This DTO is used to transfer flight-related data between layers of the application.
    /// </summary>
    public record FlightDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightDto"/> class with default values.
        /// </summary>
        public FlightDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightDto"/> class using a <see cref="flight"/> entity.
        /// Copies the properties from the entity to the DTO.
        /// </summary>
        /// <param name="flight">The flight entity containing flight details.</param>
        public FlightDto(Flight flight)
        {
            FlightId = flight.Id;
            ArrivalDayNr = flight.ArrivalDayNr;
            DepartureDayNr = flight.DepartureDayNr;
            DepartureTime = flight.DepartureTime;
            ArrivalTime = flight.ArrivalTime;
            DepartureAirport = flight.DepartureAirport == null ? null : new AirportDto(flight.DepartureAirport);
            ArrivalAirport = flight.ArrivalAirport == null ? null : new AirportDto(flight.ArrivalAirport);
            FlightNumber = flight.FlightNumber;
            ConfirmationNumber = flight.ConfirmationNumber;
            DepartureMovementTime = flight.DepartureMovementTime;
            DepartureMovementDestination = flight.DepartureMovementDestination;
            DepartureMovementMode = flight.DepartureMovementMode;
            DepartureMovementNotes = flight.DepartureMovementNotes;
            VacationId = flight.VacationId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the flight.
        /// </summary>
        public string FlightId { get; init; } = null!;

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
        public TimeSpan? DepartureTime { get; init; }

        /// <summary>
        /// Gets or sets the arrival time of the flight.
        /// </summary>
        public TimeSpan? ArrivalTime { get; init; }

        /// <summary>
        /// Gets or sets the departure airport for the flight.
        /// </summary>
        public AirportDto? DepartureAirport { get; init; } 

        /// <summary>
        /// Gets or sets the arrival airport for the flight.
        /// </summary>
        public AirportDto? ArrivalAirport { get; init; }

        /// <summary>
        /// Gets or sets the flight number.
        /// </summary>
        public string? FlightNumber { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the routing information for the flight.
        /// </summary>
        public string Routing => $"{DepartureAirport?.Code} - {ArrivalAirport?.Code}";

        /// <summary>
        /// Gets or sets the confirmation number for the flight.
        /// </summary>
        public string? ConfirmationNumber { get; init; } = string.Empty;

        /// <summary>
        /// Gets the vacation details associated with the current entity.
        /// </summary>
        public string? VacationId { get; init; }

        #endregion

        #region Departure Movements

        /// <summary>
        /// Gets or sets the time of the departure movement.
        /// </summary>
        public TimeSpan? DepartureMovementTime { get; init; }

        /// <summary>
        /// Gets or sets the destination of the departure movement.
        /// </summary>
        public string? DepartureMovementDestination { get; init; }

        /// <summary>
        /// Gets or sets the mode of the departure movement.
        /// </summary>
        public string? DepartureMovementMode { get; init; }

        /// <summary>
        /// Gets or sets any notes associated with departure movement.
        /// </summary>
        public string? DepartureMovementNotes { get; init; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current DTO into a <see cref="Flight"/> entity.
        /// </summary>
        /// <returns>A new instance of the <see cref="Flight"/> entity with the DTO's data.</returns>
        public Flight ToFlight()
        {
            return new Flight
            {
                Id = FlightId,
                ArrivalDayNr = ArrivalDayNr,
                DepartureDayNr = DepartureDayNr,
                DepartureTime = DepartureTime,
                ArrivalTime = ArrivalTime,
                DepartureAirportId = DepartureAirport?.Id,
                ArrivalAirportId = ArrivalAirport?.Id,
                FlightNumber = FlightNumber,
                ConfirmationNumber = ConfirmationNumber,
                VacationId = VacationId,
                DepartureMovementTime = DepartureMovementTime,
                DepartureMovementDestination = DepartureMovementDestination,
                DepartureMovementMode = DepartureMovementMode,
                DepartureMovementNotes = DepartureMovementNotes
            };
        }

        /// <summary>
        /// Updates the values of an existing <see cref="Flight"/> entity with the properties of this DTO.
        /// </summary>
        /// <param name="flight">The vacation price that needs to be updated</param>
        public void UpdateFlightValues(in Flight flight)
        {
            flight.Id = FlightId;
            flight.ArrivalDayNr = ArrivalDayNr;
            flight.DepartureDayNr = DepartureDayNr;
            flight.DepartureTime = DepartureTime;
            flight.ArrivalTime = ArrivalTime;
            flight.DepartureAirportId = DepartureAirport?.Id;
            flight.ArrivalAirportId = ArrivalAirport?.Id;
            flight.FlightNumber = FlightNumber;
            flight.ConfirmationNumber = ConfirmationNumber;
            flight.DepartureMovementTime = DepartureMovementTime;
            flight.DepartureMovementDestination = DepartureMovementDestination;
            flight.DepartureMovementMode = DepartureMovementMode;
            flight.DepartureMovementNotes = DepartureMovementNotes;
        }

        #endregion
    }
}
