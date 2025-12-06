using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// Represents a modal dialog component that displays a map centered on a specified lodging location.
    /// </summary>
    /// <remarks>This component is typically used to show the geographic location of a lodging on an
    /// interactive map within a modal dialog. The map is centered using the provided latitude and longitude parameters,
    /// and an optional title can be displayed. The component integrates with MudBlazor's dialog system and Google Maps
    /// functionality.</remarks>
    public partial class LodgingMapModal : ComponentBase
    {
        private GoogleMap _map1 = null!;
        private MapOptions _mapOptions = null!;
        private readonly Stack<Marker> _markers = new Stack<Marker>();

        /// <summary>
        /// Gets or sets the dialog instance associated with the current component.
        /// </summary>
        /// <remarks>This property is typically used within a dialog component to interact with the
        /// dialog's lifecycle, such as closing or cancelling the dialog. The value is provided via Blazor's cascading
        /// parameter mechanism and may be null if the component is not hosted within a dialog.</remarks>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the title to display for the component.
        /// </summary>
        [Parameter] public string? Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets the latitude coordinate for the location.
        /// </summary>
        [Parameter] public double Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude coordinate value for the component.
        /// </summary>
        [Parameter] public double Lng { get; set; }

        /// <summary>
        /// Cancels the current dialog operation.
        /// </summary>
        /// <remarks>This method closes the dialog and signals a cancellation to any awaiting code. Use
        /// this method to dismiss the dialog without confirming or applying changes.</remarks>
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        /// <summary>
        /// Performs post-initialization logic for the map, adding a marker if valid latitude and longitude values are
        /// set.
        /// </summary>
        /// <remarks>This method should be called after the map has been initialized to ensure that any
        /// initial marker is added based on the current coordinates. If both latitude and longitude are zero, no marker
        /// will be added.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task OnAfterMapInit()
        {
            if (Lat != 0 || Lng != 0)
            {
                var latLng = new LatLngLiteral(Lat, Lng);
                var marker = await Marker.CreateAsync(_map1.JsRuntime, new MarkerOptions(latLng, _map1, Title));

                _markers.Push(marker);

                StateHasChanged();
            }
        }

        /// <summary>
        /// Asynchronously initializes the component and configures the map options before rendering.
        /// </summary>
        /// <remarks>Overrides the base implementation to set up map options using the current latitude
        /// and longitude values prior to invoking the base initialization logic.</remarks>
        /// <returns>A task that represents the asynchronous initialization operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            _mapOptions = new MapOptions(13, new LatLngLiteral(Lat, Lng), MapTypeId.Roadmap);
            await base.OnInitializedAsync();
        }

    }
}
