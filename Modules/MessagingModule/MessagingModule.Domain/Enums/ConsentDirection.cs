using System.ComponentModel;

namespace MessagingModule.Domain.Enums
{
    /// <summary>
    /// Specifies the direction of consent in a messaging or permission context.
    /// </summary>
    public enum ConsentDirection
    {
        /// <summary>
        /// Consent applies in both directions (to and from).
        /// </summary>
        [Description("To And From")]
        ToAndFrom = 0,

        /// <summary>
        /// Consent is given to send messages or permissions to the recipient.
        /// </summary>
        [Description("To")]
        To = 1,

        /// <summary>
        /// Consent is given to receive messages or permissions from the sender.
        /// </summary>
        [Description("From")]
        From = 2
    }
}
