using System.Text;

namespace ConectOne.Domain.Mailing.Extensions
{
    /// <summary>
    /// Provides extension methods for processing and loading mailing templates, including token replacement and
    /// embedded resource retrieval.
    /// </summary>
    public static class MailingExtensions
    {
        /// <summary>
        /// Performs token replacements in the given template string.
        /// </summary>
        /// <param name="template">The template string containing tokens to replace.</param>
        /// <param name="replacements">A dictionary mapping tokens (keys) to their replacement values.</param>
        /// <returns>The template with all specified tokens replaced by their corresponding values.</returns>
        public static string ReplaceTokens(string template, Dictionary<string, string> replacements)
        {
            foreach (var kvp in replacements)
            {
                template = template.Replace(kvp.Key, kvp.Value);
            }
            return template;
        }

        /// <summary>
        /// Loads an embedded template from the specified assembly and resource file name.
        /// </summary>
        /// <param name="assembly">The assembly containing the embedded resource template.</param>
        /// <param name="templateFile">The name of the resource file containing the template.</param>
        /// <returns>A string containing the loaded template content.</returns>
        /// <exception cref="Exception">Throws if the template file cannot be found.</exception>
        public static async Task<string> LoadTemplateAsync(System.Reflection.Assembly assembly, string templateFile)
        {
            // Attempt to retrieve the embedded resource stream.
            using var stream = assembly.GetManifestResourceStream(templateFile);
            if (stream is null)
                throw new Exception($"Template file '{templateFile}' not found in assembly '{assembly.FullName}'.");

            using var reader = new StreamReader(stream, Encoding.UTF8);
            return await reader.ReadToEndAsync();
        }
    }
}
