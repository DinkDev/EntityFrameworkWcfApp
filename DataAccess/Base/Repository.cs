namespace DataAccess.Base
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Domain.DataAccess;
    using Domain.Models.Base;

    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected DbContext Context { get; }
        protected bool UsingUow { get; set; }

        public Repository(DbContext context, bool usingUow = false)
        {
            Context = context;
            UsingUow = usingUow;
        }

        public async Task<TEntity> GetAsync(TKey id)
        {
            var key = id is IComplexKey idArray ? idArray.ToArray() : new object[] { id };

            return await Context.Set<TEntity>().FindAsync(key).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().Where(predicate).ToListAsync().ConfigureAwait(false);
        }

        public async Task<Tuple<bool, TEntity>> CreateAsync(TEntity item)
        {
            var entry = Context.Set<TEntity>().Add(item);

            if (UsingUow)
            {
                var added = await Context.SaveChangesAsync().ConfigureAwait(false);
                return Tuple.Create(added > 0, entry);
            }

            return Tuple.Create(true, item);
        }

        public async Task<Tuple<bool, TEntity>> UpdateAsync(TEntity item)
        {
            Context.Set<TEntity>().Attach(item);
            var entry = Context.Entry(item);
            entry.State = EntityState.Modified;

            if (UsingUow)
            {
                var updated = await Context.SaveChangesAsync().ConfigureAwait(false);
                return Tuple.Create(updated > 0, entry.Entity);
            }

            return Tuple.Create(true, item);
        }

        public async Task<bool> DeleteAsync(TEntity item)
        {
            Context.Set<TEntity>().Remove(item);

            if (UsingUow)
            {
                var deleted = await Context.SaveChangesAsync().ConfigureAwait(false);
                return deleted > 0;
            }

            return true;
        }
    }
}