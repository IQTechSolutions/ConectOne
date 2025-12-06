using ConectOne.Application.ViewModels;
using ConectOne.Domain.Enums;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Places;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LocationModule.Blazor.Components
{
    /// <summary>
    /// Represents a component for managing and interacting with address-related data, including map integration and
    /// autocomplete functionality.
    /// </summary>
    /// <remarks>This class provides functionality for handling address input, displaying and interacting with
    /// a map, and managing address-related events. It supports features such as address type categorization, address
    /// validation, and integration with Google Maps for autocomplete and marker placement.</remarks>
    public partial class AddressNoMap
    {
        private MudTextField<string> autoCompleteField;
        private readonly Stack<Marker> markers = new Stack<Marker>();
        private Autocomplete autocomplete;
        private string message;
        private GoogleMap map1;
        private MapOptions mapOptions;

        private bool otherFieldsDisabled = true;

        /// <summary>
        /// Gets or sets the type of address associated with the current instance.
        /// </summary>
        /// <remarks>This property specifies the category of the address, such as residential, business,
        /// or other.</remarks>
        [Parameter] public AddressType AddressType { get; set; }

        /// <summary>
        /// Gets or sets the address information for the current context.
        /// </summary>
        /// <remarks>This property is typically used to store or retrieve address-related data, such as
        /// street, city, and postal code. Ensure that the <see cref="AddressViewModel"/> instance is properly
        /// initialized before accessing its members.</remarks>
        [Parameter] public AddressViewModel Address { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when the address is changed.
        /// </summary>
        [Parameter] public EventCallback<AddressViewModel> AddressChanged { get; set; }

        /// <summary>
        /// Gets the header text corresponding to the current address type.
        /// </summary>
        private string Header
        {
            get
            {
                return AddressType switch
                {
                    AddressType.Physical => "Physical Address",
                    AddressType.Shipping => "Shipping Address",
                    AddressType.Billing => "Billing Address",
                    _ => ""
                };
            }
        }

        /// <summary>
        /// Initializes the autocomplete functionality for the map and sets up event listeners to handle place
        /// selection.
        /// </summary>
        /// <remarks>This method configures an autocomplete input field with specified options, including
        /// country restrictions. When a place is selected, it updates the map's center and zoom level, adds a marker at
        /// the selected location,  and populates the <see cref="Address"/> object with details from the selected place.
        /// It also invokes the  <see cref="AddressChanged"/> event to notify subscribers of the updated
        /// address.</remarks>
        /// <returns></returns>
        private async Task OnAfterMapInit()
        {

            autocomplete = await Autocomplete.CreateAsync(map1.JsRuntime, autoCompleteField.InputReference.ElementReference, new AutocompleteOptions
            {
                StrictBounds = false,
                ComponentRestrictions = new ComponentRestrictions { Country = new[] { "za" } }
            });

            //await autocomplete.SetFields(new []{ "address_components", "geometry", "icon", "name" });
            await autocomplete.SetFields(new[] { "address_components", "geometry", "name" });

            await autocomplete.AddListener("place_changed", async () =>
            {
                var place = await autocomplete.GetPlace();

                if (place?.Geometry == null)
                {
                    message = "No results available for " + place?.Name;
                }
                else if (place.Geometry.Location != null)
                {
                    await map1.InteropObject.SetCenter(place.Geometry.Location);
                    await map1.InteropObject.SetZoom(13);

                    Address.Longitude = place.Geometry.Location.Lng;
                    Address.Latitude = place.Geometry.Location.Lat;

                    var marker = await Marker.CreateAsync(map1.JsRuntime, new MarkerOptions
                    {
                        Position = place.Geometry.Location,
                        Map = map1.InteropObject,
                        Title = place.Name
                    });

                    markers.Push(marker);

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

                    message = "Displaying result for " + place.Name;
                }
                else if (place.Geometry.Viewport != null)
                {
                    await map1.InteropObject.FitBounds(place.Geometry.Viewport, 5);
                    message = "Displaying result for " + place.Name;
                }

                await AddressChanged.InvokeAsync(Address);

                otherFieldsDisabled = false;

                StateHasChanged();
            });
        }

        /// <summary>
        /// Initializes the component and sets up the default map options.
        /// </summary>
        /// <remarks>This method configures the map with a default zoom level, center coordinates,  and
        /// map type. It is called automatically during the component's initialization  lifecycle.</remarks>
        protected override void OnInitialized()
        {
            mapOptions = new MapOptions
            {
                Zoom = 13,
                Center = new LatLngLiteral
                {
                    Lat = -31.9241074,
                    Lng = 23.7627237
                },
                MapTypeId = MapTypeId.Roadmap
            };

        }
    }
}
