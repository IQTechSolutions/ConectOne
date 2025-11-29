using FilingModule.Domain.Enums;
using ProductsModule.Domain.Entities;

namespace ProductsModule.Domain.DataTransferObjects;

/// <summary>
/// Represents detailed information about a product, including identifiers, descriptions, and associated images.
/// </summary>
/// <remarks>This data transfer object (DTO) is designed to encapsulate product details for use in various
/// application layers,  such as APIs or services. It includes key product attributes like SKU, barcode, and
/// descriptions, as well as  image URLs for display purposes.</remarks>
public record ProductDetailsDto
{
	#region Constructors

	/// <summary>
	/// Initializes a new instance of the <see cref="ProductDetailsDto"/> class.
	/// </summary>
	public ProductDetailsDto() { }

	/// <summary>
	/// Initializes a new instance of the <see cref="ProductDetailsDto"/> class using the specified <see cref="Product"/>
	/// object.
	/// </summary>
	/// <remarks>This constructor extracts relevant product information, including identifiers, descriptions, and
	/// images,  and maps them to the corresponding properties of the <see cref="ProductDetailsDto"/> instance.  The first
	/// image of type <see cref="UploadType.Cover"/> is used as the cover image,  and all images of type <see
	/// cref="UploadType.Image"/> are added to the list of other image URLs.</remarks>
	/// <param name="product">The <see cref="Product"/> object containing the details to populate the DTO.  This parameter cannot be <see
	/// langword="null"/>.</param>
	public ProductDetailsDto(Product product)
	{
		ProductId = product.Id;
		Sku = product.SKU;
		Name = product.Name;
		DisplayName = product.DisplayName;
        ShortDescription = product.ShortDescription;
        Description = product.Description;

        if (product.Images.Any(c => c.Image.ImageType.Equals(UploadType.Cover)))
        {
            CoverImageUrl = product.Images.FirstOrDefault(c => c.Image.ImageType.Equals(UploadType.Cover)).Image.RelativePath;
        }
		OtherImageUrls = new List<string>();    

		if(product.Images.Any(c => c.Image.ImageType == UploadType.Image))
		{
		   foreach(var image in product.Images.Where(c => c.Image.ImageType == UploadType.Image))
			{
				OtherImageUrls.Add(image.Image.RelativePath);
			}
		}
		
		
	}

	#endregion

	/// <summary>
	/// Gets the unique identifier for the product.
	/// </summary>
	public string ProductId { get; init; }

	/// <summary>
	/// Gets the stock-keeping unit (SKU) identifier for the product.
	/// </summary>
	public string Sku { get; init; }

	/// <summary>
	/// Gets the name associated with the object.
	/// </summary>
	public string Name { get; init; }

	/// <summary>
	/// Gets the display name associated with the object.
	/// </summary>
	public string DisplayName { get; init; }

	/// <summary>
	/// Gets the URL of the cover image associated with the item.
	/// </summary>
	public string CoverImageUrl { get; init; }	

	/// <summary>
	/// Gets a brief description or summary of the object.
	/// </summary>
	public string ShortDescription { get; init; }

	/// <summary>
	/// Gets the description associated with the object.
	/// </summary>
	public string Description { get; init; }

	/// <summary>
	/// Gets a collection of URLs pointing to additional images associated with the item.
	/// </summary>
    public ICollection<string> OtherImageUrls { get; init; }
}
