namespace Domain.DataAccess
{
    using System;

    public interface IUnitOfWork : IDisposable
    {
        #region Entity Repositories

        ICreditRiskRepository CreditRisks { get; }

        ICustomerRepository Customers { get; }

        IInventoryRepository Inventories { get; }

        IOrderRepository Orders { get; }

        #endregion

        #region Aggregate functions

        // TODO: add any cross entity queries here

        #endregion

        #region Transactions

        /// <summary>
        /// Save all changes, and commit a transaction if one was started.
        /// </summary>
        int Commit();

        /// <summary>
        ///  Begin a transaction.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        ///  Rollback all changes under the transaction.
        /// </summary>
        void RollbackTransaction();

        #endregion

        #region Utilities

        /// <summary>
        /// Connection test - should always be called at app startup!
        /// </summary>
        /// <returns></returns>
        bool CanConnect();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool IsDirty(object entity);

        /// <summary>
        /// Used to setup the database
        /// </summary>
        void InitializeDatabase();

        #endregion

    }
}