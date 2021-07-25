namespace DataAccess
{
    using System.Data.Entity;
    using Base;
    using Domain.DataAccess;
    using Domain.Models;

    public class OrderRepository : Repository<Order, int>, IOrderRepository
    {
        public OrderRepository(DbContext context, bool usingUow = false) : base(context, usingUow)
        {
        }
    }
}