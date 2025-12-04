using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Interfaces;
using FilingModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using MudBlazor.Services;
using Radzen.Blazor;

namespace AdvertisingModule.Blazor.Components.Mobile
{
    public partial class AffiliateAndAddDisplayComponent
    {
        private int _selectedIndex;
        private int _columnSize;
        private RadzenCarousel _affiliateCarousel = null!;
        private RadzenCarousel _addCarousel = null!;
        private List<AffiliateDto> _affiliates = [];
        private List<AdvertisementDto> _advertisements = [];
        private List<AffiliateDto> _selectedAffiliates = [];
        private List<AdvertisementDto> _selectedAdvertisements = [];


        /// <summary>
        /// Gets the CSS height value for the affiliate carousel, based on the number of rows per slide.
        /// </summary>
        private string _affiliateCarouselHeight => $"{AffiliateRowsPerSlide * 175}px";

        /// <summary>
        /// Gets the calculated height of the advertisement carousel in pixels.
        /// </summary>
        private string _advertisementCarouselHeight => $"{AddRowsPerSlide * 175}px";

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        [Inject] private IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to navigate to different pages or to access the current navigation state.</remarks>
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query affiliate information.
        /// </summary>
        [Inject] private IAffiliateQueryService AffiliateQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query advertisements.
        /// </summary>
        [Inject] private IAdvertisementQueryService AdvertisementQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display transient notification messages to the user.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Use this
        /// service to show snack bar notifications, such as alerts or status messages, within the application's user
        /// interface.</remarks>
        [Inject] private ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to retrieve information about the browser viewport.
        /// </summary>
        /// <remarks>This property is typically set by the dependency injection framework. It provides
        /// access to viewport-related functionality, such as obtaining the current size or responding to resize
        /// events.</remarks>
        [Inject] private IBrowserViewportService BrowserViewportService { get; set; }

        /// <summary>
        /// Gets or sets the interval, in milliseconds, between operation executions.
        /// </summary>
        [Parameter] public int Interval { get; set; } = 5000;

