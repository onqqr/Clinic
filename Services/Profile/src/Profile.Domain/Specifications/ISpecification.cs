using System.Linq.Expressions;

namespace Clinic.Profile.Domain.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; }

    Expression<Func<T, object>>? OrderBy { get; }

    Expression<Func<T, object>>? ThenBy { get; }

    bool AsNoTracking { get; }
}
