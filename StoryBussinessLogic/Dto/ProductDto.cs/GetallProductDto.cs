using StoryDates.cs.BussinessEntities;

namespace StoryBussinessLogic.Dto.ProductDto.cs
{
    public class GetallProductDto : BaseProductDto
    {
        public int ProductId { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ProductCategory> productCategories { get; set; } = null!;
    }
}
