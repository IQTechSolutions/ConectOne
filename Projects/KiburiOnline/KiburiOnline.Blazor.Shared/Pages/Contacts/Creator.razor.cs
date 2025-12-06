using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Contacts
{
    /// <summary>
    /// Represents the page for creating a new contact.
    /// This component provides a form to input contact details and save them to the server.
    /// </summary>
    public partial class Creator
    {
        private List<BreadcrumbItem> _items =
        [
            new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new("Contacts", href: "/contacts", icon: Icons.Material.Filled.People),
            new("Create New Contact", href: null, disabled: true, icon: Icons.Material.Filled.Create)
        ];
        private string _imageSource = "_content/Accomodation.Blazor/images/profileImage128x128.png";
        private ContactViewModel _contact = new();
        private string? _coverImageToUpload;

        /// <summary>
        /// Gets or sets the type of contact represented by this property.
        /// </summary>
        [Parameter] public int ContactType { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for managing navigation and URI manipulation
        /// in Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display notifications or messages.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing contact-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that the  dependency is properly configured in the service container before accessing this
        /// property.</remarks>
        [Inject] public IContactService ContactService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for performing image processing operations.
        /// </summary>
        [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

        /// <summary>
        /// Handles the event when the cover image is changed.
        /// </summary>
        /// <param name="coverImage">The new cover image URL.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
            _coverImageToUpload = _imageSource;
        }

        /// <summary>
        /// Creates a new contact by sending the contact details to the server.
        /// </summary>
        private async Task CreateAsync()
        {
            var result = await ContactService.AddAsync(_contact.ToDto());
            result.ProcessResponseForDisplay(SnackBar, async () =>
            {
                if (!string.IsNullOrEmpty(_coverImageToUpload))
                {
                    var request = new Base64ImageUploadRequest()
                    {
                        Name = $"{_contact.Name} {_contact.Surname} Cover Image",
                        ImageType = UploadType.Cover,
                        Base64String = _coverImageToUpload
                    };
                    var imageUploadResult = await ImageProcessingService.UploadImage(request);
                    if (imageUploadResult.Succeeded)
                    {
                        var additionResult = await ContactService.AddImage(new AddEntityImageRequest() { EntityId = _contact.ContactId, ImageId = imageUploadResult.Data.Id });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }

                SnackBar.Add("contact created successfully", Severity.Success);
                NavigationManager.NavigateTo("contacts");
            });
        }

        /// <summary>
        /// Cancels the creation process and navigates back to the contacts list.
        /// </summary>
        private void Cancel()
        {
            NavigationManager.NavigateTo("/contacts");
        }

        /// <summary>
        /// Called when the component is initialized. Sets the contact type for the new contact.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}
