

using System.ComponentModel.DataAnnotations.Schema;

namespace StoryDates.cs.BussinessEntities
{
    public sealed class OrderDetails
    { 
        public int orderDetailsId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

 

    }
}
