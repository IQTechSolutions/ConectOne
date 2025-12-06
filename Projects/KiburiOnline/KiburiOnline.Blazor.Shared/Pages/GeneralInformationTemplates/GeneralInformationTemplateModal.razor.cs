using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Radzen;
using Radzen.Blazor;

namespace KiburiOnline.Blazor.Shared.Pages.GeneralInformationTemplates;

/// <summary>
/// Modal component for creating or editing general information templates.
/// </summary>
public partial class GeneralInformationTemplateModal
{
    private List<CustomVariableTagDto> _availableCustomVariablePlaceholders = [];

    /// <summary>
    /// Gets the current instance of the dialog being managed by this component.
    /// </summary>
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to manage custom variable tags.
    /// </summary>
    [Inject] public ICustomVariableTagService CustomVariableTagService { get; set; } = null!;

    /// <summary>
    /// Template data.
    /// </summary>
    [Parameter] public GeneralInformationTemplateViewModel Template { get; set; } = new GeneralInformationTemplateViewModel();

    /// <summary>
    /// Saves the current template and closes the dialog.
    /// </summary>
    /// <remarks>This method finalizes the current operation by saving the template and closing the associated
    /// dialog. Ensure that the <c>Template</c> object is properly initialized before calling this method.</remarks>
    private void SaveAsync()
    {
        MudDialog.Close(Template);
    }

    /// <summary>
    /// Cancels the current dialog operation.
    /// </summary>
    /// <remarks>This method triggers the cancellation of the dialog, closing it and signaling that the
    /// operation was aborted. Use this method when the user opts to cancel the dialog interaction.</remarks>
    public void Cancel()
    {
        MudDialog.Cancel();
    }

    /// <summary>
    /// Handles the event triggered when the "Add Placeholder" dropdown button is clicked in the HTML editor.
    /// </summary>
    /// <remarks>If <paramref name="text"/> is <see langword="null"/>, no action is performed. Otherwise, the
    /// specified text is inserted into the editor.</remarks>
    /// <param name="text">The placeholder text to be inserted. Can be <see langword="null"/> if no text is selected.</param>
    /// <param name="editor">The <see cref="RadzenHtmlEditor"/> instance where the placeholder text will be inserted.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    private async Task OnAddPlaceHolderEditorDropdownButtonClicked(string? text, RadzenHtmlEditor editor)
    {
        if (text != null)
        {
            await InsertText(editor, text);
        }
    }

    /// <summary>
    /// Inserts the specified text into the provided HTML editor with a bold, blue-styled format.
    /// </summary>
    /// <remarks>The inserted text will be wrapped in a <c>&lt;strong&gt;</c> tag and styled with a blue
    /// color.</remarks>
    /// <param name="editor">The <see cref="RadzenHtmlEditor"/> instance where the text will be inserted. Cannot be null.</param>
    /// <param name="text">The text to insert into the editor. Cannot be null or empty.</param>
    /// <returns></returns>
    private async Task InsertText(RadzenHtmlEditor editor, string text)
    {
        await editor.ExecuteCommandAsync(HtmlEditorCommands.InsertHtml, $"<strong style=\"color: blue\">{text}</strong>");
    }

    /// <summary>
    /// Asynchronously initializes the component and retrieves the available custom variable placeholders.
    /// </summary>
    /// <remarks>This method fetches a collection of custom variable tags from the provider and updates the
    /// internal state  with the retrieved data if the operation succeeds. It also invokes the base class's
    /// initialization logic.</remarks>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var availableCustomVariablePlaceholderResult = await CustomVariableTagService.GetAllAsync();
        if (availableCustomVariablePlaceholderResult.Succeeded)
        {
            _availableCustomVariablePlaceholders = availableCustomVariablePlaceholderResult.Data.ToList();
        }

        await base.OnInitializedAsync();
    }
}
