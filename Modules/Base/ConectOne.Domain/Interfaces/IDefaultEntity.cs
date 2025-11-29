namespace ConectOne.Domain.Interfaces
{
    /// <summary>
    /// Gets a value indicating whether the entity is marked as the default instance.
    /// </summary>
    public interface IDefaultEntity
    {
        /// <summary>
        /// Gets a value indicating whether the default option or behavior is enabled.
        /// </summary>
        bool Default { get; }
    }
}
