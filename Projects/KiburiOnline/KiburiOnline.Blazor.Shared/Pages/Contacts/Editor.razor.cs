using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Contacts
{
    /// <summary>
    /// The Editor component is responsible for editing guide details.
    /// </summary>
    public partial class Editor
    {
        private string _imageSource = "_content/Accomodation.Blazor/images/profileImage128x128.png";
        private string? _coverImageToUpload;
        private ContactViewModel _contact = new();
        private List<BreadcrumbItem> _items =
        [
            new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new("Guides", href: "/guides", icon: Icons.Material.Filled.People),
            new("Update Guide", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
        ];

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage contact information.
        /// </summary>
        [Inject] public IContactService ContactService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the image processing service used to perform image-related operations.
        /// </summary>
        [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        /// <remarks>The <see cref="Configuration"/> property is typically used to retrieve application
        /// settings and configuration values, such as connection strings, API keys, or other environment-specific
        /// settings. Ensure that the property is properly initialized before accessing its values.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage navigation and URI manipulation in
        /// Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the teacher to be edited.
        /// </summary>
        [Parameter] public string ContactId { get; set; } = null!;

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
        /// Updates the teacher details.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateAsync()
        {
            var result = await ContactService.EditAsync(_contact.ToDto());
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
                        if (_contact.Images.Any(c => c.ImageType == UploadType.Cover))
                        {
                            var removalResult = await ContactService.RemoveImage(_contact.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                            if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                        }
                        var additionResult = await ContactService.AddImage(new AddEntityImageRequest() { EntityId = _contact.ContactId, ImageId = imageUploadResult.Data.Id });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }
                SnackBar.AddSuccess("Team member successfully updated");
                NavigationManager.NavigateTo("contacts");
            });
        }

        /// <summary>
        /// Cancels the edit operation and navigates back to the teachers list.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task CancelAsync()
        {
            NavigationManager.NavigateTo("contacts");
        }

        /// <summary>
        /// Called after the component has rendered. If this is the first render,
        /// it fetches the teacher details from the server and updates the contact view model.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first render of the component.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                // Fetch the teacher details from the server
                var result = await ContactService.GetByIdAsync(ContactId);
                if (result.Succeeded)
                {
                    // Update the contact view model with the fetched data
                    _contact = new ContactViewModel(result.Data);
                    _imageSource = $"{Configuration["ApiConfiguration:BaseApiAddress"]}/{_contact.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover)?.RelativePath}";
                }
                // Display any error messages using the SnackBar component
                SnackBar.AddErrors(result.Messages);
                StateHasChanged();
            }
        }
    }
}
