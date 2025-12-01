using System.Security.Claims;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.Parents;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using MapOptions = GoogleMapsComponents.Maps.MapOptions;
using Marker = GoogleMapsComponents.Maps.Marker;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Events
{
    /// <summary>
    /// The Details component is responsible for displaying the details of a specific school event.
    /// It includes functionality to show event information, navigate to related pages, and display a map.
    /// </summary>
    public partial class Details
    {
        /// <summary>
        /// The current authentication state, used to determine the logged-in user's identity.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Injected notification state manager for managing notifications.
        /// </summary>
        [Inject] public ISchoolEventQueryService SchoolEventQueryService { get; set; } = null!;

        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IParentQueryService ParentQueryService { get; set; } = null!;

        /// <summary>
        /// The ID of the event to be displayed.
        /// </summary>
        [Parameter] public string EventId { get; set; } = null!;

        /// <summary>
        /// The school event details.
        /// </summary>
        private SchoolEventDto _schoolEvent = new();

        /// <summary>
        /// The parent details, if the user is a parent.
        /// </summary>
        private ParentDto? _parent;

        /// <summary>
        /// Indicates whether the map is visible.
        /// </summary>
        private readonly bool _mapVisible;

        /// <summary>
        /// The Google Map instance.
        /// </summary>
        private GoogleMap _map1 = null!;

        /// <summary>
        /// The options for configuring the Google Map.
        /// </summary>
        private MapOptions _mapOptions = null!;

        /// <summary>
        /// Stack of markers to be displayed on the map.
        /// </summary>
        private readonly Stack<Marker> _markers = new();

        /// <summary>
        /// Indicates whether the component has completed loading and can render content.
        /// </summary>
        private bool _loaded;

        /// <summary>
        /// The current user's claims principal.
        /// </summary>
        private ClaimsPrincipal _user = null!;

        /// <summary>
        /// The ID of the current user.
        /// </summary>
        private string _userId = null!;
        
        /// <summary>
        /// Shows or hides the map based on the event's Google Map link.
        /// </summary>
        private void ShowHideMap()
        {
            if (string.IsNullOrEmpty(_schoolEvent.GoogleMapLink)) return;
            NavigationManager.NavigateTo(_schoolEvent.GoogleMapLink);
        }

        /// <summary>
        /// Initializes the map and adds a marker for the event location.
        /// </summary>
        private async Task OnAfterMapInit()
        {
            var marker = await Marker.CreateAsync(_map1.JsRuntime, new MarkerOptions
            {
                Map = _map1.InteropObject,
                Title = _schoolEvent.Name,
            });
            _markers.Push(marker);
            StateHasChanged();
        }

        /// <summary>
        /// Asynchronously initializes the component, setting up user information, map options, and event data.
        /// </summary>
        /// <remarks>This method retrieves the current user's authentication state and initializes
        /// user-specific data, including their ID and role-based information. It also configures map options and
        /// fetches event details from the data provider. If the user is in the "Parent" role, additional
        /// parent-specific data is retrieved. The method ensures that all necessary data is loaded before the component
        /// is fully initialized.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            _user = authState.User;
            _userId = _user.GetUserId();

            _mapOptions = new MapOptions
            {
                Zoom = 13,
                Center = new LatLngLiteral(-33.8744833, 18.6266834),
                MapTypeId = MapTypeId.Roadmap
            };

            var eventResult = await SchoolEventQueryService.SchoolEventAsync(EventId);
            if (eventResult.Succeeded)
            {
                _schoolEvent = eventResult.Data;
                if (_user.IsInRole("Parent"))
                {
                    var parentResult = await ParentQueryService.ParentByEmailAsync(_user.GetUserEmail());
                    if (parentResult.Succeeded) _parent = parentResult.Data;
                }
            }
            _loaded = true;
            await base.OnInitializedAsync();
        }
    }
}