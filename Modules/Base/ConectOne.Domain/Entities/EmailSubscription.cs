namespace ConectOne.Domain.Entities
{
    /// <summary>
    /// Represents an email subscription with subscriber's first name, last name, and email address.
    /// </summary>
    public class EmailSubscription : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the email address of the subscriber.
        /// </summary>
        public string EmailAddress { get; set; } = null!;

        /// <summary>
        /// Gets or sets the first name of the subscriber.
        /// </summary>
        public string? FirstName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the last name of the subscriber.
        /// </summary>
        public string? LastName { get; set; } = null!;
    }
}
