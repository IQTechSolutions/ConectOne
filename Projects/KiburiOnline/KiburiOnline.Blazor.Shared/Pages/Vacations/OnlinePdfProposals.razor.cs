namespace KiburiOnline.Blazor.Shared.Pages.Vacations
{
    public partial class OnlinePdfProposals
    {
        private IEnumerable<string> entries;

        /// <summary>
        /// Initializes the component and populates the entries collection with directories from the current working
        /// directory, excluding hidden directories and build output folders.
        /// </summary>
        /// <remarks>This method is called by the framework during the component's initialization phase.
        /// Override this method to perform additional setup when the component is first created.</remarks>
        protected override void OnInitialized()
        {
            entries = Directory.GetDirectories(Directory.GetCurrentDirectory())
                .Where(entry =>
                {
                    var name = Path.GetFileName(entry);

                    return !name.StartsWith(".") && name != "bin" && name != "obj";
                });

        }
    }
}
