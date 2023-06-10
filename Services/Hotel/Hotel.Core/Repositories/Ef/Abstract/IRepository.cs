using System.Linq.Expressions;
using Shared.Dtos;

namespace Hotel.Core.Repositories.Ef.Abstract;

public interface IRepository<TEntity>
{
    Task<Response<TEntity>> GetByIdAsync(Guid id);
    Task<Response<TEntity>> GetByIdAsync(int id);
    Task<Response<TEntity>> GetByPredicate(Expression<Func<TEntity, bool>> predicate);
    Task<Response<PaginatedList<TEntity>>> GetAllAsync(int pageIndex, int pageSize);
    Task<Response<PaginatedList<TEntity>>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, int pageIndex,
        int pageSize);
    Task<Response<int>> CountAsync();
    Task<Response<int>> CountWhereAsync(Expression<Func<TEntity, bool>> predicate);
    Task<Response<TEntity>> AddAsync(TEntity entity);
    Task<Response<TEntity>> UpdateAsync(TEntity entity);
    Task<Response<bool>> RemoveAsync(Guid id);
    Task<Response<bool>> RemoveAsync(int id);
}