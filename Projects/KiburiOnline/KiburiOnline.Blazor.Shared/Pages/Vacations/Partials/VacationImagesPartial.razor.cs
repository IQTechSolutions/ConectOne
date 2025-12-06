using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations.Partials
{
    public partial class VacationImagesPartial
    {
        #region Injected Services

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service is typically used to display brief, non-intrusive
        /// notifications to the user. Ensure that the service is properly injected before use.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that a valid  implementation of <see cref="IVacationService"/> is registered in the dependency
        /// container.</remarks>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for performing image processing operations.
        /// </summary>
        /// <remarks>This property is automatically injected and should be configured in the dependency
        /// injection container.</remarks>
        [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// Gets or sets the vacation details for the current context.
        /// </summary>
        [Parameter, EditorRequired] public VacationViewModel Vacation { get; set; } = new VacationViewModel();

        /// <summary>
        /// Gets or sets the callback that is invoked to navigate to the next tab.
        /// </summary>
        /// <remarks>This property is typically used to handle user interactions for advancing to the next
        /// tab in a tabbed interface. Ensure that the callback is properly assigned to handle the navigation
        /// logic.</remarks>
        [Parameter] public EventCallback NextTab { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when the "Previous Tab" action is triggered.
        /// </summary>
        /// <remarks>This callback is typically used to handle navigation to the previous tab in a tabbed
        /// interface. Assign a method or delegate to this property to define the behavior when the action
        /// occurs.</remarks>
        [Parameter] public EventCallback PreviousTab { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when the cancel action is triggered.
        /// </summary>
        /// <remarks>Use this property to specify the action to perform when a cancel event occurs, such
        /// as closing a dialog or reverting changes.</remarks>
        [Parameter] public EventCallback Cancel { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked to handle update events.
        /// </summary>
        /// <remarks>This callback is typically used to notify the component of changes or trigger updates
        /// in response to user actions or other events.</remarks>
        [Parameter] public EventCallback Update { get; set; }

        #endregion

        #region Image Methods

        /// <summary>
        /// Asynchronously retrieves all vacation slider images.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IEnumerable{T} of ImageDto
        /// objects representing the vacation slider images.  If the retrieval fails, an empty collection is returned.</returns>
        private async Task<IEnumerable<ImageDto>> LoadAllVacationImages()
        {
            var gallerImages = await ImageProcessingService.AllImagesAsync();
            if (gallerImages.Succeeded)
            {
                return gallerImages.Data;
            }

            return [];
        }

        #region Slider Images

        /// <summary>
        /// Sets the specified image as the main slider image.
        /// </summary>
        /// <remarks>This method updates the image collection by adding the specified image with a
        /// selector indicating it is the main image.</remarks>
        /// <param name="dto">The image data transfer object representing the image to be set as the main slider image.</param>
        /// <returns></returns>
        private async Task SetAsMainSliderImage(ImageDto dto)
        {
            if (NextTab.HasDelegate)
            {
                Vacation.Images.Add(dto with { Selector = "Main" });
                StateHasChanged();
            }
            else
            {
                if (Vacation.Images.Any(c => c.ImageType == UploadType.Slider && c.Selector == "Main"))
                {
                    var removalResult = await VacationService.RemoveVacationImage(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Slider && c.Selector == "Main").Id);
                    if (!removalResult.Succeeded)
                        SnackBar.AddErrors(removalResult.Messages);
                    else
                        Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Slider && c.Selector == "Main"));
                }

                var addResult = await VacationService.AddVacationImage(new AddEntityImageRequest(Vacation.VacationId, dto.Id, "Main"));
                if (addResult.Succeeded)
                {
                    Vacation.Images.Add(dto with { Selector = "Main" });
                    StateHasChanged();
                }
                else
                    SnackBar.AddErrors(addResult.Messages);
            }
        }

        /// <summary>
        /// Removes the main slider image from the collection of images.
        /// </summary>
        /// <remarks>This method removes the first image from the collection that is identified as a cover
        /// image with the selector "Main".</remarks>
        /// <param name="dto">The data transfer object containing image details. This parameter is not used in the current implementation.</param>
        private async Task RemoveMainSliderImage(ImageDto dto)
        {
            if (NextTab.HasDelegate)
            {
                Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover && c.Selector == "Main"));
            }
            else
            {
                var removalResult = await VacationService.RemoveVacationImage(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Slider && c.Selector == "Main").Id);
                if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                else Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Slider && c.Selector == "Main"));
            }
            
        }

        /// <summary>
        /// Sets the specified image as the summary slider image.
        /// </summary>
        /// <remarks>This method adds the provided image to the collection of images with a selector
        /// indicating it is for the summary slider.</remarks>
        /// <param name="dto">The image data transfer object to be set as the summary slider image. Must not be null.</param>
        /// <returns></returns>
        private async Task SetAsSummarySliderImage(ImageDto dto)
        {
            if (NextTab.HasDelegate)
            {
                Vacation.Images.Add(dto with { Selector = "Summary" });
            }
            else
            {
                if (Vacation.Images.Any(c => c.ImageType == UploadType.Slider && c.Selector == "Summary"))
                {
                    var removalResult = await VacationService.RemoveVacationImage(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Slider && c.Selector == "Main").Id);
                    if (!removalResult.Succeeded)
                        SnackBar.AddErrors(removalResult.Messages);
                    else
                        Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Slider && c.Selector == "Summary"));
                }

                var addResult = await VacationService.AddVacationImage(new AddEntityImageRequest(Vacation.VacationId, dto.Id, "Summary"));
                if (addResult.Succeeded)
                    Vacation.Images.Add(dto with { Selector = "Summary" });
                else
                    SnackBar.AddErrors(addResult.Messages);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private async Task RemoveSummarySliderImage(ImageDto dto)
        {
            if (NextTab.HasDelegate)
            {
                Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Slider && c.Selector == "Summary"));
            }
            else
            {
                var removalResult = await VacationService.RemoveVacationImage(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Slider && c.Selector == "Summary").Id);
                if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                else Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Slider && c.Selector == "Summary"));
            }
        }

        #endregion

        #region Banner Images

        /// <summary>
        /// Sets the specified image as the primary banner image for the vacation.
        /// </summary>
        /// <remarks>This method assigns the provided image a display order of 1, indicating its position
        /// as the primary banner image.</remarks>
        /// <param name="dto">The image data transfer object containing the image details to be set as the banner.</param>
        /// <returns></returns>
        private async Task SetAsBanner1Image(ImageDto dto)
        {
            if (NextTab.HasDelegate)
            {
                Vacation.Images.Add(dto with { Order = 1 });
            }
            else
            {
                if (Vacation.Images.Any(c => c.ImageType == UploadType.Banner && c.Order == 1))
                {
                    var removalResult = await VacationService.RemoveVacationImage(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 1).Id);
                    if (!removalResult.Succeeded)
                        SnackBar.AddErrors(removalResult.Messages);
                    else
                        Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 1));
                }

                var addResult = await VacationService.AddVacationImage(new AddEntityImageRequest(Vacation.VacationId, dto.Id, "", 1));
                if (addResult.Succeeded)
                    Vacation.Images.Add(dto with { Order = 1 });
                else
                    SnackBar.AddErrors(addResult.Messages);
            }
        }

        /// <summary>
        /// Removes the first banner image from the collection of images associated with the vacation.
        /// </summary>
        /// <remarks>This method specifically targets and removes the image with the type <see
        /// cref="UploadType.Banner"/> and an order of 1. If no such image exists, the method performs no
        /// action.</remarks>
        /// <param name="dto">The data transfer object containing image information. This parameter is not used in the current
        /// implementation.</param>
        /// <returns></returns>
        private async Task RemoveBanner1Image(ImageDto dto)
        {
            if (NextTab.HasDelegate)
            {
                Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 1));
            }
            else
            {
                var removalResult = await VacationService.RemoveVacationImage(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 1).Id);
                if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                else Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 1));
            }
        }

        /// <summary>
        /// Sets the specified image as the second banner image for the vacation.
        /// </summary>
        /// <remarks>The image is added to the collection of images with an order value of 2, indicating
        /// its position as the second banner.</remarks>
        /// <param name="dto">The image data transfer object containing the image details to be set as the second banner.</param>
        /// <returns></returns>
        private async Task SetAsBanner2Image(ImageDto dto)
        {
            if (NextTab.HasDelegate)
            {
                Vacation.Images.Add(dto with { Order = 2 });
            }
            else
            {
                if (Vacation.Images.Any(c => c.ImageType == UploadType.Banner && c.Order == 2))
                {
                    var removalResult = await VacationService.RemoveVacationImage(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 2).Id);
                    if (!removalResult.Succeeded)
                        SnackBar.AddErrors(removalResult.Messages);
                    else
                        Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 2));
                }

                var addResult = await VacationService.AddVacationImage(new AddEntityImageRequest(Vacation.VacationId, dto.Id, "", 2));
                if (addResult.Succeeded)
                    Vacation.Images.Add(dto with { Order = 2 });
                else
                    SnackBar.AddErrors(addResult.Messages);
            }
        }

        /// <summary>
        /// Removes the second banner image from the vacation's image collection.
        /// </summary>
        /// <remarks>This method searches for and removes the image with the type <see
        /// cref="UploadType.Banner"/> and order 2 from the vacation's image collection.</remarks>
        /// <param name="dto">The data transfer object containing image information. This parameter is not used in the current
        /// implementation.</param>
        /// <returns></returns>
        private async Task RemoveBanner2Image(ImageDto dto)
        {
            if (NextTab.HasDelegate)
            {
                Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 2));
            }
            else
            {
                var removalResult = await VacationService.RemoveVacationImage(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 2).Id);
                if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                else Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 2));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private async Task SetAsBanner3Image(ImageDto dto)
        {
            if (NextTab.HasDelegate)
            {
                Vacation.Images.Add(dto with { Order = 3 });
            }
            else
            {
                if (Vacation.Images.Any(c => c.ImageType == UploadType.Banner && c.Order == 3))
                {
                    var removalResult = await VacationService.RemoveVacationImage(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 3).Id);
                    if (!removalResult.Succeeded)
                        SnackBar.AddErrors(removalResult.Messages);
                    else
                        Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 3));
                }

                var addResult = await VacationService.AddVacationImage(new AddEntityImageRequest(Vacation.VacationId, dto.Id, "", 3));
                if (addResult.Succeeded)
                    Vacation.Images.Add(dto with { Order = 3 });
                else
                    SnackBar.AddErrors(addResult.Messages);
            }
        }

        /// <summary>
        /// Removes the third banner image from the vacation's image collection.
        /// </summary>
        /// <remarks>This method searches for an image with the type <see cref="UploadType.Banner"/> and
        /// an order of 3, and removes it from the collection.</remarks>
        /// <param name="dto">The data transfer object containing image information. This parameter is not used in the current
        /// implementation.</param>
        /// <returns></returns>
        private async Task RemoveBanner3Image(ImageDto dto)
        {
            if (NextTab.HasDelegate)
            {
                Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 3));
            }
            else
            {
                var removalResult = await VacationService.RemoveVacationImage(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 3).Id);
                if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                else Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 3));
            }
        }

        /// <summary>
        /// Sets the specified image as the fourth banner image in the collection.
        /// </summary>
        /// <remarks>This method adds the image to the collection with a predefined order of 4, indicating
        /// its position as the fourth banner image.</remarks>
        /// <param name="dto">The image data transfer object to be set as the fourth banner image. Must not be null.</param>
        /// <returns></returns>
        private async Task SetAsBanner4Image(ImageDto dto)
        {
            if (NextTab.HasDelegate)
            {
                Vacation.Images.Add(dto with { Order = 4 });
            }
            else
            {
                if (Vacation.Images.Any(c => c.ImageType == UploadType.Banner && c.Order == 4))
                {
                    var removalResult = await VacationService.RemoveVacationImage(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 4).Id);
                    if (!removalResult.Succeeded)
                        SnackBar.AddErrors(removalResult.Messages);
                    else
                        Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 4));
                }

                var addResult = await VacationService.AddVacationImage(new AddEntityImageRequest(Vacation.VacationId, dto.Id, "", 4));
                if (addResult.Succeeded)
                    Vacation.Images.Add(dto with { Order = 4 });
                else
                    SnackBar.AddErrors(addResult.Messages);
            }
        }

        /// <summary>
        /// Removes the image with the specified banner type and order from the vacation images.
        /// </summary>
        /// <remarks>This method specifically targets and removes the image with an <see
        /// cref="UploadType"/> of <see langword="Banner"/> and an order of 4 from the collection of images.</remarks>
        /// <param name="dto">The data transfer object containing image information. This parameter is not used in the current
        /// implementation.</param>
        /// <returns></returns>
        private async Task RemoveBanner4Image(ImageDto dto)
        {
            if (NextTab.HasDelegate)
            {
                Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 4));
            }
            else
            {
                var removalResult = await VacationService.RemoveVacationImage(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 4).Id);
                if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                else Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 4));
            }
        }

        #endregion

        #region Map Images

        /// <summary>
        /// Adds the specified image to the collection of images associated with the vacation.
        /// </summary>
        /// <param name="dto">The image data transfer object to be added to the vacation's image collection.</param>
        /// <returns></returns>
        private async Task SetAsMapImage(ImageDto dto)
        {
            if (NextTab.HasDelegate)
            {
                Vacation.Images.Add(dto);
            }
            else
            {
                if (Vacation.Images.Any(c => c.ImageType == UploadType.Map))
                {
                    var removalResult = await VacationService.RemoveVacationImage(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Map).Id);
                    if (!removalResult.Succeeded)
                        SnackBar.AddErrors(removalResult.Messages);
                    else
                        Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Map));
                }

                var addResult = await VacationService.AddVacationImage(new AddEntityImageRequest(Vacation.VacationId, dto.Id));
                if (addResult.Succeeded)
                    Vacation.Images.Add(dto);
                else
                    SnackBar.AddErrors(addResult.Messages);
            }
        }

        /// <summary>
        /// Removes the map image from the collection of images associated with the vacation.
        /// </summary>
        /// <remarks>This method removes the first image of type <see cref="UploadType.Map"/> from the
        /// vacation's image collection. If no such image exists, the collection remains unchanged.</remarks>
        /// <param name="dto">The data transfer object containing image information. This parameter is not used in the current
        /// implementation.</param>
        /// <returns></returns>
        private async Task RemoveMapImage(ImageDto dto)
        {
            if (NextTab.HasDelegate)
            {
                Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Map));
            }
            else
            {
                var removalResult = await VacationService.RemoveVacationImage(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Map).Id);
                if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                else Vacation.Images.Remove(Vacation.Images.FirstOrDefault(c => c.ImageType == UploadType.Map));
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Handles the event when the selected tab changes.
        /// </summary>
        private async Task OnNextTab()
        {
            await NextTab.InvokeAsync();
        }

        /// <summary>
        /// Invokes the action to navigate to the previous tab asynchronously.
        /// </summary>
        /// <remarks>This method triggers the <see cref="PreviousTab"/> delegate, allowing the caller to
        /// handle the logic for navigating to the previous tab. Ensure that the <see cref="PreviousTab"/> delegate is
        /// not null before invoking this method to avoid runtime exceptions.</remarks>
        /// <returns></returns>
        private async Task OnPreviousTab()
        {
            await PreviousTab.InvokeAsync();
        }

        /// <summary>
        /// Invokes the <see cref="Cancel"/> event asynchronously to signal a cancellation action.
        /// </summary>
        /// <remarks>This method triggers the cancellation logic by invoking the associated event
        /// callback. Ensure that the <see cref="Cancel"/> event is properly configured before calling this
        /// method.</remarks>
        /// <returns></returns>
        private async Task OnCancel()
        {
            await Cancel.InvokeAsync();
        }

        /// <summary>
        /// Invokes the <see cref="Update"/> event asynchronously.
        /// </summary>
        /// <remarks>This method triggers the <see cref="Update"/> event, allowing subscribers to handle
        /// the update operation. Ensure that any event handlers attached to <see cref="Update"/> are thread-safe and
        /// capable of handling asynchronous execution.</remarks>
        /// <returns></returns>
        private async Task OnUpdate()
        {
            await Update.InvokeAsync();
        }

        #endregion
    }
}
