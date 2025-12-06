using AccomodationModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Radzen;
using Radzen.Blazor;

namespace KiburiOnline.Blazor.Shared.Pages.ShortDescriptionTemplates;

/// <summary>
/// Represents a modal dialog for editing a short description template.
/// </summary>
/// <remarks>This class provides functionality for managing a modal dialog that allows users to edit a short
/// description template. It includes options to save changes, cancel the dialog, and insert placeholders into an HTML
/// editor.</remarks>
public partial class ShortDescriptionTemplateModal
{
    /// <summary>
    /// Gets the current instance of the dialog being managed by this component.
    /// </summary>
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    /// <summary>
    /// Gets or sets the template used to render the short description view.
    /// </summary>
    [Parameter] public ShortDescriptionTemplateViewModel Template { get; set; } = new ShortDescriptionTemplateViewModel();

    /// <summary>
    /// Saves the current template and closes the dialog.
    /// </summary>
    /// <remarks>This method finalizes the current operation by saving the template and closing the associated
    /// dialog. Ensure that the <c>Template</c> object is properly initialized before invoking this method.</remarks>
    private void SaveAsync()
    {
        MudDialog.Close(Template);
    }

    /// <summary>
    /// Cancels the current dialog operation.
    /// </summary>
    /// <remarks>This method triggers the cancellation of the dialog, closing it and signaling that the
    /// operation was aborted. Use this method to programmatically dismiss the dialog when user interaction is no longer
    /// required.</remarks>
    public void Cancel()
    {
        MudDialog.Cancel();
    }

    /// <summary>
    /// Handles the event triggered when the "Add Placeholder" dropdown button is clicked in the editor.
    /// </summary>
    /// <remarks>If the <paramref name="date"/> parameter is not <see langword="null"/>, the method inserts
    /// the specified date into the provided <paramref name="editor"/> instance.</remarks>
    /// <param name="date">The date string to be inserted into the editor. Can be <see langword="null"/>. If <see langword="null"/>, no
    /// action is performed.</param>
    /// <param name="editor">The <see cref="RadzenHtmlEditor"/> instance where the date will be inserted.</param>
    /// <returns></returns>
    private async Task OnAddPlaceHolderEditorDropdownButtonClicked(string? placeholderText, RadzenHtmlEditor editor)
    {
        if (string.IsNullOrEmpty(placeholderText))
        {
            await InsertText(editor, placeholderText);
        }
    }

    /// <summary>
    /// Inserts the specified date into the provided HTML editor with a styled format.
    /// </summary>
    /// <remarks>The date is inserted as bold text with a blue color using the HTML `<strong>` tag and inline
    /// styling.</remarks>
    /// <param name="editor">The <see cref="RadzenHtmlEditor"/> instance where the date will be inserted. Cannot be null.</param>
    /// <param name="date">The date string to insert. Cannot be null or empty.</param>
    /// <returns></returns>
    private async Task InsertText(RadzenHtmlEditor editor, string placeholderText)
    {
        await editor.ExecuteCommandAsync(HtmlEditorCommands.InsertHtml, $"<strong style=\"color: blue\">{placeholderText}</strong>");
    }
}
