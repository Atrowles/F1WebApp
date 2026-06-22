using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace F1WebAPI.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Add(TEntity t);
        Task<TEntity> GetById(long id);
        Task<TEntity> Delete(long id);
        Task<TEntity> Update(TEntity t);

        Task<bool> AddEntities(List<TEntity> entities);
        Task<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetWhereNoTracking(Expression<Func<TEntity, bool>> predicate);
    }
}
