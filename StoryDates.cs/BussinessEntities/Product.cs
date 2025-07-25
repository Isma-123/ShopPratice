

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using StoryDates.cs.BaseDate;

namespace StoryDates.cs.BussinessEntities
{
    [Table("", Schema = "")]
    public sealed class Product : BaseEntitie
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public string ImageUrl { get; set; } = null!;

        public ICollection<ProductCategory> productCategories { get; set; } = null!;

    }
}
