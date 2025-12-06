using ConectOne.Application.ViewModels;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Places;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace LocationModule.Blazor.Components
{
    /// <summary>
    /// Represents a form component for editing address information, including map-based selection and autocomplete
    /// functionality.
    /// </summary>
    /// <remarks>AddressEditForm integrates address editing with Google Maps and autocomplete features,
    /// allowing users to select or refine address details interactively. The form exposes events for valid submission,
    /// navigation, and cancellation, enabling parent components to respond to user actions. This component is typically
    /// used within a Blazor application where address input and validation are required.</remarks>
    public partial class AddressEditForm
    {
        private Autocomplete _autocomplete;
        private MudTextField<string> _autoCompleteBox;
        private GoogleMap _map1;
        private MapOptions _mapOptions;
        private string _message;
        private readonly Stack<Marker> _markers = new Stack<Marker>();

        /// <summary>
        /// Gets or sets the reference address form to be used within the component.
        /// </summary>
        /// <remarks>This property allows you to provide an <see cref="EditForm"/> instance that
        /// represents the address form referenced by the component. Assigning a value enables integration with form
        /// validation and submission logic.</remarks>
        [Parameter] public EditForm AddressFromReference { get; set; }

        /// <summary>
        /// Gets or sets the address information to be displayed or edited in the component.
        /// </summary>
        [Parameter] public AddressViewModel Address { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when the form is submitted with valid input.
        /// </summary>
        /// <remarks>The callback receives the serialized form data as a string. Use this property to
        /// handle successful form submissions, such as saving data or updating the UI. If not set, no action is taken
        /// on valid submission.</remarks>
        [Parameter] public EventCallback<string> OnValidSubmit { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when a back navigation action occurs.
        /// </summary>
        /// <remarks>The callback receives a string parameter that can be used to provide context or
        /// navigation information to the handler. Assign this property to handle custom logic when the user initiates a
        /// back action, such as navigating to a previous page or closing a dialog.</remarks>
        [Parameter] public EventCallback<string> OnBack { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when a cancel action is triggered.
        /// </summary>
        /// <remarks>The callback receives a string parameter that typically represents the reason or
        /// context for the cancellation. Assign this property to handle cancellation events, such as when a user closes
        /// a dialog or cancels an operation.</remarks>
        [Parameter] public EventCallback<string> OnCancel { get; set; }

        /// <summary>
        /// Invokes the valid submit event asynchronously when a form is submitted with valid data.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DoValidSubmit()
        {
            await OnValidSubmit.InvokeAsync();
        }

        /// <summary>
        /// Invokes the asynchronous back navigation action.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DoOnBack()
        {
            await OnBack.InvokeAsync();
        }

        /// <summary>
        /// Invokes the cancellation callback asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DoOnCancel()
        {
            await OnCancel.InvokeAsync();
        }

        /// <summary>
        /// Initializes the autocomplete functionality for the map input box and configures event handling for place
        /// selection.
        /// </summary>
        /// <remarks>This method sets up autocomplete with location restrictions and attaches a listener
        /// to handle place selection events. When a user selects a place, the map is updated to display the location,
        /// and address details are populated accordingly. This method should be called after the map component has been
        /// initialized.</remarks>
        /// <returns>A task that represents the asynchronous initialization operation.</returns>
        private async Task OnAfterMapInit()
        {

            _autocomplete = await Autocomplete.CreateAsync(_map1.JsRuntime, _autoCompleteBox.InputReference.ElementReference, new AutocompleteOptions
            {
                StrictBounds = false,
                ComponentRestrictions = new ComponentRestrictions { Country = new[] { "za" } }
            });

            //await autocomplete.SetFields(new []{ "address_components", "geometry", "icon", "name" });
            await _autocomplete.SetFields(new[] { "address_components", "geometry", "name" });

            await _autocomplete.AddListener("place_changed", async () =>
            {
                var place = await _autocomplete.GetPlace();

                if (place?.Geometry == null)
                {
                    _message = "No results available for " + place?.Name;
                }
                else if (place.Geometry.Location != null)
                {
                    await _map1.InteropObject.SetCenter(place.Geometry.Location);
                    await _map1.InteropObject.SetZoom(13);

                    Address.Longitude = place.Geometry.Location.Lng;
                    Address.Latitude = place.Geometry.Location.Lat;

                    var marker = await Marker.CreateAsync(_map1.JsRuntime, new MarkerOptions
                    {
                        Position = place.Geometry.Location,
                        Map = _map1.InteropObject,
                        Title = place.Name
                    });

                    _markers.Push(marker);

                    foreach (var component in place.AddressComponents)
                    {
                        var componentType = component.Types[0];

                        switch (componentType)
                        {
                            case "street_number":
                                {
                                    Address.StreetNumber = component.LongName;
                                    break;
                                }

                            case "route":
                                {
                                    Address.StreetName = component.LongName;
                                    break;
                                }

                            case "sublocality_level_1":
                                {
                                    Address.Suburb = component.LongName;
                                    break;
                                }

                            case "sublocality_level_2":
                                {
                                    Address.Suburb = component.LongName;
                                    break;
                                }

                            case "postal_code":
                                {
                                    Address.PostalCode = component.LongName;
                                    break;
                                }

                            case "locality":
                                {
                                    Address.City = component.LongName;
                                    break;
                                }

                            case "administrative_area_level_1":
                                {
                                    Address.Province = component.LongName;
                                    break;
                                }
                            case "country":
                                {
                                    Address.Country = component.LongName;
                                    break;
                                }
                        }
                    }

                    _message = "Displaying result for " + place.Name;
                }
                else if (place.Geometry.Viewport != null)
                {
                    await _map1.InteropObject.FitBounds(place.Geometry.Viewport, 5);
                    _message = "Displaying result for " + place.Name;
                }

                StateHasChanged();
            });
        }
    }
}
