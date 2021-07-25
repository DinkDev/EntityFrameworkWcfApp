namespace Domain.Models.Base
{
    using System.ComponentModel.DataAnnotations;

    public class EntityBase
    {
        [Key]
        public int Id { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}