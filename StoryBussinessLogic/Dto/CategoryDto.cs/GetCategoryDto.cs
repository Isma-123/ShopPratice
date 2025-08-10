using StoryDates.cs.BussinessEntities;


namespace StoryBussinessLogic.Dto.CategoryDto.cs
{
    public class GetCategoryDto : BaseCategoryDto
    {
        public int CategoryId { get; set; }
        public bool IsActive { get; internal set; }
        public ICollection<ProductCategory> ProductCategoriesDto { get; set; } = null!;
    }
}
