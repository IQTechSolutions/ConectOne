using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Interfaces.Discipline;

namespace SchoolsModule.Blazor.Components.Discipline;

/// <summary>
/// Serves as the base component for managing and displaying a learner's disciplinary history, including incidents and
/// actions. Provides functionality to load, display, and save disciplinary data.
/// </summary>
/// <remarks>This component interacts with an HTTP provider to retrieve and update disciplinary data for a
/// specific learner. It also uses a snackbar service to display messages and notifications to the user.</remarks>
public partial class LearnerDisciplinaryHistoryBase : ComponentBase
{
    protected List<DisciplinaryIncidentViewModel> _incidents = new();
    protected List<DisciplinaryActionViewModel> _actions = new();
    protected Dictionary<string, string> _actionLookup = new();
    protected bool _loaded;
    protected DisciplinaryIncidentViewModel _newIncident = new() { Date = DateTime.Today };

    /// <summary>
    /// Gets or sets the HTTP provider used for making HTTP requests.
    /// </summary>
    [Inject] public IDisciplinaryActionService DisciplinaryActionService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to manage disciplinary incidents within the application.
    /// </summary>
    [Inject] public IDisciplinaryIncidentService DisciplinaryIncidentService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework. Ensure that the
    /// service is properly configured before use.</remarks>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique identifier for the learner.
    /// </summary>
    [Parameter] public string LearnerId { get; set; } = null!;
    
    /// <summary>
    /// Asynchronously initializes the component and performs any required setup logic.
    /// </summary>
    /// <remarks>This method is called by the Blazor framework during the component's initialization phase. 
    /// It ensures that any asynchronous setup tasks are completed before the component is rendered.</remarks>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Asynchronously loads disciplinary actions and incidents for the current learner,  and updates the internal state
    /// with the retrieved data.
    /// </summary>
    /// <remarks>This method retrieves disciplinary actions and incidents from the data provider  and maps
    /// them to their respective view models. The loaded data is stored in  internal collections for further use. Any
    /// error messages encountered during the  retrieval process are added to the snack bar for user
    /// notification.</remarks>
    /// <returns></returns>
    private async Task LoadAsync()
    {
        var actions = await DisciplinaryActionService.AllActionsAsync();
        if (actions.Succeeded)
        {
            _actions = actions.Data.Select(a => new DisciplinaryActionViewModel(a)).ToList();
            _actionLookup = _actions.Where(a => a.DisciplinaryActionId != null)
                                     .ToDictionary(a => a.DisciplinaryActionId!, a => a.Name);
        }

        var incidents = await DisciplinaryIncidentService.IncidentsByLearnerAsync(LearnerId);
        if (incidents.Succeeded)
        {
            _incidents = incidents.Data.Select(i => new DisciplinaryIncidentViewModel(i)).ToList();
        }

        SnackBar.AddErrors(actions.Messages);
        SnackBar.AddErrors(incidents.Messages);

        _loaded = true;
    }

    /// <summary>
    /// Saves the current disciplinary incident asynchronously.
    /// </summary>
    /// <remarks>This method sends the current disciplinary incident data to the server for saving.  Upon a
    /// successful save, it resets the incident data, reloads the necessary state,  and updates the UI to reflect the
    /// changes.</remarks>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task SaveAsync()
    {
        _newIncident.LearnerId = LearnerId;
        var result = await DisciplinaryIncidentService.CreateAsync(_newIncident.ToDto());
        result.ProcessResponseForDisplay(SnackBar, async () =>
        {
            SnackBar.AddSuccess("Incident logged");
            _newIncident = new DisciplinaryIncidentViewModel { Date = DateTime.Today };
            await LoadAsync();
            StateHasChanged();
        });
    }
}
