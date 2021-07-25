namespace DataAccess
{
    using System.Data.Entity;
    using Base;
    using Domain.DataAccess;
    using Domain.Models;

    public class InventoryRepository : Repository<Inventory, int>, IInventoryRepository
    {
        public InventoryRepository(DbContext context, bool usingUow = false) : base(context, usingUow)
        {
        }
    }
}