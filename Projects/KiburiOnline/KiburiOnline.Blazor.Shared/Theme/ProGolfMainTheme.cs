using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Theme
{
    /// <summary>
    /// Provides the main visual theme for the ProGolf application, including customized color palette, typography,
    /// layout properties, and component styling.
    /// </summary>
    /// <remarks>This theme is designed for use with MudBlazor components and applies a distinctive look and
    /// feel tailored to the ProGolf brand. It sets specific colors, font families, and layout settings to ensure a
    /// consistent user experience across the application. To apply this theme, assign an instance of <see
    /// cref="ProGolfMainTheme"/> to the MudBlazor theme provider in your application's setup.</remarks>
    public class ProGolfMainTheme : MudTheme
    {
        /// <summary>
        /// Initializes a new instance of the ProGolfMainTheme class with predefined color palette, layout, typography,
        /// shadow, and z-index settings for the Pro Golf application theme.
        /// </summary>
        /// <remarks>This constructor sets up default visual styles, including colors, font families, font
        /// sizes, and other UI properties tailored for a professional golf application. The initialized theme can be
        /// used to ensure consistent appearance across the application's components.</remarks>
        public ProGolfMainTheme()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = "F90",
                Secondary = Colors.DeepPurple.Accent2,
                Background = "#fff",
                Tertiary = "#fdb714",
                Warning = Colors.Red.Accent2,
                AppbarBackground = Colors.Blue.Darken1,
                DrawerBackground = "#FFF",
                DrawerText = "rgba(0,0,0, 0.7)",
                Success = "#06d79c"
            };

            LayoutProperties = new LayoutProperties()
            {
                DefaultBorderRadius = "3px"
            };

            Typography = new Typography()
            {
                Default = new DefaultTypography()
                {
                    FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                    FontSize = ".875rem",
                    FontWeight = "400",
                    LineHeight = "1.43",
                    LetterSpacing = ".01071em"
                },
                H1 = new H1Typography()
                {
                    FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                    FontSize = "6rem",
                    FontWeight = "300",
                    LineHeight = "1.167",
                    LetterSpacing = "-.01562em"
                },
                H2 = new H2Typography()
                {
                    FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                    FontSize = "3.75rem",
                    FontWeight = "300",
                    LineHeight = "1.2",
                    LetterSpacing = "-.00833em"
                },
                H3 = new H3Typography()
                {
                    FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                    FontSize = "3rem",
                    FontWeight = "400",
                    LineHeight = "1.167",
                    LetterSpacing = "0"
                },
                H4 = new H4Typography()
                {
                    FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                    FontSize = "2.125rem",
                    FontWeight = "400",
                    LineHeight = "1.235",
                    LetterSpacing = ".00735em"
                },
                H5 = new H5Typography()
                {
                    FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                    FontSize = "1.5rem",
                    FontWeight = "400",
                    LineHeight = "1.334",
                    LetterSpacing = "0"
                },
                H6 = new H6Typography()
                {
                    FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                    FontSize = "1.25rem",
                    FontWeight = "400",
                    LineHeight = "1.6",
                    LetterSpacing = ".0075em"
                },
                Button = new ButtonTypography()
                {
                    FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                    FontSize = ".875rem",
                    FontWeight = "500",
                    LineHeight = "1.75",
                    LetterSpacing = ".02857em"
                },
                Body1 = new Body1Typography()
                {
                    FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                    FontSize = "1rem",
                    FontWeight = "400",
                    LineHeight = "1.5",
                    LetterSpacing = ".00938em"
                },
                Body2 = new Body2Typography()
                {
                    FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                    FontSize = ".875rem",
                    FontWeight = "400",
                    LineHeight = "1.43",
                    LetterSpacing = ".01071em"
                },
                Caption = new CaptionTypography()
                {
                    FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                    FontSize = ".75rem",
                    FontWeight = "400",
                    LineHeight = "1.66",
                    LetterSpacing = ".03333em"
                },
                Subtitle2 = new Subtitle2Typography()
                {
                    FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                    FontSize = ".875rem",
                    FontWeight = "500",
                    LineHeight = "1.57",
                    LetterSpacing = ".00714em"
                }
            };
            Shadows = new Shadow();
            ZIndex = new ZIndex();
        }
    }
}
