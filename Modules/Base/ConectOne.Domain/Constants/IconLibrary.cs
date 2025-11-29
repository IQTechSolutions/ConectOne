using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using ConectOne.Domain.Extensions;

namespace ConectOne.Domain.Constants;

/// <summary>
/// Central catalogue of icon choices that can be shared across modules.
/// </summary>
public static class IconLibrary
{
    /// <summary>
    /// Provides a read-only collection of available icon options for use in business and school modules, including SVG
    /// and font-based icons representing various categories and services.
    /// </summary>
    /// <remarks>The collection includes icons sourced from business-specific SVGs, legacy school icons, and
    /// Line Awesome font-based icons to support a wide range of use cases. This list is intended for scenarios where a
    /// predefined set of icons is required, such as populating icon pickers or displaying category imagery. The
    /// collection is static and does not change at runtime.</remarks>
    private static readonly IReadOnlyList<IconOption> _iconOptions = new ReadOnlyCollection<IconOption>(new[]
    {
        // Business specific SVG icons
        Image("/_content/BusinessModule.Blazor/images/static/business-icons/automotive.svg", "Automotive"),
        Image("/_content/BusinessModule.Blazor/images/static/business-icons/beauty.svg", "Beauty"),
        Image("/_content/BusinessModule.Blazor/images/static/business-icons/childcare.svg", "Child Care"),
        Image("/_content/BusinessModule.Blazor/images/static/business-icons/education.svg", "Education"),
        Image("/_content/BusinessModule.Blazor/images/static/business-icons/finance.svg", "Finance"),
        Image("/_content/BusinessModule.Blazor/images/static/business-icons/fitness.svg", "Fitness"),
        Image("/_content/BusinessModule.Blazor/images/static/business-icons/food.svg", "Food & Dining"),
        Image("/_content/BusinessModule.Blazor/images/static/business-icons/health.svg", "Health"),
        Image("/_content/BusinessModule.Blazor/images/static/business-icons/home-services.svg", "Home Services"),
        Image("/_content/BusinessModule.Blazor/images/static/business-icons/legal.svg", "Legal"),
        Image("/_content/BusinessModule.Blazor/images/static/business-icons/marketing.svg", "Marketing"),
        Image("/_content/BusinessModule.Blazor/images/static/business-icons/pets.svg", "Pet Care"),
        Image("/_content/BusinessModule.Blazor/images/static/business-icons/real-estate.svg", "Real Estate"),
        Image("/_content/BusinessModule.Blazor/images/static/business-icons/shopping.svg", "Shopping"),
        Image("/_content/BusinessModule.Blazor/images/static/business-icons/technology.svg", "Technology"),
        Image("/_content/BusinessModule.Blazor/images/static/business-icons/travel.svg", "Travel"),
        Image("/_content/BusinessModule.Blazor/images/static/business-icons/wedding.svg", "Wedding & Events"),

        // Legacy school icons
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/academicicon.svg"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/activitiesicon.svg"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/AFTERCARE.svg"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/athleticsicon.svg"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/Calendar.svg"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/chessicon.svg"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/Choir1.svg", "Choir"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/Choir2.svg", "Choir Alt"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/crosscountryicon.svg", "Cross Country"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/classicon.svg", "Class"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/CLOTHING BANK ICON.svg", "Clothing Bank"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/commsWhite.svg", "Communication"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/communication.svg", "Communication Alt"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/CONCERT 1.svg", "Concert"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/CONCERT 2.svg", "Concert Alt"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/CONTACT ICON.svg", "Contact"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/cricketicon.svg", "Cricket"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/cultureicon.svg", "Culture"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/CURRICULUM.svg", "Curriculum"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/CUSTOMISE ICON.svg", "Customise"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/CUSTOMISE NOTIFICATIONS ICON.svg", "Notifications"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/DEMARCATION.svg", "Demarcation"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/DISBURSE MONEY.svg", "Disburse Money"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/DRAMA.svg", "Drama"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/Eisteddfod.svg", "Eisteddfod"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/EMERGENCIES ICON.svg", "Emergencies"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/eventsicon.svg", "Events"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/EVERADS.svg", "EverAds"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/EVERCIRCULAR.svg", "EverCircular"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/everfocusicon.svg", "EverFocus"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/EVERSDALTEXT.svg", "Eversdal"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/EXAMS.svg", "Exams"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/FOCUS.svg", "Focus"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/GR.svg", "Grade"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/hockeyicon.svg", "Hockey"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/homeicon.svg", "Home"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/LOAD WALLET.svg", "Load Wallet"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/MARKET PLACE ICON.svg", "Marketplace"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/MESSAGE ICON.svg", "Messages"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/MINI STATEMENT.svg", "Mini Statement"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/MONEY ICON.svg", "Money"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/musicicon.svg", "Music"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/mydocuments.svg", "My Documents"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/netbalicon.svg", "Netball"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/Notifications.svg", "Notifications"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/personiconprofile.svg", "Profile"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/REWARDS  ICON.svg", "Rewards"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/rugby.svg", "Rugby"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/SEND MONEY.svg", "Send Money"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/sport.svg", "Sport"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/STAFF.svg", "Staff"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/swimmingicon.svg", "Swimming"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/talkicon.svg", "Talk"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/tennisicon.svg", "Tennis"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/TERM LETTERS ICON.svg", "Term Letters"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/TIMES TERMS.svg", "Times & Terms"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/WALLET  ICON.svg", "Wallet"),
        Image("/_content/SchoolsModule.Blazor/images/static/icons2/WEB WHITE.svg", "Web"),

        // Line Awesome font based icons for extended coverage
        Font("las la-briefcase", "Briefcase"),
        Font("las la-building", "Building"),
        Font("las la-city", "City Services"),
        Font("las la-store", "Storefront"),
        Font("las la-shopping-bag", "Shopping Bag"),
        Font("las la-shopping-basket", "Shopping Basket"),
        Font("las la-tshirt", "Apparel"),
        Font("las la-utensils", "Restaurant"),
        Font("las la-wine-bottle", "Beverages"),
        Font("las la-bread-slice", "Bakery"),
        Font("las la-coffee", "Coffee"),
        Font("las la-pizza-slice", "Pizza"),
        Font("las la-ice-cream", "Ice Cream"),
        Font("las la-hotel", "Hotel"),
        Font("las la-spa", "Spa"),
        Font("las la-suitcase-rolling", "Travel"),
        Font("las la-plane", "Flights"),
        Font("las la-ship", "Cruises"),
        Font("las la-bus", "Bus"),
        Font("las la-taxi", "Taxi"),
        Font("las la-car", "Automotive"),
        Font("las la-gas-pump", "Fuel"),
        Font("las la-tools", "Repairs"),
        Font("las la-home", "Home"),
        Font("las la-hammer", "Construction"),
        Font("las la-paint-roller", "Decor"),
        Font("las la-leaf", "Gardening"),
        Font("las la-solar-panel", "Solar"),
        Font("las la-lightbulb", "Innovation"),
        Font("las la-server", "Hosting"),
        Font("las la-laptop-code", "Software"),
        Font("las la-robot", "Automation"),
        Font("las la-shield-alt", "Security"),
        Font("las la-balance-scale", "Law"),
        Font("las la-gavel", "Legal Services"),
        Font("las la-hand-holding-usd", "Donations"),
        Font("las la-piggy-bank", "Savings"),
        Font("las la-wallet", "Wallet"),
        Font("las la-chart-line", "Growth"),
        Font("las la-chart-pie", "Analytics"),
        Font("las la-coins", "Investments"),
        Font("las la-percentage", "Discounts"),
        Font("las la-bullhorn", "Announcements"),
        Font("las la-comments", "Communication"),
        Font("las la-people-carry", "Logistics"),
        Font("las la-dolly", "Delivery"),
        Font("las la-shipping-fast", "Courier"),
        Font("las la-recycle", "Recycling"),
        Font("las la-first-aid", "First Aid"),
        Font("las la-clinic-medical", "Clinic"),
        Font("las la-stethoscope", "Healthcare"),
        Font("las la-heartbeat", "Wellness"),
        Font("las la-dumbbell", "Gym"),
        Font("las la-running", "Running"),
        Font("las la-bicycle", "Cycling"),
        Font("las la-swimmer", "Swimming"),
        Font("las la-basketball-ball", "Basketball"),
        Font("las la-football-ball", "Football"),
        Font("las la-golf-ball", "Golf"),
        Font("las la-futbol", "Soccer"),
        Font("las la-volleyball-ball", "Volleyball"),
        Font("las la-table-tennis", "Table Tennis"),
        Font("las la-bowling-ball", "Bowling"),
        Font("las la-baseball-ball", "Baseball"),
        Font("las la-child", "Child Care"),
        Font("las la-baby", "Infant Care"),
        Font("las la-graduation-cap", "Education"),
        Font("las la-chalkboard-teacher", "Tutoring"),
        Font("las la-book-open", "Library"),
        Font("las la-atom", "Science"),
        Font("las la-microscope", "Research"),
        Font("las la-theater-masks", "Theatre"),
        Font("las la-music", "Music"),
        Font("las la-guitar", "Guitar"),
        Font("las la-camera", "Photography"),
        Font("las la-video", "Video"),
        Font("las la-film", "Film"),
        Font("las la-brush", "Art"),
        Font("las la-palette", "Design"),
        Font("las la-tree", "Outdoors"),
        Font("las la-campground", "Camping"),
        Font("las la-fish", "Fishing"),
        Font("las la-dog", "Pets"),
        Font("las la-cat", "Cats"),
        Font("las la-paw", "Animals"),
        Font("las la-apple-alt", "Nutrition"),
        Font("las la-seedling", "Sustainability"),
        Font("las la-globe-africa", "Travel Africa"),
        Font("las la-passport", "Passport"),
        Font("las la-gift", "Gifts"),
        Font("las la-birthday-cake", "Celebrations"),
        Font("las la-ring", "Jewellery"),
        Font("las la-crown", "Luxury"),
        Font("las la-star", "Featured"),
        Font("las la-award", "Awards"),
        Font("las la-medal", "Achievement"),
        Font("las la-lightbulb", "Ideas"),
        Font("las la-headset", "Support"),
        Font("las la-phone-volume", "Hotline"),
        Font("las la-life-ring", "Help Desk"),
        Font("las la-hands-helping", "Non-profit"),
        Font("las la-praying-hands", "Community"),
        Font("las la-peace", "Advocacy"),
        Font("las la-bible", "Faith"),
        Font("las la-pray", "Prayer"),
        Font("las la-mosque", "Mosque"),
        Font("las la-church", "Church"),
        Font("las la-synagogue", "Synagogue"),
        Font("las la-glass-cheers", "Events"),
        Font("las la-calendar-alt", "Calendar"),
        Font("las la-clipboard-list", "Tasks"),
        Font("las la-tasks", "Checklist"),
        Font("las la-box-open", "Inventory"),
        Font("las la-warehouse", "Warehouse"),
        Font("las la-database", "Database"),
        Font("las la-credit-card", "Payments"),
        Font("las la-file-invoice-dollar", "Billing"),
        Font("las la-cash-register", "Point of Sale"),
        Font("las la-gem", "Jewellery"),
        Font("las la-feather-alt", "Stationery"),
        Font("las la-broom", "Cleaning"),
        Font("las la-pump-soap", "Sanitising"),
        Font("las la-cogs", "Engineering"),
        Font("las la-helmet-safety", "Safety"),
        Font("las la-hard-hat", "Construction Safety"),
        Font("las la-biohazard", "Biohazard"),
        Font("las la-thermometer-half", "Temperature"),
        Font("las la-cloud-sun", "Weather"),
        Font("las la-cloud-moon-rain", "Storm"),
        Font("las la-wind", "Wind"),
        Font("las la-snowflake", "Snow"),
        Font("las la-moon", "Night"),
        Font("las la-sun", "Sun"),
        Font("las la-hurricane", "Emergency"),
        Font("las la-fire-extinguisher", "Fire Safety"),
        Font("las la-burn", "Fire"),
        Font("las la-water", "Water"),
        Font("las la-mountain", "Adventure"),
        Font("las la-compass", "Guided Tours"),
        Font("las la-map", "Maps"),
        Font("las la-route", "Routes"),
        Font("las la-hiking", "Hiking"),
        Font("las la-binoculars", "Sightseeing"),
        Font("las la-drum", "Percussion"),
        Font("las la-drum-steelpan", "Steelpan"),
        Font("las la-keyboard", "Keyboard"),
        Font("las la-microphone", "Audio"),
        Font("las la-headphones", "Headphones"),
        Font("las la-broadcast-tower", "Broadcast"),
        Font("las la-radiation", "Hazard"),
        Font("las la-vials", "Laboratory"),
        Font("las la-prescription-bottle-alt", "Pharmacy"),
        Font("las la-capsules", "Medication"),
        Font("las la-procedures", "Hospital"),
        Font("las la-ambulance", "Ambulance"),
        Font("las la-wheelchair", "Accessibility"),
        Font("las la-clipboard-check", "Compliance"),
        Font("las la-file-contract", "Contracts"),
        Font("las la-id-card", "Identification"),
        Font("las la-user-tie", "Professional Services"),
        Font("las la-user-md", "Doctor"),
        Font("las la-user-nurse", "Nurse"),
        Font("las la-users", "Community"),
        Font("las la-user-friends", "Friends"),
        Font("las la-user-graduate", "Learners"),
        Font("las la-user-cog", "Admin"),
        Font("las la-user-shield", "Security"),
        Font("las la-users-cog", "Teams"),
        Font("las la-user-astronaut", "Innovation"),
    });

