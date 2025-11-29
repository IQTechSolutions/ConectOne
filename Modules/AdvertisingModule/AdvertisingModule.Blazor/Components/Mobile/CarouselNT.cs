using Microsoft.AspNetCore.Components;

namespace AdvertisingModule.Blazor.Components.Mobile;

/// <summary>
/// A Blazor component that displays a collection of items in a sliding carousel layout, allowing users to cycle through
/// items in groups.
/// </summary>
/// <remarks>The carousel automatically advances to the next set of items at a specified interval. The number of
/// items shown per slide and the interval between slides can be customized. To define how each item is rendered,
/// provide an item template via the ItemTemplate parameter. This component is not thread-safe and is intended for use
/// within Blazor applications.</remarks>
/// <typeparam name="TItem">The type of the data item to display in each slide of the carousel.</typeparam>
public partial class Carousel<TItem> : ComponentBase, IDisposable
{
    private int _currentIndex = 0;
    private System.Timers.Timer? _timer;

    /// <summary>
    /// Gets or sets the collection of items to be rendered by the component.
    /// </summary>
    [Parameter] public List<TItem> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets the template used to render each item in the collection.
    /// </summary>
    /// <remarks>The template receives the item of type TItem as its context. Use this property to customize
    /// the appearance and layout of individual items when rendering a list or collection.</remarks>
    [Parameter] public RenderFragment<TItem>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets the number of items to display in each slide.
    /// </summary>
    [Parameter] public int ItemsPerSlide { get; set; } = 6;

    /// <summary>
    /// Gets or sets the interval, in milliseconds, between executions or events.
    /// </summary>
    [Parameter] public int Interval { get; set; } = 5000;

    /// <summary>
    /// Initializes the component and starts the timer that controls automatic slide transitions.
    /// </summary>
    /// <remarks>This method is called by the framework when the component is first initialized. It sets up a
    /// recurring timer to advance slides at the specified interval. The timer is started immediately and will continue
    /// to trigger slide changes until the component is disposed.</remarks>
    protected override void OnInitialized()
    {
        _timer = new System.Timers.Timer(Interval);
        _timer.Elapsed += (sender, args) => InvokeAsync(NextSlide);
        _timer.AutoReset = true;
        _timer.Start();
    }

    /// <summary>
    /// Advances to the next slide in the collection, updating the current slide index.
    /// </summary>
    /// <remarks>If the end of the collection is reached, the index wraps around to the beginning. No action
    /// is taken if the collection is empty.</remarks>
    private void NextSlide()
    {
        if (Items.Count == 0) return;
        _currentIndex = (_currentIndex + ItemsPerSlide) % Items.Count;
        StateHasChanged();
    }

    /// <summary>
    /// Releases all resources used by the current instance.
    /// </summary>
    /// <remarks>Call this method when the instance is no longer needed to free associated resources promptly.
    /// After calling this method, the instance should not be used.</remarks>
    public void Dispose()
    {
        _timer?.Dispose();
    }
}