namespace DataAccess.Base
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using Domain.DataAccess;
    using Domain.Models.Base;

    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : EntityBase
    {
        protected DbContext Context { get; }
        protected bool UsingUow { get; set; }

        public Repository(DbContext context, bool usingUow = false)
        {
            Context = context;
            UsingUow = usingUow;
        }

        public TEntity Get(TKey id)
        {
            var key = id is IComplexKey idArray ? idArray.ToArray() : new object[] { id };

            return Context.Set<TEntity>().Find(key);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).ToList();
        }

        public Tuple<bool, TEntity> Create(TEntity item)
        {
            var entry = Context.Set<TEntity>().Add(item);

            if (UsingUow)
            {
                var added = Context.SaveChanges();
                return Tuple.Create(added > 0, entry);
            }

            return Tuple.Create(true, item);
        }

        public Tuple<bool, TEntity> Update(TEntity item)
        {
            Context.Set<TEntity>().Attach(item);
            var entry = Context.Entry(item);
            entry.State = EntityState.Modified;

            if (UsingUow)
            {
                var updated = Context.SaveChanges();
                return Tuple.Create(updated > 0, entry.Entity);
            }

            return Tuple.Create(true, item);
        }

        public bool Delete(TEntity item)
        {
            if (!Context.Set<TEntity>().Select(e => e.Id).Contains(item.Id))
            {
                Context.Set<TEntity>().Attach(item);
            }
            Context.Set<TEntity>().Remove(item);

            if (UsingUow)
            {
                var deleted = Context.SaveChanges();
                return deleted > 0;
            }

            return true;
        }
    }
}