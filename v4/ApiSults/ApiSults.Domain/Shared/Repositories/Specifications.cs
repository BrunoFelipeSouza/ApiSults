using System.Linq.Expressions;

namespace ApiSults.Domain.Shared.Repositories;

public class Specification<T> : ISpecification<T>
{
    private Expression<Func<T, bool>> _expression = x => true;

    public Expression<Func<T, bool>> ToExpression() => _expression;

    public Specification<T> IsEqual(Expression<Func<T, object>> property, object value)
        => IsEqualIfHasValue(property, value);

    public Specification<T> IsEqualIfHasValue(Expression<Func<T, object>> property, object? value)
    {
        if (value is null) return this;

        var propertyType = property.Body.Type;

        var equalExpression = Expression.Equal(property.Body, Expression.Constant(value, propertyType));
        var parameter = _expression.Parameters[0];
        var updatedBody = new ParameterReplacer(property.Parameters[0], parameter).Visit(equalExpression);
        var combinedExpression = Expression.AndAlso(_expression.Body, updatedBody!);

        _expression = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);

        return this;
    }

    public Specification<T> IsBetween<TProp>(
        Expression<Func<T, TProp?>> property,
        TProp start,
        TProp end) where TProp : struct, IComparable<TProp>
        => IsBetweenIfHasValue(property, start, end);

    public Specification<T> IsBetweenIfHasValue<TProp>(
        Expression<Func<T, TProp?>> property,
        TProp? start,
        TProp? end) where TProp : struct, IComparable<TProp>
    {
        var parameter = _expression.Parameters[0];
        Expression? updatedBody = _expression.Body;

        if (start.HasValue)
        {
            var startExpression = Expression.GreaterThanOrEqual(property.Body, Expression.Constant(start.Value));
            updatedBody = Expression.AndAlso(updatedBody!, new ParameterReplacer(property.Parameters[0], parameter).Visit(startExpression));
        }

        if (end.HasValue)
        {
            var endExpression = Expression.LessThanOrEqual(property.Body, Expression.Constant(end.Value));
            updatedBody = Expression.AndAlso(updatedBody!, new ParameterReplacer(property.Parameters[0], parameter).Visit(endExpression));
        }

        _expression = Expression.Lambda<Func<T, bool>>(updatedBody!, parameter);
        return this;
    }

    public Specification<T> IsLessThanOrEqual<TProp>(Expression<Func<T, TProp?>> property, TProp value) where TProp : struct, IComparable<TProp>
        => IsLessThanOrEqualIfHasValue(property, value);

    public Specification<T> IsLessThanOrEqualIfHasValue<TProp>(Expression<Func<T, TProp?>> property, TProp? value) where TProp : struct, IComparable<TProp>
    {
        if (value.HasValue)
        {
            var parameter = property.Parameters[0];
            var body = Expression.LessThanOrEqual(property.Body, Expression.Constant(value.Value, property.Body.Type));
            _expression = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(_expression.Body, body), parameter);
        }
        return this;
    }

    private sealed class ParameterReplacer(
        ParameterExpression oldParameter,
        ParameterExpression newParameter) : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter = oldParameter;
        private readonly ParameterExpression _newParameter = newParameter;

        protected override Expression VisitParameter(ParameterExpression node)
            => node == _oldParameter ? _newParameter : base.VisitParameter(node);
    }
}
