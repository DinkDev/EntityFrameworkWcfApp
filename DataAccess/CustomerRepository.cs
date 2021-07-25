namespace DataAccess
{
    using System.Data.Entity;
    using Base;
    using Domain.DataAccess;
    using Domain.Models;

    public class CustomerRepository : Repository<Customer, int>, ICustomerRepository
    {
        public CustomerRepository(DbContext context, bool usingUow = false) : base(context, usingUow)
        {
        }
    }
}