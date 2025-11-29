namespace PayFast.ApiTypes
{
    /// <summary>
    /// Represents a container for data of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of the data contained in the response.</typeparam>
    public class GenericData<T>
    {
        /// <summary>
        /// Gets or sets the response data of the operation.
        /// </summary>
        public T response { get; set; }
    }
}
