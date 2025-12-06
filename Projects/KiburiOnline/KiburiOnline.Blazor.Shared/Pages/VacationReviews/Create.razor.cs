// ReSharper disable MustUseReturnValue

using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using FeedbackModule.Application.ViewModels;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.VacationReviews
{
    /// <summary>
    /// Code-behind for the Create Review page in the Blazor application.
    /// Handles the creation of new reviews, including image uploads and form submission.
    /// </summary>
    public partial class Create
    {
        #region Injections
        
        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage navigation and URI manipulation in
        /// Blazor applications.
        /// </summary>
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the dialog service used to display modal dialogs and notifications.
        /// </summary>
        [Inject] private IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display notifications or messages.
        /// </summary>
        [Inject] private ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] private IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that a valid  implementation of <see cref="IVacationService"/> is registered in the dependency
        /// container.</remarks>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage and review vacation requests.
        /// </summary>
        [Inject] public IVacationReviewService VacationReviewService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for performing image processing operations.
        /// </summary>
        [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

        #endregion

        #region Private Fields

        /// <summary>
        /// The view model for the review being created.
        /// </summary>
        private ReviewViewModel _review = new();

        /// <summary>
        /// The URL of the profile image.
        /// </summary>
        private string _profileImageUrl = "images/NoImage.jpg";
        private string? _profileImageToUpload;

        /// <summary>
        /// The URL of the company logo image.
        /// </summary>
        private string _companyLogoUrl = "images/NoImage.jpg";
        private string? _companyLogoToUpload;

        /// <summary>
        /// The list of available vacation extension to select from.
        /// </summary>
        private List<VacationDto> _availableVacations = [];

        [Parameter] public string? VacationId { get; set; }

        #endregion

        #region Image Change Handlers

        /// <summary>
        /// Handles the change of the profile image.
        /// Updates the profile image URL and the review's profile image path.
        /// </summary>
        /// <param name="profileImage">The new profile image URL.</param>
        private void ProfileImageChanged(MudCropperResponse profileImage)
        {
            _profileImageUrl = profileImage.Base64String;
            _profileImageToUpload = _profileImageUrl;
        }

        /// <summary>
        /// Handles the change of the company logo image.
        /// Updates the company logo URL and the review's company logo path.
        /// </summary>
        /// <param name="logo">The new company logo URL.</param>
        private void CompanyLogoChanged(MudCropperResponse logo)
        {
            _companyLogoUrl = logo.Base64String;
            _companyLogoToUpload = _companyLogoUrl;
        }

        #endregion

        #region Create Review

        /// <summary>
        /// Creates a new review by sending the review data to the server.
        /// Displays a confirmation dialog and navigates based on the user's choice.
        /// </summary>
        private async Task CreateAsync()
        {
            var creationResult = await VacationReviewService.UpdateVacationReviewAsync(_review.ToDto());
            if (!creationResult.Succeeded) SnackBar.AddErrors(creationResult.Messages);

            if (!string.IsNullOrEmpty(_profileImageToUpload))
            {
                var request = new Base64ImageUploadRequest()
                {
                    Name = $"{_review.Name} Profile Image",
                    ImageType = UploadType.Profile,
                    Base64String = _profileImageToUpload
                };
                var imageUploadResult = await ImageProcessingService.UploadImage(request);
                if (imageUploadResult.Succeeded)
                {
                    var additionResult = await VacationReviewService.AddImage(new AddEntityImageRequest() { EntityId = _review.Id, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }

            if (!string.IsNullOrEmpty(_companyLogoToUpload))
            {
                var request = new Base64ImageUploadRequest()
                {
                    Name = $"{_review.Name} Cover Image",
                    ImageType = UploadType.Logo,
                    Base64String = _companyLogoToUpload
                };
                var imageUploadResult = await ImageProcessingService.UploadImage(request);
                if (imageUploadResult.Succeeded)
                {
                    var additionResult = await VacationReviewService.AddImage(new AddEntityImageRequest() { EntityId = _review.Id
                        , ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }

            SnackBar.Add($"Action Successful. Review was successfully created.", Severity.Success);
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Would you like to add another review?" },
                { x => x.ButtonText, "Yes" },
                { x => x.CancelButtonText, "No" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;
            if (result!.Canceled)
            {
                NavigationManager.NavigateTo($"/reviews");
            }
            else
            {
                _review = new ReviewViewModel() { EntityId = _review.EntityId};
                _profileImageUrl = "images/NoImage.jpg";
                _companyLogoUrl = "images/NoImage.jpg";
                StateHasChanged();
            }
        }

        #endregion

        #region Cancel Creation

        /// <summary>
        /// Cancels the review creation and navigates back to the golf courses page.
        /// </summary>
        private void CancelAsync()
        {
            NavigationManager.NavigateTo("/golfcourses");
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the component and sets parameters on first render.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first render of the component.</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var vacationsResult = await VacationService.AllVacationsAsync(new VacationPageParameters());
                if (vacationsResult.Succeeded)
                    _availableVacations = vacationsResult.Data.ToList();

                _profileImageUrl = !_review.Images.Any(c => c.ImageType == UploadType.Profile)  ? "images/NoImage.jpg" : $"{Configuration["ApiConfiguration:BaseApiAddress"]}\\{_review.Images.FirstOrDefault(c => c.ImageType == UploadType.Profile).RelativePath.TrimStart('/')}";
                _companyLogoUrl = !_review.Images.Any(c => c.ImageType == UploadType.Logo) ? "images/NoImage.jpg" : $"{Configuration["ApiConfiguration:BaseApiAddress"]}\\{_review.Images.FirstOrDefault(c => c.ImageType == UploadType.Logo).RelativePath.TrimStart('/')}";


                StateHasChanged();
            }
        }

        #endregion
    }
}
