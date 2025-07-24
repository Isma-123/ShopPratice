

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StoryDates.cs.BussinessEntities
{
    public sealed class Product
    {
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; } = null!;

        public int CategoryId { get; set; }
        public ICollection<Category> Categories { get; set; } = null!;
       
    }
}
