using MessagingModule.Domain.Entities;

namespace MessagingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a chat group, containing its unique identifier and name.
    /// </summary>
    /// <remarks>This DTO is used to transfer chat group data between different layers of the application. It
    /// provides a lightweight representation of a chat group, suitable for serialization and communication.</remarks>
    public record ChatGroupDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatGroupDto"/> class.
        /// </summary>
        public ChatGroupDto()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatGroupDto"/> class using the specified <see
        /// cref="chatGroup"/>.
        /// </summary>
        /// <param name="chatGroup">The chat group entity used to populate the DTO. Cannot be <see langword="null"/>.</param>
        public ChatGroupDto(ChatGroup chatGroup)
        {
            Id = chatGroup.Id;
            Name = chatGroup.Name;
        }

        /// <summary>
        /// Gets the unique identifier for the entity.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Gets the name associated with the current instance.
        /// </summary>
        public string Name { get; init; }
    };
}
