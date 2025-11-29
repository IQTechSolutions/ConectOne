using BloggingModule.Domain.Entities;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;

namespace BloggingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for a blog post.
    /// This DTO is used to transfer blog post data between different layers of the application.
    /// </summary>
    [Serializable]
    public record BlogPostDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogPostDto"/> class.
        /// </summary>
        public BlogPostDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogPostDto"/> class from a <see cref="BlogPost"/> entity.
        /// </summary>
        /// <param name="blogPost">The blog post entity.</param>
        public BlogPostDto(BlogPost blogPost)
        {
            BlogPostId = blogPost.Id;
            Title = blogPost.Title;
            Featured = blogPost.Featured;
            Description = blogPost.Description;
            Content = blogPost.Content;
            CoverImageUrl = blogPost.Images.FirstOrDefault(c => c.Image.ImageType == UploadType.Cover)?.Image.RelativePath;
            DocumentLinks = !string.IsNullOrEmpty(blogPost.DocumentLinks) ? blogPost.DocumentLinks!.Split(";").ToList() : new List<string>();
            Documents = blogPost.Documents.Select(c => new DocumentDto() { FileName = c.Document.FileName, Size = c.Document.Size, Url = c.Document.RelativePath }).ToList();
            CreatedTime = blogPost.CreatedOn;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ID of the blog post.
        /// </summary>
        public string? BlogPostId { get; init; }

        /// <summary>
        /// Gets or sets the URL of the cover image.
        /// </summary>
        public string? CoverImageUrl { get; init; }

        /// <summary>
        /// Gets or sets the title of the blog post.
        /// </summary>
        public string Title { get; init; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the blog post is featured.
        /// </summary>
        public bool Featured { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether the blog post has been read.
        /// </summary>
        public bool Read { get; set; }

        /// <summary>
        /// Gets or sets the description of the blog post.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets or sets the content of the blog post.
        /// </summary>
        public string Content { get; init; } = null!;

        /// <summary>
        /// Gets or sets the ID of the category to which the blog post belongs.
        /// </summary>
        public string? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the creation time of the blog post.
        /// </summary>
        public DateTime? CreatedTime { get; init; }

        /// <summary>
        /// Gets or sets the collection of image URLs associated with the blog post.
        /// </summary>
        public IEnumerable<string> Images { get; set; } = new List<string>();

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
        /// Converts this DTO back into a <see cref="BlogPost"/> entity.
        /// </summary>
        /// <returns>A <see cref="BlogPost"/> entity representing the blog post.</returns>
        public BlogPost ToBlogPost()
        {
            var blogPost = new BlogPost()
            {
                Id = BlogPostId!,
                Title = Title,
                Featured = Featured,
                Description = Description,
                Content = Content,
                DocumentLinks = string.Join(";", DocumentLinks),
                CreatedOn = DateTime.Now
            };

            return blogPost;
        }

        #endregion
    }
}
