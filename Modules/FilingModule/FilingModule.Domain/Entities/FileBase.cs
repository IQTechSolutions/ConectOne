using System.ComponentModel.DataAnnotations;
using ConectOne.Domain.Entities;

namespace FilingModule.Domain.Entities
{
    /// <summary>
    /// Represents the base class for file entities, providing common properties and methods
    /// for handling file metadata such as name, type, size, and paths.
    /// </summary>
    public class FileBase : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileBase"/> class.
        /// </summary>
        protected FileBase() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileBase"/> class with the specified filename, type, and size.
        /// </summary>
        /// <param name="filename">The name of the file.</param>
        /// <param name="type">The MIME type of the file.</param>
        /// <param name="size">The size of the file in bytes.</param>
        protected FileBase(string filename, string type, long size)
        {
            FileName = Path.GetFileNameWithoutExtension(filename) + "_" + Guid.NewGuid() + Path.GetExtension(filename);
            ContentType = type;
            Size = size;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileBase"/> class with the specified filename, type, size, and path.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="type">The MIME type of the file.</param>
        /// <param name="length">The size of the file in bytes.</param>
        /// <param name="path">The relative path where the file is stored.</param>
        protected FileBase(string fileName, string type, long length, string path)
        {
            FileName = Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid() + Path.GetExtension(fileName);
            ContentType = type;
            Size = length;
            RelativePath = Path.Combine(path, FileName);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the display name of the file.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        [Required]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the MIME type of the file.
        /// </summary>
        public string? ContentType { get; set; }

        /// <summary>
        /// Gets or sets the size of the file in bytes.
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the relative path where the file is stored.
        /// </summary>
        public string? RelativePath { get; set; }

        #endregion
    }
}
