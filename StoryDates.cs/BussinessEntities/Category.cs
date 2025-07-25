
using StoryDates.cs.BaseDate;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.PortableExecutable;

namespace StoryDates.cs.BussinessEntities
{

    [Table("", Schema = "")]
    public sealed class Category : BaseEntitie
    {
        [Key]
        [Required]
        public int CategoryId { get; set; }
        [MaxLength(100)]
        [Required]
        public string CategoryName { get; set; } = null!;

        public ICollection<ProductCategory> productCategories { get; set; } = null!;
    }
}
