using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using AccomodationModule.Domain.Entities;
using ConectOne.Domain.Interfaces;
using ConectOne.EntityFrameworkCore.Sql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Progolf.Blazor.ServerClient;

using AssemblyReference = KiburiOnline.Blazor.Shared.AssemblyReference;

/// <summary>
/// Default implementation of <see cref="ISitemapGenerator"/> that discovers static Blazor routes
/// and augments them with dynamic content sourced from the database.
/// </summary>
public sealed class SitemapGenerator(GenericDbContextFactory dbContextFactory) : ISitemapGenerator
{
    private readonly GenericDbContextFactory _dbContextFactory = dbContextFactory;

    /// <summary>
    /// Contains the set of assemblies to be scanned for relevant types or resources.
    /// </summary>
    private static readonly Assembly[] AssembliesToScan =
    [
        typeof(Program).Assembly,
        typeof(AssemblyReference).Assembly
    ];

    /// <summary>
    /// Represents the XML namespace for the standard sitemap protocol as defined by sitemaps.org.
    /// </summary>
    /// <remarks>This namespace is used when generating or parsing XML documents that conform to the sitemap
    /// protocol specification. It ensures that elements are correctly identified as part of the sitemap
    /// schema.</remarks>
    private static readonly XNamespace SitemapNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";

    /// <summary>
    /// Represents a compiled regular expression used to match parameterized route segments enclosed in curly braces.
    /// </summary>
    /// <remarks>This regular expression is intended to identify route patterns that contain parameters, such
    /// as "/users/{id}". It matches any substring enclosed in curly braces within a route template.</remarks>
    private static readonly Regex ParameterisedRouteMatcher = new("[{].+[}]", RegexOptions.Compiled);

    /// <summary>
    /// Contains the list of route prefixes that are considered private or restricted within the application.
    /// </summary>
    /// <remarks>These route prefixes are typically used to identify endpoints that require authentication or
    /// special access permissions. The list can be used to enforce access control or to filter requests based on their
    /// URL path.</remarks>
    private static readonly string[] PrivateRoutePrefixes =
    [
        "/Account",
        "/login",
        "/signin",
        "/register",
        "/sign-up",
        "/404",
        "/access-denied",
        "/airports",
        "/cities",
        "/countries",
        "/custom-variable-tags",
        "/customerQueries",
        "/day-tour-activity-templates",
        "/dd",
        "/destinations/create",
        "/Error",
        "/Error2",
        "/forgot-password",
        "/gallery",
        "/general-information-templates",
        "/gifts",
        "/golfcourses",
        "/golfcourses/create",
        "/lodgings",
        "/lodgings/create",
        "/lodgingTypes",
        "/meal-addition-templates",
        "/meet-and-greet-templates",
        "/packages",
        "/packages/create",
        "/restaurants",
        "/reviews/create",
        "/reviews",
        "/short-description-templates",
        "/terms-and-conditions-templates",
        "/thank-you",
        "/vacation-title-templates",
        "/vacationhosts",
        "/vacationhosts/create",
        "/vacations/summaries",
        "/testimonials/all",
        "/pdfProposals"
    ];

    /// <summary>
    /// Generates an XML sitemap document containing URLs discovered from the application.
    /// </summary>
    /// <remarks>The generated sitemap conforms to the standard sitemap protocol and includes the location and
    /// last modification date for each discovered route, if available.</remarks>
    /// <param name="baseUri">The base URI to use as the root for all URLs in the generated sitemap. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an XDocument representing the
    /// generated sitemap in XML format.</returns>
    public async Task<XDocument> GenerateAsync(Uri baseUri, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(baseUri);

        var baseUrl = baseUri.ToString().TrimEnd('/');
        var routes = await DiscoverRoutesAsync(cancellationToken).ConfigureAwait(false);

        var urlSet = new XElement(SitemapNamespace + "urlset");

        foreach (var (path, lastModified) in routes.OrderBy(r => r.Path, StringComparer.OrdinalIgnoreCase))
        {
            var urlElement = new XElement(SitemapNamespace + "url", new XElement(SitemapNamespace + "loc", $"{baseUrl}{path}"));

            if (lastModified is not null)
            {
                urlElement.Add(new XElement(SitemapNamespace + "lastmod", FormatAsSitemapDate(lastModified.Value)));
            }

            urlSet.Add(urlElement);
        }

        return new XDocument(urlSet);
    }

    /// <summary>
    /// Discovers all available application routes and returns them as a collection of sitemap entries.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only collection of <see cref="SitemapEntry"/> objects representing the discovered routes. The collection
    /// is empty if no routes are found.</returns>
    private async Task<IReadOnlyCollection<SitemapEntry>> DiscoverRoutesAsync(CancellationToken cancellationToken)
    {
        var routes = new Dictionary<string, DateTime?>(StringComparer.OrdinalIgnoreCase);

        foreach (var assembly in AssembliesToScan)
        {
            foreach (var route in DiscoverStaticRoutes(assembly))
            {
                if (!routes.ContainsKey(route))
                {
                    routes.Add(route, null);
                }
            }
        }

        await AddPackageRoutesAsync(routes, cancellationToken).ConfigureAwait(false);

        return routes.Select(kvp => new SitemapEntry(kvp.Key, kvp.Value)).ToList();
    }

