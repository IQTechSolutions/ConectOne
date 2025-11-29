namespace MessagingModule.Domain.Enums
{
    /// <summary>
    /// Defines the types of consent that a parent may be required to give for a learner 
    /// to participate in certain events or activities.
    /// </summary>
    public enum ConsentTypes
    {
        /// <summary>
        /// Indicates that consent is required for providing transport to or from an event.
        /// </summary>
        Transport = 0,

        /// <summary>
        /// Indicates that consent is required for the learner’s attendance at an event.
        /// </summary>
        Attendance = 1,

        /// <summary>
        /// Indicates that no consent is required for the learner at an event.
        /// </summary>
        None = 2
    }
}
