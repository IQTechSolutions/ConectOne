using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a flight associated with a vacation or vacation extension.
    /// Includes details such as date, time, airports, and routing information.
    /// </summary>
    public class Flight : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Flight"/> class.
        /// </summary>
        public Flight() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Flight"/> class by copying the details from an existing flight.
        /// </summary>
        /// <param name="flight">The flight instance from which to copy the details. Cannot be <see langword="null"/>.</param>
        public Flight(Flight flight)
        {
            ArrivalDayNr = flight.ArrivalDayNr;
            DepartureDayNr = flight.DepartureDayNr;
            DepartureTime = flight.DepartureTime;
            ArrivalTime = flight.ArrivalTime;
            DepartureAirportId = flight.DepartureAirportId;
            ArrivalAirportId = flight.ArrivalAirportId;
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
        /// Gets or sets the departure day number.
        /// </summary>
        public int? DepartureDayNr { get; set; }

        /// <summary>
        /// Gets or sets the arrival day number.
        /// </summary>
        public int ArrivalDayNr { get; set; }

        /// <summary>
        /// Gets or sets the departure time of the flight.
        /// </summary>
        public TimeSpan? DepartureTime { get; set; }

        /// <summary>
        /// Gets or sets the arrival time of the flight.
        /// </summary>
        public TimeSpan? ArrivalTime { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the departure airport.
        /// </summary>
        [ForeignKey(nameof(DepartureAirport))]public string? DepartureAirportId { get; set; }

        /// <summary>
        /// Gets or sets the departure airport for the flight.
        /// </summary>
        public Airport? DepartureAirport { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the arrival airport.
        /// </summary>
        [ForeignKey(nameof(ArrivalAirport))] public string? ArrivalAirportId { get; set; }

        /// <summary>
        /// Gets or sets the arrival airport for the flight.
        /// </summary>
        public Airport? ArrivalAirport { get; set; }

        /// <summary>
        /// Gets or sets the flight number.
        /// </summary>
        [MaxLength(1000)] public string? FlightNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the confirmation number for the flight.
        /// </summary>
        [MaxLength(1000)] public string? ConfirmationNumber { get; set; } = string.Empty;

        #endregion

        #region Departure Movements

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

        #region Relationships

        /// <summary>
        /// Gets or sets the ID of the associated vacation.
        /// </summary>
        [ForeignKey(nameof(Vacation))] public string? VacationId { get; set; }

        /// <summary>
        /// Navigation property to the associated vacation.
        /// </summary>
        public Vacation? Vacation { get; set; }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates a new instance of the <see cref="Flight"/> class that is a copy of the current instance.
        /// </summary>
        /// <remarks>The cloned <see cref="Flight"/> object will have the same properties as the original,
        /// except for the <c>Id</c>, which will be newly generated to ensure uniqueness.</remarks>
        /// <returns>A new <see cref="Flight"/> object that is a copy of this instance, with a unique identifier.</returns>
        public Flight Clone()
        {
            return new Flight(this) { Id = Guid.NewGuid().ToString() };
        }

        #endregion
    }
}
