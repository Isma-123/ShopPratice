

namespace StoryDates.cs.BussinessEntities
{
    public class ProductCategory
    { 
        public int ProductId { get; set; }

        public Product product { get; set; } = null!;


        public int CategoryId { get; set; } 
        public Category category {  get; set; } = null!;   
    } 
}
