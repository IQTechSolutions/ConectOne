using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using IdentityModule.Domain.Entities;

namespace BloggingModule.Domain.Entities
{
    /// <summary>
    /// Represents a view model for a blog entry, including associated user and blog post information.
    /// </summary>
    /// <remarks>This class serves as a data transfer object for displaying blog entry details,  including the
    /// user who created the entry and the associated blog post.</remarks>
    public class BlogEntryView : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the unique identifier for the associated user.
        /// </summary>
        [ForeignKey(nameof(User))] public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user information associated with the current context.
        /// </summary>
        public UserInfo User { get; set; }
        
        /// <summary>
        /// Gets or sets the unique identifier for the associated blog post.
        /// </summary>
        [ForeignKey(nameof(BlogPost))] public string BlogPostId { get; set; }

        /// <summary>
        /// Gets or sets the blog post associated with the current context.
        /// </summary>
        public BlogPost BlogPost { get; set; }
    }
}
