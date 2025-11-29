using Microsoft.EntityFrameworkCore;

namespace ConectOne.EntityFrameworkCore.Sql
{
    /// <summary>
    /// Provides a factory for creating instances of <see cref="GenericDbContext"/> with pre-configured options.
    /// </summary>
    /// <remarks>This factory is typically used in scenarios where dependency injection is not available,  or
    /// when creating multiple instances of <see cref="GenericDbContext"/> is required.  The factory ensures that the
    /// <see cref="GenericDbContext"/> instances are initialized  with the specified <see
    /// cref="DbContextOptions{GenericDbContext}"/>.</remarks>
    public class GenericDbContextFactory : IDbContextFactory<GenericDbContext>
    {
        private readonly DbContextOptions<GenericDbContext> _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericDbContextFactory"/> class with the specified options.
        /// </summary>
        /// <param name="options">The options to be used by the <see cref="GenericDbContext"/> instances created by this factory. Cannot be
        /// null.</param>
        public GenericDbContextFactory(DbContextOptions<GenericDbContext> options)
        {
            _options = options;
        }

        /// <summary>
        /// Creates and returns a new instance of <see cref="GenericDbContext"/> configured with the current options.
        /// </summary>
        /// <remarks>The returned <see cref="GenericDbContext"/> instance is configured using the options
        /// provided during the construction of this class. Ensure proper disposal of the returned context to release
        /// any resources it holds.</remarks>
        /// <returns>A new instance of <see cref="GenericDbContext"/> configured with the current options.</returns>
        public GenericDbContext CreateDbContext()
        {
            return new GenericDbContext(_options);
        }
    }
}