        /// <summary>
        /// Gets or sets the number of affiliate rows to display per slide.
        /// </summary>
        [Parameter] public int AffiliateRowsPerSlide { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of rows to add to each slide.
        /// </summary>
        /// <remarks>The value must be a positive integer. Adjust this property to control how many rows
        /// are included when generating or displaying each slide.</remarks>
        [Parameter] public int AddRowsPerSlide { get; set; } = 2;

        /// <summary>
        /// Gets or sets the number of slides to display at once.
        /// </summary>
        [Parameter] public int SlideQty { get; set; } = 3;

        /// <summary>
        /// Represents a collection of affiliate slide groups, organized by group name.
        /// </summary>
        public Dictionary<string, List<CarouselItemViewModel>> _affiliateSlideGroups = new Dictionary<string, List<CarouselItemViewModel>>();

        /// <summary>
        /// Represents a mapping of group identifiers to their associated advertisement slide collections.
        /// </summary>
        /// <remarks>Each key in the dictionary corresponds to a group identifier, and the associated
        /// value is a list of advertisements belonging to that group. This field is intended for internal use and
        /// should not be accessed directly from outside the containing class.</remarks>
        public Dictionary<string, List<CarouselItemViewModel>> _adverTisementSlideGroups = new Dictionary<string, List<CarouselItemViewModel>>();

        /// <summary>
        /// Asynchronously initializes the component and loads required data when the component is first rendered.
        /// </summary>
        /// <remarks>This method is called by the Blazor framework during the component's initialization
        /// phase. Override this method to perform asynchronous operations such as data loading or service subscriptions
        /// before the component is rendered.</remarks>
        /// <returns>A task that represents the asynchronous initialization operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var affiliateResult = await AffiliateQueryService.AllAffiliatesAsync();
            if (affiliateResult.Succeeded) _affiliates = affiliateResult.Data.ToList();

            var advertisementResult = await AdvertisementQueryService.AllAdvertisementsAsync();
            if (advertisementResult.Succeeded) _advertisements = advertisementResult.Data.ToList();

            await BrowserViewportService.SubscribeAsync(this, fireImmediately: true);

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Asynchronously releases resources used by the instance and unsubscribes it from browser viewport
        /// notifications.
        /// </summary>
        /// <returns>A ValueTask that represents the asynchronous dispose operation.</returns>
        public async ValueTask DisposeAsync()
        {
            await BrowserViewportService.UnsubscribeAsync(this);
        }

        /// <summary>
        /// Updates the slide groupings for advertisements and affiliates based on the specified browser viewport event,
        /// and triggers a UI refresh asynchronously.
        /// </summary>
        /// <remarks>This method recalculates the grouping of advertisements and affiliates to match the
        /// current viewport size, ensuring that the displayed content adapts responsively. It should be called whenever
        /// the browser viewport changes to maintain correct layout.</remarks>
        /// <param name="browserViewportEventArgs">The event data containing information about the current browser viewport, including the responsive
        /// breakpoint to use for layout calculations. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task NotifyBrowserViewportChangeAsync(BrowserViewportEventArgs browserViewportEventArgs)
        {
            _selectedAdvertisements.Clear();
            _selectedAffiliates.Clear();

            _affiliateSlideGroups.Clear();
            _adverTisementSlideGroups.Clear();

            var ammountOfAffiliateSlides = SlideQty;
            var ammountOfAddSlides = SlideQty;

            _columnSize = browserViewportEventArgs.Breakpoint == Breakpoint.Xs 
                          || browserViewportEventArgs.Breakpoint == Breakpoint.Sm 
                          || browserViewportEventArgs.Breakpoint == Breakpoint.Md ? 4 : 3;
            var slideColumnCount = _columnSize == 4 ? 3 : 4;
            var affiliateSlideObjectCount = _columnSize * AffiliateRowsPerSlide;
            var addSlideObjectCount = _columnSize * AddRowsPerSlide;

            var wishfulaffiliateCount = affiliateSlideObjectCount * SlideQty;
            var wishfullAddCount = addSlideObjectCount * SlideQty;


            if (_affiliates.Count() < wishfulaffiliateCount)
            {
                double decammountOfAffiliateSlides = Convert.ToDouble(_affiliates.Count) / Convert.ToDouble(affiliateSlideObjectCount);
                if (decammountOfAffiliateSlides > 0 && decammountOfAffiliateSlides < affiliateSlideObjectCount)
                    ammountOfAffiliateSlides = 1;
                else
                    ammountOfAffiliateSlides = (int)decammountOfAffiliateSlides;
            }
                

            for (int i = 0; i < ammountOfAffiliateSlides; i++)
            {
                var selection = _affiliates.Skip(i * (affiliateSlideObjectCount)).Take(affiliateSlideObjectCount).ToList();
                _affiliateSlideGroups.Add($"Affilaite Slide {i + 1}", selection.Select(c => new CarouselItemViewModel(c.Title, c.Description, c.Url, c.Images.ToList())).ToList());
                _selectedAffiliates.AddRange(selection);
            }

            foreach (var item in _selectedAffiliates)
            {
                _affiliates.Remove(_affiliates.FirstOrDefault(x => x.Id == item.Id));
            }

            var combinedList = _advertisements.Select(c => new CarouselItemViewModel(c.Title, c.Description, c.Url, c.Images.ToList())).ToList();
            if (_advertisements.Count() < wishfullAddCount)
            {
                combinedList.AddRange(_affiliates.Select(c => new CarouselItemViewModel(c.Title, c.Description, c.Url, c.Images.ToList())));
                if (combinedList.Count() < wishfullAddCount)
                {
                    double decammountOfAddSlides = Convert.ToDouble(combinedList.Count()) / Convert.ToDouble(addSlideObjectCount);
                    if (decammountOfAddSlides > 0 && decammountOfAddSlides < addSlideObjectCount)
                        ammountOfAddSlides = 1;
                    else
                        ammountOfAddSlides = (int)decammountOfAddSlides;
                }
            }

            for (int i = 0; i < ammountOfAddSlides; i++)
            {
                var selection = combinedList.Skip(i * (addSlideObjectCount)).Take(addSlideObjectCount).ToList();
                _affiliateSlideGroups.Add($"Affilaite Slide {i + 1}", selection.Select(c => new CarouselItemViewModel(c.Title, c.Description, c.Url, c.Images.ToList())).ToList());
            }

            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Gets the options that control how viewport resize events are observed and reported.
        /// </summary>
        /// <remarks>Use this property to access the current configuration for resize event observation,
        /// such as the reporting rate and whether notifications are sent only on breakpoint changes. The returned
        /// options are read-only and reflect the observer's current settings.</remarks>
        ResizeOptions IBrowserViewportObserver.ResizeOptions { get; } = new()
        {
            ReportRate = 250,
            NotifyOnBreakpointOnly = true
        };

        /// <summary>
        /// Gets the unique identifier for this instance.
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();
    }

    public class CarouselItemViewModel
    {
        public CarouselItemViewModel(){}

        public CarouselItemViewModel(string title, string description, string url, List<ImageDto> images) 
        {
            Title = title;
            Description = description;
            Url = url;
            Images = images;
        }
        public string Title { get; init; } = null!;
        public string? Description { get; init; }
        public string? Url { get; init; }

        public ICollection<ImageDto>? Images { get; init; } = [];
    }
}
