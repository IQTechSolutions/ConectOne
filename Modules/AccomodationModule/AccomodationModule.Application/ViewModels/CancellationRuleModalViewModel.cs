using Microsoft.AspNetCore.Mvc.Rendering;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents the view model for managing cancellation rules in a modal dialog.
    /// </summary>
    /// <remarks>This view model is used to define and configure cancellation rules for a lodging entity. It
    /// includes properties for specifying the lodging identifier, cancellation parameters,  and available rule
    /// options.</remarks>
    public class CancellationRuleModalViewModel 
    {
        /// <summary>
        /// Gets or sets the unique identifier for a lodging entity.
        /// </summary>
        public string LodgingId { get; set; }

        /// <summary>
        /// Gets or sets the number of days allowed for cancellation.
        /// </summary>
        public int CancellationDays { get; set; }

        /// <summary>
        /// Gets or sets the amount value.
        /// </summary>
        public double Ammount { get; set; } = 1;

        /// <summary>
        /// Gets or sets the rule associated with the current operation or configuration.
        /// </summary>
        public string Rule { get; set; }

        /// <summary>
        /// Gets or sets the collection of rules available for selection.
        /// </summary>
        public ICollection<SelectListItem> Rules { get; set; }

    }
}
