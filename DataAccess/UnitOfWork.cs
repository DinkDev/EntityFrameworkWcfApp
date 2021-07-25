namespace DataAccess
{
    using System;
    using System.Data.Entity;
    using Domain.DataAccess;
    using EF;

    public class UnitOfWork : IUnitOfWork
    {
        public AutoLotContext Context { get; }
        public DbContextTransaction Transaction { get; set; }

        public UnitOfWork(AutoLotContext context)
        {
            Context = context;

            CreditRisks = new CreditRiskRepository(Context, true);
            Customers = new CustomerRepository(Context, true);
            Inventories = new InventoryRepository(Context, true);
            Orders = new OrderRepository(Context, true);
        }

        #region Entity Repositories

        public ICreditRiskRepository CreditRisks { get; }
        public ICustomerRepository Customers { get; }
        public IInventoryRepository Inventories { get; }
        public IOrderRepository Orders { get; }

        #endregion

        #region Aggregate functions

        // TODO: add any cross entity queries here

        #endregion

        #region Transactions

        public void BeginTransaction()
        {
            if (Transaction != null)
            {
                throw new InvalidOperationException("Cannot start more than one transaction");
            }
            Transaction = Context.Database.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
                Transaction.Dispose();
                Transaction = null;
            }
        }

        public int Commit()
        {
            var count = Context.SaveChanges();

            if (Transaction != null)
            {
                Transaction.Commit();
                Transaction.Dispose();
                Transaction = null;
            }

            return count;
        }

        #endregion

        #region Utilities

        public bool CanConnect()
        {
            return Context.Database.Exists();
        }

        public bool IsDirty(object entity)
        {
            return Context.ChangeTracker.HasChanges();
        }

        public void InitializeDatabase()
        {
            Database.SetInitializer(new AutoLotInitializer());

            Context.Database.Initialize(false);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            RollbackTransaction();

            Context.Dispose();
        }

        #endregion
    }
}
