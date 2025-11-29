using ConectOne.Domain.RequestFeatures;
using FilingModule.Domain.Enums;

namespace FilingModule.Domain.RequestFeatures;

/// <summary>
///     Encapsulates paging, filtering and sorting input for client requests targeting
///     image‑based resources.  In addition to the base paging fields inherited from
///     <see cref="UploadType"/>, the class exposes strongly‑typed properties
///     that allow callers to narrow the result set to <i>featured</i> images and/or
///     limit responses to a specific <see cref="Feautured"/>.
/// </summary>
/// <remarks>
///     <para>
///         <b>Why nullable?</b> Both <see cref="ImageType"/> and <see cref="RequestParameters"/>
///         are declared as nullable so that callers can omit them entirely—in which
///         case no additional filter clause is generated—rather than forcing the
///         caller to explicitly pass sentinel values (e.g. <c>false</c> or <c>0</c>).
///     </para>
/// </remarks>
public sealed class ImagePageParameters : RequestParameters
{
    /// <summary>
    ///     When <see langword="true"/> the query engine will return <i>only</i> images
    ///     whose <c>IsFeatured</c> flag is set; when <see langword="false"/> it will
    ///     return <i>only</i> non‑featured images.  If <see langword="null"/> (default)
    ///     the "featured" dimension is ignored entirely.
    /// </summary>
    public bool? Feautured { get; set; }

    /// <summary>
    ///     Filters the result set to a specific <see cref="UploadType"/> (e.g. <c>Avatar</c>,
    ///     <c>Banner</c>, <c>Document</c>).  Supplying <see langword="null"/> disables
    ///     the filter.
    /// </summary>
    public UploadType? ImageType { get; set; }
}