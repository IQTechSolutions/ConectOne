using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Radzen;
using Radzen.Blazor;

namespace KiburiOnline.Blazor.Shared.Pages.TermsAndConditionsTemplates;

/// <summary>
/// Represents a modal dialog for managing and editing terms and conditions templates.
/// </summary>
/// <remarks>This component provides functionality for editing a terms and conditions template, including
/// inserting custom variable placeholders and saving or canceling changes. It interacts with a  cascading <see
/// cref="IMudDialogInstance"/> to handle dialog actions and uses an injected  <see cref="IBaseHttpProvider"/> to fetch
/// available custom variable placeholders.</remarks>
public partial class TermsAndConditionsTemplateModal
{
    private List<CustomVariableTagDto> _availableCustomVariablePlaceholders = [];

    /// <summary>
    /// Gets or sets the current instance of the dialog.
    /// </summary>
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    [Inject] public ICustomVariableTagService CustomVariableTagService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the template for the terms and conditions.
    /// </summary>
    [Parameter] public TermsAndConditionsTemplateViewModel Template { get; set; } = new TermsAndConditionsTemplateViewModel();

    /// <summary>
    /// Asynchronously saves the current template and closes the dialog.
    /// </summary>
    /// <remarks>This method closes the dialog using the specified template. Ensure that the template is
    /// correctly set before calling this method.</remarks>
    private void SaveAsync()
    {
        MudDialog.Close(Template);
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
    /// Handles the event when the placeholder editor dropdown button is clicked.
    /// </summary>
    /// <remarks>If <paramref name="text"/> is <see langword="null"/>, no action is taken.</remarks>
    /// <param name="text">The text to be inserted into the editor. Can be <see langword="null"/>.</param>
    /// <param name="editor">The <see cref="RadzenHtmlEditor"/> instance where the text will be inserted.</param>
    private async Task OnAddPlaceHolderEditorDropdownButtonClicked(string? text, RadzenHtmlEditor editor)
    {
        if (text != null)
        {
            await InsertText(editor, text);
        }
    }

    /// <summary>
    /// Inserts the specified text into the RadzenHtmlEditor with a bold and blue style.
    /// </summary>
    /// <remarks>The text is inserted as HTML with a bold tag and blue color styling. Ensure that the editor
    /// is initialized before calling this method.</remarks>
    /// <param name="editor">The <see cref="RadzenHtmlEditor"/> instance where the text will be inserted.</param>
    /// <param name="text">The text to be inserted into the editor. Cannot be null or empty.</param>
    private async Task InsertText(RadzenHtmlEditor editor, string text)
    {
        await editor.ExecuteCommandAsync(HtmlEditorCommands.InsertHtml, $"<strong style=\"color: blue\">{text}</strong>");
    }

    /// <summary>
    /// Asynchronously initializes the component by retrieving and processing custom variable tags.
    /// </summary>
    /// <remarks>This method fetches a collection of custom variable tags from the provider and updates the
    /// internal list if the retrieval is successful. It then calls the base implementation of <see
    /// cref="OnInitializedAsync"/>.</remarks>
    /// <returns></returns>
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
