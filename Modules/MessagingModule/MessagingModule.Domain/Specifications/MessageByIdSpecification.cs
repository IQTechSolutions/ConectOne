using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using MessagingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MessagingModule.Domain.Specifications
{
    /// <summary>
    /// Specification for retrieving a single message by identifier.
    /// </summary>
    public class MessageByIdSpecification : Specification<Message>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageByIdSpecification"/> class.
        /// </summary>
        /// <param name="id">Message identifier.</param>
        /// <param name="includeDocuments">Whether to eagerly load documents.</param>
        public MessageByIdSpecification(string id, bool includeDocuments = false)
        {
            Criteria = m => m.Id == id;

            if (includeDocuments)
                AddInclude(m => m.Include(x => x.Documents));
        }
    }
}
