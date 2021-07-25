namespace Domain.DataAccess
{
    using Models;

    public interface ICustomerRepository : IRepository<Customer, int>
    {
    }
}