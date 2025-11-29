namespace ConectOne.Domain.Entities
{
    /// <summary>
    /// Represents an audit record that captures changes made to a database entity, including details about the
    /// operation, affected data, and user responsible.
    /// </summary>
    /// <remarks>Use this class to track and review modifications to entities for auditing or compliance
    /// purposes. Each instance contains information about the type of operation performed, the table affected, the user
    /// who made the change, and the before-and-after values of the modified data.</remarks>
    public class Audit : EntityBase<int>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user associated with this instance.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the type identifier associated with the object.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the database table associated with this entity.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the date and time value associated with this instance.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the previous value or state associated with the current entity or operation.
        /// </summary>
        public string? OldValues { get; set; }

        /// <summary>
        /// Gets or sets the new values associated with the current operation.
        /// </summary>
        public string? NewValues { get; set; }

        /// <summary>
        /// Gets or sets a comma-separated list of the database columns affected by the operation.
        /// </summary>
        public string? AffectedColumns { get; set; }

        /// <summary>
        /// Gets or sets the primary key used to authenticate requests to the service.
        /// </summary>
        public string PrimaryKey { get; set; }
    }
}
