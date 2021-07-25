namespace DataAccess.EF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using Domain.Models;

    /// <summary>
    /// Formerly AutoLotEntities
    /// </summary>
    public partial class AutoLotContext : DbContext
    {
        public AutoLotContext()
            : base("name=AutoLotConnection")
        {
            //DbInterception.Add(new ConsoleWriterInterceptor());

            //DatabaseLogger.StartLogging();
            //DbInterception.Add(DatabaseLogger);

            //var context = (this as IObjectContextAdapter).ObjectContext;
            //context.ObjectMaterialized += OnObjectMaterialized;
            //context.SavingChanges += OnSavingChanges;
        }

        protected override void Dispose(bool disposing)
        {
            //DbInterception.Remove(DatabaseLogger);
            //DatabaseLogger.StopLogging();
            //base.Dispose(disposing);
        }

        private void OnSavingChanges(object sender, EventArgs eventArgs)
        {
            // Sender is of type ObjectContext. It can get current and original values,
            // and cancel/modify the save operation as desired.
            if (sender is ObjectContext context)
            {
                foreach (ObjectStateEntry item in
                    context.ObjectStateManager.GetObjectStateEntries(EntityState.Modified | EntityState.Added))
                {
                    //Do something important here
                    if (item.Entity is Inventory inventory)
                    {
                        if (inventory.Color == "Red")
                        {
                            item.RejectPropertyChanges(nameof(inventory.Color));
                        }
                    }
                }
            }
        }

        public virtual DbSet<CreditRisk> CreditRisks { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Inventory> Cars { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Car)
                .WillCascadeOnDelete(false);
        }
    }
}
