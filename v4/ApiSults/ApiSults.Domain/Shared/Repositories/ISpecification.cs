using System.Linq.Expressions;

namespace ApiSults.Domain.Shared.Repositories;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> ToExpression();
}