using GroupingModule.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations;
using FilingModule.Domain.DataTransferObjects;

namespace GroupingModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for a category.
    /// This view model is used for binding category data in the UI.
    /// </summary>
    public class CategoryViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryViewModel"/> class.
        /// </summary>
        public CategoryViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryViewModel"/> class from a <see cref="CategoryDto"/>.
        /// </summary>
        /// <param name="category">The DTO containing category data.</param>
        public CategoryViewModel(CategoryDto category)
        {
            CategoryId = category.CategoryId;
            ParentCategoryId = category.ParentCategoryId;
            CoverImageUrl = category.CoverImageUrl;
            BannerImageUrl = category.BannerImageUrl;
            IconImageUrl = category.IconImageUrl;
            Name = category.Name;
            Active = category.Active;
            Featured = category.Featured;
            Description = category.Description;
            DisplayAsMenuItem = category.DisplayCategoryInMainMenu;
            DisplayAsSliderItem = category.DisplayAsSliderItem;
            SubCategoryCount = category.SubCategoryCount;
            EntityCount = category.EntityCount;
            Images = category.Images ?? [];
        }

        #endregion

        /// <summary>
        /// Gets or sets the ID of the category.
        /// </summary>
        public string? CategoryId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the ID of the parent category.
        /// </summary>
        public string? ParentCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        [Required(ErrorMessage = "Name is a required field")]public string Name { get; set; } 

        /// <summary>
        /// Gets or sets the description of the category.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the category is active.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the category is featured.
        /// </summary>
        public bool Featured { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the category should be displayed as a menu item.
        /// </summary>
        public bool DisplayAsMenuItem { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the category should be displayed as a slider item.
        /// </summary>
        public bool DisplayAsSliderItem { get; set; }

        /// <summary>
        /// Gets or sets the web tags associated with the current entity.
        /// </summary>
        public string? WebTags { get; set; }

        /// <summary>
        /// Gets or sets the slogan of the category.
        /// </summary>
        public string? Slogan { get; set; }

        /// <summary>
        /// Gets or sets the sub-slogan of the category.
        /// </summary>
        public string? SubSlogan { get; set; }

        /// <summary>
        /// Gets or sets the URL of the cover image.
        /// </summary>
        public string? CoverImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL of the banner image.
        /// </summary>
        public string? BannerImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL of the icon image.
        /// </summary>
        public string? IconImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the count of subcategories in the category.
        /// </summary>
        public int SubCategoryCount { get; set; }

        /// <summary>
        /// Gets a value indicating whether the category has subcategories.
        /// </summary>
        public bool HasSubCategories => SubCategoryCount > 0;

        /// <summary>
        /// Gets or sets the count of entities in the category.
        /// </summary>
        public int EntityCount { get; set; }

        /// <summary>
        /// Gets a value indicating whether the category has entities.
        /// </summary>
        public bool HasEntities => EntityCount > 0;

        /// <summary>
        /// Gets or sets a value indicating whether the category is selected.
        /// </summary>
        public bool SelectedItem { get; set; } = false;

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public ICollection<ImageDto> Images { get; set; } = [];

        #region Methods

        /// <summary>
        /// Converts the current category entity to a <see cref="CategoryDto"/> object.
        /// </summary>
        /// <remarks>The returned <see cref="CategoryDto"/> contains all relevant properties of the
        /// category, including identifiers, images, metadata, and display settings.</remarks>
        /// <returns>A <see cref="CategoryDto"/> object representing the current category.</returns>
        public CategoryDto ToDto()
        {
            return new CategoryDto()
            {
                CategoryId = CategoryId,
                CoverImageUrl = CoverImageUrl,
                BannerImageUrl = BannerImageUrl,
                IconImageUrl = IconImageUrl,
                Name = Name,
                Description = Description,
                ParentCategoryId = ParentCategoryId,
                EntityCount = EntityCount,
                SubCategoryCount = SubCategoryCount,
                Active = Active,
                Featured = Featured,
                DisplayCategoryInMainMenu = DisplayAsMenuItem,
                DisplayAsSliderItem = DisplayAsSliderItem,
                Images = Images,
                WebTags = WebTags
            };
        }

        #endregion
    }
}
