using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using LinqKit;
using MessagingModule.Domain.Entities;
using MessagingModule.Domain.RequestFeatures;
using NeuralTech.Base.Enums;

namespace MessagingModule.Domain.Specifications;

/// <summary>
/// Specification for retrieving unread notifications for a specific receiver with optional message type filtering.
/// </summary>
public sealed class UnreadNotificationsSpecification : Specification<Notification>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnreadNotificationsSpecification"/> class.
    /// </summary>
    /// <param name="parameters">Filtering parameters.</param>
    public UnreadNotificationsSpecification(NotificationPageParameters parameters)
    {
        var predicate = PredicateBuilder.New<Notification>(n => n.ReceiverId == parameters.ReceiverId && n.OpenedDate == null);

        if (parameters.MessageType != null)
        {
            predicate = parameters.MessageType == MessageType.ActivityGroup
                ? predicate.And(n => n.MessageType == MessageType.ActivityCategory || n.MessageType == MessageType.ActivityGroup)
                : predicate.And(n => n.MessageType == parameters.MessageType);
        }

        Criteria = predicate;
    }
}
