using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using FeedbackModule.Domain.DataTransferObjects;
using FeedbackModule.Domain.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.VacationReviews;

/// <summary>
/// Represents a modal dialog for reviewing vacation details.
/// </summary>
/// <remarks>This component is used to display and manage the review process for vacation requests. It interacts
/// with a dialog instance to handle user actions such as saving or canceling the review.</remarks>
public partial class VacationReviewModal
{
    private List<ReviewDto> Reviews { get; set; } = [];
    private readonly Func<ReviewDto?, string> _reviewConverter = p => $"{p?.Name} - {p?.CompanyName}";

    /// <summary>
    /// Gets or sets the current instance of the dialog.
    /// </summary>
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique identifier for a vacation.
    /// </summary>
    [Parameter] public string? VacationId { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the vacation extension.
    /// </summary>
    [Parameter] public string? VacationExtensionId { get; set; }

    /// <summary>
    /// Gets or sets the review details for a vacation.
    /// </summary>
    [Parameter] public VacationReviewViewModel? Review { get; set; } = new VacationReviewViewModel();

    /// <summary>
    /// Gets or sets the service used to manage and review vacation requests.
    /// </summary>
    [Inject] public IVacationReviewService VacationReviewService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="ISnackbar"/> service used to display snack bar notifications.
    /// </summary>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Asynchronously saves the current review and closes the dialog.
    /// </summary>
    /// <remarks>This method closes the dialog using the current review as the result. Ensure that the review
    /// is in a valid state before calling this method.</remarks>
    private void SaveAsync()
    {
        MudDialog.Close(Review);
    }

    /// <summary>
    /// Cancels the current dialog operation.
    /// </summary>
    /// <remarks>This method terminates the dialog and triggers any cancellation logic associated with it. It
    /// should be called when the dialog needs to be closed without completing the intended operation.</remarks>
    public void Cancel()
    {
        MudDialog.Cancel();
    }

    /// <summary>
    /// Asynchronously initializes the component's state by setting the review entity ID and retrieving reviews.
    /// </summary>
    /// <remarks>This method sets the <see cref="Review{TEntity}.EntityId"/> based on the provided vacation identifiers
    /// and fetches a list of reviews from the data provider. If the retrieval is successful, the reviews are stored;
    /// otherwise, error messages are displayed using the snack bar.</remarks>
    protected override async Task OnInitializedAsync()
    {
        Review.EntityId = string.IsNullOrEmpty(VacationId) ? VacationExtensionId : VacationId;

        var response = await VacationReviewService.VacationReviewListAsync(VacationId);
        if (response.Succeeded)
        {
            Reviews = response.Data.ToList();
        }
        else
        {
            SnackBar.AddErrors(response.Messages);
        }
        await base.OnInitializedAsync();
    }
}
