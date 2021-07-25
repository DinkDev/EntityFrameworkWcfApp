namespace Domain.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        Task<TEntity> GetAsync(TKey id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task<Tuple<bool, TEntity>> CreateAsync(TEntity item);

        Task<Tuple<bool, TEntity>> UpdateAsync(TEntity item);

        Task<bool> DeleteAsync(TEntity item);
    }
}