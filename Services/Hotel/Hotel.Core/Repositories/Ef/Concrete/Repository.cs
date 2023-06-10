using Hotel.Core.Repositories.Ef.Abstract;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Enums;
using System.Linq.Expressions;
using System.Net;
using Hotel.Core.Contexts.Ef;

namespace Hotel.Core.Repositories.Ef.Concrete;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly HotelDbContext _dbContext;

    public Repository(HotelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<TEntity>> GetByIdAsync(int id)
    {
        try
        {
            var response = await _dbContext.Set<TEntity>().FindAsync(id);
            return response is null
                ? Response<TEntity>.Fail(ErrorCodes.NotFound, HttpStatusCode.NotFound)
                : Response<TEntity>.Success(response);
        }
        catch (Exception)
        {
            return Response<TEntity>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<TEntity>> GetByPredicate(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var response = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
            return response is null
                ? Response<TEntity>.Fail(ErrorCodes.NotFound, HttpStatusCode.NotFound)
                : Response<TEntity>.Success(response);
        }
        catch (Exception)
        {
            return Response<TEntity>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<PaginatedList<TEntity>>> GetAllAsync(int pageIndex, int pageSize)
    {
        try
        {
            var response = await PaginatedList<TEntity>.CreateAsync(_dbContext.Set<TEntity>(), pageIndex, pageSize);
            return !response.Items.Any()
                ? Response<PaginatedList<TEntity>>.Fail(ErrorCodes.NotFound, HttpStatusCode.NotFound)
                : Response<PaginatedList<TEntity>>.Success(response);
        }
        catch (Exception)
        {
            return Response<PaginatedList<TEntity>>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<PaginatedList<TEntity>>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize)
    {
        try
        {
            var response = await PaginatedList<TEntity>.CreateAsync(_dbContext.Set<TEntity>().Where(predicate), pageIndex, pageSize);
            return !response.Items.Any()
                ? Response<PaginatedList<TEntity>>.Fail(ErrorCodes.NotFound, HttpStatusCode.NotFound)
                : Response<PaginatedList<TEntity>>.Success(response);
        }
        catch (Exception)
        {
            return Response<PaginatedList<TEntity>>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<int>> CountAsync()
    {
        try
        {
            return Response<int>.Success(await _dbContext.Set<TEntity>().CountAsync());
        }
        catch (Exception)
        {
            return Response<int>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<int>> CountWhereAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            return Response<int>.Success(await _dbContext.Set<TEntity>().CountAsync(predicate));
        }
        catch (Exception)
        {
            return Response<int>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<TEntity>> AddAsync(TEntity entity)
    {
        try
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return Response<TEntity>.Success(entity);
        }
        catch (Exception)
        {
            return Response<TEntity>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<TEntity>> UpdateAsync(TEntity entity)
    {
        try
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Response<TEntity>.Success(entity);
        }
        catch (Exception)
        {
            return Response<TEntity>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<TEntity>> GetByIdAsync(Guid id)
    {
        try
        {
            var response = await _dbContext.Set<TEntity>().FindAsync(id);
            return response is null
                ? Response<TEntity>.Fail(ErrorCodes.NotFound, HttpStatusCode.NotFound)
                : Response<TEntity>.Success(response);
        }
        catch (Exception)
        {
            return Response<TEntity>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<bool>> RemoveAsync(Guid id)
    {
        try
        {
            var entity = await GetByIdAsync(id);
            if (entity.Data == null) return Response<bool>.Success(false);
            _dbContext.Entry(entity.Data).Property("IsDeleted").CurrentValue = true;
            _dbContext.Entry(entity.Data).Property("IsActive").CurrentValue = false;
            _dbContext.Entry(entity.Data).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Response<bool>.Success(true);
        }
        catch (Exception)
        {
            return Response<bool>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<bool>> RemoveAsync(int id)
    {
        try
        {
            var entity = await GetByIdAsync(id);
            if (entity.Data == null) return Response<bool>.Success(false);
            _dbContext.Entry(entity.Data).Property("IsDeleted").CurrentValue = true;
            _dbContext.Entry(entity.Data).Property("IsActive").CurrentValue = false;
            _dbContext.Entry(entity.Data).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Response<bool>.Success(true);
        }
        catch (Exception)
        {
            return Response<bool>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }
}
