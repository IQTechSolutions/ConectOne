using BloggingModule.Domain.DataTransferObjects;
using BloggingModule.Domain.Entities;
using FilingModule.Domain.DataTransferObjects;

namespace BloggingModule.Application.ViewModels
{
    /// <summary>
    /// Represents the view model for a blog post, encapsulating various details
    /// such as title, content, images, documents, and metadata.
    /// </summary>
    public class BlogPostViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogPostViewModel"/> class.
        /// </summary>
        public BlogPostViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogPostViewModel"/> class from a <see cref="BlogPostDto"/>.
        /// </summary>
        /// <param name="dto">The data transfer object containing blog post information.</param>
        public BlogPostViewModel(BlogPostDto dto)
        {
            BlogPostId = string.IsNullOrEmpty(dto.BlogPostId) ? Guid.NewGuid().ToString() : dto.BlogPostId;
            CoverImageUrl = dto.CoverImageUrl;
            Images = dto.Images;
            Title = dto.Title;
            Featured = dto.Featured;
            Description = dto.Description;
            Content = dto.Content;
            Created = dto.CreatedTime.Value;
            Documents = dto.Documents;
            DocumentLinks = dto.DocumentLinks;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ID of the blog post.
        /// </summary>
        public string BlogPostId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the URL of the cover image.
        /// </summary>
        public string? CoverImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the collection of image URLs associated with the blog post.
        /// </summary>
        public IEnumerable<string> Images { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the title of the blog post.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the blog post is featured.
        /// </summary>
        public bool Featured { get; set; }

        /// <summary>
        /// Gets or sets the description of the blog post.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the content of the blog post.
        /// </summary>
        public string Content { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the category to which the blog post belongs.
        /// </summary>
        public string? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the number of likes for the blog post.
        /// </summary>
        public int Likes { get; set; } = 0;

        /// <summary>
        /// Gets or sets the creation date of the blog post.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the ID of the previous blog post.
        /// </summary>
        public string? PreviousId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the next blog post.
        /// </summary>
        public string? NextId { get; set; }

        /// <summary>
        /// Gets or sets the collection of document files associated with the blog post.
        /// </summary>
        public List<DocumentDto> Documents { get; set; } = new List<DocumentDto>();

        /// <summary>
        /// Gets or sets the collection of document links associated with the blog post.
        /// </summary>
        public List<string> DocumentLinks { get; set; } = new List<string>();

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current <see cref="BlogPost"/> instance to a <see cref="BlogPostDto"/>.
        /// </summary>
        /// <returns>A <see cref="BlogPostDto"/> containing the data from the current <see cref="BlogPost"/> instance.</returns>
        public BlogPostDto ToDto()
        {
            return new BlogPostDto
            {
                BlogPostId = BlogPostId,
                Title = Title,
                Featured = Featured,
                Description = Description,
                Content = Content,
                CoverImageUrl = CoverImageUrl,
                CategoryId = CategoryId,
                DocumentLinks = DocumentLinks,
                Documents = Documents
            };
        }

        #endregion
    }
}
