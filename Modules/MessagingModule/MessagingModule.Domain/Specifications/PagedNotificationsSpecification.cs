using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using LinqKit;
using MessagingModule.Domain.Entities;
using MessagingModule.Domain.RequestFeatures;
using NeuralTech.Base.Enums;

namespace MessagingModule.Domain.Specifications;

/// <summary>
/// Specification for retrieving paginated notifications with optional filters.
/// </summary>
public sealed class PagedNotificationsSpecification : Specification<Notification>
{
    /// <summary>
    /// Creates a new instance of the <see cref="PagedNotificationsSpecification"/> class.
    /// </summary>
    /// <param name="parameters">Paging and filtering parameters.</param>
    /// <param name="applyPaging">Whether to apply pagination to the query.</param>
    public PagedNotificationsSpecification(NotificationPageParameters parameters)
    {
        var predicate = PredicateBuilder.New<Notification>(true);

        if (!string.IsNullOrWhiteSpace(parameters.ReceiverId))
            predicate = predicate.And(n => n.ReceiverId == parameters.ReceiverId);

        if (parameters.MessageType != null)
        {
            predicate = parameters.MessageType == MessageType.ActivityGroup
                ? predicate.And(n => n.MessageType == MessageType.ActivityCategory || n.MessageType == MessageType.ActivityGroup)
                : predicate.And(n => n.MessageType == parameters.MessageType);
        }

        if (!string.IsNullOrWhiteSpace(parameters.EntityId))
            predicate = predicate.And(n => n.EntityId == parameters.EntityId);

        Criteria = predicate;
    }
}
