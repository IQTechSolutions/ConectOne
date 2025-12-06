using AccomodationModule.Application.ViewModels;
using ConectOne.Blazor.Extensions;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Places;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Lodgings
{
    /// <summary>
    /// Represents a component for managing and interacting with lodging address details, including map integration and
    /// address selection.
    /// </summary>
    /// <remarks>This class provides functionality for handling lodging address information, such as province
    /// selection, address autocomplete, and map-based interactions. It integrates with Google Maps and supports address
    /// component parsing, marker management, and province selection.</remarks>
    public partial class LodgingAddress
    {
        private GoogleMap _map1 = null!;

        private MapOptions _mapOptions = null!;
        private Autocomplete _autocomplete = null!;
        private MudTextField<string> _autoCompleteBox = null!;
        private readonly Stack<Marker> _markers = [];

        /// <summary>
        /// Gets or sets the lodging information to be displayed or managed in the view.
        /// </summary>
        [Parameter, EditorRequired] public LodgingViewModel Lodging { get; set; } = new LodgingViewModel();

        /// <summary>
        /// Gets or sets the injected instance of <see cref="ISnackbar"/> used to display notifications or messages.
        /// </summary>
        [Inject] private ISnackbar SnackBar { get; set; } = null!;
        

        /// <summary>
        /// Initiates the selection process for provinces, optionally pre-selecting a province based on the provided ID.
        /// </summary>
        /// <remarks>This method retrieves a list of available provinces and processes the response for
        /// display. If a valid <paramref name="selectedProvinceId"/> is provided and matches an available province, 
        /// the corresponding province will be set as the selected province.</remarks>
        /// <param name="selectedProvinceId">The ID of the province to pre-select, if applicable. If <see langword="null"/>, no province will be
        /// pre-selected.</param>
        /// <returns></returns>
        private async Task InitiateProvinceSelections(int? selectedProvinceId)
        {
            // var result = await ProvinceService.PagedProvincesAsync(new ProvincePageParameters());

            // result.ProcessResponseForDisplay(SnackBar, () =>
            // {
            //     AvailableProvinces = result.Data.Select(c => new ProvinceViewModel(c)).ToList();
            //     if (selectedProvinceId is not null && AvailableProvinces.FirstOrDefault(c => c.ProvinceId == selectedProvinceId) is not null)
            //     {
            //         Lodging.Address.Province = AvailableProvinces.FirstOrDefault(c => c.ProvinceId == selectedProvinceId);
            //     }
            // });
        }

        /// <summary>
        /// Initializes the autocomplete functionality for the map and sets up event listeners for place selection.
        /// </summary>
        /// <remarks>This method configures the autocomplete input box with specified options, including
        /// country restrictions. When a place is selected, it updates the map's center and zoom level, creates a marker
        /// at the selected location,  and populates the lodging address details based on the place's address
        /// components.</remarks>
        /// <returns></returns>
        private async Task OnAfterMapInit()
        {
            var latLng = new LatLngLiteral(-31.9241074, 23.7627237);

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
                    SnackBar.AddError("No results available for " + place?.Name);
                }
                else if (place.Geometry.Location != null)
                {
                    await _map1.InteropObject.SetCenter(place.Geometry.Location);
                    await _map1.InteropObject.SetZoom(13);

                    Lodging.Address.Lng = place.Geometry.Location.Lng;
                    Lodging.Address.Lat = place.Geometry.Location.Lat;

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
                                    Lodging.Address.StreetNumber = component.LongName;
                                    break;
                                }

                            case "route":
                                {
                                    Lodging.Address.StreetName = component.LongName;
                                    break;
                                }

                            case "sublocality_level_1":
                                {
                                    Lodging.Address.Suburb = component.LongName;
                                    break;
                                }

                            case "sublocality_level_2":
                                {
                                    Lodging.Address.Suburb = component.LongName;
                                    break;
                                }

                            case "postal_code":
                                {
                                    Lodging.Address.PostalCode = component.LongName;
                                    break;
                                }

                            case "locality":
                                {
                                    Lodging.Address.City = component.LongName;
                                    break;
                                }
                            case "country":
                                {
                                    Lodging.Address.Country = component.LongName;
                                    break;
                                }
                        }
                    }
                }
                else if (place.Geometry.Viewport != null)
                {
                    await _map1.InteropObject.FitBounds(place.Geometry.Viewport, 5);
                }

                StateHasChanged();
            });
        }

        /// <summary>
        /// Asynchronously performs initialization logic when the component is first initialized.
        /// </summary>
        /// <remarks>This method is invoked automatically by the Blazor framework during the component's
        /// lifecycle. Override this method to include custom initialization logic, such as setting up state or 
        /// fetching data. Ensure that any asynchronous operations are awaited to complete initialization  before the
        /// component is rendered.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            //   _mapOptions = new MapOptions(13, new(Lodging.Address.Lat, Lodging.Address.Lng), MapTypeId.Roadmap);

            // await InitiateProvinceSelections(Lodging.Address.ProviceId);

            await base.OnInitializedAsync();
        }
    }
}
