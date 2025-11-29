using FilingModule.Domain.Entities;
using GroupingModule.Domain.Entities;

namespace BloggingModule.Domain.Entities
{
    /// <summary>
    /// Represents a blog post, including its content, metadata, and associated collections such as categories,
    /// comments, and documents.
    /// </summary>
    /// <remarks>The BlogPost class provides properties for managing the main content, author information,
    /// engagement metrics (such as likes and shares), and related entities. It inherits from FileCollection, enabling
    /// file-related operations specific to blog posts. Use the associated collections to access or modify categories,
    /// comments, documents, and views linked to the blog post.</remarks>
    public class BlogPost : FileCollection<BlogPost, string>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the title of the item.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets the rating value.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the number of likes associated with the item.
        /// </summary>
        public int Likes { get; set; }

        /// <summary>
        /// Gets or sets the number of social media shares associated with the content.
        /// </summary>
        public int SocialShares { get; set; }

        /// <summary>
        /// Gets or sets the content of the message.
        /// </summary>
        public string Content { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description text associated with the object.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is marked as featured.
        /// </summary>
        public bool Featured { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a reply should be sent.
        /// </summary>
        public bool Reply { get; set; }

        /// <summary>
        /// Gets or sets the tags associated with the entity.
        /// </summary>
        public string? Tags { get; set; }

        /// <summary>
        /// Gets or sets the name of the author associated with the content.
        /// </summary>
        public string? Author { get; set; }

        /// <summary>
        /// Gets a value indicating whether the current instance has an associated author.
        /// </summary>
        public bool HasAuthor => !string.IsNullOrEmpty(Author);

        /// <summary>
        /// Gets or sets the URL associated with the current object.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Url"/> property contains a non-empty, non-null string.
        /// </summary>
        public bool HasUrl => !string.IsNullOrEmpty(Url);

        /// <summary>
        /// Gets or sets the links to associated documents.
        /// </summary>
        public string? DocumentLinks { get; set; }

        #endregion

        #region Collections

        /// <summary>
        /// Gets or sets the collection of categories associated with the blog post.
        /// </summary>
        public virtual ICollection<EntityCategory<BlogPost>> Categories { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of comments associated with the blog post.
        /// </summary>
        public virtual ICollection<Comment<BlogPost>> Comments { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of document files associated with blog posts.
        /// </summary>
        public ICollection<EntityDocument<BlogPost, string>> Documents { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of views associated with the blog entry.
        /// </summary>
        public ICollection<BlogEntryView> Views { get; set; } = [];

        #endregion

        #region Overrides

        /// <summary>
        /// Returns a string representation of the current object.
        /// </summary>
        /// <returns>The value of the <see cref="Title"/> property.</returns>
        public override string ToString()
        {
            return Title;
        }

        #endregion
    }
}