    /// <summary>
    /// Discovers all static route templates defined by non-abstract Blazor components in the specified assembly that do
    /// not require authorization.
    /// </summary>
    /// <remarks>Only components that inherit from ComponentBase, are not abstract, and do not have an
    /// authorization requirement are considered. Route templates are included only if they meet specific inclusion
    /// criteria.</remarks>
    /// <param name="assembly">The assembly to scan for Blazor components with static route templates. Cannot be null.</param>
    /// <returns>An enumerable collection of route template strings for components that do not require authorization. The
    /// collection is empty if no matching routes are found.</returns>
    private static IEnumerable<string> DiscoverStaticRoutes(Assembly assembly)
    {
        var componentTypes = assembly
            .ExportedTypes
            .Where(type => typeof(ComponentBase).IsAssignableFrom(type) && !type.IsAbstract);

        foreach (var component in componentTypes)
        {
            if (HasAuthorizationRequirement(component))
            {
                continue;
            }

            foreach (var attribute in component.GetCustomAttributes<RouteAttribute>())
            {
                if (!ShouldIncludeRoute(attribute.Template))
                {
                    continue;
                }

                yield return NormaliseRoute(attribute.Template!);
            }
        }
    }

    /// <summary>
    /// Determines whether the specified member is decorated with an <see cref="AuthorizeAttribute"/>.
    /// </summary>
    /// <param name="component">The reflection metadata for the member to inspect. Cannot be null.</param>
    /// <returns>true if the member has an <see cref="AuthorizeAttribute"/> applied; otherwise, false.</returns>
    private static bool HasAuthorizationRequirement(MemberInfo component) =>
        component.GetCustomAttributes(inherit: true).OfType<AuthorizeAttribute>().Any();

    /// <summary>
    /// Determines whether a route template should be included based on its format and content.
    /// </summary>
    /// <remarks>A route template is excluded if it is null, empty, consists only of whitespace, matches a
    /// parameterized route pattern, or begins with a prefix designated as private. This method is typically used to
    /// filter out routes that should not be exposed or processed further.</remarks>
    /// <param name="template">The route template to evaluate. Can be null or empty.</param>
    /// <returns>true if the route template is non-empty, does not match a parameterized pattern, and does not start with a
    /// private route prefix; otherwise, false.</returns>
    private static bool ShouldIncludeRoute(string? template)
    {
        if (string.IsNullOrWhiteSpace(template))
        {
            return false;
        }

        if (ParameterisedRouteMatcher.IsMatch(template))
        {
            return false;
        }

        if (PrivateRoutePrefixes.Any(prefix => template.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Normalizes a route template by ensuring it begins with a leading slash ('/').
    /// </summary>
    /// <param name="template">The route template to normalize. Leading and trailing whitespace is ignored.</param>
    /// <returns>A normalized route template that starts with a leading slash. If the input is already normalized, it is returned
    /// unchanged.</returns>
    private static string NormaliseRoute(string template)
    {
        var trimmed = template.Trim();
        return trimmed.StartsWith('/') ? trimmed : $"/{trimmed}";
    }

    /// <summary>
    /// Adds or updates package route entries in the specified dictionary with the latest available package slugs and
    /// their last modified dates.
    /// </summary>
    /// <remarks>Only packages that are published, have a non-empty slug, and are not past their availability
    /// cut-off date are included. Existing entries in the dictionary for the same route will be overwritten.</remarks>
    /// <param name="routes">A dictionary mapping route paths to their corresponding last modified dates. Existing entries for package routes
    /// will be updated; new routes will be added as needed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task AddPackageRoutesAsync(Dictionary<string, DateTime?> routes, CancellationToken cancellationToken)
    {
        await using var context = _dbContextFactory.CreateDbContext();

        var today = DateTime.UtcNow.Date;

        var packages = await context.Set<Vacation>()
            .AsNoTracking()
            .Where(v => v.Published && !string.IsNullOrWhiteSpace(v.Slug) &&
                        (v.AvailabilityCutOffDate == null || v.AvailabilityCutOffDate.Value.Date >= today))
            .Select(v => new { v.Slug, v.LastModifiedOn })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        foreach (var package in packages)
        {
            var path = $"/packages/{package.Slug!.Trim()}";
            routes[path] = package.LastModifiedOn ?? routes.GetValueOrDefault(path);
        }
    }

    /// <summary>
    /// Formats the specified date and time as a string in the W3C Datetime format used by XML sitemaps.
    /// </summary>
    /// <remarks>This format is required for the <lastmod> element in XML sitemaps, as specified by the
    /// sitemap protocol. The returned string always represents the time in UTC.</remarks>
    /// <param name="dateTime">The date and time value to format. The value is converted to Coordinated Universal Time (UTC) before formatting.</param>
    /// <returns>A string representation of the date and time in the format "yyyy-MM-ddTHH:mm:ssZ", suitable for use in XML
    /// sitemaps.</returns>
    private static string FormatAsSitemapDate(DateTime dateTime) => dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

    /// <summary>
    /// Represents an entry in a sitemap, including its URL path and the date it was last modified.
    /// </summary>
    /// <param name="Path">The relative or absolute URL path of the sitemap entry. Cannot be null or empty.</param>
    /// <param name="LastModified">The date and time when the sitemap entry was last modified, or null if unknown.</param>
    private sealed record SitemapEntry(string Path, DateTime? LastModified);
}
