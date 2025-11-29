using ConectOne.Domain.Interfaces;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using GroupingModule.Domain.Entities;

namespace GroupingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for a category.
    /// This DTO is used to transfer category data between different layers of the application.
    /// </summary>
    public record CategoryDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryDto"/> class.
        /// </summary>
        public CategoryDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryDto"/> class with the specified parameters.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <param name="parentCategoryId">The ID of the parent category.</param>
        /// <param name="name">The name of the category.</param>
        /// <param name="description">The description of the category.</param>
        /// <param name="slogan">The slogan of the category.</param>
        /// <param name="subSlogan">The sub-slogan of the category.</param>
        /// <param name="coverImageUrl">The URL of the cover image.</param>
        /// <param name="bannerImageUrl">The URL of the banner image.</param>
        /// <param name="iconImageUrl">The URL of the icon image.</param>
        /// <param name="entityCount">The count of entities in the category.</param>
        /// <param name="subcategoryCount">The count of subcategories in the category.</param>
        /// <param name="active">Indicates whether the category is active.</param>
        /// <param name="featured">Indicates whether the category is featured.</param>
        /// <param name="displayCategoryInMainMenu">Indicates whether the category should be displayed in the main menu.</param>
        /// <param name="displayCategoryInSlider">Indicates whether the category should be displayed as a slider item.</param>
        public CategoryDto(string? categoryId, string? parentCategoryId, string name, string? description, string slogan, string subSlogan, string coverImageUrl, string bannerImageUrl, string iconImageUrl, int entityCount, int subcategoryCount, bool active, bool featured, bool displayCategoryInMainMenu, bool displayCategoryInSlider = false)
        {
            CategoryId = string.IsNullOrEmpty(categoryId) ? Guid.NewGuid().ToString() : categoryId;
            CoverImageUrl = coverImageUrl;
            BannerImageUrl = bannerImageUrl;
            IconImageUrl = iconImageUrl;
            Name = name;
            Description = description;
            ParentCategoryId = parentCategoryId;
            EntityCount = entityCount;
            SubCategoryCount = subcategoryCount;
            Active = active;
            Featured = featured;
            DisplayCategoryInMainMenu = displayCategoryInMainMenu;
            DisplayAsSliderItem = displayCategoryInSlider;
        }

        #endregion

        /// <summary>
        /// Gets the ID of the category.
        /// </summary>
        public string CategoryId { get; init; } = null!;

        /// <summary>
        /// Gets the ID of the parent category.
        /// </summary>
        public string? ParentCategoryId { get; init; }

        /// <summary>
        /// Gets the URL of the cover image.
        /// </summary>
        public string? CoverImageUrl { get; init; }

        /// <summary>
        /// Gets the URL of the banner image.
        /// </summary>
        public string? BannerImageUrl { get; init; }

        /// <summary>
        /// Gets the URL of the icon image.
        /// </summary>
        public string? IconImageUrl { get; init; }

        /// <summary>
        /// Gets the name of the category.
        /// </summary>
        public string Name { get; init; } = null!;

        /// <summary>
        /// Gets the description of the category.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets or sets the web tags associated with the current entity.
        /// </summary>
        public string? WebTags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the category should be displayed in the main menu.
        /// </summary>
        public bool DisplayCategoryInMainMenu { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the category should be displayed as a slider item.
        /// </summary>
        public bool DisplayAsSliderItem { get; set; }

        public bool ShowSubCategories { get; set; } = false;

        /// <summary>
        /// Gets a value indicating whether the category is active.
        /// </summary>
        public bool Active { get; init; } = true;

        /// <summary>
        /// Gets a value indicating whether the category is featured.
        /// </summary>
        public bool Featured { get; init; }

        /// <summary>
        /// Gets or sets the count of entities in the category.
        /// </summary>
        public int EntityCount { get; set; }

        /// <summary>
        /// Gets a value indicating whether the category has entities.
        /// </summary>
        public bool HasEntities => EntityCount > 0;

        /// <summary>
        /// Gets the count of subcategories in the category.
        /// </summary>
        public int SubCategoryCount { get; init; }

        /// <summary>
        /// Gets a value indicating whether the category has subcategories.
        /// </summary>
        public bool HasSubCategories => SubCategoryCount > 0;

        /// <summary>
        /// Gets or sets the count of unread notifications for the category.
        /// </summary>
        public int UnreadNotifications { get; set; }

        /// <summary>
        /// Gets or sets the count of not viewed items for the category.
        /// </summary>
        public int NotViewedCount { get; set; }

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public ICollection<ImageDto> Images { get; set; } = [];

        /// <summary>
        /// Converts a <see cref="Category{T}"/> entity to a <see cref="CategoryDto"/>.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="category">The category entity to convert.</param>
        /// <returns>A <see cref="CategoryDto"/> representing the category entity.</returns>
        public static CategoryDto ToCategoryDto<T>(Category<T> category) where T : IAuditableEntity<string>
        {
            if (category is not null)
            {
                return new CategoryDto(category.Id, category.ParentCategoryId, category.Name, category.Description, category.Slogan, category.SubSlogan,
                    category.Images.Where(c => c.Image.ImageType == UploadType.Cover).FirstOrDefault()?.Image?.RelativePath,
                    category.Images.Where(c => c.Image.ImageType == UploadType.Banner).FirstOrDefault()?.Image.RelativePath,
                    category.Images.Where(c => c.Image.ImageType == UploadType.Icon).FirstOrDefault()?.Image.RelativePath,
                    category.EntityCollection.Count(), category.SubCategories.Count(), category.Active, category.Featured, category.DisplayCategoryInMainManu, category.DisplayAsSliderItem)
                {
                    Images = category?.Images == null ? [] : category.Images.Select(c => ImageDto.ToDto(c)).ToList()
                };
            }

            return new CategoryDto();
        }

        /// <summary>
        ///     Creates a fully populated <see cref="Category{T}"/> domain entity from the current DTO/view‑model instance.
        ///     Only mutable, top‑level scalar properties are copied; navigation collections such as <c>SubCategories</c>
        ///     and <c>EntityCollection</c> are initialised by the <see cref="Category{T}"/> constructor itself to ensure
        ///     consistent defaults.
        /// </summary>
        /// <typeparam name="T">
        ///     The target entity type that will later be associated with the category via an
        ///     <see cref="EntityCategory{T}"/> join.  Constrained to <see cref="IAuditableEntity{String}"/> so that audit
        ///     information is always available.
        /// </typeparam>
        /// <returns>
        ///     A new <see cref="Category{T}"/> instance with its key and metadata fields populated from the source object.
        ///     The method does not perform any validation; callers should invoke the relevant validation pipeline—or rely
        ///     on data‑annotations—before persisting the returned entity.
        /// </returns>
        public Category<T> ToCategory<T>() where T : IAuditableEntity<string>
        {
            return new Category<T>
            {
                Id = this.CategoryId,
                Name = this.Name,
                Description = this.Description,
                WebTags = this.WebTags,
                Active = this.Active,
                Featured = this.Featured,
                DisplayAsSliderItem = this.DisplayAsSliderItem,
                DisplayCategoryInMainManu = this.DisplayCategoryInMainMenu,
                ParentCategoryId = this.ParentCategoryId
            };
        }
    }
}
