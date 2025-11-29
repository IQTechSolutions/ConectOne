namespace ConectOne.Domain.ResultWrappers
{
    /// <summary>
    /// Represents the base interface for operation results, including success status and messages.
    /// </summary>
    public interface IBaseResult
    {
        /// <summary>
        /// Gets or sets the messages associated with the result.
        /// </summary>
        List<string> Messages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the operation succeeded.
        /// </summary>
        bool Succeeded { get; set; }
    }

    /// <summary>
    /// Represents the base interface for operation results with a data payload, including success status and messages.
    /// </summary>
    /// <typeparam name="T">The type of the data payload.</typeparam>
    public interface IBaseResult<out T> : IBaseResult
    {
        /// <summary>
        /// Gets the data payload of the result.
        /// </summary>
        T Data { get; }
    }
}