    private static readonly IReadOnlyList<string> _imageIconValues = new ReadOnlyCollection<string>(
        _iconOptions.Where(icon => !icon.IsFontIcon).Select(icon => icon.Value).Distinct().ToList());

    /// <summary>
    /// Gets the full list of available icons.
    /// </summary>
    public static IReadOnlyList<IconOption> IconOptions => _iconOptions;

    /// <summary>
    /// Gets the icon values that map to static image paths.
    /// </summary>
    public static IReadOnlyList<string> ImageIconValues => _imageIconValues;

    /// <summary>
    /// Returns a friendly name for the supplied icon value.
    /// </summary>
    public static string GetDisplayName(string? iconValue)
    {
        if (string.IsNullOrWhiteSpace(iconValue))
        {
            return "Icon";
        }

        var option = _iconOptions.FirstOrDefault(icon => string.Equals(icon.Value, iconValue, StringComparison.OrdinalIgnoreCase));
        if (option is not null)
        {
            return option.DisplayName;
        }

        if (IconValueHelper.IsFontIcon(iconValue))
        {
            var cssClass = IconValueHelper.GetFontIconClass(iconValue);
            var normalizedCss = Regex.Replace(cssClass, "(^la[srb]?)\\s+", string.Empty, RegexOptions.IgnoreCase);
            normalizedCss = Regex.Replace(normalizedCss, "[-_]+", " ").Trim();

            if (!string.IsNullOrWhiteSpace(normalizedCss))
            {
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(normalizedCss.ToLowerInvariant());
            }

            return "Icon";
        }

        return GenerateDisplayNameFromFilePath(iconValue);
    }

