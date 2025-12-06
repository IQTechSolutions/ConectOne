using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Constants;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace Accomodation.Blazor.Components.GolfCourses;

/// <summary>
/// Represents a Blazor component for managing and displaying a table of golf courses.
/// </summary>
/// <remarks>The <see cref="GolfCourseTable"/> component provides functionality for displaying, searching,
/// filtering,  and managing golf course data in a tabular format. It integrates with server-side data sources to fetch 
/// paginated and sorted data, and supports user authorization checks for various operations such as creating,  editing,
/// and deleting golf courses. This component is designed to work with Blazor's dependency injection  and cascading
/// parameters for authentication and authorization.</remarks>
public partial class GolfCourseTable
{
    private bool _dense = false;
    private bool _striped = true;
    private bool _bordered = false;
    private bool _showTable = false;

    private bool _canSearchGolfCourses;
    private bool _canCreateGolfCourse;
    private bool _canEditGolfCourse;
    private bool _canDeleteGolfCourse;

    private MudTable<GolfCourseViewModel> _table = null!;
    private RequestParameters _pageParameters = new();

    /// <summary>
    /// Gets or sets the task that provides the current authentication state.
    /// </summary>
    /// <remarks>This property is typically used in Blazor components to access the user's
    /// authentication state. The <see cref="AuthenticationState"/> contains information about the user's identity
    /// and authentication status.</remarks>
    [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for handling authorization operations.
    /// </summary>
    [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to manage and retrieve golf course data.
    /// </summary>
    [Inject] public IGolfCoursesService GolfCoursesService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display dialogs to the user.
    /// </summary>
    /// <remarks>The injected <see cref="IDialogService"/> instance enables showing modal dialogs, alerts, or
    /// prompts within the component. Assign this property to provide dialog functionality, typically via dependency
    /// injection.</remarks>
    [Inject] public IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display transient messages to the user, such as notifications or alerts.
    /// </summary>
    /// <remarks>Inject this property to provide feedback to users through snackbars. The service
    /// implementation determines how messages are displayed and managed.</remarks>
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the application configuration settings.
    /// </summary>
    [Inject] public IConfiguration Configuration { get; set; } = null!;

    /// <summary>
    /// Clears all applied filters and reloads the server data for the table.
    /// </summary>
    /// <remarks>This method resets the request parameters to their default state and triggers a reload of the
    /// table's data from the server. Use this method to ensure the table displays unfiltered data.</remarks>
    private void ClearFiltersAsync()
    {
	    _pageParameters = new RequestParameters();
	    _table.ReloadServerData();
    }

    /// <summary>
    /// Initiates a search operation by reloading server data for the associated table.
    /// </summary>
    /// <remarks>This method asynchronously reloads data from the server to refresh the table's content.
    /// Ensure that the table is properly configured before invoking this method.</remarks>
    /// <returns></returns>
    private async Task Search()
    {
	    await _table.ReloadServerData();
    }

    /// <summary>
    /// Retrieves a paginated list of golf courses based on the specified table state.
    /// </summary>
    /// <remarks>This method fetches golf course data from a paginated data source, applying the specified
    /// sorting and pagination parameters. The <paramref name="state"/> parameter determines the page number, page size,
    /// and sorting order. The returned <see cref="TableData{T}"/> object includes the total number of items and a
    /// collection of golf courses mapped to view models.</remarks>
    /// <param name="state">The table state containing pagination and sorting information, such as page number, page size, and sort
    /// direction.</param>
    /// <param name="token">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="TableData{T}"/> object containing the total number of items and a collection of golf course view
    /// models.</returns>
	public async Task<TableData<GolfCourseViewModel>> GetGolfCoursesAsync(TableState state, CancellationToken token)
    {
        try
        {
            _pageParameters.PageNr = state.Page+1;
            _pageParameters.PageSize = state.PageSize;

            _pageParameters.OrderBy = state.SortDirection switch
            {
                SortDirection.Ascending => $"{state.SortLabel} desc",
                SortDirection.Descending => $"{state.SortLabel} asc",
                _ => null
            };

            var pagingResponse = await GolfCoursesService.PagedGolfCoursesAsync(_pageParameters);

            return new TableData<GolfCourseViewModel>() { TotalItems = pagingResponse.Data.Count(), Items = pagingResponse.Data.Select(c => new GolfCourseViewModel(c)) };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
			
    }
    
    /// <summary>
    /// Deletes a golf course from the application after user confirmation.
    /// </summary>
    /// <remarks>This method displays a confirmation dialog to the user before proceeding with the deletion.
    /// If the user cancels the confirmation dialog, the deletion is not performed. If the deletion fails, error
    /// messages are displayed to the user.</remarks>
    /// <param name="golfCourseId">The unique identifier of the golf course to be deleted. Cannot be null or empty.</param>
    /// <returns></returns>
    private async Task Delete(string golfCourseId)
    {
        var parameters = new DialogParameters<ConformtaionModal>
        {
            { x => x.ContentText, "Are you sure you want to remove this golf course from this application?" },
            { x => x.ButtonText, "Yes" },
            { x => x.Color, Color.Success }
        };

        var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var deletionResult = await GolfCoursesService.RemoveImage(golfCourseId);
            if(!deletionResult.Succeeded) Snackbar.AddErrors(deletionResult.Messages);
				
            await _table.ReloadServerData();
        }
    }

    /// <summary>
    /// Asynchronously initializes the component and evaluates the user's authorization for various golf course-related
    /// permissions.
    /// </summary>
    /// <remarks>This method checks the current user's authorization for specific actions, such as searching,
    /// creating, editing, and deleting golf courses. The results are stored internally and may affect the component's
    /// behavior or UI.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;
        var currentUser = authState.User;

        _canSearchGolfCourses = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.GolfCourse.Search)).Succeeded;
        _canCreateGolfCourse = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.GolfCourse.Create)).Succeeded;
        _canEditGolfCourse = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.GolfCourse.Edit)).Succeeded;
        _canDeleteGolfCourse = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.GolfCourse.Delete)).Succeeded;

        await base.OnInitializedAsync();
    }
}