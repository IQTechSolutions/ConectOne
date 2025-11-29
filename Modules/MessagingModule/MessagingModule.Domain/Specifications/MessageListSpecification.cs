using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using MessagingModule.Domain.Entities;
using MessagingModule.Domain.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace MessagingModule.Domain.Specifications
{
    /// <summary>
    /// Specification for filtering and optionally paginating <see cref="Message"/> entities.
    /// </summary>
    public class MessageListSpecification : Specification<Message>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageListSpecification"/> class.
        /// </summary>
        /// <param name="parameters">Pagination and filtering parameters.</param>
        /// <param name="receiverId">Optional receiver identifier.</param>
        /// <param name="senderId">Optional sender identifier.</param>
        /// <param name="includeDocuments">Whether to eagerly load documents.</param>
        /// <param name="applyPaging">Whether to apply paging to the query.</param>
        public MessageListSpecification(
            MessagePageParameters parameters,
            string? receiverId = null,
            string? senderId = null,
            bool includeDocuments = false,
            bool applyPaging = true)
        {
            var search = parameters.SearchText?.Trim().ToLower();

            Criteria = m =>
                (receiverId == null || m.ReceiverId == receiverId) &&
                (senderId == null || m.SenderId == senderId) &&
                (parameters.EntityId == null || m.EntityId == parameters.EntityId) &&
                (parameters.MessageType == null || m.MessageType == parameters.MessageType) &&
                m.Public == parameters.Public &&
                (parameters.StartDateFilter == null || (m.DeliveredTime != null && m.DeliveredTime.Value.Date > parameters.StartDateFilter.Value)) &&
                (parameters.EndDateFilter == null || (m.DeliveredTime != null && m.DeliveredTime.Value.Date < parameters.EndDateFilter.Value)) &&
                (string.IsNullOrEmpty(search) || (m.Description.ToLower().Contains(search) || m.Subject.ToLower().Contains(search)));

            OrderBy = q => q.OrderByDescending(m => m.CreatedOn);

            if (applyPaging)
            {
                Skip = (parameters.PageNr - 1) * parameters.PageSize;
                Take = parameters.PageSize;
            }

            if (includeDocuments)
            {
                AddInclude(m => m.Include(x => x.Documents));
            }
        }
    }
}
