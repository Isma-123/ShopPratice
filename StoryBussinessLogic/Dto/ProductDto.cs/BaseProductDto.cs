

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StoryBussinessLogic.Dto.ProductDto.cs
{
    public abstract class BaseProductDto
    {
      
      
        public string Name { get; set; } = null!;      
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; } 
        public string ImageUrl { get; set; } = null!;
        public DateTime date { get; set; } = DateTime.Now;
    

    }
}
