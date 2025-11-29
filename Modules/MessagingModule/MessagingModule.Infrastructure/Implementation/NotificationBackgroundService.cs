using MessagingModule.Infrastructure.Queues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MessagingModule.Infrastructure.Implementation
{
    /// <summary>
    /// A hosted background service that continuously checks an <see cref="EmailQueue"/> for messages 
    /// to send. Once it finds messages, it uses an <see cref="IEmailSender"/> to deliver them.
    /// The loop continues until the application stops or the service is canceled.
    /// </summary>
    public class NotificationBackgroundService : BackgroundService
    {
        // Used for logging service messages and errors.
        private readonly ILogger<NotificationBackgroundService> _logger;

        // ServiceScopeFactory allows creating a new scope to resolve scoped/transient services 
        // (e.g., EmailQueue, IEmailSender) independently in each loop iteration.
        private readonly IServiceScopeFactory _scopeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationBackgroundService"/>.
        /// </summary>
        /// <param name="logger">
        /// Logger for writing informational and error messages to the application logs.
        /// </param>
        /// <param name="scopeFactory">
        /// Factory used to create service scopes for retrieving scoped services.
        /// </param>
        public NotificationBackgroundService(ILogger<NotificationBackgroundService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        /// <summary>
        /// The main execution method for the background service. It runs continuously until 
        /// cancellation is requested, checking for email messages in the queue and sending them.
        /// </summary>
        /// <param name="stoppingToken">
        /// Token that signals when the service should stop processing and shut down.
        /// </param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NotificationBackgroundService is starting.");

            // Continue until the application stops or this service is canceled.
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Create a new DI scope so each iteration can retrieve fresh, scoped services.
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        // Retrieve the queued emails and the sending service from DI
                        var emailQueue = scope.ServiceProvider.GetRequiredService<NotificationQueue>();
                  //      var emailService = scope.ServiceProvider.GetRequiredService<IPushNotificationService>();



                        // Check if there's an email message waiting to be sent
                        if (emailQueue.TryDequeue(out var emailMessage))
                        {
                            _logger.LogInformation("Sending email {To}", emailMessage.Notification.Title);

                            // Attempt to send the email asynchronously
                        //    var result = await emailService.SendNotifications(emailMessage.Users, emailMessage.Notification);

                            // If sending failed, log the error messages
                            //if (!result.Succeeded)
                            //    _logger.LogError("Failed to send email: {Errors}", string.Join(',', result.Messages));
                        }
                        else
                        {
                            // No pending emails, so wait briefly before checking again.
                            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log any unexpected errors that occur during execution.
                    _logger.LogError(ex, "Error occurred executing NotificationBackgroundService.");
                }
            }

            _logger.LogInformation("NotificationBackgroundService is stopping.");
        }
    }
}