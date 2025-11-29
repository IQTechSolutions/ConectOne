using ConectOne.Domain.RequestFeatures;

namespace FilingModule.Domain.RequestFeatures;

/// <summary>
///     Wraps paging, sorting and filtering options for document‑centric queries.  Consumers
///     can optionally scope the result set to a single company and/or restrict output to
///     documents that have been marked as publicly visible.
/// </summary>
/// <remarks>
///     Inherits common paging members such as <c>PageNumber</c>, <c>PageSize</c>, <c>OrderBy</c>,
///     and <c>SearchTerm</c> from <see cref="RequestParameters"/>.  Those fields are intentionally
///     located in a shared base class to avoid repetition across different <i>PageParameters</i>
///     flavours within the module.
/// </remarks>
public sealed class DocumentPageParameters : RequestParameters
{
    /// <summary>
    ///     Optional identifier of the owning company.  When supplied, the query will return
    ///     only documents whose <c>CompanyId</c> matches the provided value; when
    ///     <see langword="null"/> the filter is ignored and documents from <b>all</b> companies
    ///     are considered.
    /// </summary>
    public string? CompanyId { get; set; }

    /// <summary>
    ///     Indicates whether the consumer is interested exclusively in documents flagged as
    ///     public (<c>true</c>) or private (<c>false</c>).  The default value (<c>false</c>)
    ///     ensures that non‑public documents remain hidden unless explicitly requested.
    /// </summary>
    public bool IsPublic { get; set; } = false;
}