

namespace StoryBussinessLogic.Dto.CategoryDto.cs
{
    public class UpdateCategoryDto : BaseCategoryDto
    {

        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
    }
}
