namespace FilingModule.Domain.Entities
{
    public class Document : FileBase
    {
        /// <summary>
        /// Gets or sets the description of the document file.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the document file is public.
        /// Defaults to <c>false</c>.
        /// </summary>
        public bool IsPublic { get; set; } = false;
    }
}
