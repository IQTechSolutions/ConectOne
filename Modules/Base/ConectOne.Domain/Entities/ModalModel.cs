namespace ConectOne.Domain.Entities
{
    /// <summary>
    /// Represents a simple model for displaying modal dialogs in the user interface.
    /// This model can be passed to a view or component that renders a modal (pop-up) window.
    /// </summary>
    public class ModalModel
    {
        /// <summary>
        /// The title or header text displayed at the top of the modal.
        /// Typically used to describe the purpose or topic of the modal dialog.
        /// </summary>
        public string? HeaderString { get; set; }

        /// <summary>
        /// The main content of the modal, which can be plain text or HTML-formatted text.
        /// This text provides information, instructions, or asks the user to confirm an action.
        /// </summary>
        public string? ContentString { get; set; }

        /// <summary>
        /// An optional URL to which the user should be redirected or navigated after the modal is closed.
        /// This can be used in scenarios where the modal's action leads to a new page or the completion
        /// of a process that requires navigating away from the current view.
        /// </summary>
        public string? ReturnUrl { get; set; }
    }
}