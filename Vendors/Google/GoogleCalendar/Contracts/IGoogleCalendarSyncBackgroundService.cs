namespace GoogleCalendar.Contracts
{
    /// <summary>
    /// Defines a background service responsible for periodically synchronizing data 
    /// with Google Calendar. Implementations should handle connecting to the calendar 
    /// and processing any updates or scheduling tasks as needed.
    /// </summary>
    public interface IGoogleCalendarSyncBackgroundService
    {
        /// <summary>
        /// Executes the core synchronization work repeatedly until a cancellation 
        /// is requested via the <paramref name="stoppingToken"/>.
        /// </summary>
        /// <param name="stoppingToken">
        /// Token used to signal that the service should stop processing 
        /// and gracefully exit the loop.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation 
        /// of the sync process.
        /// </returns>
        Task DoWork(CancellationToken stoppingToken);
    }
}
