namespace Domain.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        TEntity Get(TKey id);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        Tuple<bool, TEntity> Create(TEntity item);

        Tuple<bool, TEntity> Update(TEntity item);

        bool Delete(TEntity item);
    }
}