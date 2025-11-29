using System.Reflection;

namespace ConectOne.Domain.Mailing.TemplateSender
{
    /// <summary>
    /// Represents information about a template, including its associated assembly and template file path.
    /// </summary>
    public class TemplateInfo
    {
        public TemplateInfo(Assembly assembly, string templateFile)
        {
            Assembly = assembly;
            TemplateFile = templateFile;
        }

        /// <summary>
        /// Gets or sets the assembly associated with this instance.
        /// </summary>
        public Assembly Assembly { get; set; } = null!;

        /// <summary>
        /// Gets or sets the file path of the template to be used.
        /// </summary>
        public string TemplateFile { get; set; } = null!;
    }
}
