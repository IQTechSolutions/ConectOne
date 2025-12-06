using System.Xml.Linq;

namespace ConectOne.Domain.Interfaces;

/// <summary>
/// Generates sitemap documents that are compatible with the <see href="https://www.sitemaps.org/protocol.html">Sitemaps protocol</see>.
/// </summary>
public interface ISitemapGenerator
{
    /// <summary>
    /// Builds a sitemap document for the current application.
    /// </summary>
    /// <param name="baseUri">The base URI of the executing request (e.g. <c>https://www.example.com/</c>).</param>
    /// <param name="cancellationToken">Token that can be used to cancel the work.</param>
    /// <returns>An <see cref="XDocument"/> representing the sitemap.</returns>
    Task<XDocument> GenerateAsync(Uri baseUri, CancellationToken cancellationToken = default);
}
