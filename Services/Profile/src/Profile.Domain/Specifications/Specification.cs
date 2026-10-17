using System.Linq.Expressions;

namespace Clinic.Profile.Domain.Specifications;

public abstract class Specification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; private set; }

    public Expression<Func<T, object>>? OrderBy { get; private set; }

    public Expression<Func<T, object>>? ThenBy { get; private set; }

    public bool AsNoTracking { get; protected set; } = true;

    protected void AddCriteria(Expression<Func<T, bool>> criteria)
    {
        Criteria = Criteria is null
            ? criteria
            : Criteria.AndAlso(criteria);
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void ApplyThenBy(Expression<Func<T, object>> thenByExpression)
    {
        ThenBy = thenByExpression;
    }
}
