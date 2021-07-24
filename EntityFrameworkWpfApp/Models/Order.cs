namespace EntityFrameworkWpfApp.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using DataAccess.Base;

    public partial class Order : EntityBase
    {
        public int CustomerId { get; set; }
        public int CarId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }
        [ForeignKey(nameof(CarId))]
        public virtual Inventory Car { get; set; }
    }
}
