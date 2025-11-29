using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GroupingModule.Application.ViewModels;

namespace ProductsModule.Application.ViewModels
{
	/// <summary>
	/// Represents the details of a product, including its name, description, identifiers, and associated metadata.
	/// </summary>
	/// <remarks>This view model is typically used to display or edit product information in a user interface.  It
	/// includes properties for product attributes such as name, display name, descriptions, and various identifiers,  as
	/// well as collections for related brands, categories, and suppliers.</remarks>
    public class ProductDetailsViewModel
	{
		/// <summary>
		/// Gets or sets the name of the product.
		/// </summary>
		[Required(ErrorMessage = "The name of the product is required")]
		[MaxLength(200, ErrorMessage = "Maximum length for the name is 50 characters.")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the display name of the entity.
		/// </summary>
		[DisplayName(@"Display Name")]
		[MaxLength(200, ErrorMessage = "Maximum length for the display name is 200 characters.")]
		public string DisplayName { get; set; }

		/// <summary>
		/// Gets or sets the URL of the cover image associated with the item.
		/// </summary>
		public string CoverImageUrl { get; set; }

		/// <summary>
		/// Gets or sets the short description of the product.
		/// </summary>
		[DisplayName(@"Short Description")]
		[Required(ErrorMessage = "The short description of the product is required")]
		[MaxLength(1000, ErrorMessage = "Maximum length for the short discription is 1000 characters.")]
		[DataType(DataType.MultilineText)]
		public string ShortDescription { get; set; }

		/// <summary>
		/// Gets or sets the serial number associated with the entity.
		/// </summary>
		/// <remarks>The serial number is used to uniquely identify the entity. Ensure that the value does not exceed
		/// the maximum length of 30 characters.</remarks>
		[DisplayName(@"Serial Number")]
		[MaxLength(30, ErrorMessage = "Maximum length for the serial nr is 30 characters.")]
		public string? SerialNr { get; set; }

		/// <summary>
		/// Gets or sets the barcode associated with the item.
		/// </summary>
		/// <remarks>The barcode is used to uniquely identify the item. Ensure that the value does not exceed 50
		/// characters to avoid validation errors.</remarks>
		[DisplayName(@"Bar-Code")]
		[MaxLength(50, ErrorMessage = "Maximum length for the bar-code is 50 characters.")]
		public string? Barcode { get; set; }

		/// <summary>
		/// Gets or sets the stock keeping unit (SKU) for the product.
		/// </summary>
		[MaxLength(30, ErrorMessage = "Maximum length for the SKU is 30 characters.")]
		public string? Sku { get; set; }

		/// <summary>
		/// Gets or sets the manufacturer's code associated with the product.
		/// </summary>
		public string? ManufacturersCode { get; set; }

		/// <summary>
		/// Gets or sets the Google category code associated with the item.
		/// </summary>
		public string? GoogleCategoryCode { get; set; }

		/// <summary>
		/// Gets or sets the Google product type associated with the item.
		/// </summary>
		public string? GoogleProductType { get; set; }

		/// <summary>
		/// Gets or sets the collection of categories to be displayed or processed.
		/// </summary>
		public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
	}
}
