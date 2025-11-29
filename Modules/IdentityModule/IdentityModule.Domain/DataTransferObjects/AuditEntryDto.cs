namespace IdentityModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents an audit log entry that captures changes made to a database entity.
    /// </summary>
    /// <remarks>This data transfer object (DTO) is used to encapsulate information about a specific change
    /// made to a database entity, including details such as the user who made the change, the type of operation
    /// performed, the affected table, and the old and new values of the modified data. It is typically used for
    /// tracking and auditing purposes.</remarks>
    public record AuditEntryDto
    {
        /// <summary>
        /// Gets the unique identifier for the entity.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Gets the unique identifier for the user.
        /// </summary>
        public string UserId { get; init; }

        /// <summary>
        /// Gets the type of the entity represented by this instance.
        /// </summary>
        public string Type { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public string TableName { get; init; }

        /// <summary>
        /// Gets the date and time value associated with this instance.
        /// </summary>
        public DateTime DateTime { get; init; }

        /// <summary>
        /// Gets the previous values associated with the object before any modifications.
        /// </summary>
        public string OldValues { get; init; }

        /// <summary>
        /// Gets the new values associated with the operation.
        /// </summary>
        public string NewValues { get; init; }

        /// <summary>
        /// Gets the names of the columns affected by the operation.
        /// </summary>
        public string AffectedColumns { get; init; }

        /// <summary>
        /// Gets the primary key associated with the entity.
        /// </summary>
        public string PrimaryKey { get; init; }
    }
}
