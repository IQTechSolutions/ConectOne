// ReSharper disable MustUseReturnValue

using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.ResultWrappers;
using FeedbackModule.Application.ViewModels;
using FeedbackModule.Domain.DataTransferObjects;
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
    /// Code-behind for the Update Review page in the Blazor application.
    /// Handles the updating of existing reviews, including image uploads and form submission.
    /// </summary>
    public partial class Update
    {
        #region Injected Services

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage navigation and URI manipulation in
        /// Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display notifications or messages.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage and review vacation requests.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection. Ensure that a valid
        /// implementation of <see cref="IVacationReviewService"/> is provided before using this property.</remarks>
        [Inject] public IVacationReviewService VacationReviewService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that a valid  implementation of <see cref="IVacationService"/> is registered in the service
        /// container.</remarks>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for performing image processing operations.
        /// </summary>
        [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// The ID of the review to be updated.
        /// </summary>
        [Parameter] public string ReviewId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for a vacation.
        /// </summary>
        [Parameter] public string? VacationId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the vacation extension.
        /// </summary>
        [Parameter] public string? VacationExtensionId { get; set; } = null!;

        #endregion

        #region Private Fields

        /// <summary>
        /// The view model for the review being updated.
        /// </summary>
        private ReviewViewModel _review = new();

        /// <summary>
        /// The URL of the profile image.
        /// </summary>
        private string _profileImageUrl = "_content/Feedback.Blazor/images/NoImage.jpg";
        private string? _profileImageToUpload;

        /// <summary>
        /// The URL of the company logo image.
        /// </summary>
        private string _companyLogoUrl = "_content/Feedback.Blazor/images/NoImage.jpg";
        private string? _companyLogoToUpload;

        /// <summary>
        /// The list of available vacations to select from.
        /// </summary>
        private List<VacationDto> _availableVacations = [];

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
        /// <param name="companyLogo">The new company logo URL.</param>
        private void CompanyLogoChanged(MudCropperResponse companyLogo)
        {
            _companyLogoUrl = companyLogo.Base64String;
            _companyLogoToUpload = _companyLogoUrl;
        }

        #endregion

        #region Update Review

        /// <summary>
        /// Updates the review by sending the updated review data to the server.
        /// Displays a success message upon successful update.
        /// </summary>
        private async Task UpdateAsync()
        {
            var updateResult = await VacationReviewService.UpdateVacationReviewAsync(_review.ToDto());
            updateResult.ProcessResponseForDisplay(SnackBar, async () =>
            {
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
                        if (_review.Images.Any(c => c.ImageType == UploadType.Profile))
                        {
                            var removalResult = await VacationReviewService.RemoveImage(_review.Images.FirstOrDefault(c => c.ImageType == UploadType.Profile).Id);
                            if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                        }

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
                        if (_review.Images.Any(c => c.ImageType == UploadType.Logo))
                        {
                            var removalResult = await VacationReviewService.RemoveImage(_review.Images.FirstOrDefault(c => c.ImageType == UploadType.Logo).Id);
                            if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                        }

                        var additionResult = await VacationReviewService.AddImage(new AddEntityImageRequest()
                        {
                            EntityId = _review.Id,
                            ImageId = imageUploadResult.Data.Id
                        });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }

                SnackBar.Add($"Action Successful. Review was successfully updated.", Severity.Success);
            });
        }

        #endregion

        #region Cancel Update

        /// <summary>
        /// Cancels the review update and navigates back to the reviews page.
        /// </summary>
        private void CancelAsync()
        {
            if (!string.IsNullOrEmpty(VacationId))
            {
                NavigationManager.NavigateTo($"/vacations/update/{VacationId}");
            }
            else if (!string.IsNullOrEmpty(VacationExtensionId))
            {
                NavigationManager.NavigateTo($"/vacations/extensions/update/{VacationExtensionId}");
            }
            else
            {
                NavigationManager.NavigateTo($"/vacations");
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the component by loading the review data and setting up metadata and image URLs.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first render of the component.</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                IBaseResult<ReviewDto> result = await VacationReviewService.VacationReviewAsync(ReviewId); 
                _review = new ReviewViewModel(result.Data);

                var vacationsResult = await VacationService.AllVacationsAsync(new VacationPageParameters());
                if (vacationsResult.Succeeded)
                    _availableVacations = vacationsResult.Data.ToList();

                _profileImageUrl = !_review.Images.Any(c => c.ImageType == UploadType.Profile) ? "_content/FeedbackModule.Blazor/images/NoImage.jpg" : $"{Configuration["ApiConfiguration:BaseApiAddress"]}\\{_review.Images.FirstOrDefault(c => c.ImageType == UploadType.Profile).RelativePath.TrimStart('/')}";
                _companyLogoUrl = !_review.Images.Any(c => c.ImageType == UploadType.Logo) ? "_content/FeedbackModule.Blazor/images/NoImage.jpg" : $"{Configuration["ApiConfiguration:BaseApiAddress"]}\\{_review.Images.FirstOrDefault(c => c.ImageType == UploadType.Logo).RelativePath.TrimStart('/')}";

                StateHasChanged();
            }
        }

        #endregion
    }
}
