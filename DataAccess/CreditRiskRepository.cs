namespace DataAccess
{
    using System.Data.Entity;
    using Base;
    using Domain.DataAccess;
    using Domain.Models;

    public class CreditRiskRepository : Repository<CreditRisk,int>, ICreditRiskRepository
    {
        public CreditRiskRepository(DbContext context, bool usingUow = false) : base(context, usingUow)
        {
        }
    }
}