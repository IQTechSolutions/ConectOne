using System.ComponentModel;

namespace AccomodationModule.Domain.Enums
{
    /// <summary>
    /// Represents the types of payments available in the system.
    /// </summary>
    /// <remarks>This enumeration defines various payment types that can be used in different contexts, such
    /// as booking a seat, making a deposit, or handling ad hoc payments. Each value corresponds to a specific payment
    /// category.</remarks>
    public enum PaymentType
    {
        /// <summary>
        /// Represents the action of booking a seat for an event or service.
        /// </summary>
        /// <remarks>This enumeration value is typically used to indicate a user action or state related
        /// to reserving a seat. Ensure that the associated booking system is properly configured before using this
        /// value.</remarks>
        [Description("Reservation Fee")] ReservationFee,

        /// <summary>
        /// Represents a deposit transaction in the system.
        /// </summary>
        /// <remarks>This enumeration value is typically used to identify deposit-related operations or
        /// transactions.</remarks>
        [Description("Deposit")] Deposit,

        /// <summary>
        /// Represents an ad hoc operation or configuration.
        /// </summary>
        /// <remarks>This enumeration value is typically used to indicate a non-standard or temporary
        /// setup.</remarks>
        [Description("Ad Hoc")] AdHoc,

        /// <summary>
        /// Gets or sets the balance value.
        /// </summary>
        [Description("Balance")] Balance
    }
}
