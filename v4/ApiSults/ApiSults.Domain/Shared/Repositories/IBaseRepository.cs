using ApiSults.Domain.Shared;

namespace ApiSults.Domain.Shared.Repositories;

public interface IBaseRepository<T> where T : Entity
{
    Task AddAsync(T entity);
    Task<T> GetByIdAsync(long id);
    Task AddListAsync(IEnumerable<T> entities);
    void RemoveRange(IEnumerable<T> entities);
    void Remove(T entity);
    void Update(T entity);
}
