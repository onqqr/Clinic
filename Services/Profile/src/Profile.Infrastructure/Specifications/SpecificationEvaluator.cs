using Clinic.Profile.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Profile.Infrastructure.Specifications;

public static class SpecificationEvaluator
{
    public static IQueryable<T> GetQuery<T>(
        IQueryable<T> inputQuery,
        ISpecification<T> specification)
        where T : class
    {
        var query = inputQuery;

        if (specification.AsNoTracking)
        {
            query = query.AsNoTracking();
        }

        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }

        if (specification.OrderBy is not null)
        {
            var orderedQuery = query.OrderBy(specification.OrderBy);

            query = specification.ThenBy is not null
                ? orderedQuery.ThenBy(specification.ThenBy)
                : orderedQuery;
        }

        return query;
    }
}
