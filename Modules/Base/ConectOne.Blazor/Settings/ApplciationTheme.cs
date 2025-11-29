using MudBlazor;

namespace ConectOne.Blazor.Settings
{
    /// <summary>
    /// Provides predefined light and dark application themes, including palette, typography, and layout settings, for
    /// consistent UI styling.
    /// </summary>
    /// <remarks>Use this class to access standard theme configurations for applications that utilize
    /// MudTheme-based styling. The provided themes can be applied directly or used as a base for customizations. All
    /// typography and layout properties are set to recommended defaults for a cohesive appearance.</remarks>
    public class ApplciationTheme
    {
        /// <summary>
        /// Provides the default typography settings for various text elements, including headings, body text, buttons,
        /// and captions.
        /// </summary>
        /// <remarks>The default typography defines font families, sizes, weights, line heights, and
        /// letter spacing for each supported text style. These settings are intended to ensure visual consistency
        /// across the application's user interface. The configuration can be used as a baseline or fallback when custom
        /// typography is not specified.</remarks>
        private static readonly Typography DefaultTypography = new()
        {
            Default = new DefaultTypography()
            {
                FontFamily = ["Montserrat", "Helvetica", "Arial", "sans-serif"],
                FontSize = ".875rem",
                FontWeight = "400",
                LineHeight = "1.43",
                LetterSpacing = ".01071em"
            },
            H1 = new H1Typography()
            {
                FontFamily = ["Montserrat", "Helvetica", "Arial", "sans-serif"],
                FontSize = "6rem",
                FontWeight = "300",
                LineHeight = "1.167",
                LetterSpacing = "-.01562em"
            },
            H2 = new H2Typography()
            {
                FontFamily = ["Montserrat", "Helvetica", "Arial", "sans-serif"],
                FontSize = "3.75rem",
                FontWeight = "300",
                LineHeight = "1.2",
                LetterSpacing = "-.00833em"
            },
            H3 = new H3Typography()
            {
                FontFamily = ["Montserrat", "Helvetica", "Arial", "sans-serif"],
                FontSize = "3rem",
                FontWeight = "400",
                LineHeight = "1.167",
                LetterSpacing = "0"
            },
            H4 = new H4Typography()
            {
                FontFamily = ["Montserrat", "Helvetica", "Arial", "sans-serif"],
                FontSize = "2.125rem",
                FontWeight = "400",
                LineHeight = "1.235",
                LetterSpacing = ".00735em"
            },
            H5 = new H5Typography()
            {
                FontFamily = ["Montserrat", "Helvetica", "Arial", "sans-serif"],
                FontSize = "1.5rem",
                FontWeight = "400",
                LineHeight = "1.334",
                LetterSpacing = "0"
            },
            H6 = new H6Typography()
            {
                FontFamily = ["Montserrat", "Helvetica", "Arial", "sans-serif"],
                FontSize = "1.25rem",
                FontWeight = "400",
                LineHeight = "1.6",
                LetterSpacing = ".0075em"
            },
            Button = new ButtonTypography()
            {
                FontFamily = ["Montserrat", "Helvetica", "Arial", "sans-serif"],
                FontSize = ".875rem",
                FontWeight = "500",
                LineHeight = "1.75",
                LetterSpacing = ".02857em"
            },
            Body1 = new Body1Typography()
            {
                FontFamily = ["Montserrat", "Helvetica", "Arial", "sans-serif"],
                FontSize = "1rem",
                FontWeight = "400",
                LineHeight = "1.5",
                LetterSpacing = ".00938em"
            },
            Body2 = new Body2Typography()
            {
                FontFamily = ["Montserrat", "Helvetica", "Arial", "sans-serif"],
                FontSize = ".875rem",
                FontWeight = "400",
                LineHeight = "1.43",
                LetterSpacing = ".01071em"
            },
            Caption = new CaptionTypography()
            {
                FontFamily = ["Montserrat", "Helvetica", "Arial", "sans-serif"],
                FontSize = ".75rem",
                FontWeight = "400",
                LineHeight = "1.66",
                LetterSpacing = ".03333em"
            },
            Subtitle2 = new Subtitle2Typography()
            {
                FontFamily = ["Montserrat", "Helvetica", "Arial", "sans-serif"],
                FontSize = ".875rem",
                FontWeight = "500",
                LineHeight = "1.57",
                LetterSpacing = ".00714em"
            }
        };

        /// <summary>
        /// Provides the default layout properties used when no custom values are specified.
        /// </summary>
        /// <remarks>This instance defines baseline layout settings, such as the default border radius,
        /// that are applied throughout the application unless overridden. Use this object to access standard layout
        /// values for consistent styling.</remarks>
        private static readonly LayoutProperties DefaultLayoutProperties = new()
        {
            DefaultBorderRadius = "3px"
        };

        /// <summary>
        /// Provides a predefined light color theme for MudBlazor components.
        /// </summary>
        /// <remarks>This theme configures palette, typography, and layout properties suitable for
        /// applications that require a light visual appearance. It can be assigned to the Theme property of MudBlazor
        /// components to apply consistent styling across the application.</remarks>
        public static MudTheme LightTheme = new()
        {

            PaletteLight = new PaletteLight()
            {
                Primary = "#1a2148",
                AppbarBackground = "#1a2148",
                Background = Colors.Gray.Lighten5,
                DrawerBackground = "#FFF",
                DrawerText = "rgba(0,0,0, 0.7)",
                Success = "#007E33"
            },
            Typography = DefaultTypography,
            LayoutProperties = DefaultLayoutProperties
        };

        /// <summary>
        /// Provides a predefined dark color theme for use with MudBlazor components.
        /// </summary>
        /// <remarks>This theme applies a dark color palette, typography, and layout properties suitable
        /// for applications that require a dark user interface. It can be assigned to the Theme property of MudBlazor
        /// components to enable consistent dark styling across the application.</remarks>
        public static MudTheme DarkTheme = new()
        {
            PaletteDark = new PaletteDark()
            {
                Primary = "#1a2148",
                Success = "#007E33",
                Black = "#27272f",
                Background = "#32333d",
                BackgroundGray = "#27272f",
                Surface = "#373740",
                DrawerBackground = "#27272f",
                DrawerText = "rgba(255,255,255, 0.50)",
                AppbarBackground = "#373740",
                AppbarText = "rgba(255,255,255, 0.70)",
                TextPrimary = "rgba(255,255,255, 0.70)",
                TextSecondary = "rgba(255,255,255, 0.50)",
                ActionDefault = "#adadb1",
                ActionDisabled = "rgba(255,255,255, 0.26)",
                ActionDisabledBackground = "rgba(255,255,255, 0.12)",
                DrawerIcon = "rgba(255,255,255, 0.50)"
            },
            Typography = DefaultTypography,
            LayoutProperties = DefaultLayoutProperties
        };
    }
}
