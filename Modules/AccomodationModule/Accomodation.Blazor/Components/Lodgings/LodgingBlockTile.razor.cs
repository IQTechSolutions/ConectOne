using Accomodation.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Components.Lodgings
{
    /// <summary>
    /// Represents a UI tile component for displaying information about a lodging block, including its name, image,
    /// location, and rate.
    /// </summary>
    /// <remarks>This component is typically used to present lodging options within a grid or list, providing
    /// quick access to details and map functionality. The tile exposes parameters for binding lodging data and supports
    /// interaction via dialog services for map display. All parameters are required and must be provided for correct
    /// rendering.</remarks>
    public partial class LodgingBlockTile
    {
        /// <summary>
        /// Gets or sets the service used to display dialogs within the component.
        /// </summary>
        /// <remarks>The dialog service enables showing modal or non-modal dialogs to users, such as
        /// alerts, confirmations, or custom forms. Inject this property to access dialog functionality in Blazor
        /// components.</remarks>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage and perform navigation within the
        /// application.
        /// </summary>
        /// <remarks>Use this property to programmatically navigate to different URIs or to access
        /// information about the current navigation state. This property is typically injected by the framework and
        /// should not be set manually in most scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name associated with the component.
        /// </summary>
        /// <remarks>This property is required and must be set to a non-null, non-empty value. The name is
        /// typically used to identify the component within its context, such as in markup or when referencing it
        /// programmatically.</remarks>
        [Parameter, EditorRequired] public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the source URL of the image to display.
        /// </summary>
        [Parameter, EditorRequired] public string ImageSrc { get; set; } = null!;

        /// <summary>
        /// Gets or sets the URL to be used for the request or resource.
        /// </summary>
        /// <remarks>This property is required and must be a valid, absolute URL. Relative URLs or empty
        /// strings are not supported.</remarks>
        [Parameter, EditorRequired] public string Url { get; set; } = null!;

        /// <summary>
        /// Gets or sets the rate value used for calculations or processing.
        /// </summary>
        [Parameter, EditorRequired] public double Rate { get; set; }

        /// <summary>
        /// Gets or sets the latitude coordinate in degrees.
        /// </summary>
        /// <remarks>Valid values range from -90.0 to 90.0, where positive values indicate locations north
        /// of the equator and negative values indicate locations south of the equator.</remarks>
        [Parameter, EditorRequired] public double Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude coordinate value for the location.
        /// </summary>
        [Parameter, EditorRequired] public double Lng { get; set; }

        /// <summary>
        /// Displays a modal dialog showing a map for the current lodging location.
        /// </summary>
        /// <remarks>The map dialog is initialized with the lodging's name and coordinates. This method is
        /// asynchronous and should be awaited to ensure the dialog is shown before proceeding.</remarks>
        /// <returns>A task that represents the asynchronous operation of displaying the map dialog.</returns>
        private async Task ShowMap()
        {
            var parameters = new DialogParameters<LodgingMapModal>();

            parameters.Add(x => x.Title, Name);
            parameters.Add(x => x.Lat, Lat);
            parameters.Add(x => x.Lng, Lng);

            await DialogService.ShowAsync<LodgingMapModal>("Show Map", parameters);
        }
    }
}
