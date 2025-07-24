
using System.ComponentModel.DataAnnotations.Schema;

namespace StoryDates.cs.BussinessEntities
{
    public sealed class Order
    {
        public int OrderId { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int OrderDetailsId { get; set; }
        public ICollection<OrderDetails> Details { get; set; } = null!;

    }
}
