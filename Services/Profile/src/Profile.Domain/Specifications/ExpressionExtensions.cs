using System.Linq.Expressions;

namespace Clinic.Profile.Domain.Specifications;

internal static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> AndAlso<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(T), "x");

        var leftBody = new ReplaceParameterVisitor(left.Parameters[0], parameter)
            .Visit(left.Body);
        var rightBody = new ReplaceParameterVisitor(right.Parameters[0], parameter)
            .Visit(right.Body);

        var body = Expression.AndAlso(leftBody!, rightBody!);

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    private sealed class ReplaceParameterVisitor(
        ParameterExpression source,
        ParameterExpression target) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == source ? target : base.VisitParameter(node);
        }
    }
}
