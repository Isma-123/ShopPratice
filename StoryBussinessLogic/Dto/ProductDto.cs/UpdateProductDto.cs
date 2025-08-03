

namespace StoryBussinessLogic.Dto.ProductDto.cs
{
    public class UpdateProductDto : BaseProductDto
    {
        public int ProductId { get; set; }
        public bool IsActive { get; set; }
    }
}
