using BloggingModule.Domain.Entities;
using BloggingModule.Domain.RequestFeatures;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace BloggingModule.Infrastructure.Specifications;

/// <summary>
/// Specification used to filter and page <see cref="BlogPost"/> entities
/// based on provided <see cref="BlogPostPageParameters"/>.
/// </summary>
public sealed class PagedBlogPostSpecification : Specification<BlogPost>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedBlogPostSpecification"/> class.
    /// </summary>
    /// <param name="parameters">Filtering and paging options for blog posts.</param>
    /// <param name="applyPaging">Determines whether pagination should be applied.</param>
    public PagedBlogPostSpecification(BlogPostPageParameters parameters, bool applyPaging = false)
    {
        Criteria = PredicateBuilder.New<BlogPost>(true);

        if (!string.IsNullOrEmpty(parameters.CategoryId))
            Criteria = Criteria.And(p => p.Categories.Any(c => c.CategoryId == parameters.CategoryId));

        if (!string.IsNullOrWhiteSpace(parameters.SearchText))
            Criteria = Criteria.And(p => p.Title.ToLower().Contains(parameters.SearchText.ToLower()));

        if (parameters.StartDateFilter.HasValue)
            Criteria = Criteria.And(p => p.CreatedOn >= parameters.StartDateFilter);

        if (parameters.EndDateFilter.HasValue)
            Criteria = Criteria.And(p => p.CreatedOn <= parameters.EndDateFilter);

        AddInclude(q => q.Include(p => p.Images));

        if (!string.IsNullOrEmpty(parameters.OrderBy))
        {
            var parts = parameters.OrderBy.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            var property = parts[0];
            var desc = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);
            OrderBy = desc
                ? q => q.OrderByDescending(e => EF.Property<object>(e, property))
                : q => q.OrderBy(e => EF.Property<object>(e, property));
        }

        if (applyPaging)
        {
            Skip = (parameters.PageNr - 1) * parameters.PageSize;
            Take = parameters.PageSize;
        }
    }
}