    /// <summary>
    /// Determines whether the supplied icon value requires an upload when persisting.
    /// </summary>
    public static bool RequiresUpload(string? iconValue) => IconValueHelper.RequiresUpload(iconValue);

    /// <summary>
    /// Determines whether the supplied icon represents a CSS based icon.
    /// </summary>
    public static bool IsFontIcon(string? iconValue) => IconValueHelper.IsFontIcon(iconValue);

    /// <summary>
    /// Extracts the CSS class from the stored icon value.
    /// </summary>
    public static string GetCssClass(string iconValue) => IconValueHelper.GetFontIconClass(iconValue);

    /// <summary>
    /// Creates a new IconOption representing an image file, with an optional display name.
    /// </summary>
    /// <param name="path">The file system path to the image. This value must not be null or empty.</param>
    /// <param name="displayName">An optional display name for the image. If null, a display name is generated from the file path.</param>
    /// <returns>An IconOption instance initialized with the specified image path and display name.</returns>
    private static IconOption Image(string path, string? displayName = null)
        => new(path, displayName ?? GenerateDisplayNameFromFilePath(path));

    /// <summary>
    /// Creates a new icon option representing a font-based icon with the specified CSS class and display name.
    /// </summary>
    /// <param name="cssClass">The CSS class that identifies the font icon. Cannot be null or empty.</param>
    /// <param name="displayName">The display name to associate with the icon. This value is used for descriptive or accessibility purposes.</param>
    /// <returns>An IconOption instance configured with the specified font icon CSS class and display name.</returns>
    private static IconOption Font(string cssClass, string displayName)
        => new($"{IconValueHelper.FontIconPrefix}{cssClass}", displayName);

