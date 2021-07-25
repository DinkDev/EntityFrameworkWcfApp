namespace Domain.DataAccess
{
    using Models;

    public interface IOrderRepository : IRepository<Order, int>
    {
    }
}