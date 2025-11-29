namespace ConectOne.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for initializing or seeding data in an application.
    /// </summary>
    /// <remarks>Implement this interface to provide custom logic for populating initial data, such as default
    /// records or test data, typically during application startup or database migrations.</remarks>
    public interface IDataSeeder
    {
        /// <summary>
        /// Initializes the component and prepares it for use.
        /// </summary>
        void Initialize();
    }
}
