using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Interfaces;
using FilingModule.Domain.Entities;

namespace GroupingModule.Domain.Entities;

/// <summary>
///     Represents a hierarchical <b>category</b> node that can be attached to any entity
///     implementing <see cref="T"/>.  Categories support rich metadata
///     (name, description, display flags, slogans, web‑tags) and can be organised into a
///     parent/child tree allowing unlimited levels of nesting.
/// </summary>
/// <typeparam name="T">
///     The entity type that can be linked to the category via <see cref="IAuditableEntity{TId}"/>.
///     The type parameter is constrained to <see cref="IAuditableEntity"/> to ensure
///     that every associated entity exposes audit information such as creation and update timestamps.
/// </typeparam>
public class Category<T> : FileCollection<Category<T>, string> where T : IAuditableEntity<string>
{
    #region Public Properties

    /// <summary>
    ///     Human‑readable name for the category.  Required; limited to 100 characters.
    /// </summary>
    [Required(ErrorMessage = "Category Name is Required"), MaxLength(100, ErrorMessage = "Maximum length for the Name is 100 characters.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Optional long‑form description rendered in back‑office UIs and public storefronts.
    ///     Supports multi‑line input; capped at 5000 characters.
    /// </summary>
    [DataType(DataType.MultilineText), MaxLength(5000, ErrorMessage = "Maximum length for the Description is 5000 characters.")]
    public string? Description { get; set; }

    /// <summary>
    ///     Indicates whether the category is active (<c>true</c>) or soft‑disabled (<c>false</c>).
    ///     Deactivated categories are excluded from most front‑end listings but remain in the database
    ///     to preserve historical references.
    /// </summary>
    public bool Active { get; set; } = true;

    /// <summary>
    ///     When <c>true</c> the category is promoted in UI widgets such as home‑page carousels or
    ///     search‑suggest dropdowns.
    /// </summary>
    public bool Featured { get; set; } = true;

    /// <summary>
    ///     Comma‑separated list of SEO‑optimised tags ("slugified" keywords) used to surface the category
    ///     in web searches and tag clouds.
    /// </summary>
    [Display(Name = "Tags")] public string? WebTags { get; set; }

    /// <summary>
    ///     Controls whether the category should appear as a top‑level item in the main navigation menu.
    /// </summary>
    [Display(Name = "Display in Main Menu")] public bool DisplayCategoryInMainManu { get; set; }

    /// <summary>
    ///     Signals that the category may be showcased inside a hero/slider component on the landing page.
    /// </summary>
    [Display(Name = "Display As Slider Item")] public bool DisplayAsSliderItem { get; set; }

    /// <summary>
    ///     Short marketing line displayed alongside the category name (e.g. "Everything you need for summer").
    /// </summary>
    public string? Slogan { get; set; }

    /// <summary>
    ///     Secondary slogan or tagline used where double‑line marketing copy is supported.
    /// </summary>
    [Display(Name = "Sub‑Slogan")] public string? SubSlogan { get; set; }

    #endregion

    #region Parent Category

    /// <summary>
    ///     Foreign‑key reference to the parent category.  <see langword="null"/> for root‑level categories.
    /// </summary>
    [ForeignKey(nameof(ParentCategory))] public string? ParentCategoryId { get; set; }

    /// <summary>
    ///     Navigation property pointing to the parent <see cref="Category{T}"/> instance.
    /// </summary>
    public Category<T>? ParentCategory { get; set; }

    #endregion

    #region Collection Navigation Properties

    /// <summary>
    ///     Child categories nested under the current node.  The collection is initialised eagerly to
    ///     simplify consumption in LINQ queries and Razor components.
    /// </summary>
    [InverseProperty(nameof(ParentCategory))] public virtual ICollection<Category<T>> SubCategories { get; set; } = new List<Category<T>>();

    /// <summary>
    ///     Links to concrete entities of type <typeparamref name="T"/> that belong to this category.
    ///     The join entity <see cref="EntityCategory{T}"/> allows many‑to‑many relationships.
    /// </summary>
    public virtual ICollection<EntityCategory<T>> EntityCollection { get; set; } = new List<EntityCategory<T>>();

    #endregion

    #region Read‑only Convenience Properties

    /// <summary>
    ///     <see langword="true"/> if the category has at least one child category; otherwise <see langword="false"/>.
    /// </summary>
    public bool HasSubCategories => SubCategories is { Count: > 0 };

    #endregion
}
