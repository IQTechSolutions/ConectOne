using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace ConectOne.Blazor.StateManagers
{
    /// <summary>
    /// The PageHistoryState class tracks the user's navigation history using 
    /// an internal list of visited page URIs. It enables the user to navigate 
    /// back to a previous page, much like a browser's back-button functionality. 
    /// 
    /// The class:
    /// 1. Subscribes to NavigationManager's LocationChanged event.
    /// 2. Maintains a list of visited pages (URIs).
    /// 3. Ensures the history size doesn't exceed a certain limit.
    /// 4. Provides a NavigateBack method for returning to the most recent page in history.
    /// 5. Exposes a CanGoBack property to check if a back navigation is possible.
    /// </summary>
    public class PageHistoryState
    {
        // The NavigationManager for registering location changes and performing navigations.
        private readonly NavigationManager _navigationManager;

        // Minimum number of history items that should be kept when trimming.
        private const int minHistorySize = 256;

        // Additional 'overflow' room allowed in the list before trimming.
        private const int additionalHistorySize = 64;

        // Internal list storing the URIs visited by the user.
        private List<string> previousPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageHistoryState"/> class.
        /// Subscribes to the NavigationManager's LocationChanged event to track 
        /// every new location (page) the user visits.
        /// </summary>
        /// <param name="navigationManager">An instance of Blazor's NavigationManager.</param>
        public PageHistoryState(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;

            // Allocate list to a size at least as large as the min + additional size
            previousPages = new List<string>(minHistorySize + additionalHistorySize);

            // Add the current page to the history as the first entry
            previousPages.Add(_navigationManager.Uri);

            // Register a callback whenever the navigation location changes
            _navigationManager.LocationChanged += UpdateNavigationList;
        }

        /// <summary>
        /// Callback invoked whenever a location change occurs. 
        /// Adds the new page (URI) to the history list, ensuring
        /// the list doesn't exceed its maximum size.
        /// </summary>
        private void UpdateNavigationList(object sender, LocationChangedEventArgs e)
        {
            // If list is too large, remove older entries
            EnsureSize();

            // Add the newly navigated location to history
            previousPages.Add(e.Location);
        }

        /// <summary>
        /// Checks if the history list exceeds the minHistorySize + additionalHistorySize. 
        /// If it does, trim the oldest entries to maintain a reasonable list size.
        /// </summary>
        private void EnsureSize()
        {
            if (previousPages.Count < minHistorySize + additionalHistorySize)
                return;

            // Remove the oldest entries, keeping a minimum "core" history
            previousPages.RemoveRange(0, previousPages.Count - minHistorySize);
        }

        /// <summary>
        /// Navigates the user back to the previous page in the history. 
        /// Removes the two most recent entries (the current page and its predecessor),
        /// then navigates to the page that was before those.
        /// </summary>
        public void NavigateBack()
        {
            // If there's no previous page, do nothing
            if (!CanGoBack) return;

            // The most recent page is the last entry; the one before that is the "back" target
            var backPageUrl = previousPages[^2];

            // Remove the last two entries: current page and the "back" target from the list
            previousPages.RemoveRange(previousPages.Count - 2, 2);

            // Perform the actual navigation
            _navigationManager.NavigateTo(backPageUrl);
        }

        /// <summary>
        /// Optional local method to simplify calling NavigateBack 
        /// (e.g., could be used as an onClick event).
        /// </summary>
        private void NavigateBackClick()
        {
            NavigateBack();
        }

        /// <summary>
        /// Indicates whether there is at least one previous page in history 
        /// to which we can navigate back.
        /// </summary>
        public bool CanGoBack => previousPages.Count > 1;
    }
}
