using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using MessagingModule.Domain.Entities;

namespace MessagingModule.Domain.Specifications;

/// <summary>
/// Specification for retrieving a notification by its identifier.
/// </summary>
public sealed class NotificationByIdSpecification : Specification<Notification>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationByIdSpecification"/> class.
    /// </summary>
    /// <param name="notificationId">The notification identifier.</param>
    public NotificationByIdSpecification(string notificationId)
    {
        Criteria = n => n.Id == notificationId;
    }
}