    /// <summary>
    /// Generates a display-friendly name from the specified file path by normalizing and formatting the file name.
    /// </summary>
    /// <param name="path">The file path from which to extract and format the display name. Can be a relative or absolute path.</param>
    /// <returns>A display name derived from the file name in title case, with hyphens and underscores replaced by spaces.
    /// Returns "Icon" if the file name is empty or consists only of whitespace or separators.</returns>
    private static string GenerateDisplayNameFromFilePath(string path)
    {
        var fileName = Path.GetFileNameWithoutExtension(path);
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return "Icon";
        }

        var normalized = Regex.Replace(fileName, "[-_]+", " ").Trim();
        if (string.IsNullOrEmpty(normalized))
        {
            return "Icon";
        }

        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(normalized.ToLowerInvariant());
    }
}

/// <summary>
/// Represents a selectable icon from the shared icon library.
/// </summary>
/// <param name="Value">The stored value for the icon.</param>
/// <param name="DisplayName">A friendly display name for UI scenarios.</param>
public sealed record IconOption(string Value, string DisplayName)
{
    /// <summary>
    /// Gets a value indicating whether the icon is rendered via a CSS class instead of an image path.
    /// </summary>
    public bool IsFontIcon => IconValueHelper.IsFontIcon(Value);
}
