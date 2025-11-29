namespace ConectOne.Domain.Mailing.Entities
{
    /// <summary>
    /// Represents a single recipient of an email, storing both the name and the email address.
    /// </summary>
    public class EmailRecipient(string name, string email)
    {
        /// <summary>
        /// The name of the recipient (e.g., "John Doe").
        /// </summary>
        public string Name { get; set; } = name;

        /// <summary>
        /// The email address of the recipient (e.g., "john.doe@example.com").
        /// </summary>
        public string Email { get; set; } = email;
    }
}